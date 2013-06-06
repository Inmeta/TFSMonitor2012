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

	public class BurndownQuery : QueryBase<BurndownChart, SprintBurndown>
	{
		#region Properties

		public override string Description { get { return "Loading burndown-data..."; } }

		#endregion // Properties

		public BurndownQuery(BurndownChart model, int? pollingInterval)	: base(model.Id, model, pollingInterval)
        {
        }

		public BurndownQuery(BurndownChart model, int? pollingInterval, Action<BurndownQuery> completed)
			: this(model, pollingInterval)
		{
			ViewModelEvents.Instance.BurndownQueryCompleted.Subscribe(completed, ThreadOption.UIThread, false, q => q.SourceId == this.SourceId);
		}

		public void Query(bool flush)
		{
			Query<BurndownQuery>(flush, q => q.SourceId == this.SourceId);
		}

        protected override void Execute()
        {
			// Ugly....
			if (this.Arguments.SprintBurndownType is SprintRemainingHoursBurndownType)
			{
				this.Results = new SprintRemainingHoursBurndown(this.Arguments.TfsIteration.TeamProject, this.Arguments.TfsIteration.IterationId.Value, new TimeSpan(0, 12, 0, 0, 0), this.Arguments.ExcludeWeekEnds, this.Arguments.EstimateFieldName,
					this.Arguments.RemainingFieldName);
			}
			else if (this.Arguments.SprintBurndownType is SprintRemainingTasksBurndownType)
			{
				this.Results = new SprintRemaningTasksBurndown(this.Arguments.TfsIteration.TeamProject, this.Arguments.TfsIteration.IterationId.Value, new TimeSpan(0, 12, 0, 0, 0), this.Arguments.ExcludeWeekEnds, this.Arguments.EstimateFieldName,
					this.Arguments.RemainingFieldName);
			}
			else
			{
				Debug.Assert(false);
			}
			using (var tfs = TfsService.Instance.Connect())
			{
				this.Results.Load(tfs, this.Arguments.TfsWorkItemFilter);
			}
        }

		public override void PublishCompleted()
		{
			ViewModelEvents.Instance.BurndownQueryCompleted.Publish(this);
		}

		public void Enque()
		{
			this.EnqueQuery<BuildQuery>(q => q.SourceId == this.SourceId);
		}

	}
}
