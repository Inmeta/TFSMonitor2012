using Twitterizer;

namespace Osiris.Tfs.Monitor
{
	#region Using

	using System;
	using System.Collections.Generic;
	using System.Configuration;
	using System.Data;
	using System.Linq;
	using System.Windows;
	using System.Windows.Threading;
	using System.Diagnostics;
	using Osiris.Tfs.Monitor.Models;
	using Osiris.Tfs.Monitor.Properties;
	using System.Net;
	using System.Security.Principal;
	using System.Reflection;

	#endregion // Using


	public partial class App : Application
	{
		#region Properties

		#endregion // Properties

		#region Methods

		/// <summary>
		/// When application is started as a screensaver, the framework
		/// intialized the app with following parameters:
		/// 
		///		/c			Show configuration window
		///		/p [time]	Preview mode
		///		/s			Normal mode
		///
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void OnStartup(Object sender, StartupEventArgs e)
		{
			if (Settings.Default.CallUpgrade)
			{
				Settings.Default.Upgrade();
				Settings.Default.CallUpgrade = false;
			}

			UpdateTfsServiceConnection();

			// No parameters? Run standard mode.
			if (e.Args.Length <= 0)
			{
				this.MainWindow = new ApplicationView();
			}
			else
			{
				// Console mode?
				if (e.Args[0].ToLower() == "/c")
				{
					//this.MainWindow = new ScreenSaverConsoleView();
				}

				// Preview mode?
				if (e.Args[0].ToLower() == "/p")
				{
					this.MainWindow = new ScreensaverView((e.Args.Length > 1) ? e.Args[1] : null);
				}

				// Screen saver mode?
				if (e.Args[0].ToLower() == "/s")
				{
					this.MainWindow = new ScreensaverView();
				}

				// Open document?
				if (e.Args[0].ToLower().EndsWith(".tmdoc"))
				{
					this.MainWindow = new ApplicationView(new ApplicationVm(e.Args[0]));
				}

			}

			if (this.MainWindow != null)
			{
				this.MainWindow.Show();
			}
		}

		private void TestTwitterizer()
		{
			string query = "#Inmeta";
			int pageNumber = 1;

			var options = new SearchOptions()
			{
				PageNumber = pageNumber,
				NumberPerPage = 2
			};

			var searchResult = TwitterSearch.Search(query, options);

			while (searchResult.Result == RequestResult.Success && pageNumber < 5)
			{
				Debug.WriteLine("==== PAGE {0} ====", pageNumber);
				Debug.WriteLine("");

				foreach (var tweet in searchResult.ResponseObject)
				{
					Debug.WriteLine("[{0}] {1,-10}: {2}", tweet.CreatedDate, tweet.FromUserScreenName, tweet.Text);
				}

				pageNumber++;
				options.PageNumber = pageNumber;
				searchResult = TwitterSearch.Search(query, options);
				Debug.WriteLine("");
			}

		}

		/// <summary>
		/// Called when application exists. Clean up.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void OnExit(object sender, ExitEventArgs e)
		{
		}

		/// <summary>
		/// Display unhandled exception in application
		/// 
		/// TODO: Log details in eventlog.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
		{
			Debug.WriteLine("StackTrace:\r\n" + e.Exception.StackTrace);

			EventLogger.Write(e.Exception);

			
			var msg = "";

			try 
			{
				#if (!DEBUG)
				RepportException(e.Exception);
				#endif // if (!DEBUG)
				msg = e.Exception.Message + "\n\nAn exception has been repported to TFS.";
			}
			catch (Exception repEx)
			{
				msg += e.Exception.Message + "\n\nOsiris TFS Monitor was not able to repport the exception to TFS:\n\n" + repEx.Message;
			}

			MessageBox.Show(
				string.Format(
				"An error has been detected in the application that has caused the application to shutdown:\n\n{0}", msg),
				"Osiris TFS Monitor",
				MessageBoxButton.OK,
				MessageBoxImage.Error);

			#if (!DEBUG)
			if (Application.Current != null)
			{
				Application.Current.Shutdown();
			}
			e.Handled = true;
			#endif // if (!DEBUG)
		}

		/// <summary>
		/// Update tfs service connection settings.
		/// </summary>
		private void UpdateTfsServiceConnection()
		{
			var tfsSettings = new TfsMonitorSettings();
			tfsSettings.Load(Settings.Default);
		}

		private void RepportException(Exception ex)
		{
			return;


			var client = new OsirisExceptionClient.Service(); //.TFSExceptionReporter();
			client.Credentials = new NetworkCredential("OsirisErrorReporter", "1qaz2WSX");

			string teamProject = "Osiris - Internal";
			string reporter = "n/a";
			string userName = "n/a";

			var currentNTUser = System.Security.Principal.WindowsIdentity.GetCurrent();
			if (currentNTUser != null)
			{
				reporter = currentNTUser.Name;
			}

			string exceptionClass = ex.TargetSite.DeclaringType.FullName;
			string exceptionMethod = ex.TargetSite.Name;
			string exceptionSource = ex.Source;
			string version = Assembly.GetExecutingAssembly().GetName().Version.ToString();
			string stackTrace = ex.StackTrace;

			client.AddNewException(teamProject,
								   reporter,
								   "",
								   version,
								   ex.Message,
								   ex.GetType().ToString(),
								   "Osiris TFS Monitor Exception: " + ex.Message,
								   stackTrace,
								   exceptionClass,
								   exceptionMethod,
								   exceptionSource,
								   "",
								   userName
				);
		}

		#endregion // Methods

	}
}
