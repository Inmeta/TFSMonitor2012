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

	#endregion // Using

	/*

	/// <summary>
	/// Client delegate handler for query complete
	/// </summary>
	/// <param name="teamProjects"></param>
	public delegate void TeamProjectsQueryCompletedHandler(TeamProjectCollection teamProjects);

	/// <summary>
	/// Client delegate handler for query failure
	/// </summary>
	/// <param name="ex"></param>
	public delegate void TeamProjectsQueryFailureHandler(Exception ex);

	public class TeamProjectsQuery : TfsServiceQuery
	{
		#region Fields
		
		TeamProjectsQueryCompletedHandler _completedHandler;
		TeamProjectsQueryFailureHandler _failureHandler;
		TeamProjectCollection _result;

		#endregion // Fields

		public override string Description { get { return "Getting team projects..."; } }

		#region Constructors

		private TeamProjectsQuery() : base(null, 60, true)
		{
		}

		public TeamProjectsQuery(Dispatcher disp, TeamProjectsQueryCompletedHandler completedHandler, 
			TeamProjectsQueryFailureHandler failureHandler) : base(disp, 60, true)
		{
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
			_result = new TeamProjectCollection();
			_result.Load(tfs);
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

		public override bool Compare(TfsServiceQuery qry)
		{
			// No params, so always true
			return true;
		}

		public override void ExecuteFromCache(TfsServiceQuery qry)
		{
			var tpQry = qry as TeamProjectsQuery;
			_completedHandler(tpQry._result);
		}

		public static TeamProjectCollection Query(TeamFoundationServer tfs)
		{
			var qry = new TeamProjectsQuery();
			var data = qry.GetFromCache() as TeamProjectsQuery;
			if (data != null)
			{
				return data._result;
			}
			qry.Execute(tfs);

			return qry._result;
		}

		#endregion // Methods
	}
	*/
}
