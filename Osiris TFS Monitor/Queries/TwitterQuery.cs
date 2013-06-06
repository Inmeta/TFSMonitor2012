namespace Osiris.Tfs.Monitor
{
	#region Using

	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using Microsoft.TeamFoundation.Client;
	using System.Windows.Threading;
	using Osiris.Tfs.Monitor.Models;
	using Microsoft.Practices.Composite.Presentation.Events;
	using System.Diagnostics;

	#endregion // Using

	public class TwitterQuery : QueryBase<Twitter, IEnumerable<Tweet>>
	{
		Action<TwitterQuery> _completed;

		#region Properties

		public override string Description { get { return "Loading tweets..."; } }

		#endregion // Properties

		public TwitterQuery(int modelId, int? pollingInterval, Twitter twitter) : base(modelId, twitter, pollingInterval)
        {
        }

		public TwitterQuery(int modelId, int? pollingInterval, Twitter twitter, Action<TwitterQuery> completed)
			: base(modelId, twitter, pollingInterval)
		{
			_completed = completed;
			ViewModelEvents.Instance.TwitterQueryCompleted.Subscribe(completed, ThreadOption.UIThread, false, q => q.SourceId == this.SourceId);
		}

		public void Query(bool flush)
		{
			var result = Query<TwitterQuery>(flush, q => q.SourceId == this.SourceId, false);
			if (result != null && _completed != null)
			{
				_completed(this);
			}
		}

        protected override void Execute()
        {
			this.Arguments.LoadTweets();
			this.Results = this.Arguments.Tweets;
        }

		public override void PublishCompleted()
		{
			ViewModelEvents.Instance.TwitterQueryCompleted.Publish(this);
		}
	}
}
