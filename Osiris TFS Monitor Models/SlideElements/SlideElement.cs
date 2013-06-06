namespace Osiris.Tfs.Monitor.Models
{
	#region Using

	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
using System.Xml.Serialization;
	using Osiris.Tfs.Report;
	using Osiris.TFS.Monitor.Common;
	using System.Diagnostics;

	#endregion // Using

	[XmlInclude(typeof(BurndownChart))]
	[XmlInclude(typeof(WebPage))]
	[XmlInclude(typeof(BuildMonitor))]
	[XmlInclude(typeof(SprintBurndownType))]
	[XmlInclude(typeof(SprintRemainingHoursBurndownType))]
	[XmlInclude(typeof(SprintRemainingTasksBurndownType))]
	[XmlInclude(typeof(Twitter))]
	public abstract class SlideElement
	{
        #region Fields

        protected TfsIteration _tfsIteration;

		protected TfsWorkItemFilter _tfsWorkItemFilter;

		protected int _updateInterval; // Seconds

        #endregion // Fields

		#region Properties

		public virtual TfsIteration TfsIteration { get { return null; } set { _tfsIteration = value; } }

		public virtual TfsWorkItemFilter TfsWorkItemFilter { get { return null; } set { _tfsWorkItemFilter = value; } }

		public int UpdateInterval
		{
			get { return _updateInterval; }
			set
			{
				_updateInterval = value;
			}
		}

		[XmlIgnore]
		public int Id { get; private set; }

		#endregion // Properties

		public SlideElement()
		{
			// Defaults
			this.Id = AutoIdGenerator.GenerateId();
		}
	}
}
