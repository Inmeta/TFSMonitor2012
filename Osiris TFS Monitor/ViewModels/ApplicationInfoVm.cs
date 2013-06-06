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
using System.Reflection;

	#endregion // Using

	public class ApplicationInfoVm : ViewModelBase
	{
		#region Events

		public delegate void CloseEvent();

		#endregion // Events

		#region Properties

		public ICommand OkCommand { get; private set; }

		public CloseEvent CloseHandler { get; set; }

		public string Version { get { return Assembly.GetExecutingAssembly().GetName().Version.ToString(); } }

		#endregion // Properties

		#region Constructors

		public ApplicationInfoVm()
		{
			this.OkCommand = new DelegateCommand(Ok_Executed);
		}

		#endregion // Constructors

		#region Methods

		private void Ok_Executed()
		{
			// Now signal to view to close
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
