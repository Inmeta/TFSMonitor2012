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
	public class IterationWiCollection : List<IterationWi>
	{
		#region Fields

		string _query = @"SELECT [System.Id], [System.WorkItemType] FROM WorkItems WHERE [System.TeamProject] = '{0}' AND [System.WorkItemType] = 'Iteration'";

		#endregion // Fields

		#region Constructors

		public IterationWiCollection()
		{
		}

		/*public IterationCollection(NodeCollection itRootNodes)
		{
			foreach (Node n in itRootNodes)
			{
				if (n.IsIterationNode)
				{
					this.Add(new Iteration(n));
				}
			}
		}*/

		/*public IterationCollection(NodeCollection itRootNodes, IEnumerable<WorkItem> itWis)
		{
			foreach (Node n in itRootNodes)
			{
				if (n.IsIterationNode)
				{
					this.Add(new Iteration(n, itWis));
				}
			}	
		}*/

		#endregion // Constructors

		#region Methods

		public void Load(TfsTeamProjectCollection tfsProjects, string projectName, int? iterationId)
		{
			var wis = tfsProjects.GetService<WorkItemStore>();
			Load(wis, projectName);

			this.Sort(new IterationWiComparer());
			foreach (var it in this)
			{
				it.Sort();
			}
		}

		private void Load(WorkItemStore wis, string projectName)
		{
			var sw = new Stopwatch();
			sw.Start();

			// Query iteration-WI's within teamproject
			string query = string.Format(_query, projectName); 
			var wiCol = wis.Query(query);

			foreach (WorkItem wi in wiCol)
			{
				AddNode(wi);
			}

			sw.Stop();
			Debug.WriteLine("Iteration WI's query: " + sw.ElapsedMilliseconds.ToString() + "ms");

		}

		private void AddNode(WorkItem wi)
		{
			var itPath = wi.IterationPath;
			var paths = ParseIterationPath(itPath);

			if (paths.Count() > 0)
			{
				paths.RemoveAt(0);
			}

			if (paths.Count() > 1)
			{
				var firstPath = paths.First();
				var node = this.FirstOrDefault(i => i.IterationPart == firstPath);
				if (node != null)
				{
					paths.RemoveAt(0);
					node.Add(wi, paths);
				}
				else
				{
					this.Add(new IterationWi(wi, paths));
				}
			}
			else
			{
				this.Add(new IterationWi(wi, paths));
			}
		}

		private List<string> ParseIterationPath(string path)
		{
			var paths = new List<string>();
			int start = 0;
			int end = path.IndexOf('\\');
			while (end >= 0)
			{
				if (end > start)
				{
					paths.Add(path.Substring(start, end-start));
				}
				start = end + 1;
				end = path.IndexOf('\\', start);
			}
			paths.Add(path.Substring(start));
			return paths;
		}

		#endregion // Methods
	}

	public class IterationWi
	{
		#region Properties

		public int? IterationId { get; set; }

		public int? IterationWiId { get; set; }

		public string IterationPart { get; set; }

		public string Name { get; set; }
		
		public IterationWiCollection Children { get; private set; }

		#endregion // Properties

		#region Constructors

		public IterationWi(WorkItem wi, List<string> paths)
		{
			if (paths.Count() > 1)
			{
				this.Children = new IterationWiCollection();
				var firstPath = paths.First();
				this.Name = firstPath;
				this.IterationPart = firstPath;
				this.IterationId = null;
				paths.RemoveAt(0);
				this.Children.Add(new IterationWi(wi, paths));
			}
			else
			{
				this.Name = wi.Title;
				this.IterationId = wi.IterationId;
				this.IterationWiId = wi.Id;				
			}
		}

		#endregion // Constructors

		public void Add(WorkItem wi, List<string> paths)
		{
			if (paths.Count() > 1)
			{
				var firstPath = paths.First();
				var node = this.Children.FirstOrDefault(i => i.Name == firstPath);
				if (node != null)
				{
					paths.RemoveAt(0);
					node.Add(wi, paths);
				}
				else
				{
					this.Children.Add(new IterationWi(wi, paths));
				}
			}
			else
			{
				this.Children.Add(new IterationWi(wi, paths));
			}
		}

		public void Sort()
		{
			if (this.Children == null)
			{
				return;
			}
			this.Children.Sort(new IterationWiComparer());
			foreach (var it in this.Children)
			{
				it.Sort();
			}
		}
	}

	public class IterationWiComparer : IComparer<IterationWi>
	{
		public int Compare(IterationWi x, IterationWi y)
		{
			if (x == null)
			{
				if (y == null)
				{
					// If x is null and y is null, they're
					// equal. 
					return 0;
				}
				else
				{
					// If x is null and y is not null, y
					// is greater. 
					return -1;
				}
			}
			else
			{
				// If x is not null...
				//
				if (y == null)
				// ...and y is null, x is greater.
				{
					return 1;
				}
				else
				{
					if (x.IterationId == null && y.IterationId != null)
					{
						return -1;
					}
					if (x.IterationId != null && y.IterationId == null)
					{
						return 1;
					}

					return x.Name.CompareTo(y.Name);
				}
			}
		}
	}
}
