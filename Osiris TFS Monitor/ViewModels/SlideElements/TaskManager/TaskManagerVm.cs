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
	using Osiris.TFS.Monitor.Common;
	using Microsoft.TeamFoundation.Build.Client;
	using Microsoft.Practices.Composite.Presentation.Events;

	#endregion // Using

	public interface ITaskManagerView
	{
	}

	public class TaskManagerVm : SlideElementVm
	{
		#region Fields

		bool _isLoaded = false;
		TaskManager _model;
		ITaskManagerView _view;

		#endregion // Fields

		#region Properties

		public ITaskManagerView View 
		{
			get 
			{ 
                return _view; 
            }
			set
			{
				_view = value;
				if (_view != null)
				{
					Load();
				}
				else
				{
					//if (_autoUnload)
					{
						Unload();
					}
				}
			}
		}

		#endregion // Properties

		#region Constructors

		public TaskManagerVm(TaskManager model)
		{
			_model = model;
		}

		#endregion // Constructors

		#region Methods

		private void Load()
		{
			_isLoaded = true;
		}

		public override void Unload()
		{
			_isLoaded = false;
		}

		#endregion // Methods
	}
}
