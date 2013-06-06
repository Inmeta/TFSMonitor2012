using System.Diagnostics.Contracts;

namespace Osiris.Tfs.Monitor.Models
{
	#region Using

	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
using System.Windows.Threading;
	using System.Windows.Forms;
	using System.Diagnostics;
	using System.Xml.Serialization;

	#endregion // Using

	public class BuildMonitor : SlideElement
	{
		#region Fields

		int _rows;
		int _columns;
		BuildFilterStatus _status;
		List<string> _teamProjects;

		#endregion // Fields
	
		#region Properties

		[XmlIgnore]
		public BuildFilter BuildFilter
		{
			get
			{
				Contract.Requires(this.TeamProjects != null);
				return new BuildFilter(this.MaxBuilds, this.TeamProjects, this.Status);
			}
		}

		public string Title { get; set; }

		[XmlIgnore]
		public int MaxBuilds { get { return 1000; } } 

		public int Rows
		{
			get { return _rows; }
			set
			{
				_rows = value;
			}
		}

		public int Columns 
		{
			get { return _columns; }
			set
			{
				_columns = value;
			}
		}

		[XmlIgnore]
		public BuildFilterStatus Status
		{
			get { return _status; }
			set
			{
				_status = value;
			}
		}

		[XmlElement("Status")]
		public int FooInt32
		{
			get { return (int)_status; }
			set { _status = (BuildFilterStatus)value; }
		} 

		public List<string> TeamProjects
		{
			get { return _teamProjects; }
			set
			{
				_teamProjects = value;
			}
		}
		
		#endregion // Properties

		#region Constructors

		public BuildMonitor()
		{
			// Default values
			this.Title = "Untitled";
			this.Rows = 10;
			this.Columns = 1;
			this.TeamProjects = new List<string>();
			this.Status = BuildFilterStatus.Failed | BuildFilterStatus.Succeeded | BuildFilterStatus.InProgress | BuildFilterStatus.PartiallySucceeded;
			this.UpdateInterval = 60 * 5; // Five minutes;
		}

		#endregion // Constructors

		#region Methods

		#endregion // Methods
	}
}
