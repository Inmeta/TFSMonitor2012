namespace Osiris.Tfs.Monitor
{
	#region Using

	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading;
using Osiris.TFS.Monitor.Common;
	using System.Diagnostics;
	using System.Collections;

	#endregion // Using

	public interface IQuery
	{
		void ExecuteBase();
		bool IsRemoved { get; }
		bool IsPollingType { get; }
		bool TickDownPolling();
		string Description { get; }
		void Remove();
		void ResetPolling();
		int SourceId { get; }
		void SetPollingInterval(int newInterval);
	}

	public abstract class QueryBase<Arg, Result> : IQuery where Arg : IEquatable<Arg> where Result : class
	{
		#region Fields

		static int _generatedId = 0;
		bool _removed = false;
		private double? _pollingInterval; // When value set, polling is enabled with polling every pecified ms
		DateTime _pollingStart;
		static private List<QueryBase<Arg, Result>> _cache = new List<QueryBase<Arg, Result>>();
	

		#endregion // Fields

		#region Properties

		public Arg Arguments { get; set; }
		public Result Results { get; protected set; }
		public Exception Exception { get; private set; }
		public int Id { get; private set; }
		public abstract string Description { get; }
		public bool IsPollingType { get { return _pollingInterval.HasValue; } }
		public bool IsRemoved { get { return _removed; } }
		public int SourceId { get; private set; }

		#endregion // Properties

		#region Methods

		protected QueryBase(int sourceId, Arg arg, int? pollingInterval)
		{
			this.Id = _generatedId++;
			this.SourceId = sourceId;
			this.Arguments = arg;
			_pollingStart = DateTime.Now;
			_pollingInterval = pollingInterval * 1000;
		}

		public void ExecuteBase()
		{
			try
			{
				ResetPolling();
				Execute();
				EnqueCache();
				this.Exception = null;
				PublishCompleted();
			}
			catch (Exception e)
			{
				EventLogger.Write(e);
				this.Exception = e;
				PublishCompleted();
			}
		}

		private void EnqueCache()
		{
			lock (_cache)
			{
				var existing = _cache.SingleOrDefault(q => q.Arguments.Equals(this.Arguments));
				if (existing != null)
				{
					_cache.Remove(existing);
				}
				_cache.Add(this);
			}
		}

		protected abstract void Execute();

		public void Remove()
		{
			_removed = true;
		}

		public bool TickDownPolling()
		{
			if (DateTime.Now.Subtract(_pollingStart).TotalMilliseconds >= _pollingInterval)
			{
				_pollingStart = DateTime.Now;
				return true;
			}

			return false;
		}

		public void ResetPolling()
		{
			_pollingStart = DateTime.Now;
		}

		public abstract void PublishCompleted();

		public void SetPollingInterval(int newInterval)
		{
			double calc = newInterval * 1000;
			_pollingInterval = (calc > 0) ? calc : double.MaxValue;
		}

		public void UpdateArguments(Arg arg)
		{
			this.Arguments = arg;
		}

		private QueryBase<Arg, Result> FromCache()
		{
			QueryBase<Arg, Result> cached;
			lock (_cache)
			{
				cached = _cache.SingleOrDefault(q => q.Arguments.Equals(this.Arguments));
			}
			return cached;
		}

		/// <summary>
		/// Run new query, dispose current queries of this type.
		/// </summary>
		protected Result Query<T>(bool flush, Predicate<T> filter, bool publish = true) where T : IQuery
		{
			this.Results = null;

			// Exists in cache?
			if (!flush)
			{
				var cached = FromCache();
				if (cached != null)
				{
					this.Results = cached.Results;
					if (publish)
					{
						PublishCompleted();
					}
					if (!_pollingInterval.HasValue)
					{
						return this.Results;
					}
				}
			}

			// Create new query, stop all active
			lock (QueryManager.Instance.Queries)
			{
				// For non-polling queries: Dispose
				QueryManager.Instance.Queries.OfType<T>().Where(q => filter(q) && (!q.IsPollingType)).ToList().ForEach(q => q.Remove());

				// For polling queries: Update arguments and reset polling interval
				QueryManager.Instance.Queries.OfType<T>().Where(q => filter(q) && q.IsPollingType).ToList().ForEach(q => (q as QueryBase<Arg, Result>).UpdateArguments(this.Arguments));
			}

			QueryManager.Instance.Query(this);

			return this.Results;
		}

		protected void EnqueQuery<T>(Predicate<T> filter) where T : IQuery
		{
			lock (QueryManager.Instance.Queries)
			{
				// For non-polling queries: Dispose
				QueryManager.Instance.Queries.OfType<T>().Where(q => filter(q) && (!q.IsPollingType)).ToList().ForEach(q => q.Remove());

				// For polling queries: Update arguments and reset polling interval
				QueryManager.Instance.Queries.OfType<T>().Where(q => filter(q) && q.IsPollingType).ToList().ForEach(q => (q as QueryBase<Arg, Result>).UpdateArguments(this.Arguments));

				QueryManager.Instance.Queries.Enqueue(this);
			}
		}

		#endregion // Methods
	}
}
