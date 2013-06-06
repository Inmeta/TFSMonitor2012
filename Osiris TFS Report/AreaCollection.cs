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
	
	public class AreaCollection : List<Area>
	{
		#region Constructors

		public AreaCollection() { }
		
		public AreaCollection(NodeCollection areaRootNodes)
		{
			foreach (Node n in areaRootNodes)
			{
				if (n.IsAreaNode)
				{
					this.Add(new Area(n));
				}
			}	
		}

		#endregion // Constructors

		#region Methods

		public void Load(WorkItemStore wis, Project project)
		{
			// Add nodes of type Iteration to tree
			foreach (Node n in project.AreaRootNodes)
			{
				if (n.IsAreaNode)
				{
					this.Add(new Area(n));
				}
			}
		}

		#endregion // Methods
	}

	public class Area
	{
		#region Properties

		public Node Node { get; private set; }

		public AreaCollection Children { get; private set; }

		#endregion // Properties

		#region Constructors

		public Area(Node areaNode)
		{
			this.Node = areaNode;
			this.Children = new AreaCollection(areaNode.ChildNodes);
		}

		#endregion // Constructors
	}
}
