namespace Osiris.Tfs.Monitor
{
	#region Using

	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Windows.Input;

	#endregion // Using

	public static class AppCommands
	{
		#region Properties

		public static RoutedUICommand ApplicationClose { get; private set; }
		public static RoutedUICommand ApplicationOptions { get; private set; }
		public static RoutedUICommand SlideShowFromBeginning { get; private set; }
		public static RoutedUICommand StopSlideshow { get; private set; }
		public static RoutedUICommand FileOpen { get; private set; }
		public static RoutedUICommand FileSave { get; private set; }
		public static RoutedUICommand FileSaveAs { get; private set; }
		public static RoutedUICommand SlideNew { get; private set; }
		public static RoutedUICommand InsertSlideElement { get; private set; }
		public static RoutedUICommand SlideEdit { get; private set; }
		public static RoutedUICommand ChangeIteration { get; private set; }
		public static RoutedUICommand Rename { get; private set; }
		public static RoutedUICommand Delete { get; private set; }
		public static RoutedUICommand ExcludeWeekEnds { get; private set; }
		public static RoutedUICommand ApplicationNew { get; private set; }
		public static RoutedUICommand SlideRefresh { get; private set; }

		#endregion // Properties

		#region Constructors

		static AppCommands()
		{
			ApplicationClose = AddCommand("Close application", "ApplicationClose");
			FileOpen = AddCommand("Open File", "FileOpen", Key.O);
			FileSave = AddCommand("Save File", "FileSave", Key.S);
			FileSaveAs = AddCommand("Save File As...", "FileSaveAs");
			ApplicationOptions = AddCommand("Options", "Options");
			SlideShowFromBeginning = AddCommand("StartSlideshow", "StartSlideshow", Key.F5);
			StopSlideshow = AddCommand("StopSlideshow", "StopSlideshow", Key.Escape);
			SlideNew = AddCommand("SlideNew", "SlideNew", Key.N);
			InsertSlideElement = AddCommand("InsertSlideElement", "InsertSlideElement");
			SlideEdit = AddCommand("SlideEdit", "SlideNew");
			ChangeIteration = AddCommand("ChangeIteration", "ChangeIteration");
			Rename = AddCommand("Rename", "Rename", Key.F2);
			Delete = AddCommand("Delete", "Delete", Key.Delete);
			ExcludeWeekEnds = AddCommand("ExcludeWeekEnds", "ExcludeWeekEnds");
			ApplicationNew = AddCommand("Create new application", "ApplicationNew");
			SlideRefresh = AddCommand("Refresh selected slide", "SlideRefresh");
		}

		#endregion // Constructors

		#region Methods

		/// <summary>
		/// Create routed command.
		/// </summary>
		/// <param name="text"></param>
		/// <param name="name"></param>
		/// <returns></returns>
		private static RoutedUICommand AddCommand(string text, string name)
		{
			return new RoutedUICommand(text, name, typeof(AppCommands));
		}

		/// <summary>
		/// Create routed command with CTRL + key shortcut
		/// </summary>
		/// <param name="text"></param>
		/// <param name="name"></param>
		/// <param name="key"></param>
		/// <returns></returns>
		private static RoutedUICommand AddCommand(string text, string name, Key key)
		{
			return AddCommand(text, name, key, ModifierKeys.None);
		}

		/// <summary>
		/// Create routed command with CTRL + key shortcut with [SHIFT] as an option 
		/// </summary>
		/// <param name="text"></param>
		/// <param name="name"></param>
		/// <param name="key"></param>
		/// <param name="shift"></param>
		/// <returns></returns>
		private static RoutedUICommand AddCommand(string text, string name, Key key, ModifierKeys modKey)
		{
			var cmd = AddCommand(text, name);
			if (modKey != ModifierKeys.None)
			{
				cmd.InputGestures.Add(new KeyGesture(key, modKey));
			}
			return cmd;
		}

		/// <summary>
		/// Handler for CanExecute which always executes
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public static void CanExecuteHandler(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = true;
		}

		/// <summary>
		/// Bind routed command to Application's main window. Will always execute.
		/// </summary>
		/// <param name="uc"></param>
		/// <param name="cmd"></param>
		/// <param name="exeHandler"></param>
		public static void BindCommand(ICommand cmd, ExecutedRoutedEventHandler exeHandler)
		{
			BindCommand(cmd, exeHandler, AppCommands.CanExecuteHandler);
		}

		/// <summary>
		/// Bind routed command to Application's main window.
		/// </summary>
		/// <param name="uc"></param>
		/// <param name="cmd"></param>
		/// <param name="exeHandler"></param>
		/// <param name="canExeHandler"></param>
		public static void BindCommand(ICommand cmd, ExecutedRoutedEventHandler exeHandler,
			CanExecuteRoutedEventHandler canExeHandler)
		{
			var cmdBinding = new CommandBinding(cmd);
			cmdBinding.Executed += new ExecutedRoutedEventHandler(exeHandler);
			cmdBinding.CanExecute += new CanExecuteRoutedEventHandler(canExeHandler);
			System.Windows.Application.Current.MainWindow.CommandBindings.Add(cmdBinding);
		}

		#endregion // Methods
	}
}
