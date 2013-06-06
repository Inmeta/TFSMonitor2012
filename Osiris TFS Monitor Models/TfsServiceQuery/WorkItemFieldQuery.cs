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
	using Microsoft.TeamFoundation.WorkItemTracking.Client;

	#endregion // Using
	/*
	
	/// <summary>
	/// Client delegate handler for query complete
	/// </summary>
	/// <param name="WorkItemField"></param>
	public delegate void WorkItemFieldQueryCompletedHandler(List<string> fields);

	/// <summary>
	/// Client delegate handler for query failure
	/// </summary>
	/// <param name="ex"></param>
	public delegate void WorkItemFieldQueryFailureHandler(Exception ex);

	/// <summary>
	/// Query workitems of type iteration from tfs.
	/// </summary>
	public class WorkItemFieldQuery : TfsServiceQuery
	{
		#region Fields

		WorkItemFieldQueryCompletedHandler _completedHandler;
		WorkItemFieldQueryFailureHandler _failureHandler;
		string _tp;
		List<string> _result;

		#endregion // Fields

		public override string Description { get { return "Getting workitem fields..."; } }

		#region Constructors

		public WorkItemFieldQuery(Dispatcher disp, string tp, WorkItemFieldQueryCompletedHandler complatedHandler,
			WorkItemFieldQueryFailureHandler failureHandler)
			: base(disp)
		{
			_completedHandler = complatedHandler;
			_failureHandler = failureHandler;
			_tp = tp;
		}

		#endregion // Constructors

		#region Methods

		/// <summary>
		/// Called from service.
		/// </summary>
		/// <param name="tfs"></param>
		public override void Execute(TeamFoundationServer tfs)
		{
			_result = new List<string>();
			var versionControlServer = (VersionControlServer)tfs.GetService(typeof(VersionControlServer));
			var tp = versionControlServer.GetTeamProject(_tp);
			if (tp == null)
			{
				return;
			}

			var wis = (WorkItemStore)tp.TeamFoundationServer.GetService(typeof(WorkItemStore));
	
			foreach (FieldDefinition fd in wis.FieldDefinitions)
			{
				_result.Add(fd.Name);
			}
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
*/	
}
