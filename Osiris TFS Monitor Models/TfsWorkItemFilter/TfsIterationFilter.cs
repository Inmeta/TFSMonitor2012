namespace Osiris.Tfs.Monitor.Models
{
	#region Using

	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Xml.Serialization;
	using Microsoft.TeamFoundation.WorkItemTracking.Client;
	using Osiris.Tfs.Report;
	using System.Diagnostics;

	#endregion // Using



	public class TfsIterationFilter : IWorkItemFilter
	{
		#region Properties

		public int? IterationId { get; set; }

		public string IterationPath { get; set; }

		[XmlIgnore]
		public IterationCollection Iterations { get; private set; }

		[XmlIgnore]
		public string IterationName
		{
			get
			{
				if (string.IsNullOrEmpty(this.IterationPath))
				{
					return null;
				}

				string result = this.IterationPath;
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

		public TfsIterationFilter()
		{
			// Defaults
			this.IterationId = null;
			this.IterationPath = null;

			this.Iterations = new IterationCollection();
		}

		public TfsIterationFilter(WorkItemStore wis, Project project) : this()
		{
			var sw = new Stopwatch();
			sw.Start();

			this.Iterations.Load(wis, project);

			sw.Stop();
			Debug.WriteLine("TfsIterationFilter.Load: " + sw.ElapsedMilliseconds.ToString() + "ms");
		}

		public TfsIterationFilter(int itId, string itPath) : this()
		{
			this.IterationId = itId;
			this.IterationPath = itPath;
		}

		/// <summary>
		/// Copy construct
		/// </summary>
		/// <param name="it"></param>
		public TfsIterationFilter(TfsIterationFilter itf)
		{
			this.Iterations = new IterationCollection();
			Copy(itf);
		}

		#endregion // Constructor

		#region Methods

		/// <summary>
		/// Compare if alike
		/// </summary>
		/// <param name="it"></param>
		/// <returns></returns>
		public bool Compare(TfsIterationFilter itf)
		{
			return (this.IterationId == itf.IterationId);
		}

		/// <summary>
		/// Copy 
		/// </summary>
		/// <param name="it"></param>
		public void Copy(TfsIterationFilter itf)
		{
			this.IterationId = itf.IterationId;
			this.IterationPath = itf.IterationPath;
		}

		#endregion // Methods

		public bool IsWithin(Revision rev)
		{
			return true;
		}
	}
}
