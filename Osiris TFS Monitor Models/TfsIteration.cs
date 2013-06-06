namespace Osiris.Tfs.Monitor.Models
{
	#region Using

	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Xml.Serialization;
	using Osiris.TFS.Monitor.Common;
	using System.Diagnostics.Contracts;

	#endregion // Using

	public class TfsIteration
	{
		#region Properties

		[XmlIgnore]
		public int Id { get; private set; }

		// Name of Team Project
		public string TeamProject { get; set; }

		// Iteration Work Item ID
		public int? IterationId { get; set; }

		// Iteration Path
		public string IterationPath { get; set; }

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
				var posHist = new List<int>();
				while (pos >= 0)
				{
					posHist.Add((posHist.Count == 0) ? pos + 1 : posHist.Last() + pos + 1);
					result = result.Substring(pos + 1);
					pos = result.IndexOf('\\');
				}
				if (posHist.Count > 0)
				{
					result = this.IterationPath.Substring(posHist[0]);
				}

				return result;
			}
		}

		#endregion // Properties

		#region Constructor

		public TfsIteration()
		{
			// Defaults
			this.Id = AutoIdGenerator.GenerateId();
		}

		/// <summary>
		/// Copy construct
		/// </summary>
		/// <param name="it"></param>
		public TfsIteration(TfsIteration it)
		{
			Contract.Requires(it != null);

			Copy(it);
		}

		#endregion // Constructor

		#region Methods

		/// <summary>
		/// Compare if alike
		/// </summary>
		/// <param name="it"></param>
		/// <returns></returns>
		public bool Compare(TfsIteration it)
		{
			return (this.TeamProject == it.TeamProject &&
				this.IterationPath == it.IterationPath);

		}

		/// <summary>
		/// Copy 
		/// </summary>
		/// <param name="it"></param>
		public void Copy(TfsIteration it)
		{
			this.Id = it.Id;
			this.TeamProject = it.TeamProject;
			this.IterationId = it.IterationId;
			this.IterationPath = it.IterationPath;
		}

		#endregion // Methods
	}
}
