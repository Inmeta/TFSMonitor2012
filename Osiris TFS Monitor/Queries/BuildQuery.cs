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

	public class BuildQuery : QueryBase<BuildFilter, BuildCollection>
	{
		Action<BuildQuery> _completed;

		#region Properties

		public override string Description { get { return "Loading builds..."; } }

		#endregion // Properties

		public BuildQuery(int modelId, int? pollingInterval, BuildFilter filter) : base(modelId, filter, pollingInterval)
        {
        }

		public BuildQuery(int modelId, int? pollingInterval, BuildFilter filter, Action<BuildQuery> completed)
			: base(modelId, filter, pollingInterval)
		{
			_completed = completed;
			ViewModelEvents.Instance.BuildQueryCompleted.Subscribe(completed, ThreadOption.UIThread, false, q => q.SourceId == this.SourceId);
		}

		public void Query(bool flush)
		{
			var result = Query<BuildQuery>(flush, q => q.SourceId == this.SourceId, false);
			if (result != null && _completed != null)
			{
				_completed(this);
			}
		}

        protected override void Execute()
        {
            this.Results = new BuildCollection();

			using (var tfs = TfsService.Instance.Connect())
			{
				this.Results.Load(tfs, this.Arguments);
			}
        }

		public override void PublishCompleted()
		{
			ViewModelEvents.Instance.BuildQueryCompleted.Publish(this);
		}
	}
}
