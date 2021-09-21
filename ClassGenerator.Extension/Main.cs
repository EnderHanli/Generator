using ClassGenerator.Extension.Generate;
using ClassGenerator.Extension.Helper;
using ClassGenerator.Extension.Model;
using ClassGenerator.Extension.Template;
using EnvDTE;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using DbColumn = ClassGenerator.Extension.Model.DbColumn;

namespace ClassGenerator.Extension
{
    public partial class Main : Form
    {
        private string _projectPathDto;
        private string _projectPathDal;
        private string _projectPathBll;

        private Project _projectDto;
        private Project _projectDal;
        private Project _projectBll;

        private string _projectEntityClassName;
        private string _projectServiceClassName;
        private string _projectRespositoryClassName;
        private string _projectEntityParameterName;
        private string _projectRepositoryParameterName;

        private DatabaseHelper _databaseHelper;
        private Dictionary<string, List<DbModel>> _dbObjects;
        private readonly List<TreeNode> _checkedNodes;

        public Main()
        {
            InitializeComponent();
            _checkedNodes = new List<TreeNode>();
            treeView1.CheckBoxes = true;
            treeView1.AfterCheck += TreeView1_AfterCheck;
            _initialize();
            LoadData();
        }

        private void _initialize()
        {
            _projectDto = ProjectHelper.GetDataObjectProject();
            _projectDal = ProjectHelper.GetDataAccessProject();
            _projectBll = ProjectHelper.GetBusinessLayerProject();

            _projectPathDto = _projectDto.GetFullPath();
            _projectPathDal = _projectDal.GetFullPath();
            _projectPathBll = _projectBll.GetFullPath();

            _databaseHelper = new DatabaseHelper(_projectPathDto);
        }

        private void TreeView1_AfterCheck(object sender, TreeViewEventArgs e)
        {
            if (e.Action == TreeViewAction.Unknown)
                return;
            if (e.Node.Nodes.Count > 0)
            {
                CheckAllChildNodes(e.Node, e.Node.Checked);
            }
            else
            {
                if (e.Node.Checked)
                {
                    _checkedNodes.Add(e.Node);
                }
                else
                {
                    _checkedNodes.Remove(e.Node);
                }
            }
        }

        private static void CheckAllChildNodes(TreeNode treeNode, bool nodeChecked)
        {
            for (int i = 0; i < treeNode.Nodes.Count; i++)
            {
                var node = treeNode.Nodes[i];
                node.Checked = nodeChecked;
                if (node.Nodes.Count > 0)
                {
                    CheckAllChildNodes(node, nodeChecked);
                }
            }
        }

        private void BtnGenerate_Click(object sender, EventArgs e)
        {
            GenerateClass();
            MessageBox.Show("Tamamlandı");
        }

        private void BtnLoad_Click(object sender, EventArgs e)
        {
            LoadData();
        }

        private void LoadData()
        {
            _dbObjects = new Dictionary<string, List<DbModel>>();
            var serverName = _databaseHelper.GetServerName();
            var tableModels = _databaseHelper.LoadTable();
            _dbObjects.Add("Tables", tableModels);

            var viewModels = _databaseHelper.LoadViews();
            _dbObjects.Add("Views", viewModels);
            LoadTreeView(serverName, _databaseHelper.Connection.Database);
        }

        private void LoadTreeView(string server, string database)
        {
            treeView1.Nodes.Clear();

            var n = treeView1.Nodes.Add("Server(" + server + ")");

            var serverNode = n.Nodes.Add(database);

            foreach (var dbItemType in _dbObjects.Keys.ToArray())
            {
                var dbItemTypeNode = serverNode.Nodes.Add(dbItemType);
                LoadTreeNode(dbItemTypeNode);
            }

            n.Expand();
            serverNode.Expand();
        }

        private void LoadTreeNode(TreeNode n)
        {
            foreach (var t in _dbObjects[n.Text])
            {
                TreeNode node = new TreeNode();
                node.Text = t.SchemaName + "." + t.Name;
                node.Tag = t.ObjectId;
                n.Nodes.Add(node);
            }
        }

        private void GenerateClass()
        {
            foreach (var item in _checkedNodes)
            {
                GenerateClassNames(item);
                GenerateParameterNames(item);
                var columns = GenerateEntityClass(item);
                GenerateDalClass(item, columns);
                GenerateBllClass(columns);
                GenerateUserDefinedBllClass(columns);
            }
        }

        private List<DbColumn> GenerateEntityClass(TreeNode treeNode)
        {
            Microsoft.VisualStudio.Shell.ThreadHelper.ThrowIfNotOnUIThread();
            var generateEntity = new GenerateEntity(_databaseHelper);
            var objectId = long.Parse(treeNode.Tag.ToString());
            var columns = generateEntity.Generate(objectId);
            var fileName = _projectEntityClassName;

            var entityTemplate = new EntityTemplate
            {
                Session = new Dictionary<string, object>
                {
                    {"Namespace",_projectDto.Name },
                    {"ClassName",_projectEntityClassName },
                    {"DbColumns",columns }
                }
            };
            entityTemplate.Initialize();
            var result = entityTemplate.TransformText();
            File.WriteAllText(_projectPathDto + "\\" + ProjectHelper.BaseFolderName + "\\" + fileName, result);
            ProjectHelper.IncludeNewFiles(fileName);
            return columns;
        }

