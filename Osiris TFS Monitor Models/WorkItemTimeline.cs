using Osiris.Tfs.Monitor.Models;

namespace Osiris.Tfs.Report
{
	#region Usings

	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using Microsoft.TeamFoundation.WorkItemTracking.Client;
using System.Diagnostics;
using Osiris.TFS.Report;

	#endregion // Usings

	internal class WorkItemTimeline : Dictionary<DateTime, WorkItemTimeCollection>
	{
		#region Fields

		DateTime _from;
		DateTime _to;
		TimeSpan _timeInterval;
		private readonly TfsWorkItemFilter _filter;
		int _iterationId;

		#endregion // Fields

		#region Properties

		public DateTime Now { get; private set; }

		#endregion // Properties

		#region Constructors

		public WorkItemTimeline(WorkItemCollectionEx wiCol, int iterationId, DateTime from, DateTime to, TimeSpan timeInterval,
			string estimateFieldName, string remainingFieldName, TfsWorkItemFilter filter)
		{
			_iterationId = iterationId;
			_timeInterval = timeInterval;
			_filter = filter;
			_from = from.Date;
			_to = to.Date;
			this.Now = DateTime.Now;
	
			// Init collection
			for (DateTime time = _from; time <= _to; time = time.AddMinutes(_timeInterval.TotalMinutes))
			{
				this.Add(time, new WorkItemTimeCollection(estimateFieldName, remainingFieldName));
			}

			// Add current time also
			if (!this.ContainsKey(this.Now))
			{
				if (this.Now >= _from && this.Now <= _to)
				{
					this.Add(this.Now, new WorkItemTimeCollection(estimateFieldName, remainingFieldName));
				}
			}

			// Add WorkItems
			wiCol.ForEach(Add);

			/*foreach (var wi in wiCol)
			{
				//if (filter.IsWithIn(wi))
				{
					Add(wi);
				}
			}*/
		}

		#endregion // Constructors

		#region Methods

		/// <summary>
		/// Add WI to timeline
		/// </summary>
		/// <param name="wi"></param>
		private void Add(WorkItem wi)
		{
			foreach (Revision revision in wi.Revisions)
			{
				if (_filter.IsWithIn(revision))
				{
					AddRevision(revision);
				}
			}
		}

		/// <summary>
		/// Add revision to timeline
		/// </summary>
		/// <param name="rev"></param>
		private void AddRevision(Revision rev)
		{


			// WI has changed date field?
			DateTime? day = rev.Fields.GetDateTimeValue("Changed Date");

			if (!day.HasValue)
			{
				return;
			}

			// If revision is less than beginning of timeline, set it to beginning
			if (day.Value.Date < _from)
			{
				day = _from;
			}

			// If WI is greater than timeline, don't add
			if (day.Value.Date > _to)
			{
				return;
			}

			this[GetNearestInterval(day.Value)].Add(rev);
		}

		private DateTime GetNearestInterval(DateTime dt)
		{
			for (DateTime time = _from; time <= _to; time = Next(time))
			{
				if (dt <= time)
				{
					return time;
				}
			}
			return _to;
		}

		private DateTime GetNearestDownInterval(DateTime dt)
		{
			DateTime prev = _from;
			for (DateTime time = _from; time <= _to; time = time.AddMinutes(_timeInterval.TotalMinutes))
			{
				if (dt < time)
				{
					return prev;
				}
				prev = time;
			}
			return _to;
		}

		/// <summary>
		/// Polulated WI's from the day they begin to the end of the timeline
		/// </summary>
		public void Calculate()
		{
			// Init collection
			for (DateTime time = _from; time <= _to; time = Next(time))
			{
				foreach (var wiTime in this[time].Values)
				{
					PopulateWi(wiTime, Next(time));
				}

				// Remove workitems who's not in this iteration
				this[time].Filter(_iterationId);
			}
		}

		/// <summary>
		/// Recursivly add wi's forward until we find a day where it's populated
		/// or until the timeline has ended.
		/// </summary>
		/// <param name="wiTime"></param>
		/// <param name="from"></param>
		private void PopulateWi(WorkItemTime wiTime, DateTime time)
		{
			// Passed timeline?
			if (time > _to)
			{
				return;
			}

			// Already polulated?
			if (this[time].ContainsKey(wiTime.WiId))
			{
				return;
			}

			// Add
			this[time].Add(wiTime.WiId, wiTime);

			// Recursively populate
			PopulateWi(wiTime, Next(time));
		}

		public DateTime Next(DateTime dt)
		{
			if (dt == this.Now)
			{
				return GetNearestDownInterval(dt).AddMinutes(_timeInterval.TotalMinutes);
			}
			var dtNextInterval = dt.AddMinutes(_timeInterval.TotalMinutes);
			if (this.Now > dt && this.Now < dtNextInterval && this.Now < _to)
			{
				return this.Now;
			}
			return dtNextInterval;
		}

		#endregion // Methods
	}
}
