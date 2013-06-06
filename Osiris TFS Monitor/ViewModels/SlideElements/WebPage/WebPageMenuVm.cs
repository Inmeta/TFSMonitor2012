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

	#endregion // Using

	public class WebPageMenuVm : ViewModelBase
	{
		#region Fields

		WebPage _model = null;

		#endregion // Fields

		#region Properties

		public string Url { get { return _model.Url; } set { _model.Url = value; } }

		public int? RefreshInterval
		{
			get { return _model.RefreshInterval; }
			set
			{
				if (value.HasValue)
				{
					// Error or 0?
					if (value.Value > 0)
					{
						_model.RefreshInterval = value.Value;
					}
				}
			}
		}

		#endregion // Properties

		#region Constructors

		public WebPageMenuVm(WebPage model)
		{
			_model = model;
		}

		#endregion // Constructors
	}
}
