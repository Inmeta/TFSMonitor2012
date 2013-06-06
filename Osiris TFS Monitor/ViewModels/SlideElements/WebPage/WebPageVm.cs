using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using Size = System.Drawing.Size;

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

	#endregion // Using

	public class WebPageVm : SlideElementVm
	{
	    readonly WebPage _model;
	    readonly SlideElementContainerVm _container;
        DispatcherTimer _timer;
		private Size _size;
		private BitmapSource _image;
		private bool _started = false;

		public string Url { get { return _model.Url; } set { _model.Url = value; } }
		public int RefreshInterval { get { return _model.RefreshInterval; } }
		public SlideElement SlideElement { get { return _model; } }

		public BitmapSource Image { get { return _image; } set { _image = value; RaisePropertyChanged(() => Image); } }

	    public WebPageVm(SlideElementContainerVm container, WebPage model)
		{
			_container = container;
			_model = model;
            Debug.WriteLine("WebPage Constructed. Refresh: {0}", model.RefreshInterval);

            _timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(RefreshInterval) };
            _timer.Tick += OnTick;
		    _timer.IsEnabled = false;
            _timer.Start();
		}

		public void Select()
		{
			_container.Select();
		}

		public override void Unload()
		{
		}

        public override void Refresh()
        {
        }

		private void StopTimer()
		{
			if (_timer != null)
			{
				_timer.Tick -= OnTick;
				_timer.Stop();
			}
			_timer = null;
		}

        public void Pause()
        {
            //_timer.IsEnabled = false;
        }

        public void Continue(System.Drawing.Size size)
        {
			_size = size;

			if (!_started)
			{
				_started = true;
				Tick();
			}

            _timer.IsEnabled = true;
        }

		private void Tick()
		{
            var hc = new HtmlCapture(OnHtmlImageCapture, new Uri(Url), _size);
		}

        private void OnTick(object sender, EventArgs e)
        {
			Tick();
		}

        [DllImport("gdi32")]
        static extern int DeleteObject(IntPtr o);

        IntPtr _bitmapHandle = IntPtr.Zero;

        public BitmapSource LoadBitmap(System.Drawing.Bitmap source)
        {
            try
            {
                _bitmapHandle = source.GetHbitmap();
                if (_bitmapHandle == IntPtr.Zero)
                {
                    return null;
                }
            }
            catch (Exception)
            {
                // Swallowing... do better
                return null;
            }
            BitmapSource bs = null;
            try
            {
                bs = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(_bitmapHandle,
                   IntPtr.Zero, Int32Rect.Empty,
                   System.Windows.Media.Imaging.BitmapSizeOptions.FromEmptyOptions());
                bs.Freeze();
            }
            finally
            {
                DeleteObject(_bitmapHandle);
            }

            return bs;
        }

        private void OnHtmlImageCapture(object sender, Uri url, Bitmap image)
        {
            if (_bitmapHandle != IntPtr.Zero)
            {
                DeleteObject(_bitmapHandle);
            }
            Image = LoadBitmap(image);
        }

	}
}