        private void GenerateDalClass(TreeNode treeNode, List<DbColumn> columns)
        {
            Microsoft.VisualStudio.Shell.ThreadHelper.ThrowIfNotOnUIThread();
            var schemaName = GetSchemaName(treeNode);
            var tableName = GetTableName(treeNode);
            var fileName = _projectRespositoryClassName + ProjectHelper.ClassExtension;
            var entitiesSql = GenerateRepository.GetEntitiesSql(schemaName, tableName);
            var insertSql = GenerateRepository.GetInsertSql(schemaName, tableName, columns);
            var updateSql = GenerateRepository.GetUpdateSql(schemaName, tableName, columns);
            var deleteSql = GenerateRepository.GetDeleteSql(schemaName, tableName, columns);

            var repositoryTemplate = new RepositoryTemplate
            {
                Session = new Dictionary<string, object>
                {
                    {"Namespace", _projectDal.Name},
                    {"ClassName", _projectRespositoryClassName},
                    {"SchemaName", schemaName},
                    {"GetSql", entitiesSql},
                    {"InsertSql", insertSql},
                    {"UpdateSql", updateSql},
                    {"DeleteSql", deleteSql},
                    {"DtoNamespace", _projectDto.Name},
                    {"DtoClassName", _projectEntityClassName},
                    {"DtoParameterName", _projectEntityParameterName},
                    {"DbColumns", columns}
                }
            };

            repositoryTemplate.Initialize();
            var result = repositoryTemplate.TransformText();
            File.WriteAllText(_projectPathDal + "\\" + ProjectHelper.BaseFolderName + "\\" + fileName, result);
            ProjectHelper.IncludeNewFiles(fileName);
        }

        private void GenerateBllClass(List<DbColumn> columns)
        {
            Microsoft.VisualStudio.Shell.ThreadHelper.ThrowIfNotOnUIThread();
            var fileName = _projectServiceClassName + ProjectHelper.ClassExtension;

            var serviceBaseTemplate = new ServiceBaseTemplate
            {
                Session = new Dictionary<string, object>
                {
                    {"Namespace",_projectBll.Name },
                    {"ClassName",_projectServiceClassName },
                    {"DtoNamespace",_projectDto.Name },
                    {"DtoClassName",_projectEntityClassName },
                    {"DtoParameterName",_projectEntityParameterName },
                    {"DalNamespace",_projectDal.Name },
                    {"DalClassName",_projectRespositoryClassName },
                    {"DalParameterName",_projectRepositoryParameterName },
                    {"DbColumns",columns }
                }
            };
            serviceBaseTemplate.Initialize();
            var result = serviceBaseTemplate.TransformText();
            var filePath = _projectPathBll + "\\" + ProjectHelper.BaseFolderName + "\\" + fileName;
            File.WriteAllText(filePath, result);
            ProjectHelper.IncludeNewFiles(fileName);
        }

        private void GenerateUserDefinedBllClass(List<DbColumn> columns)
        {
            Microsoft.VisualStudio.Shell.ThreadHelper.ThrowIfNotOnUIThread();
            var fileName = _projectServiceClassName + ProjectHelper.ClassExtension;
            var filePath = _projectPathBll + "\\" + ProjectHelper.UserDefinedFolderName + "\\" + fileName;
            if (File.Exists(filePath) == true)
                return;

            var serviceUserDefinedTemplate = new ServiceUserDefinedTemplate
            {
                Session = new Dictionary<string, object>
                {
                    {"Namespace",_projectBll.Name },
                    {"ClassName",_projectServiceClassName },
                    {"DtoNamespace",_projectDto.Name },
                    {"DtoClassName",_projectEntityClassName },
                    {"DtoParameterName",_projectEntityParameterName },
                    {"DalNamespace",_projectDal.Name },
                    {"DalClassName",_projectRespositoryClassName },
                    {"DalParameterName",_projectRepositoryParameterName },
                    {"DbColumns",columns }
                }
            };
            serviceUserDefinedTemplate.Initialize();
            var result = serviceUserDefinedTemplate.TransformText();
            File.WriteAllText(filePath, result);
            ProjectHelper.IncludeNewFiles(fileName, ProjectHelper.UserDefinedFolderName);
        }

        private static string GetClassName(string tableName)
        {
            var className = "";
            var tableStrings = tableName.Split('_');
            if (tableStrings.Length == 1)
            {
                className = tableName;
            }
            else
            {
                for (int i = 1; i < tableStrings.Length; i++)
                {
                    className += tableStrings[i];
                }
            }
            return className;
        }

        private void GenerateClassNames(TreeNode treeNode)
        {
            var tableName = GetTableName(treeNode);
            string className = ProjectHelper.GetPascalCase(GetClassName(tableName));
            _projectEntityClassName = className + ProjectHelper.DataTransferObjectSuffix;
            _projectRespositoryClassName = className + ProjectHelper.RepositorySuffix;
            _projectServiceClassName = className + ProjectHelper.ServiceSuffix;
        }

        private void GenerateParameterNames(TreeNode treeNode)
        {
            var tableName = GetTableName(treeNode);
            var parameterName = ProjectHelper.GetCamelCase(GetClassName(tableName));
            _projectEntityParameterName = parameterName + ProjectHelper.DataTransferObjectSuffix;
            _projectRepositoryParameterName = parameterName + ProjectHelper.RepositorySuffix;
        }

        private static string GetTableName(TreeNode treeNode)
        {
            var treeNodeStrings = treeNode.Text.Split('.');
            return treeNodeStrings.Length == 1 ? treeNode.Text : treeNodeStrings[1];
        }

        private static string GetSchemaName(TreeNode treeNode)
        {
            var schemaName = "";
            var treeNodeStrings = treeNode.Text.Split('.');
            if (treeNodeStrings.Length > 1)
                schemaName = treeNodeStrings[0];
            return schemaName;
        }
    }
}
