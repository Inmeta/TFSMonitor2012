using System.Collections;

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

	public abstract class SprintBurndownType
	{
		public abstract string Name { get; }

		//public abstract string RemainingName { get; }

		public abstract bool DisplayEstimate { get; }
	}

	public class SprintRemainingHoursBurndownType : SprintBurndownType
	{
		public override string Name { get { return "Remaining time"; } }

		//public override string RemainingName { get { return "Remaining time"; } }

		public override bool DisplayEstimate { get { return true; } }
	}

	public class SprintRemainingTasksBurndownType : SprintBurndownType
	{
		public override string Name { get { return "Remaining tasks"; } }

		//public override string RemainingName { get { return "Remaining tasks"; } }

		public override bool DisplayEstimate { get { return false; } }
	}

	public class SprintBurndownTypeCollection : List<SprintBurndownType>
	{
		public SprintBurndownTypeCollection()
		{
			this.Add(new SprintRemainingHoursBurndownType());
			this.Add(new SprintRemainingTasksBurndownType());
		}
	}

	public abstract class SprintBurndown : IEnumerable<BurndownData> //List<BurndownData>
	{
		#region Fields

		protected int _iterationWiId;
		private string _startDateFieldName = "Start Date";
		private string _finishDateFieldName = "Finish Date";
		protected TimeSpan _timeInterval; // Minutes
		protected bool _excludeWeekEnds;
		string _estimateFieldName;
		string _remainingFieldName;
		protected readonly List<BurndownData> _data = new List<BurndownData>();

		#endregion // Fields

		#region Properties

		public string TeamProject { get; private set; }
		public WorkItem Iteration { get; private set; }
		public DateTime StartDate { get; private set; }
		public DateTime EndDate { get; private set; }
		public TimeSpan TimeInterval { get { return _timeInterval; } }
		public DateTime NowDate { get; set; }

		public abstract SprintBurndownType SprintBurndownType { get; }

		#endregion // Properties

		#region Constructors

		/// <summary>
		/// Construct with enough data in order to create sprint graph-data
		/// </summary>
		/// <param name="teamProject"></param>
		/// <param name="iterationWiId"></param>
		public SprintBurndown(string teamProject, int iterationWiId, TimeSpan timeInterval,
			bool excludeWeekEnds, string estimateFieldName, string remainingFieldName)
		{
			// Set team project
			this.TeamProject = teamProject;
			_iterationWiId = iterationWiId;
			_timeInterval = timeInterval;
			_excludeWeekEnds = excludeWeekEnds;
			_estimateFieldName = estimateFieldName;
			_remainingFieldName = remainingFieldName;
		}

		#endregion // Constructors

		#region Methods

		/// <summary>
		/// Load chart-data into this object.
		/// </summary>
		/// <param name="tfs"></param>
		/// <returns></returns>
		public void Load(TfsTeamProjectCollection tfsProjects)
		{
			// Get workitem store
			var wis = tfsProjects.GetService<WorkItemStore>(); // (WorkItemStore)tfs.GetService(typeof(WorkItemStore));

			// Get iteration path
			LoadIteration(wis, _iterationWiId);

			// Load chart-data
			LoadChartData(wis);
		}

		/// <summary>
		/// Find specified iteration work item id in TFS. This work item
		/// contains the iteration path.
		/// </summary>
		/// <param name="wis"></param>
		/// <param name="iterationWiId"></param>
		/// <returns></returns>
		private void LoadIteration(WorkItemStore wis, int iterationWiId)
		{
			try
			{
				var wi = wis.GetWorkItem(iterationWiId);
			
				// Start date
				DateTime? startDate = wi.Fields.GetDateTimeValue(_startDateFieldName);
				if (!startDate.HasValue)
				{
					throw new TfsReportException("Iteration workitem id: " + iterationWiId.ToString() + " start-date not specified.");
				}
				this.StartDate = startDate.Value.Date;

				// Finish date
				DateTime? endDate = wi.Fields.GetDateTimeValue(_finishDateFieldName);
				if (!endDate.HasValue)
				{
					throw new TfsReportException("Iteration workitem id: " + iterationWiId.ToString() + " finish-date not specified.");
				}
				this.EndDate = endDate.Value.Date;

				// Store iteration work item
				this.Iteration = wi;
			}
			catch (DeniedOrNotExistException ex)
			{
				throw new TfsReportException("Iteration workitem id: " + iterationWiId.ToString() + " not found or access denied. " + ex.Message);
			}
		}

		/// <summary>
		/// Load chart data
		/// </summary>
		/// <param name="wis"></param>
		/// <returns></returns>
		void LoadChartData(WorkItemStore wis)
		{
			var wiCol = new WorkItemCollectionEx(wis);
			wiCol.Load(this.TeamProject, this.Iteration.IterationPath);

			// Create timeline
			var timeline = new WorkItemTimeline(wiCol, this.Iteration.IterationId, this.StartDate, this.EndDate, _timeInterval,
				_estimateFieldName, _remainingFieldName);

			// Calculate timeline
			timeline.Calculate();

			this.NowDate = timeline.Now;

			// Create chart-data basen on timeline
			//CreateChartData(timeline);

			// Version of CreateChartData with current date also
			for (DateTime time = this.StartDate.Date; time <= this.EndDate.Date; time = timeline.Next(time))
			{
				CreateChartData(time, timeline[time]);
			}
		}

		internal virtual void CreateChartData(WorkItemTimeline timeline) { }

		/// <summary>
		/// Create chart data for each interval between start- and end-date of burndown iteration.
		/// 
		/// Note: Also includes current time.
		/// </summary>
		/// <param name="time">Point of time</param>
		/// <param name="wis">WorkItems for this point of time</param>
		internal virtual void CreateChartData(DateTime time, WorkItemTimeCollection wis) { }

		#endregion // Methods

		public IEnumerator<BurndownData> GetEnumerator()
		{
			return _data.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}

}
