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
	/// Collection of iteration-nodes. Each node also holds instance of
	/// iteration work item matching the iteration path of the node.
	/// </summary>
	public class IterationCollection : List<Iteration>
	{
		#region Constructors

		public IterationCollection()
		{
		}

		public IterationCollection(NodeCollection itRootNodes)
		{
			foreach (Node n in itRootNodes)
			{
				if (n.IsIterationNode)
				{
					this.Add(new Iteration(n));
				}
			}
		}

		public IterationCollection(NodeCollection itRootNodes, IEnumerable<WorkItem> itWis)
		{
			foreach (Node n in itRootNodes)
			{
				if (n.IsIterationNode)
				{
					this.Add(new Iteration(n, itWis));
				}
			}	
		}

		#endregion // Constructors

		#region Methods

		/// <summary>
		/// For backwards-compatability, when loading filter we dont need to
		/// get workitemstore more than once....
		/// </summary>
		/// <param name="tp"></param>
		public void Load(TfsTeamProjectCollection tfsProjects, string projectName)
		{
			var wis = tfsProjects.GetService<WorkItemStore>();

			foreach (Project p in wis.Projects)
			{
				if (p.Name == projectName)
				{
					Load(wis, p);
					return;
				}
			}
		}

		public void Load(WorkItemStore wis, Project project)
		{
			// Add nodes of type Iteration to tree
			foreach (Node n in project.IterationRootNodes)
			{
				if (n.IsIterationNode)
				{
					this.Add(new Iteration(n));
				}
			}
		}

		#endregion // Methods
	}

	public class Iteration
	{
		#region Properties

		public Node Node { get; private set; }

		public WorkItem IterationWi { get; private set; }

		public IterationCollection Children { get; private set; }

		#endregion // Properties

		#region Constructors

		public Iteration(Node itNode)
		{
			this.Node = itNode;
			this.Children = new IterationCollection(itNode.ChildNodes);
		}

		public Iteration(Node itNode, IEnumerable<WorkItem> itWis)
		{
			this.Node = itNode;
			this.Children = new IterationCollection(itNode.ChildNodes, itWis);

			// Has iteration workitem?
			this.IterationWi = itWis.FirstOrDefault(wi => wi.IterationPath == itNode.Path);
		}

		#endregion // Constructors
	}

}
