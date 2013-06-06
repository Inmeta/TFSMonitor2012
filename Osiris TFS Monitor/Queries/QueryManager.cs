using System.Diagnostics;

namespace Osiris.Tfs.Monitor
{
	#region Using

	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading;
	using Osiris.TFS.Monitor.Common;

	#endregion // Using

	public class QueryManager //: IDisposable
	{
		#region Fields

		static QueryManager _instance;
		Queue<IQuery> _queries = new Queue<IQuery>();
		bool _disposed = false;

		#endregion // Fields

		#region Properties

		public Queue<IQuery> Queries { get { return _queries; } }

		public static QueryManager Instance
		{
			get
			{
				if (_instance == null)
				{
					_instance = new QueryManager();
				}
				return _instance;
			}
		}

		#endregion // Properties

		#region Constructors

		private QueryManager()
		{
			StartBackgroundThread();
		}

		#endregion // Constructors

		#region Methods

		/// <summary>
		/// Initiaize thread
		/// </summary>
		private void StartBackgroundThread()
		{
			// Start thread
			var queryThread = new Thread(new ThreadStart(BackgroundThread));
			queryThread.SetApartmentState(ApartmentState.STA);
			queryThread.IsBackground = true;
			queryThread.Start();
		}

		/// <summary>
		/// Called when worker-thread starts.
		/// </summary>
		private void BackgroundThread()
		{
			//int tick = 0;
			while (!_disposed)
			{
				// Fetch first query in queue
				var qry = GetQueuedQuery();
				if (qry != null)
				{
					ViewModelEvents.Instance.QueryStarted.Publish(qry);

					qry.ExecuteBase();

					ViewModelEvents.Instance.QueryStarted.Publish(null);
				}

				Thread.Sleep(100);
				/*if (++tick == 10)
				{
					Debug.WriteLine("No. queries: " + _queries.Count.ToString());
					tick = 0;
				}*/
			}
		}

		private IQuery GetQueuedQuery()
		{
			lock (_queries)
			{
				for (int i = 0; i < _queries.Count; i++)
				{
					var qry = _queries.Dequeue();

					// Disposed?
					if (qry.IsRemoved)
					{
						continue;
					}

					// Run once query
					if (!qry.IsPollingType)
					{
						return qry;
					}

					// Polling query
					_queries.Enqueue(qry);
					if (!qry.TickDownPolling())
					{
						continue;
					}
					return qry;
				}
			}

			// No queries to run
			return null;
		}

		public void Query(IQuery qry)
		{
			lock (_queries)
			{
				_queries.Enqueue(qry);
			}
		}

		public void UpdatePollingInterval<T>(Predicate<T> filter, int interval) where T : IQuery
		{
			lock (QueryManager.Instance.Queries)
			{
				_queries.OfType<T>().Where(q => filter(q)).ToList().ForEach(q => q.SetPollingInterval(interval));
			}
		}

		#endregion // Methods
	}
}
