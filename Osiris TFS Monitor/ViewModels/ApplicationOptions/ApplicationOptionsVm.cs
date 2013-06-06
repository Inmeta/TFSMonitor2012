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
	/// Options for Application
	/// </summary>
	public class ApplicationOptionsVm : ViewModelBase
	{
		#region Events

		public delegate void CloseEvent();

		#endregion // Events

		#region Fields

		OptionVm _selectedOption;

		#endregion // Fields

		#region Properties

		public TfsMonitorSettings Model { get; private set; }

		public ObservableCollection<OptionVm> Options { get; private set; }

		public OptionVm SelectedOption 
		{
			get { return _selectedOption; }
			set
			{
				_selectedOption = value;
				RaisePropertyChanged(() => SelectedOption); 
			}
		}

		public ICommand OkCommand { get; private set; }

		public ICommand CancelCommand { get; set; }

		public CloseEvent CloseHandler { get; set; }

		#endregion // Properties

		#region Constructors

		public ApplicationOptionsVm()
		{
			this.Model = new TfsMonitorSettings();
			this.Model.Load(Settings.Default);

			var options = new ObservableCollection<OptionVm>();
			options.Add(new TfsOptionsVm(this));
			options.Add(new TfsIntegrationOptionsVm(this));
			options.Add(new SlideshowOptionsVm(this));
			this.Options = options;
			this.SelectedOption = options.First();

			this.OkCommand = new DelegateCommand(Ok_Executed);
			this.CancelCommand = new DelegateCommand(Cancel_Executed);
		}

		#endregion // Constructors

		#region Methods

		private void Ok_Executed()
		{
			// Save options
			this.Model.Save(Settings.Default);

			// Now signal to view to close
			Close();

			// Allow others to apply changes
			ViewModelEvents.Instance.SettingsUpdated.Publish(this.Model);
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
