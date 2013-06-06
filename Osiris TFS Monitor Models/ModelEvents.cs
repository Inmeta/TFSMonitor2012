namespace Osiris.Tfs.Monitor
{
	#region Using

	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using Osiris.Tfs.Monitor.Models;
	using System.Windows;
	using System.Windows.Media;
	using Microsoft.TeamFoundation.VersionControl.Client;
	using Microsoft.Practices.Composite.Presentation.Events;

	#endregion // Using

	public class BuildQueryCompletedArg
	{
		public BuildMonitor Model { get; private set; }
		public BuildCollection Result { get; private set; }

		public BuildQueryCompletedArg(BuildMonitor model, BuildCollection builds)
		{
			this.Model = model;
			this.Result = builds;
		}
	}

	public class QueryFailedArg
	{
		public object Model { get; private set; }
		public Exception Exception { get; private set; }

		public QueryFailedArg(object model, Exception ex)
		{
			this.Model = model;
			this.Exception = ex;
		}
	}

	public class ModelEvents
	{
		#region Fields

		static ModelEvents _instance;

		#endregion // Fields

		#region Properties

		public static ModelEvents Instance
		{
			get
			{
				if (_instance == null)
				{
					_instance = new ModelEvents();
				}
				return _instance;
			}
		}

		public CompositePresentationEvent<BuildQueryCompletedArg> BuildQueryCompleted { get; private set; }
		public CompositePresentationEvent<QueryFailedArg> QueryFailed { get; private set; }
		//public CompositePresentationEvent<SlideElement> SlideElementUpdated { get; private set; }

		#endregion // Properties

		#region Constructors

		private ModelEvents()
		{
			this.BuildQueryCompleted = new CompositePresentationEvent<BuildQueryCompletedArg>();
			this.QueryFailed = new CompositePresentationEvent<QueryFailedArg>();
			//this.SlideElementUpdated = new CompositePresentationEvent<SlideElement>();
		}

		#endregion // Constructors
	}
}
