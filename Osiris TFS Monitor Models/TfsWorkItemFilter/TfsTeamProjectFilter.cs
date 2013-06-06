namespace Osiris.Tfs.Monitor.Models
{
	#region Using

	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;

	#endregion // Using

	public class TfsTeamProjectFilter
	{
		#region Fields

		#endregion // Fields

		#region Properties

		public string TeamProjectName { get; set; }

		#endregion // Properties

		#region Constructor

		public TfsTeamProjectFilter(string tpName)
		{
			this.TeamProjectName = tpName;
		}

		/// <summary>
		/// Copy construct
		/// </summary>
		/// <param name="it"></param>
		public TfsTeamProjectFilter(TfsTeamProjectFilter tpf)
		{
			Copy(tpf);
		}

		#endregion // Constructor

		#region Methods

		public void Load()
		{
		}

		/// <summary>
		/// Compare if alike
		/// </summary>
		/// <param name="it"></param>
		/// <returns></returns>
		public bool Compare(TfsTeamProjectFilter tpf)
		{
			return false;
		}

		/// <summary>
		/// Copy 
		/// </summary>
		/// <param name="it"></param>
		public void Copy(TfsTeamProjectFilter tpf)
		{
		}

		#endregion // Methods
	}
}
