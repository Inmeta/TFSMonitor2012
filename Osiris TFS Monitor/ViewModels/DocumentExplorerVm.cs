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
	using System.Collections.ObjectModel;

	#endregion // Using

	public interface IDocumentExplorerView
	{
		void SlideInserted(SlideThumbnailVm vm);
	}

	public class DocumentExplorerVm : ViewModelBase
	{
		#region Fields

		//TfsMonitorDocument _doc;
		IDocumentExplorerView _view;
		SlideThumbnailVm _selected;

		#endregion // Fields

		#region Properties

		public ObservableCollection<SlideThumbnailVm> Slides { get; private set; }

		public SlideThumbnailVm Selected 
		{
			get { return _selected; }
			set
			{
				_selected = value;
				ViewModelEvents.Instance.SlideSelected.Publish((value == null) ? null : value.Slide);
				RaisePropertyChanged(() => Selected);
				if (_selected != null)
				{
					_selected.Select();
				}
			}
		}

		public ICommand RenameCommand { get; set; }
		public ICommand DeleteCommand { get; set; }

		#endregion // Properties

		#region Constructors

		public DocumentExplorerVm(IDocumentExplorerView view)
		{
			//_doc = doc;
			_view = view;
			this.Slides = new ObservableCollection<SlideThumbnailVm>();

			this.RenameCommand = new DelegateCommand(OnRename);
			this.DeleteCommand = new DelegateCommand(OnDelete);

			ViewModelEvents.Instance.NewSlideAdded.Subscribe(OnNewSlideAdded);
			ViewModelEvents.Instance.NewDocument.Subscribe(OnNewDocument);
			ViewModelEvents.Instance.SlideDeleted.Subscribe(OnSlideDeleted);
		}

		#endregion // Constructors

		#region Methods

		private void OnNewSlideAdded(Slide slide)
		{
			var vm = new SlideThumbnailVm(this, slide);
			this.Slides.Add(vm);
			_view.SlideInserted(vm);
		}

		private void OnNewDocument(TfsMonitorDocument doc)
		{
			//_doc = doc;

			this.Slides.Clear();
			foreach (var slide in doc.Slides)
			{
				OnNewSlideAdded(slide);
			}
		}

		private void OnSlideDeleted(Slide slide)
		{
			foreach (var s in this.Slides)
			{
				if (s.Slide == slide)
				{
					this.Slides.Remove(s);
					return;
				}
			}
		}

		private void OnDelete()
		{
			if (this.Selected == null)
			{
				return;
			}
			ViewModelEvents.Instance.DeleteSlide.Publish(this.Selected.Slide);
		}

		private void OnRename()
		{
			if (this.Selected == null)
			{
				return;
			}
			this.Selected.View.Rename();
		}

		#endregion // Methods
	}
}
