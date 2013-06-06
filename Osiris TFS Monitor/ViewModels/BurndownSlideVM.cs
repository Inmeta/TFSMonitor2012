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

	public interface IBurndownView
	{
		void UpdateChart(SprintBurndown data);
	}

	public abstract class SlideVM : ViewModelBase
	{
	}


	public class BurndownSlideVM : SlideVM, IDisposable
	{
		#region Fields

		IBurndownView _view;

		#endregion // Fields

		#region Properties

		public BurndownSlide Slide { get; private set; }

		#endregion // Properties

		#region Constructors

		public BurndownSlideVM(BurndownSlide slide)
		{
			this.Slide = slide;
		}

		public BurndownSlideVM(BurndownSlide slide, IBurndownView view)
		{
			this.Slide = slide;
			_view = view;
			
			TfsService.Instance.TimedQuery(new TfsServiceTimedQuery(new SprintBurndownQuery(Application.Current.Dispatcher, slide.TeamProject,
				slide.IterationId.Value, this.QueryCompleted, this.QueryFailed), new TimeSpan(0, 0, slide.UpdateInterval)));
		}

		#endregion // Constructors

		#region Methods

		public void QueryCompleted(SprintBurndown data)
		{
			Debug.WriteLine("Query completed!");

			if (_disposed)
			{
				return;
			}

			_view.UpdateChart(data);
		}

		public void QueryFailed(Exception ex)
		{
			Debug.WriteLine("Query failed!");

			if (_disposed)
			{
				return;
			}
		}

		#endregion // Methods

		#region IDisposable Members

		bool _disposed = false;

		public void Dispose()
		{
			_disposed = true;
		}

		#endregion // IDisposable Members
	}
}
