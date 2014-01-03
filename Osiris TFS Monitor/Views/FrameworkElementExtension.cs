namespace Osiris.Tfs.Monitor
{
	#region Using

	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Windows;
	using System.Windows.Controls;
	using System.Windows.Data;
	using System.Windows.Documents;
	using System.Windows.Input;
	using System.Windows.Media;
	using System.Windows.Media.Imaging;
	using System.Windows.Navigation;
	using System.Windows.Shapes;
	using System.ComponentModel;
	using Osiris.Tfs.Monitor;

	#endregion // Using

	/// <summary>
	/// Method exstensions for FrameworkElement
	/// </summary>
	public static class FrameworkElementExtension
	{
		#region Methods

		public static void BindDataContextIfDesigntime(this FrameworkElement fe, object obj)
		{
			// Designtime?
			if (LicenseManager.UsageMode == LicenseUsageMode.Designtime)
			{
				fe.DataContext = obj;
			}
		}

		/// <summary>
		/// Clear width and height of usercontrol if not running at designtime.
		/// </summary>
		/// <param name="uc">Exstension instance</param>
		public static void ClearWidthAndHeightForRuntime(this FrameworkElement fe)
		{
			// Designtime?
			if (LicenseManager.UsageMode != LicenseUsageMode.Designtime)
			{
				fe.Width = double.NaN;
				fe.Height = double.NaN;
			}
		}

		/// <summary>
		/// Bind routed command to FrameworkElement. Will always execute.
		/// </summary>
		/// <param name="uc"></param>
		/// <param name="cmd"></param>
		/// <param name="exeHandler"></param>
		public static void BindCommand(this UIElement elem, ICommand cmd, ExecutedRoutedEventHandler exeHandler)
		{
			elem.BindCommand(cmd, exeHandler, AppCommands.CanExecuteHandler);
		}

		/// <summary>
		/// Bind routed command to FrameworkElement.
		/// </summary>
		/// <param name="uc"></param>
		/// <param name="cmd"></param>
		/// <param name="exeHandler"></param>
		/// <param name="canExeHandler"></param>
		public static void BindCommand(this UIElement elem, ICommand cmd, ExecutedRoutedEventHandler exeHandler,
			CanExecuteRoutedEventHandler canExeHandler)
		{
			var cmdBinding = new CommandBinding(cmd);
			cmdBinding.Executed += new ExecutedRoutedEventHandler(exeHandler);
			cmdBinding.CanExecute += new CanExecuteRoutedEventHandler(canExeHandler);
			elem.CommandBindings.Add(cmdBinding);
		}

		#endregion // Methods
	}
}
