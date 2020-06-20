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
            _projectPathBll = _projectDal.GetFullPath();

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
                var columns = GenerateDtoClass(item);
                GenerateDalClass(item, columns);
                GenerateBllClass(item,columns);
            }
        }

        private List<DbColumn> GenerateDtoClass(TreeNode treeNode)
        {
            Microsoft.VisualStudio.Shell.ThreadHelper.ThrowIfNotOnUIThread();
            var generateDto = new GenerateDto(_databaseHelper);
            var objectId = long.Parse(treeNode.Tag.ToString());
            var tableName = treeNode.Text.Split('.')[1];
            var columns = generateDto.Generate(objectId);

            var dataTransferObjectTemplate = new DataTransferObjectTemplate();
            var className = ProjectHelper.GetPascalCase(tableName) + ProjectHelper.DataTransferObjectSuffix;
            var fileName = className + ProjectHelper.ClassExtension;
            dataTransferObjectTemplate.Session = new Dictionary<string, object>()
            {
                {"Namespace",_projectDto.Name },
                {"ClassName",className },
                {"DbColumns",columns }
            };
            dataTransferObjectTemplate.Initialize();
            var result = dataTransferObjectTemplate.TransformText();
            File.WriteAllText(_projectDto.GetFullPath() + "\\" + ProjectHelper.BaseFolderName + "\\" + fileName, result);
            ProjectHelper.IncludeNewFiles(fileName);
            return columns;
        }

        private void GenerateDalClass(TreeNode treeNode, List<DbColumn> columns)
        {
            Microsoft.VisualStudio.Shell.ThreadHelper.ThrowIfNotOnUIThread();
            var schemaName = treeNode.Text.Split('.')[0];
            var tableName = treeNode.Text.Split('.')[1];
            var entitiesSql = GenerateDal.GetEntitiesSql(schemaName, tableName);
            var insertSql = GenerateDal.GetInsertSql(schemaName, tableName, columns);
            var updateSql = GenerateDal.GetUpdateSql(schemaName, tableName, columns);
            var deleteSql = GenerateDal.GetDeleteSql(schemaName, tableName, columns);

            var dataAccessLayerTemplate = new DataAccessLayerTemplate();
            var className = ProjectHelper.GetPascalCase(tableName) + ProjectHelper.DataAccessLayerSuffix;
            var dtoClassName = ProjectHelper.GetPascalCase(tableName) + ProjectHelper.DataTransferObjectSuffix;
            var dtoParameterName = ProjectHelper.GetCamelCase(tableName) + ProjectHelper.DataTransferObjectSuffix;
            var fileName = className + ProjectHelper.ClassExtension;
            dataAccessLayerTemplate.Session = new Dictionary<string, object>()
            {
                {"Namespace",_projectDal.Name },
                {"DtoNamespace",_projectDto.Name },
                {"DtoClassName",dtoClassName },
                {"DtoParameterName",dtoParameterName },
                {"ClassName",className },
                {"SchemaName",schemaName },
                {"GetSql",entitiesSql},
                {"InsertSql",insertSql },
                {"UpdateSql",updateSql },
                {"DeleteSql",deleteSql },
                {"DbColumns",columns }
            };
            dataAccessLayerTemplate.Initialize();
            var result = dataAccessLayerTemplate.TransformText();
            File.WriteAllText(_projectDal.GetFullPath() + "\\" + ProjectHelper.BaseFolderName + "\\" + fileName, result);
            ProjectHelper.IncludeNewFiles(fileName);
        }

        private void GenerateBllClass(TreeNode treeNode, List<DbColumn> columns)
        {
            Microsoft.VisualStudio.Shell.ThreadHelper.ThrowIfNotOnUIThread();
            var tableName = treeNode.Text.Split('.')[1];

            var businessLogicLayerTemplate = new BusinessLogicLayerTemplate();
            var className = ProjectHelper.GetPascalCase(tableName) + ProjectHelper.BusinessLayerSuffix;
            var dtoClassName = ProjectHelper.GetPascalCase(tableName) + ProjectHelper.DataTransferObjectSuffix;
            var dtoParameterName = ProjectHelper.GetCamelCase(tableName) + ProjectHelper.DataTransferObjectSuffix;
            var dalClassName = ProjectHelper.GetPascalCase(tableName) + ProjectHelper.DataAccessLayerSuffix;
            var dalParameterName = "_" + ProjectHelper.GetCamelCase(tableName) + ProjectHelper.DataAccessLayerSuffix;
            var fileName = className + ProjectHelper.ClassExtension;
            businessLogicLayerTemplate.Session = new Dictionary<string, object>()
            {
                {"Namespace",_projectDal.Name },
                {"DtoNamespace",_projectDto.Name },
                {"DtoClassName",dtoClassName },
                {"DtoParameterName",dtoParameterName },
                {"DalNamespace",_projectDal.Name },
                {"DalClassName",dalClassName },
                {"DalParameterName",dalParameterName },
                {"ClassName",className },
                {"DbColumns",columns }
            };
            businessLogicLayerTemplate.Initialize();
            var result = businessLogicLayerTemplate.TransformText();
            File.WriteAllText(_projectBll.GetFullPath() + "\\" + ProjectHelper.BaseFolderName + "\\" + fileName, result);
            ProjectHelper.IncludeNewFiles(fileName);
        }
    }
}
