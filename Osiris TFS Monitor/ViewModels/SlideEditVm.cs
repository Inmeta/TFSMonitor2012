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

	public class SlideEditVm : ViewModelBase
	{
		#region Fields


		#endregion // Fields

		#region Properties

		public Slide Slide { get; private set; }

		public ObservableCollection<SlideElementEditContainerVm> SlideElements { get; private set; }

		#endregion // Properties

		#region Constructors

		public SlideEditVm(Slide slide)
		{
			this.Slide = slide;

			this.SlideElements = new ObservableCollection<SlideElementEditContainerVm>();
			foreach (var elem in slide.SlideElements)
			{
				var container = new SlideElementEditContainerVm(elem);
				this.SlideElements.Add(container);
			}
		}

		#endregion // Constructors

		#region Methods

		public void InsertSlideElement(ISlideElement slideElement)
		{
			var container = new SlideElementContainer(slideElement, new Position(0, 0, 0.1, 0.1));
			this.Slide.SlideElements.Add(container);
			this.SlideElements.Add(new SlideElementEditContainerVm(container));
		}

		#endregion // Methods
	}

}
