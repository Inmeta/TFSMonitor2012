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

	#endregion // Using

	public class WorkItemFieldQueryArg : IEquatable<WorkItemFieldQueryArg>
	{
		public string TeamProject { get; private set; }

		public WorkItemFieldQueryArg(string teamProject)
		{
			this.TeamProject = teamProject;
		}

		public bool Equals(WorkItemFieldQueryArg other)
		{
			return (this.TeamProject == other.TeamProject);
		}
	}

	public class WorkItemFieldQuery : QueryBase<WorkItemFieldQueryArg, List<string>>
	{
		#region Properties

		public override string Description { get { return "Loading work item fields..."; } }

		#endregion // Properties

		public WorkItemFieldQuery(int modelId, int? pollingInterval, WorkItemFieldQueryArg arg)
			: base(modelId, arg, pollingInterval)
        {
        }

		public WorkItemFieldQuery(int modelId, int? pollingInterval, WorkItemFieldQueryArg arg, Action<WorkItemFieldQuery> completed)
			: base(modelId, arg, pollingInterval)
		{
			ViewModelEvents.Instance.WorkItemFieldQueryCompleted.Subscribe(completed, ThreadOption.UIThread, false, q => q.SourceId == this.SourceId);
		}

		public void Query(bool flush)
		{
			Query<WorkItemFieldQuery>(flush, q => q.SourceId == this.SourceId);
		}

        protected override void Execute()
        {
			this.Results = new List<string>();

			using (var tfs = TfsService.Instance.Connect())
			{
				var wis = tfs.GetService<WorkItemStore>(); // //(WorkItemStore)this.Arguments.TeamProject.TeamFoundationServer.GetService(typeof(WorkItemStore));

				foreach (FieldDefinition fd in wis.FieldDefinitions)
				{
					this.Results.Add(fd.Name);
				}
			}
        }

		public override void PublishCompleted()
		{
			ViewModelEvents.Instance.WorkItemFieldQueryCompleted.Publish(this);
		}
	}
}
