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

	#endregion // Using
	/*
	public class TfsServiceTimedQuery : IDisposable
	{
		#region Fields
		
		double _updateInterval; // Milliseconds
		DateTime _timerStart;
		bool _startNow;

		#endregion // Fields

		#region Properties

		public TfsServiceQuery Query { get; private set; }

		public bool IsDisposed { get { return this.Query.IsDisposed; } }

		#endregion // Properties

		#region Constructors

		public TfsServiceTimedQuery(TfsServiceQuery qry, TimeSpan updateInterval, bool startNow)
		{
			Debug.Assert(qry != null);
			
			this.Query = qry;
			_updateInterval = updateInterval.TotalMilliseconds;
			_startNow = startNow;
			_timerStart = DateTime.Now;
		}

		#endregion // Constructors

		#region Methods

		public bool TickDown()
		{
			if (_startNow || DateTime.Now.Subtract(_timerStart).TotalMilliseconds >= _updateInterval)
			{
				_startNow = false;
				ResetTimer();
				return true;
			}

			return false;
		}

		public void ResetTimer()
		{
			_timerStart = DateTime.Now;
		}

		#endregion // Methods

		#region IDisposable Members

		public void Dispose()
		{
			this.Query.Dispose();
			//_disposed = true;
			//TfsService.Instance.RemoveQuery(this);
		}

		#endregion // IDisposable Members
	}*/
}
