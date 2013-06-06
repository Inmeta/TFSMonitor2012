namespace Osiris.Tfs.Report
{
	#region Usings

	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using Microsoft.TeamFoundation.WorkItemTracking.Client;
	using Microsoft.TeamFoundation.Client;
	using System.Diagnostics;
	using Osiris.TFS.Report;

	#endregion // Usings

	public class SprintRemaningTasksBurndown : SprintBurndown
	{
		#region Fields

		SprintBurndownType _type = new SprintRemainingTasksBurndownType();

		#endregion // Fields

		#region Constructors

		public SprintRemaningTasksBurndown (string teamProject, int iterationWiId, TimeSpan timeInterval,
			bool excludeWeekEnds, string estimateFieldName, string remainingFieldName)
			: base(teamProject, iterationWiId, timeInterval, excludeWeekEnds, estimateFieldName, remainingFieldName)
		{
		}

		#endregion // Constructors

		#region Properties

		public override SprintBurndownType SprintBurndownType { get { return _type; } }

		#endregion // Properties

		#region Methods

		/// <summary>
		/// Create chart data basen on the intermediate timeline we created
		/// </summary>
		internal override void CreateChartData(WorkItemTimeline timeline)
		{
			int numIntervals = ((int)this.EndDate.Subtract(this.StartDate).TotalMinutes) / (int)_timeInterval.TotalMinutes;

			for (DateTime day = this.StartDate; day <= this.EndDate; day = day.AddMinutes(_timeInterval.TotalMinutes))
			{
				var wiTimes = timeline[day];

				_data.Add(new BurndownData(day, wiTimes.TotalRemainingTasks(), 0));
			}
		}

		#endregion // Methods

	}
}
