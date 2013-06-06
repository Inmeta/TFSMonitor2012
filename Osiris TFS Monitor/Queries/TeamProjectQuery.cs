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
	using Osiris.Tfs.Report;

	#endregion // Using

	public class TeamProjectsQueryArg : IEquatable<TeamProjectsQueryArg>
	{
		public bool Equals(TeamProjectsQueryArg other)
		{
			// No arguments, always alike
			return true;
		}
	}

	public class TeamProjectQuery : QueryBase<TeamProjectsQueryArg, TeamProjectCollection>
	{
		#region Properties

		public override string Description { get { return "Loading team projects..."; } }

		#endregion // Properties

		public TeamProjectQuery(int sourceId, int? pollingInterval)	: base(sourceId, new TeamProjectsQueryArg(), pollingInterval)
        {
        }

		public TeamProjectQuery(int sourceId, int? pollingInterval, Action<TeamProjectQuery> completed)
			: this(sourceId, pollingInterval)
		{
			ViewModelEvents.Instance.TeamProjectsQueryCompleted.Subscribe(completed, ThreadOption.UIThread, false, q => q.SourceId == this.SourceId);
		}

		public void Query(bool flush)
		{
			Query<TeamProjectQuery>(flush, q => q.SourceId == this.SourceId);
		}

        protected override void Execute()
        {
			this.Results = new TeamProjectCollection();
			using (var tfs = TfsService.Instance.Connect())
			{
				this.Results.Load(tfs);
			}
		}

		public override void PublishCompleted()
		{
			ViewModelEvents.Instance.TeamProjectsQueryCompleted.Publish(this);
		}
	}
}
