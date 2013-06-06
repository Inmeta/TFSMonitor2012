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
	using System.Windows.Threading;
	using System.Diagnostics;
	using Microsoft.TeamFoundation.VersionControl.Client;

	#endregion // Using

	/*
	/// <summary>
	/// Client delegate handler for query complete
	/// </summary>
	/// <param name="TfsAreas"></param>
	public delegate void TfsTeamProjectFilterQueryCompletedHandler(TfsWorkItemFilter filter);

	/// <summary>
	/// Client delegate handler for query failure
	/// </summary>
	/// <param name="ex"></param>
	public delegate void TfsTeamProjectFilterQueryFailureHandler(Exception ex);

	public class TfsTeamProjectFilterQuery : TfsServiceQuery
	{
		#region Fields

		TfsTeamProjectFilterQueryCompletedHandler _completedHandler;
		TfsTeamProjectFilterQueryFailureHandler _failureHandler;
		TfsWorkItemFilter _result;
		TeamProject _teamProject;

		#endregion // Fields

		public override string Description { get { return "Getting workitem-filters for team project..."; } }

		#region Constructors

		public TfsTeamProjectFilterQuery(Dispatcher disp, TeamProject tp, 
			TfsTeamProjectFilterQueryCompletedHandler completedHandler,
			TfsTeamProjectFilterQueryFailureHandler failureHandler)
			: base(disp)
		{
			_teamProject = tp;
			_completedHandler = completedHandler;
			_failureHandler = failureHandler;
		}
		
		#endregion // Constructors

		#region Methods

		/// <summary>
		/// Called from service.
		/// </summary>
		/// <param name="tfs"></param>
		public override void Execute(TeamFoundationServer tfs)
		{
			_result = new TfsWorkItemFilter();
			_result.Load(_teamProject);
		}

		/// <summary>
		/// When query fails report to client
		/// </summary>
		/// <param name="ex"></param>
		public override void Failure(Exception ex)
		{
			_failureHandler(ex);
		}

		public override void Completed()
		{
			_completedHandler(_result);
		}

		#endregion // Methods
	}
	*/
}
