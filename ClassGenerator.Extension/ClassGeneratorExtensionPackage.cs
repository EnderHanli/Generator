using System;
using System.Runtime.InteropServices;
using System.Threading;
using EnvDTE;
using EnvDTE80;
using Microsoft.VisualStudio.Shell;
using Task = System.Threading.Tasks.Task;

namespace ClassGenerator.Extension
{

    [PackageRegistration(UseManagedResourcesOnly = true, AllowsBackgroundLoading = true)]
    [Guid(PackageGuidString)]
    [ProvideMenuResource("Menus.ctmenu", 1)]
    public sealed class ClassGeneratorExtensionPackage : AsyncPackage
    {
        public DTE2 Dte;

        public static ClassGeneratorExtensionPackage Instance;

        /// <summary>
        /// ClassGeneratorExtensionPackage GUID string.
        /// </summary>
        public const string PackageGuidString = "99ec2f91-a329-491b-9022-237baee04dfc";

        #region Package Members
        protected override async Task InitializeAsync(CancellationToken cancellationToken, IProgress<ServiceProgressData> progress)
        {
            await this.JoinableTaskFactory.SwitchToMainThreadAsync(cancellationToken);
            GenerateCommand.Initialize(this);
            await base.InitializeAsync(cancellationToken, progress);
            Dte = await GetServiceAsync(typeof(DTE)) as DTE2;
            Instance = this;
        }
        #endregion
    }
}
