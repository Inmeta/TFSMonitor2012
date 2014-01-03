namespace Osiris.Tfs.Monitor
{
	#region Using

	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Collections.ObjectModel;
	using Osiris.Tfs.Monitor.Models;
	using Osiris.Tfs.Monitor.Properties;
	using System.Diagnostics;
	using System.Windows;
	using Microsoft.Practices.Composite.Presentation.Events;
//	using System.Windows.Forms;

	#endregion // Using

	/// <summary>
	/// Options for Application
	/// </summary>
	public class StatusBarVm : ViewModelBase
	{
		string _statusMessage = "Ready";

		#region Properties

		public string StatusMessage
		{
			get { return _statusMessage; }
			set
			{
				_statusMessage = value;
				RaisePropertyChanged(() => StatusMessage);
			}
		}

		#endregion // Properties

		#region Constructors

		public StatusBarVm()
		{
			//TfsService.Instance.SetNotifyHandler(Application.Current.Dispatcher, this.TfsQueryStarted);
			ViewModelEvents.Instance.QueryStarted.Subscribe(OnQueryStarted, ThreadOption.UIThread);
		}

		#endregion // Constructors

		#region Methods

		/*private void TfsQueryStarted(TfsServiceQuery qry)
		{
			this.StatusMessage = (qry == null) ? "Ready" : qry.Description;
		}*/

		private void OnQueryStarted(IQuery qry)
		{
			this.StatusMessage = (qry == null) ? "Ready" : qry.Description;
		}

		#endregion // Methods
	}


}
