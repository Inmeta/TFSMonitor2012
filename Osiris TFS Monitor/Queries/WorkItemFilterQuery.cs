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

	#endregion // Using

	public class WorkItemFilterQueryArg : IEquatable<WorkItemFilterQueryArg>
	{
		public string TeamProject { get; private set; }

		public WorkItemFilterQueryArg(string teamProject)
		{
			this.TeamProject = teamProject;
		}

		public bool Equals(WorkItemFilterQueryArg other)
		{
			return (this.TeamProject == other.TeamProject);
		}
	}

	public class WorkItemFilterQuery : QueryBase<WorkItemFilterQueryArg, TfsWorkItemFilter>
	{
		#region Properties

		public override string Description { get { return "Loading work item filters..."; } }

		#endregion // Properties

		public WorkItemFilterQuery(int modelId, int? pollingInterval, WorkItemFilterQueryArg arg)
			: base(modelId, arg, pollingInterval)
        {
        }

		public WorkItemFilterQuery(int modelId, int? pollingInterval, WorkItemFilterQueryArg arg, Action<WorkItemFilterQuery> completed)
			: base(modelId, arg, pollingInterval)
		{
			ViewModelEvents.Instance.WorkItemFilterQueryCompleted.Subscribe(completed, ThreadOption.UIThread, false, q => q.SourceId == this.SourceId);
		}

		public void Query(bool flush)
		{
			Query<WorkItemFilterQuery>(flush, q => q.SourceId == this.SourceId);
		}

        protected override void Execute()
        {
			this.Results = new TfsWorkItemFilter();
			this.Results.Load(this.Arguments.TeamProject);
        }

		public override void PublishCompleted()
		{
			ViewModelEvents.Instance.WorkItemFilterQueryCompleted.Publish(this);
		}
	}
}
