namespace Osiris.TFS.Monitor.Common
{
	#region Using

	using System;
	using System.Threading;
	using Microsoft.Practices.Composite.Presentation.Events;

	#endregion // Using

	public class UiBackgroundLoader<T>
	{
		private readonly Func<T> _load;
		private readonly Action<T> _loadComplete;
		private CompositePresentationEvent<T> _loadCompleteEvent;

		public UiBackgroundLoader(Func<T> load, Action<T> loadComplete)
		{
			_load = load;
			_loadComplete = loadComplete;
		}

		public void Load()
		{
			_loadCompleteEvent = new CompositePresentationEvent<T>();
			_loadCompleteEvent.Subscribe(_loadComplete, ThreadOption.UIThread);
			ThreadPool.QueueUserWorkItem(Load, this);
		}

		private void Load(object obj)
		{
			_loadCompleteEvent.Publish(_load());
		}
	}

	public class UiBackgroundLoader<TArg, TResult>
	{
		private readonly Func<TArg, TResult> _load;
		private readonly Action<TResult> _loadComplete;
		private CompositePresentationEvent<TResult> _loadCompleteEvent;

		public UiBackgroundLoader(Func<TArg, TResult> load, Action<TResult> loadComplete)
		{
			_load = load;
			_loadComplete = loadComplete;
		}

		public void Load(TArg arg)
		{
			_loadCompleteEvent = new CompositePresentationEvent<TResult>();
			_loadCompleteEvent.Subscribe(_loadComplete, ThreadOption.UIThread);
			ThreadPool.QueueUserWorkItem(OnLoad, arg);
		}

		private void OnLoad(object arg)
		{
			_loadCompleteEvent.Publish(_load((TArg)arg));
		}
	}
}
