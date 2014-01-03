using Microsoft.Practices.Composite.Presentation.Events;
using Osiris.Tfs.Monitor.Properties;

namespace Osiris.Tfs.Monitor
{
	#region Usings

	using System.Windows.Threading;
	using System;
	using System.Collections.Generic;
	using System.Windows;
	using System.Windows.Interop;
	using System.Windows.Media;
	using System.Windows.Input;
	using System.Diagnostics;
	using Osiris.Tfs.Monitor.Models;
	using System.Linq;

	#endregion

	public class SlideshowView : ISlideshowView
	{
		#region Fields

		private List<FullscreenSlideView> _windows = new List<FullscreenSlideView>();
		private SlideshowVm _vm;

		#endregion // Fields

		#region Propeties

		public event EventHandler OnFullscreenClosed;

		#endregion // Propeties

		#region Constructors

		/// <summary>
		/// Construct
		/// </summary>
		public SlideshowView()
		{
			_vm = new SlideshowVm();
		}

		#endregion // Constructors

		#region Methods

		public void Show()
		{
			var screens = System.Windows.Forms.Screen.AllScreens;

			int currNum = 0;
			foreach (var screen in _vm.FullscreenSlides)
			{
				if (ApplicationVm.Instance.Document.Slides.Any(s => s.Monitors.Any(m => m.Number == currNum) || s.Monitors.Count == 0))
				{
					var wnd = this.CreateWindow(screens[screen.Key], screen.Value);
					wnd.KeyDown += new System.Windows.Input.KeyEventHandler(Window_KeyDown);
				//	wnd.Deactivated += new EventHandler(Window_Deactivated);
					wnd.Show();
				}
				currNum++;
			}

			if (_windows.Count() > 0)
			{
				_windows[0].Activate();
				_windows[0].Focus();
			}
		}

		private FullscreenSlideView CreateWindow(System.Windows.Forms.Screen display, FullscreenSlideVm vm)
		{
			var wnd = new FullscreenSlideView(vm)
			{
			    Left = display.Bounds.X,
			    Top = display.Bounds.Y,
			    Width = display.Bounds.Width,
			    Height = display.Bounds.Height,
			};
			_windows.Add(wnd);
			return wnd;
		}

		#endregion // Methods

		void Window_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Escape)
			{
				Unload();
			}
		}

		void Window_Deactivated(object sender, EventArgs e)
		{
			Unload();
		}

		/// <summary>
		/// Dispose
		/// </summary>
		/// <inheritdoc/>
		public void Unload()
		{
			_vm.Unload();
			while (_windows.Count > 0)
			{
				var wnd = _windows[0];
				wnd.Deactivated -= Window_Deactivated;
				wnd.KeyDown -= Window_KeyDown;
				_windows.Remove(wnd);
				wnd.Close();
			}

			if (this.OnFullscreenClosed != null)
			{
				OnFullscreenClosed(this, EventArgs.Empty);
			}
		}
	}
}
