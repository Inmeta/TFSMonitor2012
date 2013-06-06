using System.Xml.Serialization;

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

		public static IEnumerable<WorkItemType> GetTypes(WorkItemStore wis, Project project)
		{
			return project.WorkItemTypes.Cast<WorkItemType>().ToList();
		}

		public static IEnumerable<string> GetStates(IEnumerable<WorkItemType> types)
		{
			var states = new Dictionary<string, bool>();

			foreach (var wiType in types)
			{
				var xmldoc = wiType.Export(false);
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
		public string Name { get; set; }

		//[XmlIgnore]
		//internal WorkItemType WorkItemType { get; private set; }

		[Obsolete("For serialization only")]
		public WorkItemTypeEx() { }

		public WorkItemTypeEx(string name)
		{
			this.Name = name;
			//this.WorkItemType = wiType;
		}
	}

}
