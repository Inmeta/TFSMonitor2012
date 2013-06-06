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
	using System.Xml.Linq;
	using System.Xml;

	#endregion // Usings
	
	/// <summary>
	/// Collection of WorkItemTypeEx.
	/// </summary>
	public class WorkItemTypeExCollection : List<WorkItemTypeEx>
	{
		#region Fields

		#endregion // Fields

		#region Constructors

		public WorkItemTypeExCollection()
		{
		}

		#endregion // Constructors

		#region Methods

		public void Load(WorkItemStore wis, Project project)
		{
			foreach (WorkItemType wit in project.WorkItemTypes)
			{
				this.Add(new WorkItemTypeEx(wit));
			}
		}

		public IEnumerable<string> GetStates()
		{
			var states = new Dictionary<string, bool>();

			foreach (var wiType in this)
			{
				var xmldoc = wiType.WorkItemType.Export(false);
				var xdoc = XDocument.Load(new XmlNodeReader(xmldoc));
				var qry = from s in xdoc.Descendants("WORKFLOW").Elements("STATES").Elements("STATE")
						select (string)s.Attribute("value");

				foreach (var state in qry)
				{
					if (!states.ContainsKey(state))
					{
						states.Add(state, true);
					}
				}
			}

			return states.Keys;
		}

		#endregion // Methods
	}

	public class WorkItemTypeEx
	{
		// CoreFieldReferenceNames

		#region Properties

		public string Name { get { return this.WorkItemType.Name; } }

		public WorkItemType WorkItemType { get; private set; }

		#endregion // Properties

		#region Constructors

		public WorkItemTypeEx(WorkItemType wiType)
		{
			this.WorkItemType = wiType;
		}

		#endregion // Constructors
	}

}
