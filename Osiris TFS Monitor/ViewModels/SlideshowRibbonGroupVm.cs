namespace Osiris.Tfs.Monitor
{
	#region Using

	using System.Linq;
	using Osiris.Tfs.Monitor.Models;

	#endregion // Using

	public class SlideshowRibbonGroupVm : ViewModelBase
	{
		#region Fields

		Slide _slide;

		#endregion // Fields

		#region Properties

		public int? Duration
		{
			get
			{
				if (_slide == null)
				{
					return null;
				}
				return _slide.Duration;
			}
			set
			{
				if (value.HasValue)
				{
					// Error or 0?
					if (value.Value > 0)
					{
						_slide.Duration = value.Value;
					}
				}
			}
		}
		
		public bool DurationEnabled
		{
			get { return _slide != null; }
		}

		public int? UpdateInterval
		{
			get
			{
				if (_slide == null)
				{
					return null;
				}

				
				if (_slide.SlideElements.Count <= 0)
				{
					return null;
				}

				return _slide.SlideElements.First().Element.UpdateInterval;
			}
			set
			{
				if (value.HasValue)
				{
					// Error or 0?
					if (value.Value > 0)
					{
						_slide.SlideElements.ForEach(se => se.Element.UpdateInterval = value.Value);

						foreach (var se in _slide.SlideElements)
						{
							ViewModelEvents.Instance.UpdateIntervalChanged.Publish(se.Element);
						}
					}
				}
			}
		}

		public bool UpdateIntervalEnabled
		{
			// Refactor to see if all are alike
			get { return _slide != null; }
		}

		public bool IsMonitorsEnabled
		{
			get { return _slide != null; }
		}

		public ObservableNodeList Monitors { get; private set; }

		#endregion // Properties

		#region Constructors

		public SlideshowRibbonGroupVm()
		{
			this.Monitors = new ObservableNodeList();

			ViewModelEvents.Instance.SlideSelected.Subscribe(OnSlideSelected);
		}

		#endregion // Constructors

		#region Methods

		/// <summary>
		/// Called when zero or more SlideElements have been selected and we need
		/// to reflect this in UI.
		/// </summary>
		/// <param name="elements"></param>
		private void OnSlideSelected(Slide slide)
		{
			_slide = slide;

			RaisePropertyChanged(() => DurationEnabled);
			RaisePropertyChanged(() => Duration);
			RaisePropertyChanged(() => UpdateIntervalEnabled);
			RaisePropertyChanged(() => UpdateInterval);
			RaisePropertyChanged(() => IsMonitorsEnabled);


			this.Monitors = null;
			if (_slide != null)
			{
				this.Monitors = new ObservableNodeList();
				var monitors = MonitorCollection.Available();
				foreach (var m in monitors)
				{
					this.Monitors.Add(new MonitorItem(m, _slide.Monitors.SingleOrDefault(m2 => m2.Number == m.Number) != null, new MonitorItemChangedDelegate(OnMonitorChanged)));
				}
			}
			RaisePropertyChanged(() => Monitors);
		}

		private void OnMonitorChanged()
		{
			if (_slide == null)
			{
				return;
			}
			_slide.Monitors.Clear();
			_slide.Monitors.AddRange(this.Monitors.Where(m => m.IsSelected).Select(m => new Monitor(((MonitorItem)m).Number)));
		}

		#endregion // Methods
	}

	public delegate void MonitorItemChangedDelegate();

	public class MonitorItem : ComboWithCheckBoxNode
	{
		#region Fields

		private MonitorItemChangedDelegate _changed = null;

		#endregion // Fields

		#region Properties

		public int Number { get; private set; }

		#endregion // Properties

		#region Constructors

		public MonitorItem(Monitor m, bool selected) 
		{
			this.Number = m.Number;
			this.Title = m.Name + " (" + m.MonitorInfo + ")";
			this.IsSelected = selected;
		}

		public MonitorItem(Monitor m, bool selected, MonitorItemChangedDelegate changed) : this(m, selected)
		{
			_changed = changed;		
		}

		#endregion // Constructors

		#region Methods

		public override void Changed()
		{
			if (_changed != null)
			{
				_changed();
			}
		}

		#endregion // Methods
	}
}
