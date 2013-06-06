namespace Osiris.Tfs.Monitor.Models
{
	#region Using

	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using Microsoft.TeamFoundation.Client;
	using System.Windows.Threading;

	#endregion // Using

	/// <summary>
    /// Client delegate handler for query complete
    /// </summary>
    public delegate void BuildQueryCompletedHandler(BuildCollection buildDefinitions);

    /// <summary>
    /// Client delegate handler for query failure
    /// </summary>
    public delegate void BuildQueryFailureHandler(Exception ex);

    public class BuildQuery : TfsServiceQuery
	{
		#region Fields

		BuildQueryCompletedHandler _completedHandler;
        BuildQueryFailureHandler _failureHandler;
        BuildCollection _result;
		BuildFilter _filter;

		#endregion // Fields

		#region Properties

		public override string Description { get { return "Getting builds..."; } }

		#endregion // Properties

		public BuildQuery(Dispatcher disp, BuildFilter filter, BuildQueryCompletedHandler completedHandler,
            BuildQueryFailureHandler failureHandler) : base(disp, 0, false)
        {
            _completedHandler = completedHandler;
            _failureHandler = failureHandler;
			_filter = filter;
        }

        public override void Execute(TeamFoundationServer tfs)
        {
            _result = new BuildCollection();
			_result.Load(tfs, _filter);
        }

        public override void Failure(Exception ex)
        {
            _failureHandler(ex);
        }

        public override void Completed()
        {
            _completedHandler(_result);
        }

		public void Update(BuildFilter filter)
		{
			_filter = filter;
			this.Updated = true;
		}
    }
}
