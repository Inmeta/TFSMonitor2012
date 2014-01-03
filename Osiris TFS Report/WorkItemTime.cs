namespace Osiris.Tfs.Report
{
	#region Usings

	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using Microsoft.TeamFoundation.WorkItemTracking.Client;

	#endregion // Usings

	internal class WorkItemTime
	{
		#region Fields

		Revision _latestRevision;

		#endregion // Fields

		#region Properties

		public Revision Revision
		{
			get
			{
				return _latestRevision;
			}
		}

		public decimal DecimalValue(string field)
		{
			decimal? value = _latestRevision.Fields.GetDecimalValue(field);
			return value.HasValue ? value.Value : 0;
		}


		/*public decimal Estimate 
		{
			get
			{
				// WI has estimate?
				decimal? estimate = _latestRevision.Fields.GetDecimalValue("Estimate");
				return estimate.HasValue ? estimate.Value : 0;
			}
		}
		public decimal Remaining 
		{ 
			get
			{
				// WI has remaining hours?
				decimal? remaining = _latestRevision.Fields.GetDecimalValue("Remaining Work");
				return remaining.HasValue ? remaining.Value : 0;
			}
		}*/

		public string State 
		{ 
			get
			{
				return _latestRevision.Fields["State"].Value as string;
			}
		}
		public string WiType 
		{ 
			get
			{
				return _latestRevision.Fields["Work Item Type"].Value as string;
			}
		}

		public int WiId { get { return _latestRevision.WorkItem.Id; } }


		#endregion // Properties

		#region Constructors

		public WorkItemTime(Revision rev)
		{
			Init(rev);
		}

		#endregion // Constructors

		#region Methods

		private void Init(Revision rev)
		{

			// WI has estimate?
			//decimal? estimate = rev.Fields.GetDecimalValue("Estimate");

			// WI has remaining hours?
			//decimal? remaining = rev.Fields.GetDecimalValue("Remaining Work");

			// WI has changed date field?
			//DateTime? day = rev.Fields.GetDateTimeValue("Changed Date");

			// Correct data in TFS template?
			/*if (!estimate.HasValue || !remaining.HasValue || !day.HasValue)
			{
				estimate = 0;
				remaining = 0;
			}*/

			
			_latestRevision = rev;
			//this.Estimate = estimate;
			//this.Remaining = remaining;
			//this.State = state;
			//this.WiType = wiType;
		}

		public void Join(Revision rev)
		{
			// Latest?
			if (rev.Index > _latestRevision.Index)
			{
				Init(rev);
			}
		}

		#endregion // Methods
	}
}
