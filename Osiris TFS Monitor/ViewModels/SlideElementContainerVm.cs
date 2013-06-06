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
using System.Drawing;
using System.Windows;

	#endregion // Using

	public interface ISlideElementContainerView
	{
		SlideElementContainerVm ViewModel { get; }
		UIElement UIElement { get; }
        void InsertSlideElement(object viewModel);
	}

	public class SlideElementContainerVm : ViewModelBase
	{
		#region Fields

		SlideElementContainer _model;
		//bool _sender = false;
		bool _selected = false;
		SlideVm _parent;

		#endregion // Fields

		#region Properties 

		public SlideElementContainer Model { get { return _model; } }

		public SlideElementVm SlideElement { get; private set; }

		public Position Position 
		{ 
			get { return _model.Position; } 
			set 
			{ 
				_model.Position = value;
				//_sender = true;
			} 
		}

		public bool Selected
		{
			get { return _selected; }
			set
			{
				if (_selected != value)
				{
					_selected = value;
					RaisePropertyChanged(() => Selected);
				}
			}
		}

        ISlideElementContainerView _view;

        public ISlideElementContainerView View 
        {
            get { return _view; }
            set
            {
                _view = value;
                if (_view != null)
                {
					if (this.SlideElement == null)
					{
						bool designMode = _parent.ForceViewMode ? false : _parent.Slide.DesignMode;
						this.SlideElement = SlideElementType.CreateViewModel(this, _model.Element, designMode);
					}
					_view.InsertSlideElement(this.SlideElement);
                }
            }
        }

		public SlideVm Parent { get { return _parent; } }

		#endregion // Properties

		#region Constructors

		public SlideElementContainerVm(SlideVm slide, SlideElementContainer model)
		{
			_parent = slide;
			_model = model;
		}

		#endregion // Constructors

		#region Methods

		public void Select()
		{
			_parent.Select(this);
		}

		public void Delete()
		{
			_parent.DeleteSlideElementContainer(this);
		}

		#endregion // Methods
	}
}
