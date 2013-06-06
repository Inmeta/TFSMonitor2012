namespace Osiris.Tfs.Monitor
{
	#region Using

	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Windows;
	using System.Windows.Input;
	using System.Windows.Controls;
	using System.Diagnostics;

	#endregion // Using

	public delegate void MouseDragHandler(MouseDrag md, double offsetX, double offsetY);
	public delegate void MouseDragEndHandler();

	/// <summary>
	/// Helper class to handle mouse-drags.
	/// 
	/// ToDo: Refactor to general and not be dependent on canvas...
	/// </summary>
	public class MouseDrag
	{
		#region Fields

		bool _isDragging = false;
		Point _mouseDownPos;
		FrameworkElement _feSize;
		FrameworkElement _feEvents;
		Cursor _cursor;
		MouseDragHandler _dragHandler;
		MouseDragEndHandler _endHandler;

		#endregion // Fields

		#region Properties

		public double CanvasLeft { get; private set; }
		public double CanvasTop { get; private set; }
		public double Width { get; private set; }
		public double Height { get; private set; }
		public Point MouseDownPos { get { return _mouseDownPos; } }

		#endregion // Properties

		#region Constructors

		public MouseDrag(FrameworkElement feEvents, Cursor curs, MouseDragHandler dragHandler, MouseDragEndHandler endHandler)
			: this(null, feEvents, curs, dragHandler)
		{
			_endHandler = endHandler;
		}

		public MouseDrag(FrameworkElement feSize, FrameworkElement feEvents, Cursor curs, MouseDragHandler dragHandler)
		{
			_feSize = feSize;
			_feEvents = feEvents;
			_cursor = curs;
			_dragHandler = dragHandler;

			feEvents.MouseDown += OnMouseDown;
			feEvents.MouseEnter += OnMouseEnter;
			feEvents.MouseMove += OnMouseMove;
			feEvents.MouseUp += OnMouseUp;
			Application.Current.MainWindow.Deactivated += OnMainWindowDeactivated;
		}

		#endregion // Constructors

		#region Methods

		void OnMainWindowDeactivated(object sender, EventArgs e)
		{
			if (_isDragging)
			{
				_isDragging = false;
				Mouse.Capture(null);
				if (_endHandler != null)
				{
					_endHandler();
				}
			}
		}

		private void OnMouseDown(object sender, MouseButtonEventArgs e)
		{
			e.Handled = true;
			_feEvents.Focus();

			if (!_isDragging)
			{
				Mouse.Capture(_feEvents);

				_isDragging = true;

				// Store original positions/sizes of fe
				if (_feSize != null)
				{
					this.Height = _feSize.Height;
					this.Width = _feSize.Width;
					this.CanvasLeft = Canvas.GetLeft(_feSize);
					this.CanvasTop = Canvas.GetTop(_feSize);
				}
				_mouseDownPos = new Point(e.GetPosition(null).X, e.GetPosition(null).Y);
			}
		}

		private void OnMouseEnter(object sender, MouseEventArgs e)
		{
			e.Handled = true;
			_feEvents.Cursor = _cursor;
		}

		private void OnMouseMove(object sender, MouseEventArgs e)
		{
			e.Handled = true;
			_feEvents.Cursor = _cursor;

			if (_isDragging)
			{
				var x = e.GetPosition(null).X;
				var y = e.GetPosition(null).Y;
				_dragHandler(this, x - _mouseDownPos.X, y - _mouseDownPos.Y);
			}
		}

		private void OnMouseUp(object sender, MouseButtonEventArgs e)
		{
			e.Handled = true;
			if (_isDragging)
			{
				_isDragging = false;
				Mouse.Capture(null);
				if (_endHandler != null)
				{
					_endHandler();
				}
			}
		}

		public void UnRegisterEvents()
		{
			if (_feEvents != null)
			{
				_feEvents.MouseDown -= OnMouseDown;
				_feEvents.MouseEnter -= OnMouseEnter;
				_feEvents.MouseMove -= OnMouseMove;
				_feEvents.MouseUp -= OnMouseUp;
			}
			if (Application.Current.MainWindow != null)
			{
				Application.Current.MainWindow.Deactivated -= OnMainWindowDeactivated;
			}

			_feSize = null;
			_feEvents = null;
			_cursor = null;
			_dragHandler = null;
			_endHandler = null;
		}

		#endregion // Methods
	}
}
