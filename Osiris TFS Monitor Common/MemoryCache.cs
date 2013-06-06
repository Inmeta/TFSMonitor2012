namespace Osiris.TFS.Monitor.Common
{
	#region Using

	using System.Collections.Generic;
	using System.Threading;
	using System.Diagnostics;
	
	#endregion // Using

	public static class AutoIdGenerator
	{
		static int _generatedId = 0;

		public static int GenerateId()
		{
			return _generatedId++;
		}
	}

	public class CacheItem<T> where T : class
	{
		public T Value { get; private set; }
		public int TimeToLive { get; private set; }
		
		public CacheItem(T value)
		{
			this.Value = value;
			this.TimeToLive = -1;
		}

		public CacheItem(T value, int timeToLive)
		{
			this.Value = value;
			this.TimeToLive = timeToLive;
		}

		public bool Tick(int tick)
		{
			if (this.TimeToLive < 0)
			{
				return false;
			}

			this.TimeToLive -= tick;
			return (this.TimeToLive <= 0);
		}
	}

	/// <summary>
	/// Generall cache
	/// </summary>
	public class Cache<Key, Data> : Dictionary<Key, CacheItem<Data>> where Data : class
	{
		Timer _refreshTimer;
		int _refreshPeriod; // Seconds
		
		public Cache(int refreshPeriod = 300 /* 5 minutes */ )
		{
			_refreshPeriod = refreshPeriod;
			if (_refreshTimer == null)
			{
				_refreshTimer = new Timer(new TimerCallback(this.OnRefresh), null, 0, _refreshPeriod * 1000);
			}
		}

		private void OnRefresh(object stateInfo)
		{
			Refresh();
		}

		public Data Get(Key k)
		{
			Refresh();

			if (this.ContainsKey(k))
			{
				return this[k].Value;
			}
			return null;
		}

		public void Set(Key k, Data d)
		{
			Refresh();

			this[k] = new CacheItem<Data>(d);
		}

		public void Set(Key k, Data d, int timeToLive)
		{
			Refresh();

			this[k] = new CacheItem<Data>(d, timeToLive);
		}

		public void Flush(Key k)
		{
			Refresh();
	
			if (this.ContainsKey(k))
			{
				this.Remove(k);
			}
		}

		private void Refresh()
		{
			lock (this)
			{
				var flushList = new List<Key>();
				foreach (var item in this)
				{
					if (item.Value.Tick(_refreshPeriod))
					{
						flushList.Add(item.Key);
					}
				}
				foreach (var key in flushList)
				{
					this.Remove(key);
				}
			}
		}
	}
}
