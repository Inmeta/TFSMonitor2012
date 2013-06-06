namespace Osiris.Tfs.Monitor.Models
{
	#region Using

	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
using Osiris.Tfs.Report;
using System.Xml.Serialization;
	using Microsoft.TeamFoundation.WorkItemTracking.Client;
	using System.Diagnostics;

	#endregion // Using

	public class TfsAreaFilter : IWorkItemFilter
	{
		#region Properties

		public int AreaId { get; set; }

		public string AreaPath { get; set; }

		[XmlIgnore]
		public AreaCollection Areas { get; private set; }

		[XmlIgnore]
		public string AreaName
		{
			get
			{
				if (string.IsNullOrEmpty(this.AreaPath))
				{
					return null;
				}

				string result = this.AreaPath;
				int pos = result.IndexOf('\\');
				while (pos >= 0)
				{
					result = result.Substring(pos + 1);
					pos = result.IndexOf('\\');
				}
				return result;
			}
		}

		#endregion // Properties

		#region Constructor

		public TfsAreaFilter()
		{
			this.Areas = new AreaCollection();
		}

		public TfsAreaFilter(WorkItemStore wis, Project project) : this()
		{
			var sw = new Stopwatch();
			sw.Start();

			this.Areas.Load(wis, project);

			sw.Stop();
			Debug.WriteLine("Area's load: " + sw.ElapsedMilliseconds.ToString() + "ms");

		}

		public TfsAreaFilter(int areaId, string areaPath) : this()
		{
			this.AreaId = areaId;
			this.AreaPath = areaPath;

		}

		/// <summary>
		/// Copy construct
		/// </summary>
		/// <param name="it"></param>
		public TfsAreaFilter(TfsAreaFilter other)
		{
			Copy(other);
		}

		#endregion // Constructor

		#region Methods

		/// <summary>
		/// Compare if alike
		/// </summary>
		/// <param name="it"></param>
		/// <returns></returns>
		public bool Compare(TfsAreaFilter af)
		{
			return (this.AreaId == af.AreaId);
		}

		/// <summary>
		/// Copy 
		/// </summary>
		/// <param name="it"></param>
		public void Copy(TfsAreaFilter af)
		{
			this.AreaId = af.AreaId;
			this.AreaPath = af.AreaPath;
		}

		#endregion // Methods

		public bool IsWithin(Revision rev)
		{
			return true;
		}
	}
}
