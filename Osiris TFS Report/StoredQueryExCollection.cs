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
	/// Collection of StoredQueries.
	/// </summary>
	public class StoredQueryExCollection : List<StoredQueryEx>
	{
		#region Fields

		#endregion // Fields

		#region Constructors

		public StoredQueryExCollection()
		{
		}

		#endregion // Constructors

		#region Methods

		public void Load(WorkItemStore wis, Project project)
		{
			foreach (StoredQuery sq in project.StoredQueries)
			{
				this.Add(new StoredQueryEx(sq));				 
			} 
		}

		#endregion // Methods
	}

	public class StoredQueryEx
	{
		#region Properties

		public StoredQuery StoredQuery { get; private set; }

		public bool IsPublic { get { return this.StoredQuery.QueryScope == QueryScope.Public; } }

		public bool IsPrivate { get { return this.StoredQuery.QueryScope == QueryScope.Private; } }

		#endregion // Properties

		#region Constructors

		public StoredQueryEx(StoredQuery sq)
		{
			this.StoredQuery = sq;
		}

		#endregion // Constructors
	}

}
