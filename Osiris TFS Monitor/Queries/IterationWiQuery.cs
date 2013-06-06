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
	using Microsoft.TeamFoundation.VersionControl.Client;
	using Microsoft.TeamFoundation.WorkItemTracking.Client;
	using Osiris.Tfs.Report;

	#endregion // Using

	public class IterationWiQuery : QueryBase<IterationWiQueryArg, IterationWiCollection>
	{
		#region Properties

		public override string Description { get { return "Loading iteration work items..."; } }

		#endregion // Properties

		public IterationWiQuery(int modelId, int? pollingInterval, IterationWiQueryArg arg)
			: base(modelId, arg, pollingInterval)
        {
        }

		public IterationWiQuery(int modelId, int? pollingInterval, IterationWiQueryArg arg, Action<IterationWiQuery> completed)
			: base(modelId, arg, pollingInterval)
		{
			ViewModelEvents.Instance.IterationWiQueryCompleted.Subscribe(completed, ThreadOption.UIThread, false, q => q.SourceId == this.SourceId);
		}

		public void Query(bool flush)
		{
			Query<IterationWiQuery>(flush, q => q.SourceId == this.SourceId);
		}

        protected override void Execute()
        {
			this.Results = new IterationWiCollection();
			using (var tfs = TfsService.Instance.Connect())
			{
				this.Results.Load(tfs, this.Arguments.TeamProject, this.Arguments.IterationId);
			}
        }

		public override void PublishCompleted()
		{
			ViewModelEvents.Instance.IterationWiQueryCompleted.Publish(this);
		}
	}
}
