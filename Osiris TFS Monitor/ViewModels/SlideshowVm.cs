using Microsoft.Practices.Composite.Presentation.Events;

namespace Osiris.Tfs.Monitor
{
	#region Using

	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Diagnostics;
	using System.Windows.Input;
	using Osiris.Tfs.Monitor.Models;
	using Osiris.Tfs.Report;
	using Microsoft.TeamFoundation.VersionControl.Client;
	using System.Net;
	using Microsoft.TeamFoundation.Client;
	using Microsoft.TeamFoundation.WorkItemTracking.Client;
	using System.Windows.Threading;
	using System.ComponentModel;
	using System.Collections.ObjectModel;
	using System.Windows;
	using Osiris.Tfs.Monitor.Properties;
	using System.Timers;

	#endregion // Using

	public interface ISlideshowView
	{
	}

	public class SlideshowVm
	{
		DispatcherTimer _timer;
		private Dictionary<int, List<SlideVm>> _slides = new Dictionary<int, List<SlideVm>>();
		private bool _isLoaded = true;
		private CompositePresentationEvent<SlideshowVm> _pageChange = new CompositePresentationEvent<SlideshowVm>();

		public Dictionary<int, FullscreenSlideVm> FullscreenSlides { get; private set; }

		public SlideshowVm()
		{
			_pageChange.Subscribe(OnPageChange, ThreadOption.UIThread, false);

			// Get monitors for use with slideshow
			var monitors = GetMonitors();
			foreach (var monitor in monitors)
			{
				_slides.Add(monitor.Number, new List<SlideVm>());
			}

			// Distribute slides accross monitors
			int monitorPos = 0;
			foreach (var slide in ApplicationVm.Instance.Document.Slides)
			{
				var monNo = slide.GetNextMonitor(monitors.Select(m => m.Number).ToList(), monitorPos);
				_slides[monNo].Add(new SlideVm(slide, true, true, false));

				if (++monitorPos >= monitors.Count())
				{
					monitorPos = 0;
				}
			}

			// Pick first slide in each screen
			this.FullscreenSlides = new Dictionary<int, FullscreenSlideVm>();
			foreach (var monitor in monitors)
			{
				this.FullscreenSlides.Add(monitor.Number, new FullscreenSlideVm(_slides[monitor.Number].FirstOrDefault()));
			}

			StartTimer();
		}

		private void OnPageChange(SlideshowVm vm)
		{
			foreach (var fs in FullscreenSlides)
			{
				fs.Value.Slide = _slides[fs.Key].FirstOrDefault();
			}
		}

		private MonitorCollection GetMonitors()
		{
			var tmSettings = new TfsMonitorSettings();
			tmSettings.Load(Settings.Default);
			MonitorCollection monitors = monitors = tmSettings.SlideshowMonitors;
			if (monitors.Count() == 0)
			{
				monitors = MonitorCollection.Available();
			}
			return monitors;
		}

		/// <summary>
		/// Start timer
		/// </summary>
		public void StartTimer()
		{
			_timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(GetTickInterval()) };
			_timer.Tick += OnTick;
			_timer.Start();
		}

		public void StopTimer()
		{
			if (_timer != null)
			{
				_timer.Stop();
			}
			_timer = null;
		}

		private void OnTick(object sender, EventArgs e)
		{
			if (!_isLoaded)
			{
				return;
			}

			// Pop slides
			var popQueue = new Queue<KeyValuePair<int, SlideVm>>();
			var monitorPos = 0;
			foreach (var slides in _slides)
			{
				var popped = slides.Value.FirstOrDefault();
				if (popped != null)
				{
					slides.Value.Remove(popped);
					popQueue.Enqueue(new KeyValuePair<int, SlideVm>(monitorPos, popped));
				}
				monitorPos++;
			}

			// Rotate right
			while (popQueue.Count > 0)
			{
				var popped = popQueue.Dequeue();
				var monNo = popped.Value.Slide.GetNextMonitor(_slides.Keys.ToList(), popped.Key + 1);
				_slides[monNo].Add(popped.Value);
			}

			_pageChange.Publish(this);
			_timer.Interval = TimeSpan.FromSeconds(GetTickInterval());
		}

		private int GetTickInterval()
		{
			// Get monitors for use with slideshow
			var monitors = GetMonitors();

			foreach (var m in monitors)
			{
				if (_slides.ContainsKey(m.Number))
				{
					var slide = _slides[m.Number].FirstOrDefault();
					if (slide != null)
					{
						return slide.Slide.Duration;
					}
				}
			}
			return -1;
		}

		public void Unload()
		{
			_isLoaded = false;
			StopTimer();
			this.FullscreenSlides = null;

			foreach (var slideList in _slides)
			{
				foreach (var slide in slideList.Value)
				{
					slide.Unload();
				}
			}

			_slides = null;
		}
	}
}
