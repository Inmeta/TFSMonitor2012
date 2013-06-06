namespace Osiris.Tfs.Monitor.Models
{
	#region Using

	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using Microsoft.TeamFoundation.Client;
	using Osiris.Tfs.Report;
	using System.Threading;
	using Microsoft.TeamFoundation.VersionControl.Client;
	using System.Windows.Threading;
	using System.Diagnostics;

	#endregion // Using

	/// <summary>
	/// Client delegate handler for query complete
	/// </summary>
	/// <param name="iterations"></param>
	public delegate void SprintBurndownQueryCompletedHandler(SprintBurndown data);

	/// <summary>
	/// Client delegate handler for query failure
	/// </summary>
	/// <param name="ex"></param>
	public delegate void SprintBurndownQueryFailureHandler(Exception ex);

	/// <summary>
	/// Query workitems of type iteration from tfs.
	/// </summary>
	public class SprintBurndownQuery : TfsServiceQuery
	{
		#region Fields

		SprintBurndownQueryCompletedHandler _completedHandler;
		SprintBurndownQueryFailureHandler _failureHandler;
		string _teamProject;
		int _iterationId;
		bool _excludeWeekEnds;
		SprintBurndown _result;
		SprintBurndownType _sbType;
		string _estimateFieldName;
		string _remainingFieldName;

		#endregion // Fields

		public override string Description { get { return "Getting burndown-data..."; } }

		#region Constructors

		public SprintBurndownQuery(Dispatcher disp, string teamProject, int iterationId, bool excludeWeekEnds, SprintBurndownQueryCompletedHandler complatedHandler,
			SprintBurndownQueryFailureHandler failureHandler, SprintBurndownType sbType, string estimateFieldName, string remainingFieldName)
			: base(disp)
		{
			_completedHandler = complatedHandler;
			_failureHandler = failureHandler;
			_teamProject = teamProject;
			_iterationId = iterationId;
			_excludeWeekEnds = excludeWeekEnds;
			_sbType = sbType;
			_estimateFieldName = estimateFieldName;
			_remainingFieldName = remainingFieldName;
		}

		#endregion // Constructors

		#region Methods

		/// <summary>
		/// Called from service.
		/// </summary>
		/// <param name="tfs"></param>
		public override void Execute(TeamFoundationServer tfs)
		{
			// Ugly....
			if (_sbType is SprintRemainingHoursBurndownType)
			{
				_result = new SprintRemainingHoursBurndown(_teamProject, _iterationId, new TimeSpan(0, 12, 0, 0, 0), _excludeWeekEnds, _estimateFieldName, _remainingFieldName);
			}
			else if (_sbType is SprintRemainingTasksBurndownType)
			{
				_result = new SprintRemaningTasksBurndown(_teamProject, _iterationId, new TimeSpan(0, 12, 0, 0, 0), _excludeWeekEnds, _estimateFieldName, _remainingFieldName);
			}
			else
			{
				Debug.Assert(false);
			}
			_result.Load(tfs);
		}

		public override void Completed()
		{
			_completedHandler(_result);
		}
	
		/// <summary>
		/// When query fails report to client
		/// </summary>
		/// <param name="ex"></param>
		public override void Failure(Exception ex)
		{
			_failureHandler(ex);
		}

		#endregion // Methods
	}
}
