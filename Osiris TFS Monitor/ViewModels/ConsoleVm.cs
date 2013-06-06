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

	#endregion // Using

	public class ConsoleVm : ViewModelBase
	{
		#region Fields

		Slide _selectedSlide;

		#endregion // Fields

		#region Properties

		public TfsMonitorSettings Model { get; private set; }

		public Slide SelectedSlide
		{
			get { return _selectedSlide; }
			set
			{
				_selectedSlide = value;
				RaisePropertyChanged(() => SelectedSlide);
				RaisePropertyChanged(() => IsSlideSelected);
			}
		}

		public bool IsSlideSelected
		{ 
			get	{ return _selectedSlide != null; }
		}

		#endregion // Properties

		#region Constructors

		public ConsoleVm()
		{
			this.Model = new TfsMonitorSettings();
			this.Model.Load(Settings.Default);
		}

		#endregion // Constructors

		#region Methods

		public void Save()
		{
			this.Model.Save(Settings.Default);
		}

		#endregion // Methods
	}
}
