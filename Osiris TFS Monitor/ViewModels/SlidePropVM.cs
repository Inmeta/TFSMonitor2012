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
	using System.Windows.Media;

	#endregion // Using

	public abstract class SlidePropVM : ViewModelBase, IDisposable
	{
		#region Fields

		ISlidePropView _view;

		#endregion // Fields

		#region Properties

		public ICommand OkCommand { get; set; }
		public ICommand CancelCommand { get; set; }

		protected abstract Slide Slide { get; }

		#endregion // Properties

		#region Constructors

		public SlidePropVM(ISlidePropView view)
		{
			_view = view;
			this.OkCommand = new DelegateCommand(Ok_Executed, Ok_CanExecute);
			this.CancelCommand = new DelegateCommand(Cancel_Executed);
		}

		#endregion // Constructors

		#region Methods

		protected abstract bool CanExecuteOk();

		protected abstract void Save();


		private void Ok_Executed()
		{
			this.Save();

			ViewModelEvents.Instance.NewSlide.Publish(this.Slide);

			//ApplicationVM.Instance.AddSlide(this.Slide);

			// Now signal to view to close
			_view.Close();
		}

		private bool Ok_CanExecute()
		{
			return CanExecuteOk();
		}

		private void Cancel_Executed()
		{
			// Now signal to view to close
			_view.Close();
		}

		#endregion // Methods

		#region IDisposable Members

		public void Dispose()
		{
			throw new NotImplementedException();
		}

		#endregion // IDisposable Members
	}
}
