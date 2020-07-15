using System.Collections.Generic;
using System.IO;
using System.Linq;
using EnvDTE;
using EnvDTE80;
using Microsoft.VisualStudio.Shell;

namespace ClassGenerator.Extension.Helper
{
    public static class ProjectHelper
    {
        public const string ClassExtension = ".cs";
        public const string BaseFolderName = "Base";
        public const string UserDefinedFolderName = "UserDefined";
        public const string DataTransferObjectSuffix = "Dto";
        public const string DataAccessLayerSuffix = "Dal";
        public const string BusinessLayerSuffix = "Bll";
        public const string ProjectSuffix = ".csproj";
        private static readonly DTE2 Dte = ClassGeneratorExtensionPackage.Instance.Dte;

        public static Project GetDataObjectProject()
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            var projects = Dte.Solution.Projects;
            return projects.Cast<Project>().FirstOrDefault(project => project.UniqueName.EndsWith(DataTransferObjectSuffix + ProjectSuffix));
        }

        public static Project GetDataAccessProject()
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            return Dte.Solution.Projects.Cast<Project>().FirstOrDefault(project => project.UniqueName.EndsWith(DataAccessLayerSuffix + ProjectSuffix));
        }

        public static Project GetBusinessLayerProject()
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            var projects = Dte.Solution.Projects;
            return projects.Cast<Project>().FirstOrDefault(project => project.UniqueName.EndsWith("." + BusinessLayerSuffix + ProjectSuffix));
        }

        /// <summary>
        /// </summary>
        /// <param name="project"></param>
        /// <returns></returns>
        public static string GetFullPath(this Project project)
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            return project.Properties.Item("FullPath").Value.ToString();
        }

        public static string GetPascalCase(string name)
        {
            return (char.ToUpperInvariant(name[0]) + name.Substring(1)).Replace("_", string.Empty)
                .Replace(" ", string.Empty);
        }

        public static string GetCamelCase(string name)
        {
            return (char.ToLowerInvariant(name[0]) + name.Substring(1)).Replace("_", string.Empty)
                .Replace(" ", string.Empty);
        }

        public static void IncludeNewFiles(string fileName, string folderName = BaseFolderName)
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            var count = 0;

            foreach (Project project in Dte.Solution.Projects)
            {
                if (!project.UniqueName.EndsWith(BusinessLayerSuffix + ProjectSuffix) &&
                    !project.UniqueName.EndsWith(DataAccessLayerSuffix + ProjectSuffix) &&
                    !project.UniqueName.EndsWith(DataTransferObjectSuffix + ProjectSuffix))
                {
                    continue;
                }

                var newfiles = GetFilesNotInProject(project, fileName, folderName);

                foreach (var file in newfiles)
                    project.ProjectItems.AddFromFile(file);

                count += newfiles.Count;
            }

            Dte.StatusBar.Text =
                $"{count} new file{(count == 1 ? "" : "s")} included in the project.";
        }

        private static List<string> GetAllProjectFiles(ProjectItems projectItems, string extension)
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            var returnValue = new List<string>();

            foreach (ProjectItem projectItem in projectItems)
            {
                for (short i = 1; i <= projectItems.Count; i++)
                {
                    var fileName = projectItem.FileNames[i];
                    if (Path.GetExtension(fileName)?.ToLower() == extension)
                        returnValue.Add(fileName);
                }
                returnValue.AddRange(GetAllProjectFiles(projectItem.ProjectItems, extension));
            }

            return returnValue;
        }

        private static List<string> GetFilesNotInProject(Project project, string fileName, string folderName)
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            var returnValue = new List<string>();
            var startPath = Path.GetDirectoryName(project.FullName);
            var projectFiles = GetAllProjectFiles(project.ProjectItems, ".cs");

            if (startPath == null)
                return returnValue;

            startPath = startPath + "\\" + folderName;

            returnValue.AddRange(Directory.GetFiles(startPath, fileName, SearchOption.AllDirectories).Where(file => !projectFiles.Contains(file)));
            return returnValue;
        }
    }
}