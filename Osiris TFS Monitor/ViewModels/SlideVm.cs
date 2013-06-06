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
	using Osiris.Tfs.Monitor.Properties;
	using System.IO;
	using System.Collections.ObjectModel;
	using Microsoft.Practices.Composite.Events;
	using System.Windows.Controls;

	#endregion // Using

	public interface ISlideView
	{
		//UserControl Visual { get; }
		//void UpdateDesignMode();
	}

	public class SlideVm : ViewModelBase
	{
		#region Fields

		List<SlideElementContainerVm> _selected = new List<SlideElementContainerVm>();

		#endregion // Fields

		#region Properties

		public Slide Slide { get; private set; }

		public ObservableCollection<SlideElementContainerVm> SlideElements { get; private set; }

		public ISlideView View { get; set; }

		public bool ForceViewMode { get; private set; }

		public bool EnablePolling { get; private set; }

		public bool AutoDispose { get; private set; }

		#endregion // Properties

		#region Constructors

		/// <summary>
		/// Construct
		/// </summary>
		/// <param name="slide">Model</param>
		/// <param name="forceViewMode">If true will always use view-mode, not designmode</param>
		public SlideVm(Slide slide, bool forceViewMode, bool enablePolling, bool autoDispose)
		{
			this.Slide = slide;
			this.ForceViewMode = forceViewMode;
			this.EnablePolling = enablePolling;
			this.AutoDispose = autoDispose;

			this.SlideElements = new ObservableCollection<SlideElementContainerVm>();
			foreach (var elem in slide.SlideElements)
			{
				var container = new SlideElementContainerVm(this, elem);
				this.SlideElements.Add(container);
			}

			ViewModelEvents.Instance.RefreshSlide.Subscribe(OnRefreshSlide);
		}

		#endregion // Constructors

		#region Methods

		private void OnRefreshSlide(Slide slide)
		{
			if (slide == null || this.Slide == null || slide.Id != this.Slide.Id)
			{
				return;
			}

			this.SlideElements.ToList().ForEach(se => se.SlideElement.Refresh());
		}

		public void Select(SlideElementContainerVm container)
		{
			foreach (var c in _selected)
			{
				if (c != container)
				{
					c.Selected = false;
				}
			}
			_selected.Clear();
			_selected.Add(container);
			container.Selected = true;

			PublishSelected();			
		}

		public void Select(IEnumerable<SlideElementContainerVm> items)
		{
			foreach (var c in _selected)
			{
				if (!items.Contains(c))
				{
					c.Selected = false;
				}
			}
			_selected.Clear();

			foreach (var c in items)
			{
				if (!c.Selected)
				{
					c.Selected = true;
				}
				_selected.Add(c);
			}

			PublishSelected();			
		}

		private void PublishSelected()
		{
			var selected = new SlideElementCollection(_selected.Select(s => s.Model.Element));
			ViewModelEvents.Instance.SlideElementSelected.Publish(selected);
		}

		public void SelectAll()
		{
			foreach (var elem in this.SlideElements)
			{
				elem.Select();
			}
		}

		public void DeleteSlideElementContainer(SlideElementContainerVm container)
		{
			this.SlideElements.Remove(container);
			this.Slide.SlideElements.Remove(container.Model);
		}

		public void Unload()
		{
			foreach (var elem in this.SlideElements)
			{
				if (elem != null && elem.SlideElement != null)
				{
					elem.SlideElement.Unload();
				}
			}
		}

		#endregion // Methods

	}
}
