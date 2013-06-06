namespace Osiris.Tfs.Monitor
{
	#region Using

	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Collections.ObjectModel;
	using System.Diagnostics;
	using System.Windows.Input;
	using Osiris.Tfs.Monitor.Models;
	using System.Windows;
using System.Windows.Controls;
	using System.Windows.Media;

	#endregion // Using

	public interface IContentView
	{
		void SetContent(SlideVm slide);
		Visual Visual { get; }
	}

	public class ContentVm : ViewModelBase
	{
		#region Fields

		IContentView _view;
		Slide _slide;

		#endregion // Fields
	
		#region Properties

		public SlideVm Slide { get; private set; }

		#endregion // Properties

		#region Constructors

		public ContentVm(IContentView view)
		{
			_view = view;

			ViewModelEvents.Instance.SlideSelected.Subscribe(OnSlideSelected);
			ViewModelEvents.Instance.SlideUpdated.Subscribe(OnSlideUpdated);


			AppCommands.BindCommand(AppCommands.InsertSlideElement, InsertSlideElement_Executed, InsertSlideElement_CanExecute);
			AppCommands.BindCommand(AppCommands.SlideEdit, SlideEdit_Executed, SlideEdit_CanExecute);
		}

		#endregion // Constructors

		#region Methods

		private void OnSlideSelected(Slide slide)
		{
			_slide = slide;
			this.Slide = (_slide == null) ? null : new SlideVm(slide, false, false, true);
			_view.SetContent(this.Slide);

			if (slide != null)
			{
				ViewModelEvents.Instance.SlideContentDisplayed.Publish(new SlideContentDisplayedArgs(slide, _view.Visual));
				this.Slide.SelectAll();
			}
		}

		private void OnSlideUpdated(Slide slide)
		{
			if (_slide != slide)
			{
				return;
			}

			this.Slide = (_slide == null) ? null : new SlideVm(slide, false, false, true);
			_view.SetContent(this.Slide);
		}

		private void InsertSlideElement_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			_slide.DesignMode = true;
			var elemType = e.Parameter as SlideElementType;
			var container = new SlideElementContainer(elemType.CreateModel(), new Position(0, 0, 0.1, 0.1));
			_slide.SlideElements.Add(container);
			ViewModelEvents.Instance.SlideUpdated.Publish(_slide);
		}

		private void InsertSlideElement_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = this.Slide != null;
		}

		private void SlideEdit_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			ViewModelEvents.Instance.SlideUpdated.Publish(_slide);
		}

		private void SlideEdit_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = this.Slide != null;
		}

		#endregion // Methods
	}
}
