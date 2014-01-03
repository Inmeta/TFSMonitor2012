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
	using System.Windows.Controls;
	using System.Windows;
	using System.Windows.Media;

	#endregion // Using

	public interface ISlideThumbnailView
	{
		void SetView(Visual elem);
		bool Focus();
		void Rename();
	}

	public class SlideThumbnailVm : ViewModelBase
	{
		#region Fields

		double _width = 0;
		double _height = 0;
		UserControl _visual;

		#endregion // Fields

		#region Properties

		public ISlideThumbnailView View { get; set; }

		public Slide Slide { get; private set; }

		public string Name
		{
			get { return this.Slide.Name; }
			set 
			{ 
				this.Slide.Name = value;
				RaisePropertyChanged(() => Name);
			}
		}

		public UserControl Visual
		{
			get { return _visual; }
			set
			{
				_visual = value;
				RaisePropertyChanged(() => value);
			}
		}

		public double Width 
		{
			get { return _width; }
			set
			{
				_width = value;
				RaisePropertyChanged(() => Width);
			}
		}

		public double Height
		{
			get { return _height; }
			set
			{
				_height = value;
				RaisePropertyChanged(() => Height);
			}
		}

		//public ICommand RenameCommand { get; set; }

		//public ICommand DeleteCommand { get; set; }

		public DocumentExplorerVm Parent { get; private set; }

		#endregion // Properties

		#region Constructors

		public SlideThumbnailVm(DocumentExplorerVm parent, Slide slide)
		{
			this.Parent = parent;
			this.Slide = slide;
			//this.RenameCommand = new DelegateCommand(OnRename);
			//this.DeleteCommand = new DelegateCommand(OnDelete);
			ViewModelEvents.Instance.SlideContentDisplayed.Subscribe(OnSlideContentDisplayed);
		}

		#endregion // Constructors

		private void OnSlideContentDisplayed(SlideContentDisplayedArgs args)
		{
			if (args.Slide != this.Slide)
			{
				this.View.SetView(null);
				return;
			}
			this.View.SetView(args.Elem);
		}

		public void Select()
		{
			if (this.View != null)
			{
				this.View.Focus();
			}
		}
	
	}

}
