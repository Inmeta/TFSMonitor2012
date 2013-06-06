using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Windows.Forms;

namespace Osiris.Tfs.Monitor
{
    [ComVisible(true), ComImport()]
    [GuidAttribute("0000010d-0000-0000-C000-000000000046")]
    [InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IViewObject
    {
        [return: MarshalAs(UnmanagedType.I4)]
        [PreserveSig]
        int Draw(
            [MarshalAs(UnmanagedType.U4)] UInt32 dwDrawAspect,
            int lindex,
            IntPtr pvAspect,
            [In] IntPtr ptd,
            IntPtr hdcTargetDev,
            IntPtr hdcDraw,
            [MarshalAs(UnmanagedType.Struct)] ref Rectangle lprcBounds,
            [MarshalAs(UnmanagedType.Struct)] ref Rectangle lprcWBounds,
            IntPtr pfnContinue,
            [MarshalAs(UnmanagedType.U4)] UInt32 dwContinue);
        [PreserveSig]
        int GetColorSet([In, MarshalAs(UnmanagedType.U4)] int dwDrawAspect,
           int lindex, IntPtr pvAspect, [In] IntPtr ptd,
            IntPtr hicTargetDev, [Out] IntPtr ppColorSet);
        [PreserveSig]
        int Freeze([In, MarshalAs(UnmanagedType.U4)] int dwDrawAspect,
                        int lindex, IntPtr pvAspect, [Out] IntPtr pdwFreeze);
        [PreserveSig]
        int Unfreeze([In, MarshalAs(UnmanagedType.U4)] int dwFreeze);
        void SetAdvise([In, MarshalAs(UnmanagedType.U4)] int aspects,
          [In, MarshalAs(UnmanagedType.U4)] int advf,
          [In, MarshalAs(UnmanagedType.Interface)] IAdviseSink pAdvSink);
        void GetAdvise([In, Out, MarshalAs(UnmanagedType.LPArray)] int[] paspects,
          [In, Out, MarshalAs(UnmanagedType.LPArray)] int[] advf,
          [In, Out, MarshalAs(UnmanagedType.LPArray)] IAdviseSink[] pAdvSink);
    }


    public class HtmlCapture
    {
        private readonly System.Windows.Forms.WebBrowser _web = new System.Windows.Forms.WebBrowser();
        private readonly Timer _timer;
        private Rectangle _screen;
        private Size? _imgSize = null;
        private readonly Uri _url;

        public delegate void HtmlCaptureEvent(object sender, Uri url, Bitmap image);
        private readonly HtmlCaptureEvent _htmlImageCapture;

        public HtmlCapture(HtmlCaptureEvent htmlImageCapture, Uri url, Size imgSize)
        {
            _htmlImageCapture = htmlImageCapture;
            _url = url;
            _imgSize = imgSize;
            _screen = Screen.PrimaryScreen.Bounds;

            _web = new System.Windows.Forms.WebBrowser
            {
                Width = _screen.Width,
                Height = _screen.Height,
                ScriptErrorsSuppressed = true,
                ScrollBarsEnabled = false
            };
            //_web.Navigating += OnNavigating;
            _web.DocumentCompleted += OnDocumentCompleted;

            _timer = new Timer { Interval = 2000 };
            _timer.Tick += OnTick;

            _web.Navigate(url);
        }

        /*public void Create(string url, Size imgsz)
        {
            this._imgSize = imgsz;

            //web.Navigate(@"http://osiris-lan\msj:DataNerd07@http://tfs.osiris.no:8080/tfs/DefaultCollection/Inmeta");
            //string additionalHeaderInfo = "Authorization: Basic " + Convert.ToBase64String(Encoding.ASCII.GetBytes(@"osiris-lan\msj" + ":" + @"xxxxx")) + System.Environment.NewLine;
            //web.Navigate("http://release.osiris.no", null, null, additionalHeaderInfo);

           _web.Navigate(url);
        }*/

        void OnDocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            _timer.Start();
        }

        /*void OnNavigating(object sender, WebBrowserNavigatingEventArgs e)
        {
            _timer.Stop();
        }*/

        void OnTick(object sender, EventArgs e)
        {
            _timer.Stop();

            // Get the size of the document's body
            var body = _web.Document.Body.ScrollRectangle;

            //check if the document width/height is greater than screen width/height
            var docRectangle = new Rectangle()
            {
                Location = new Point(0, 0),
                Size = new Size(body.Width > _screen.Width ? body.Width : _screen.Width,
                 body.Height > _screen.Height ? body.Height : _screen.Height)
            };

            //set the width and height of the WebBrowser object
            _web.Width = docRectangle.Width;
            _web.Height = docRectangle.Height;

            //if the imgsize is null, the size of the image will 
            //be the same as the size of webbrowser object
            //otherwise  set the image size to imgsize
            Rectangle imgRectangle;
            if (_imgSize == null)
            {
                imgRectangle = docRectangle;
            }
            else
            {
                imgRectangle = new Rectangle()
                {
                    Location = new Point(0, 0),
                    Size = _imgSize.Value
                };
            }

            // Create a bitmap object 
            var bitmap = new Bitmap(imgRectangle.Width, imgRectangle.Height);
            //get the viewobject of the WebBrowser
            IViewObject ivo = _web.Document.DomDocument as IViewObject;

            using (var g = Graphics.FromImage(bitmap))
            {
                //get the handle to the device context and draw
                IntPtr hdc = g.GetHdc();
                ivo.Draw(1, -1, IntPtr.Zero, IntPtr.Zero,
                         IntPtr.Zero, hdc, ref imgRectangle,
                         ref docRectangle, IntPtr.Zero, 0);
                g.ReleaseHdc(hdc);
            }

            // invoke the HtmlImageCapture event
            _htmlImageCapture(this, _url, bitmap);

            _timer.Tick -= OnTick;
            _timer.Dispose();
            //_web.Navigating -= OnNavigating;
            _web.DocumentCompleted -= OnDocumentCompleted;
            _web.Dispose();
        }
    }
}
