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

        private string _projectDtoClassName;
        private string _projectBllClassName;
        private string _projectDalClassName;
        private string _projectDtoParameterName;
        private string _projectDalParameterName;

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
                var columns = GenerateDtoClass(item);
                GenerateDalClass(item, columns);
                GenerateBllClass(columns);
                GenerateUserDefinedBllClass(columns);
            }
        }

        private List<DbColumn> GenerateDtoClass(TreeNode treeNode)
        {
            Microsoft.VisualStudio.Shell.ThreadHelper.ThrowIfNotOnUIThread();
            var generateDto = new GenerateDto(_databaseHelper);
            var objectId = long.Parse(treeNode.Tag.ToString());
            var columns = generateDto.Generate(objectId);
            var fileName = _projectDtoClassName + ProjectHelper.ClassExtension;

            var dataTransferObjectTemplate = new DataTransferObjectTemplate
            {
                Session = new Dictionary<string, object>
                {
                    {"Namespace",_projectDto.Name },
                    {"ClassName",_projectDtoClassName },
                    {"DbColumns",columns }
                }
            };
            dataTransferObjectTemplate.Initialize();
            var result = dataTransferObjectTemplate.TransformText();
            File.WriteAllText(_projectPathDto + "\\" + ProjectHelper.BaseFolderName + "\\" + fileName, result);
            ProjectHelper.IncludeNewFiles(fileName);
            return columns;
        }

        private void GenerateDalClass(TreeNode treeNode, List<DbColumn> columns)
        {
            Microsoft.VisualStudio.Shell.ThreadHelper.ThrowIfNotOnUIThread();
            var schemaName = GetSchemaName(treeNode);
            var tableName = GetTableName(treeNode);
            var fileName = _projectDalClassName + ProjectHelper.ClassExtension;
            var entitiesSql = GenerateDal.GetEntitiesSql(schemaName, tableName);
            var insertSql = GenerateDal.GetInsertSql(schemaName, tableName, columns);
            var updateSql = GenerateDal.GetUpdateSql(schemaName, tableName, columns);
            var deleteSql = GenerateDal.GetDeleteSql(schemaName, tableName, columns);

            var dataAccessLayerTemplate = new DataAccessLayerTemplate
            {
                Session = new Dictionary<string, object>
                {
                    {"Namespace", _projectDal.Name},
                    {"ClassName", _projectDalClassName},
                    {"SchemaName", schemaName},
                    {"GetSql", entitiesSql},
                    {"InsertSql", insertSql},
                    {"UpdateSql", updateSql},
                    {"DeleteSql", deleteSql},
                    {"DtoNamespace", _projectDto.Name},
                    {"DtoClassName", _projectDtoClassName},
                    {"DtoParameterName", _projectDtoParameterName},
                    {"DbColumns", columns}
                }
            };

            dataAccessLayerTemplate.Initialize();
            var result = dataAccessLayerTemplate.TransformText();
            File.WriteAllText(_projectPathDal + "\\" + ProjectHelper.BaseFolderName + "\\" + fileName, result);
            ProjectHelper.IncludeNewFiles(fileName);
        }

        private void GenerateBllClass(List<DbColumn> columns)
        {
            Microsoft.VisualStudio.Shell.ThreadHelper.ThrowIfNotOnUIThread();
            var fileName = _projectBllClassName + ProjectHelper.ClassExtension;

            var businessLogicLayerTemplate = new BusinessLogicLayerTemplate
            {
                Session = new Dictionary<string, object>
                {
                    {"Namespace",_projectBll.Name },
                    {"ClassName",_projectBllClassName },
                    {"DtoNamespace",_projectDto.Name },
                    {"DtoClassName",_projectDtoClassName },
                    {"DtoParameterName",_projectDtoParameterName },
                    {"DalNamespace",_projectDal.Name },
                    {"DalClassName",_projectDalClassName },
                    {"DalParameterName",_projectDalParameterName },
                    {"DbColumns",columns }
                }
            };
            businessLogicLayerTemplate.Initialize();
            var result = businessLogicLayerTemplate.TransformText();
            var filePath = _projectPathBll + "\\" + ProjectHelper.BaseFolderName + "\\" + fileName;
            File.WriteAllText(filePath, result);
            ProjectHelper.IncludeNewFiles(fileName);
        }

        private void GenerateUserDefinedBllClass(List<DbColumn> columns)
        {
            Microsoft.VisualStudio.Shell.ThreadHelper.ThrowIfNotOnUIThread();
            var fileName = _projectBllClassName + ProjectHelper.ClassExtension;
            var filePath = _projectPathBll + "\\" + ProjectHelper.UserDefinedFolderName + "\\" + fileName;
            if (File.Exists(filePath) == true)
                return;

            var businessLogicLayerUdTemplate = new BusinessLogicLayerUDTemplate
            {
                Session = new Dictionary<string, object>
                {
                    {"Namespace",_projectBll.Name },
                    {"ClassName",_projectBllClassName },
                    {"DtoNamespace",_projectDto.Name },
                    {"DtoClassName",_projectDtoClassName },
                    {"DtoParameterName",_projectDtoParameterName },
                    {"DalNamespace",_projectDal.Name },
                    {"DalClassName",_projectDalClassName },
                    {"DalParameterName",_projectDalParameterName },
                    {"DbColumns",columns }
                }
            };
            businessLogicLayerUdTemplate.Initialize();
            var result = businessLogicLayerUdTemplate.TransformText();
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
            _projectDtoClassName = className + ProjectHelper.DataTransferObjectSuffix;
            _projectDalClassName = className + ProjectHelper.DataAccessLayerSuffix;
            _projectBllClassName = className + ProjectHelper.BusinessLayerSuffix;
        }

        private void GenerateParameterNames(TreeNode treeNode)
        {
            var tableName = GetTableName(treeNode);
            var parameterName = ProjectHelper.GetCamelCase(GetClassName(tableName));
            _projectDtoParameterName = parameterName + ProjectHelper.DataTransferObjectSuffix;
            _projectDalParameterName = parameterName + ProjectHelper.DataAccessLayerSuffix;
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
