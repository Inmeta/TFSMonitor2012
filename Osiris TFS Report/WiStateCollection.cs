namespace Osiris.Tfs.Report
{
	#region Usings

	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using Microsoft.TeamFoundation.Client;
	using System.Diagnostics;
	using Microsoft.TeamFoundation.WorkItemTracking.Client;
	using Microsoft.TeamFoundation.VersionControl.Client;
	using Microsoft.TeamFoundation.Build.Client;

	#endregion // Usings
	
	/// <summary>
	/// Collection of WiStates.
	/// </summary>
	public class WiStateCollection : List<WiState>
	{
		#region Fields

		#endregion // Fields

		#region Constructors

		public WiStateCollection()
		{
		}

		#endregion // Constructors

		#region Methods

		public void Load(WorkItemTypeExCollection wiTypes)
		{
			foreach (WorkItemTypeEx wit in wiTypes)
			{

			}
		}

		#endregion // Methods
	}

	public class WiState
	{
		#region Properties

		//public Node Node { get; private set; }

		//public WorkItem IterationWi { get; private set; }

		//public IterationCollection Children { get; private set; }

		#endregion // Properties

		#region Constructors

		/*public WiType(Node itNode, IEnumerable<WorkItem> itWis)
		{
			this.Node = itNode;
			this.Children = new IterationCollection(itNode.ChildNodes, itWis);

			// Has iteration workitem?
			this.IterationWi = itWis.FirstOrDefault(wi => wi.IterationPath == itNode.Path);
		}*/

		#endregion // Constructors
	}

}
