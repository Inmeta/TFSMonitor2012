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
	using Microsoft.TeamFoundation.WorkItemTracking.Client;
	using System.Windows.Threading;
	using System.ComponentModel;
	using System.Collections.ObjectModel;
	using System.Windows;
	using System.Collections.Specialized;

	#endregion // Using

	public interface ITaskManagerMenuView
	{
	}

	public class TaskManagerMenuVm : ViewModelBase
	{
		#region Fields

		ITaskManagerMenuView _view; 
		TaskManager _model = null;

		#endregion // Fields

		#region Properties

		public ITaskManagerMenuView View
		{
			get
			{
				return _view;
			}
			set
			{
				_view = value;
				if (_view == null)
				{
					Unload();
				}
			}
		}

		#endregion // Properties

		#region Constructors

		public TaskManagerMenuVm(TaskManager model)
		{
			_model = model;
		}

		#endregion // Constructors

		#region Methods

		private void Unload()
		{
		}

		#endregion // Methods


	}
}
