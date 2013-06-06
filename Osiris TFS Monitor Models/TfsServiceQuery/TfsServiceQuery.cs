namespace Osiris.Tfs.Monitor.Models
{
	#region Using

	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using Microsoft.TeamFoundation.Client;
	using System.Windows.Threading;
	using System.Diagnostics;
	using Osiris.TFS.Monitor.Common;

	#endregion // Using
	/*

	public abstract class TfsServiceQuery : IDisposable
	{
		#region Fields

		Dispatcher _disp;
		bool _disposed = false;
		bool _inProgress = false;
		double? _pollingInterval; // Milliseconds
		DateTime _timerStart;

		#endregion // Fields

		#region Properties

		public bool IsDisposed { get { return _disposed; } }

		public bool IsInProgress { get { return _inProgress; } }

		public abstract string Description { get; }
		
		public int TimeToLive { get; private set; }

		public bool Updated { get; set; }

		public bool AutoDispose { get; set; }

		public bool Executed { get; set; }

		#endregion // Properties

		public void EnablePolling(TimeSpan pollingInterval)
		{
			_pollingInterval = pollingInterval.TotalMilliseconds;
			_timerStart = DateTime.Now;
		}

		public void DisablePolling()
		{
			_pollingInterval = null;
		}

		public bool HasPolling { get { return _pollingInterval.HasValue; } }

		public bool TickDown()
		{
			if (DateTime.Now.Subtract(_timerStart).TotalMilliseconds >= _pollingInterval)
			{
				_timerStart = DateTime.Now;
				return true;
			}

			return false;
		}

		#region Constructor

		/// <summary>
		/// Base constructor. We need client's dispatcher when we
		/// need to invoke the clients thread.
		/// </summary>
		/// <param name="disp">Client's dispatcher</param>
		public TfsServiceQuery(Dispatcher disp) : this(disp, 0, true)	{ }

		public TfsServiceQuery(Dispatcher disp, int timeToLive, bool autoDispose)
		{
			_disp = disp;
			this.TimeToLive = timeToLive;
			this.AutoDispose = autoDispose;
			this.Executed = false;
		}

		#endregion // Constructor

		#region Methods

		public abstract void Execute(TeamFoundationServer tfs);
		public abstract void Failure(Exception ex);
		public abstract void Completed();
		public virtual bool Compare(TfsServiceQuery qry) { return false; }
		public virtual void ExecuteFromCache(TfsServiceQuery qry) { Debug.Assert(false); }
		public void ResetPolling()
		{
			_timerStart = DateTime.Now;
		}

		public void InProgress(bool inProgress)
		{
			_inProgress = inProgress;
		}

		public void InvokeCompleted()
		{
			if (_disposed)
			{
				return;
			}

			// Invoke completed handler on client's dispatcher
			_disp.BeginInvoke(DispatcherPriority.DataBind, new Action(delegate
			{
				Completed();
			}));
		}

		public void InvokeFailure(Exception ex)
		{
			if (_disposed)
			{
				return;
			}

			// Invoke failure handler on client's dispatcher
			_disp.BeginInvoke(DispatcherPriority.DataBind, new Action(delegate
			{
				Failure(ex);
			}));
		}

		public bool ExecuteFromCache()
		{
			var data = GetFromCache();
			if (data == null)
			{
				return false;
			}
			if (Compare(data))
			{
				_disp.BeginInvoke(DispatcherPriority.DataBind, new Action(delegate
				{
					ExecuteFromCache(data);
				}));
				return true;
			}

			return false;	
		}

		protected TfsServiceQuery GetFromCache()
		{
			Type t = this.GetType();
			var data = Cache<string, TfsServiceQuery>.Instance.Get(t.FullName);
			if (data == null)
			{
				return null;
			}
			return Compare(data) ? data : null;
		}

		#endregion // Methods

		#region IDisposable Members

		public void Dispose()
		{
			_disposed = true;
		}

		#endregion // IDisposable Members
	}
	  */
}
