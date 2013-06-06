namespace Osiris.Tfs.Monitor
{
	#region Using

	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Collections.ObjectModel;
	using System.Windows.Input;
	using Osiris.Tfs.Monitor.Properties;
	using Osiris.Tfs.Monitor.Models;

	#endregion // Using

	/// <summary>
	/// Dialog for selecting new slide
	/// </summary>
	public class SelectSlideTemplateVm : ViewModelBase
	{
		#region Fields

		SlideTemplateVm _selected;

		#endregion // Fields

		#region Events

		public delegate void CloseEvent();

		#endregion // Events
	
		#region Properties

		public ObservableCollection<SlideTemplateVm> Templates { get; private set; }

		public SlideTemplateVm Selected
		{ 
			get { return _selected; }
			set
			{
				_selected = value;
				RaisePropertyChanged(() => Selected);
			}
		}

		public SlideTemplateVm SelectedResult { get; private set; }


		public ICommand OkCommand { get; set; }
		public ICommand CancelCommand { get; set; }
		public CloseEvent CloseHandler { get; set; }

		#endregion // Properties

		#region Constructors

		public SelectSlideTemplateVm()
		{
			var templates = new ObservableCollection<SlideTemplateVm>()
			{
				new BlankSlideTemplateVm(),
				new BurndownChartTemplateVm(),
				new BuildMonitorTemplateVm(),
				new WebPageTemplateVm()
			};
			this.Templates = templates;
			this.Selected = templates.First();

			this.OkCommand = new DelegateCommand(Ok_Executed);
			this.CancelCommand = new DelegateCommand(Cancel_Executed);
		}

		#endregion // Constructors

		#region Methods

		private void Ok_Executed()
		{
			if (this.Selected == null)
			{
				return;
			}

			this.SelectedResult = this.Selected;


			// Now signal to view to close
			Close();
		}

		private void Cancel_Executed()
		{
			Close();
		}

		private void Close()
		{
			if (CloseHandler != null)
			{
				CloseHandler();
			}
		}

		#endregion // Methods
	}


}
