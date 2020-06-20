using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Utilities;
using System;
using System.ComponentModel.Design;

namespace ClassGenerator.Extension
{
    /// <summary>
    /// Command handler
    /// </summary>
    internal sealed class GenerateCommand
    {
        /// <summary>
        /// Command ID.
        /// </summary>
        public const int CommandId = 0x0100;

        /// <summary>
        /// Command menu group (command set GUID).
        /// </summary>
        public static readonly Guid CommandSet = new Guid("bfd2133c-98f2-4527-b255-e2edbf96f720");

        /// <summary>
        /// VS Package that provides this command, not null.
        /// </summary>
        private readonly Package _package;

        /// <summary>
        /// Initializes a new instance of the <see cref="GenerateCommand"/> class.
        /// Adds our command handlers for menu (commands must exist in the command table file)
        /// </summary>
        /// <param name="package">Owner package, not null.</param>
        private GenerateCommand(Package package)
        {
            this._package = package ?? throw new ArgumentNullException(nameof(package));

            if (!(ServiceProvider.GetService(typeof(IMenuCommandService)) is OleMenuCommandService commandService))
                return;

            var menuCommandId = new CommandID(CommandSet, CommandId);
            var menuItem = new MenuCommand(MenuItemCallback, menuCommandId);
            commandService.AddCommand(menuItem);
        }

        /// <summary>
        /// Gets the instance of the command.
        /// </summary>
        public static GenerateCommand Instance
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the service provider from the owner package.
        /// </summary>
        private IServiceProvider ServiceProvider => this._package;

        /// <summary>
        /// Initializes the singleton instance of the command.
        /// </summary>
        /// <param name="package">Owner package, not null.</param>
        public static void Initialize(Package package)
        {
            Instance = new GenerateCommand(package);
        }

        /// <summary>
        /// This function is the callback used to execute the command when the menu item is clicked.
        /// See the constructor to see how the menu item is associated with this function using
        /// OleMenuCommandService service and MenuCommand class.
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">Event args.</param>
        private static void MenuItemCallback(object sender, EventArgs e)
        {
            using (DpiAwareness.EnterDpiScope(DpiAwarenessContext.SystemAware))
            {
                var obj = new Main();
                obj.Show();
            }
        }
    }
}
