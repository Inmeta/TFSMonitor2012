namespace Osiris.Tfs.Monitor
{
	#region Using

	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Diagnostics;
	using System.Windows.Input;
	using Osiris.Tfs.Monitor.Models;
	using Osiris.Tfs.Report;
	using Microsoft.TeamFoundation.VersionControl.Client;
	using System.Net;
	using Microsoft.TeamFoundation.Client;
	using Osiris.Tfs.Monitor.Properties;
	using System.IO;
	using System.Collections.ObjectModel;
	using Microsoft.Practices.Composite.Events;
	using System.Windows;

	#endregion // Using

	public class ApplicationVm : ViewModelBase
	{
		#region Fields

		TfsMonitorDocument _doc;
		string _initialDoc;
		IApplicationView _view;
		Slide _selectedSlide = null;

		#endregion // Fields

		#region Properties

		public static ApplicationVm Instance { get; private set; }

		public TfsMonitorDocument Document { get { return _doc; } }

		public MonitorCollection SlideshowMonitors
		{
			get
			{
				var tfsSettings = new TfsMonitorSettings();
				tfsSettings.Load(Settings.Default);
				return tfsSettings.SlideshowMonitors;
			}
		}

		public bool TurnOffScreenSaver
		{
			get
			{
				var tfsSettings = new TfsMonitorSettings();
				tfsSettings.Load(Settings.Default);
				return tfsSettings.SlideshowTurnOffScreenSaver;
			}
		}

		public IApplicationView View { get { return _view; } set { _view = value; Init(); } }

		#endregion // Properties

		#region Constructors

		public ApplicationVm() : this(null)
		{
		}

		public ApplicationVm(string document)
		{
			_initialDoc = document;
			Instance = this;
			ViewModelEvents.Instance.NewSlide.Subscribe(OnNewSlide);
			ViewModelEvents.Instance.DeleteSlide.Subscribe(OnDeleteSlide);
		}

		#endregion // Constructors

		private void Init()
		{
			AppCommands.BindCommand(AppCommands.ApplicationClose, ApplicationClose_Executed);
			AppCommands.BindCommand(AppCommands.FileSave, FileSave_Executed);
			AppCommands.BindCommand(AppCommands.FileSaveAs, FileSaveAs_Executed);
			AppCommands.BindCommand(AppCommands.FileOpen, FileOpen_Executed);
			AppCommands.BindCommand(AppCommands.ApplicationOptions, ApplicationOptions_Executed);
			AppCommands.BindCommand(AppCommands.SlideShowFromBeginning, SlideshowFromBeginning_Executed, SlideshowFromBeginning_CanExecute);
			AppCommands.BindCommand(AppCommands.SlideNew, SlideNew_Executed);
			AppCommands.BindCommand(AppCommands.ApplicationNew, ApplicationNew_Executed);
			AppCommands.BindCommand(AppCommands.SlideRefresh, SlideRefresh_Executed, SlideRefresh_CanExecute);

			ViewModelEvents.Instance.FileOpen.Subscribe(Open);
			ViewModelEvents.Instance.SlideSelected.Subscribe(OnSlideSelected);

			if (!string.IsNullOrEmpty(_initialDoc))
			{
				Open(_initialDoc);
			}
		}

		private void OnSlideSelected(Slide slide)
		{
			_selectedSlide = slide;
		}

		private void OnNewSlide(Slide slide)
		{
			_doc.Slides.Add(slide);
			ViewModelEvents.Instance.NewSlideAdded.Publish(slide);
		}

		#region Methods

		void FileOpen_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			string fileName = this.View.PromptOpenDocumentFile();

			if (string.IsNullOrEmpty(fileName))
			{
				return;
			}

			if (_doc == null)
			{
				_doc = new TfsMonitorDocument();
			}
			Open(fileName);
		}

		private void Open(string fileName)
		{
			try
			{
				_doc = TfsMonitorDocument.Open(Settings.Default, fileName);
			}
			catch (Exception e)
			{
				MessageBox.Show("Unable to open document '" + fileName + "'\r\n\r\nError:\r\n" + e.Message, "Osiris TFS Monitor",
				                MessageBoxButton.OK, MessageBoxImage.Exclamation);				
			}
			ViewModelEvents.Instance.RecentFilesChanged.Publish(fileName);
			ViewModelEvents.Instance.SlideSelected.Publish(null);
			ViewModelEvents.Instance.NewDocument.Publish(_doc);

		}
		void FileSave_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			if (_doc == null || !_doc.HasFileName)
			{
				FileSaveAs_Executed(sender, e);
				return;
			}

			_doc.Save(Settings.Default);
		}

		void FileSaveAs_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			var fileName = this.View.PromptSaveDocumentFile((_doc == null || !_doc.HasFileName) ? "Untitled" : _doc.FileName);
			if (string.IsNullOrEmpty(fileName))
			{
				return;
			}

			if (_doc == null)
			{
				_doc = new TfsMonitorDocument();
			}
			_doc.SaveAs(Settings.Default, fileName);
			ViewModelEvents.Instance.RecentFilesChanged.Publish(fileName);
		}

		void ApplicationOptions_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			this.View.DisplayApplicationOptions();
		}

		void ApplicationClose_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			this.View.Close();
		}

		void ApplicationNew_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			_doc = new TfsMonitorDocument();
			ViewModelEvents.Instance.SlideSelected.Publish(null);
			ViewModelEvents.Instance.NewDocument.Publish(_doc);
		}

		void SlideRefresh_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			ViewModelEvents.Instance.RefreshSlide.Publish(_selectedSlide);
		}

		void SlideRefresh_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = (_selectedSlide != null);
		}

		void SlideshowFromBeginning_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			this.View.SlideShowFromBeginning();
		}

		void SlideshowFromBeginning_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = (_doc == null) ? false : (_doc.Slides.Count > 0);
		}

		void SlideNew_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			if (_doc == null)
			{
				_doc = new TfsMonitorDocument();
			}

			SlideTemplateVm tpl;
			if (e.Parameter != null)
			{
				tpl = e.Parameter as SlideTemplateVm;
			}
			else
			{
				tpl = this.View.SelectNewSlide();
			}

			if (tpl != null)
			{
				var slide = tpl.CreateSlide();
				_doc.Slides.Add(slide);
				ViewModelEvents.Instance.NewSlideAdded.Publish(slide);
			}
		}

		void OnDeleteSlide(Slide slide)
		{
			if (_doc == null)
			{
				Debug.Assert(false);
				return;
			}

			ViewModelEvents.Instance.SlideSelected.Publish(slide);
			ViewModelEvents.Instance.SlideDeleted.Publish(slide);
			_doc.Slides.Remove(slide);
		}

		public void FullscreenClosed()
		{
			// Rebind view to viewmodel. Dirty...
			foreach (var slide in _doc.Slides)
			{
				ViewModelEvents.Instance.SlideUpdated.Publish(slide);
			}
		}

		#endregion // Methods
	}
}
