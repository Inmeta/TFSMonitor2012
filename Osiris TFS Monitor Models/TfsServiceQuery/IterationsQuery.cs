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

	#endregion // Using

	/*
	/// <summary>
	/// Client delegate handler for query complete
	/// </summary>
	/// <param name="iterations"></param>
	public delegate void IterationsQueryCompletedHandler(IterationCollection iterations);

	/// <summary>
	/// Client delegate handler for query failure
	/// </summary>
	/// <param name="ex"></param>
	public delegate void IterationsQueryFailureHandler(Exception ex);

	/// <summary>
	/// Query workitems of type iteration from tfs.
	/// </summary>
	public class IterationsQuery : TfsServiceQuery
	{
		#region Fields

		IterationsQueryCompletedHandler _completedHandler;
		IterationsQueryFailureHandler _failureHandler;
		TeamProject _tp;
		IterationCollection _result;

		#endregion // Fields

		public override string Description { get { return "Getting iteration work items..."; } }

		#region Constructors

		public IterationsQuery(Dispatcher disp, TeamProject tp, IterationsQueryCompletedHandler complatedHandler,
			IterationsQueryFailureHandler failureHandler) : base(disp)
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
			_result = new IterationCollection();
			_result.Load(_tp);
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
