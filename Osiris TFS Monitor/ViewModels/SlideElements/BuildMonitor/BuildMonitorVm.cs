using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Windows.Media.Imaging;

namespace Osiris.Tfs.Monitor
{



	#region Using

	using System;
	using System.Linq;
    using System.Diagnostics;
    using Osiris.Tfs.Monitor.Models;
    using System.Net;
    using Osiris.Tfs.Monitor.Properties;
	using System.IO;
	using System.Collections.ObjectModel;
    using Osiris.TFS.Monitor.Common;
	using Microsoft.TeamFoundation.Build.Client;
	using Microsoft.Practices.Composite.Presentation.Events;

	#endregion // Using

	public class FailedBuildVm : BuildVm 
	{
		static readonly Uri IconUri = new Uri("pack://application:,,,/Resources/Icons/Buildstatus/Failed.png");

		public FailedBuildVm(Build b, Uri userProfilesUri, int rows) : base(b, IconUri, userProfilesUri, rows) { }
	}
	public class SucceededBuildVm : BuildVm
	{
		static readonly Uri IconUri = new Uri("pack://application:,,,/Resources/Icons/Buildstatus/Succeeded.png");

		public SucceededBuildVm(Build b, Uri userProfilesUri, int rows) : base(b, IconUri, userProfilesUri, rows) { }
	}
	public class PartiallySucceededBuildVm : BuildVm
	{
		static readonly Uri IconUri = new Uri("pack://application:,,,/Resources/Icons/Buildstatus/Partial.png");

		public PartiallySucceededBuildVm(Build b, Uri userProfilesUri, int rows) : base(b, IconUri, userProfilesUri, rows) { }
	}
	public class InProgressBuildVm : BuildVm
	{
		static readonly Uri IconUri = new Uri("pack://application:,,,/Resources/Icons/Buildstatus/InProgress.png");

		public InProgressBuildVm(Build b, Uri userProfilesUri, int rows) : base(b, IconUri, userProfilesUri, rows) { }
	}

	public abstract class BuildVm : ViewModelBase, IComparable<BuildVm>
	{
		private bool _isLoading = false;
		public bool IsLoading { get { return _isLoading; }  set { _isLoading = value; RaisePropertyChanged(() => IsLoading); } }

		int _rows;
		public int Rows 
		{ 
			get { return _rows; } 
			private set { if (value != _rows) { _rows = value; RaisePropertyChanged(() => Rows); } }
		}

		string _name;
		public string Name { get { return _name; } private set { if (value != _name) {_name  = value; RaisePropertyChanged(() => Name); } } }

		Scale2d _scale2d;
		public Scale2d Scale2d { get { return _scale2d; } private set { if (value != _scale2d) {_scale2d  = value; RaisePropertyChanged(() => Scale2d); } } }

		string _startTime;
		public string StartTime { get { return _startTime; } private set { if (value != _startTime) {_startTime  = value; RaisePropertyChanged(() => StartTime); } } }

		string _requestedBy;
		public string RequestedBy { get { return _requestedBy; } private set { if (value != _requestedBy) {_requestedBy  = value; RaisePropertyChanged(() => RequestedBy); } } }

		string _finishTime;
		public string FinishTime { get { return _finishTime; }private set { if (value != _finishTime) {_finishTime  = value; RaisePropertyChanged(() => FinishTime); } } }

		string _teamProject;
		public string TeamProject { get { return _teamProject; } private set { if (value != _teamProject) {_teamProject  = value; RaisePropertyChanged(() => TeamProject); } } }

		string _timeToBuild;
		public string TimeToBuild { get { return _timeToBuild; } private set { if (value != _timeToBuild) {_timeToBuild  = value; RaisePropertyChanged(() => TimeToBuild); } } }

		Uri _icon;
		public Uri Icon{ get { return _icon; } private set { if (value != _icon) {_icon  = value; RaisePropertyChanged(() => Icon); } } }

		Uri _userPath;
		public Uri UserPath { get { return _userPath; }  private set { if (value != _userPath) { _userPath = value; RaisePropertyChanged(() => UserPath); } } }

		protected BuildVm(Build b, Uri iconUri, Uri userProfilesUri, int rows)
		{
			this.Scale2d = new Scale2d(15);
			this.Rows = rows;
			this.Icon = iconUri;
			this.Name = b.BuildName;
			this.StartTime = b.StartTime.ToString("d.MMMM yyyy HH:mm:ss");
			this.RequestedBy = string.IsNullOrEmpty(b.RequestedFor) ? b.RequestedBy : b.RequestedFor;

		    if (!string.IsNullOrEmpty(this.RequestedBy))
		    {
		        string domain = "";
		        int domainPos = this.RequestedBy.IndexOf("\\");
		        if (domainPos > 0)
		        {
		            domain = this.RequestedBy.Substring(0, domainPos) + "/";
		            this.RequestedBy = this.RequestedBy.Substring(domainPos + 1);
		        }

		        if (userProfilesUri != null)
		        {
		            this.UserPath = new Uri(userProfilesUri.AbsoluteUri + "/" + domain + this.RequestedBy + ".jpg");
		        }
		    }

		    this.TeamProject = b.TeamProject;
			TimeSpan duration;
			string durationText;
			if (b.FinishTime.HasValue)
			{
				this.FinishTime = b.FinishTime.Value.ToString("d.MMMM yyyy HH:mm:ss");
				duration = b.FinishTime.Value.Subtract(b.StartTime);
				durationText = "Duration: ";
			}
			else
			{
				duration = DateTime.Now.Subtract(b.StartTime);
				durationText = "Elapsed: ";
			}

			this.TimeToBuild = " (" + durationText + duration.Minutes.ToString() + " minutes " + duration.Seconds.ToString()  + " seconds)";
		}

	    private BitmapSource _userImage;
		private UserImageLoaderItem _userImageItem;
	
		public BitmapSource UserImage
		{
			get
			{
				if (_userImageItem != null)
				{
					return _userImage;
				}

				this.IsLoading = true;
				_userImage = Properties.Resources.userIcon.ToBitmapSource();
				_userImageItem = new UserImageLoaderItem(this.UserPath, OnUserImageLoaded, OnUserImageLoading);
				if (this.UserPath != null)
				{
					UserImageLoader.Instance.AddRequest(_userImageItem);
				}

				return _userImageItem.Image;
			}
		}

		private void OnUserImageLoading()
		{
			//this.IsLoading = true;
		}

		private void OnUserImageLoaded(BitmapSource image)
		{
			this.IsLoading = false;
			if (image != null)
			{
				_userImage = image;
				RaisePropertyChanged(() => UserImage);
			}
		}


		public void Update(BuildVm other)
		{
			this.Rows = other.Rows;
			this.Name = other.Name;
			this.StartTime = other.StartTime;
			this.RequestedBy = other.RequestedBy;
			this.FinishTime = other.FinishTime;
			this.TeamProject = other.TeamProject;
			this.TimeToBuild = other.TimeToBuild;
			this.Icon = other.Icon;
			this.UserPath = other.UserPath;
		}

		public void SizeChanged(double width, double height)
		{
			this.Scale2d.SetSize(width, height);
			RaisePropertyChanged(() => Scale2d);
		}

		#region IComparable<Build> Members

		public int CompareTo(BuildVm other)
		{
			if (this is FailedBuildVm && this.GetType() != other.GetType())
			{
				return 1;
			}
			if (this is PartiallySucceededBuildVm && this.GetType() != other.GetType())
			{
				if (other is FailedBuildVm)
				{
					return -1;
				}
				return 1;
			}
			if (this is InProgressBuildVm && this.GetType() != other.GetType())
			{
				if (other is FailedBuildVm)
				{
					return -1;
				}
				if (other is PartiallySucceededBuildVm)
				{
					return -1;
				}
				return 1;
			}
			return this.StartTime.CompareTo(other.StartTime);
		}

		#endregion // IComparable<Build> Members

		public void Dispose()
		{
			if (_userImageItem != null)
			{
				lock (_userImageItem)
				{
					_userImageItem.IsDisposed = true;
				}
			}
		}
	}

	public interface IBuildMonitorView
	{
	}

	public class BuildMonitorVm : SlideElementVm
	{
		#region Fields

		IBuildMonitorView _view;
		BuildMonitor _model;
		string _infoText;
		Scale2d _scale2d = new Scale2d();
		int _rows = 0;
		int _columns = 0;
		bool _enablePolling;
		BuildQuery _pollingQuery;
		private readonly bool _autoUnload;
		private bool _isLoaded = false;

		#endregion // Fields

		#region Properties

		public int Columns
		{
			get { return _columns; }
			set
			{
				if (value != _columns)
				{
					_columns = value;
					RaisePropertyChanged(() => Columns);
				}
			}
		}

		public int Rows
		{
			get { return _rows; }
			set
			{
				if (value != _rows)
				{
					_rows = value;
					RaisePropertyChanged(() => Rows);
				}
			}
		}

		public IBuildMonitorView View 
		{
			get 
			{ 
				return _view; 
			}
			set
			{
				_view = value;
				if (_view != null)
				{
					Load();
				}
				else
				{
					if (_autoUnload)
					{
						Unload();
					}
				}
			}
		}

		public string InfoText
		{
			get { return _infoText; }
			set
			{
				_infoText = value;
				RaisePropertyChanged(() => InfoText);
				RaisePropertyChanged(() => ShowInfoText);
			}
		}

		public bool ShowInfoText
		{
			get { return (!string.IsNullOrEmpty(_infoText)); }
		}

		public ObservableCollection<BuildVm> Builds { get; private set; }

		string _title;
		public string Title
		{
			get
			{
				return _title;
			}
			set
			{
				if (value != _title)
				{
					_title = value;
					RaisePropertyChanged(() => Title);
				}
			}
		}

		public Scale2d Scale2d { get { return _scale2d; } }

		#endregion // Properties

		#region Constructors

		public BuildMonitorVm(BuildMonitor model, bool enablePolling, bool autoUnload = true)
		{
			_autoUnload = autoUnload;
			_model = model;
			_enablePolling = enablePolling;
			this.Title = model.Title;
			this.Builds = new ObservableCollection<BuildVm>();
		}

		#endregion // Constructors

		#region Methods

		private void Load()
		{
			if (!_isLoaded)
			{
				Debug.WriteLine("Loading BuildMonitorVm");

				_isLoaded = true;
				_pollingQuery = new BuildQuery(_model.Id, _enablePolling ? _model.UpdateInterval : (int?) null, _model.BuildFilter,
											   OnBuildQueryCompleted);
				_pollingQuery.Query(false);
				ViewModelEvents.Instance.SlideElementUpdated.Subscribe(OnSlideElementUpdated, ThreadOption.UIThread, false, s => s.Id == _model.Id);
			}
		}

		public override void Refresh()
		{
			_pollingQuery.Query(true);
		}

		private void OnSlideElementUpdated(SlideElement model)
		{
			if (_view == null || !_isLoaded)
			{
				return;
			}

			var se = model as BuildMonitor;
			if (se == null)
			{
				return;
			}

			this.Title = se.Title;
			this.Rows = se.Rows;
			this.Columns = se.Columns;
		}

		private void OnBuildQueryCompleted(BuildQuery qry)
		{
			Debug.WriteLine("OnBuildQueryCompleted()");

			if (_view == null || !_isLoaded)
			{
				return;
			}

			if (qry.Exception == null)
			{
				UpdateBuilds(qry.Results);
			}
			else
			{
				InfoText = "Requesting TFS data failed: '" + qry.Exception.Message + "'";
			}
		}

		Scale2d _panelScale2d = new Scale2d(15);

		private void UpdateBuilds(BuildCollection builds)
		{
			var appSettings = new TfsMonitorSettings();
			appSettings.Load(Settings.Default);


			//var removedBuilds = this.Builds.Select(b2 => b2.Build).Except<Build>(builds, new LambdaComparer<Build>((x, y) => x.BuildName != y.BuildName));
			//Debug.WriteLine("Removed builds: " + removedBuilds.Count());


			InfoText = null;
			this.Builds.ToList().ForEach(b => b.Dispose());
			this.Builds.Clear();

			this.Rows = _model.Rows;
			this.Columns = _model.Columns;


			// Removed builds
			/*foreach (var b in removedBuilds)
			{
				this.Builds.Remove(this.Builds.FirstOrDefault(b2 => b2.Build.Equals(b)));
			}*/


			// New / changed builds
			//var newOrChangedBuilds = builds.Except<Build>(this.Builds.Select(b2 => b2.Build), new LambdaComparer<Build>((x, y) => x.Equals(y)));
			//Debug.WriteLine("Number of changed builds: " + newOrChangedBuilds.Count().ToString());

			foreach (var bd in builds)
			{
				switch (bd.Status)
				{
					case BuildStatus.Failed:
						this.Builds.Add(new FailedBuildVm(bd, appSettings.UserProfilesUri, this.Rows));
						break;
					case BuildStatus.PartiallySucceeded:
						this.Builds.Add(new PartiallySucceededBuildVm(bd, appSettings.UserProfilesUri, this.Rows));
						break;
					case BuildStatus.InProgress:
						this.Builds.Add(new InProgressBuildVm(bd, appSettings.UserProfilesUri, this.Rows));
						break;
					case BuildStatus.Succeeded:
						this.Builds.Add(new SucceededBuildVm(bd, appSettings.UserProfilesUri, this.Rows));
						break;
				}
			}
		}

		private void Add(BuildVm build)
		{
			// Update existing?
			var existing = this.Builds.FirstOrDefault(b => b.Name == build.Name);
			if (existing != null)
			{
				// Refactor later, this is only needed because with unbinded background background will not change
				//existing.Update(build);
				this.Builds.Remove(existing);

			}
			// Add sorted
			//else
			{
				int insertPos = 0;
				foreach (var current in this.Builds)
				{
					if (current.CompareTo(build) <= 0)
					{
						this.Builds.Insert(insertPos, build);
						return;
					}
					insertPos++;
				}
				this.Builds.Insert(insertPos, build);
			}
		}

		public void SizeChanged(double width, double height)
		{
			_scale2d.SetSize(width, height);
			_panelScale2d.SetSize(width, height / this.Rows);
			RaisePropertyChanged(() => Scale2d);
		}

		public override void Unload()
		{
			Debug.WriteLine("BuildMonitorVm.Unload()");


			this.Builds.ToList().ForEach(b => b.Dispose());
			this.Builds.Clear();


			if (!_isLoaded)
			{
				return;
			}

			if (_pollingQuery != null)
			{
				ViewModelEvents.Instance.BuildQueryCompleted.Unsubscribe(OnBuildQueryCompleted);
				_pollingQuery.Remove();
				_pollingQuery = null;
			}
			ViewModelEvents.Instance.SlideElementUpdated.Unsubscribe(OnSlideElementUpdated);

			if (_view != null)
			{
				_view = null;
			}

			_isLoaded = false;
		}

		#endregion // Methods
	}

	public class UserImageLoaderItem
	{
		private CompositePresentationEvent<UserImageLoaderItem> _loadingEvent;

		public Uri Uri { get; private set; }
		public Action<BitmapSource> ResultHandler { get; private set; }
		public Action LoadingHandler { get; private set; }
		public BitmapSource Image { get; set; }

		public bool IsDisposed { get; set; }

		public UserImageLoaderItem(Uri uri, Action<BitmapSource> resultHandler, Action loadingHandler)
		{
			Uri = uri;
			ResultHandler = resultHandler;
			LoadingHandler = loadingHandler;
			_loadingEvent = new CompositePresentationEvent<UserImageLoaderItem>();
			_loadingEvent.Subscribe(OnLoading, ThreadOption.UIThread);
		}

		public void Load()
		{
			// No. Download
			try
			{
				Debug.WriteLine("Loading '" + this.Uri + "'");
				_loadingEvent.Publish(this);
				using (var wc = new WebClient())
				{
					var result = wc.DownloadData(this.Uri);
					if (result != null)
					{
						using (var stream = new MemoryStream(result))
						{
							this.Image = new Bitmap(stream).ToBitmapSource();
						}
					}
				}
			}
			catch (System.Net.WebException ex)
			{
				// ToDo: We should log that we cannot download image from Uri
				// ...
			}

		}

		private void OnLoading(UserImageLoaderItem obj)
		{
			this.LoadingHandler();
		}
	}

	public class UserImageLoader 
	{
		private static readonly UserImageLoader _instance = new UserImageLoader();
		private TFS.Monitor.Common.Cache<Uri, BitmapSource> _cache = new Cache<Uri, BitmapSource>();
		private bool _running = false;
		private Queue<UserImageLoaderItem> _que = new Queue<UserImageLoaderItem>();
		private CompositePresentationEvent<UserImageLoaderItem> _loadCompleteEvent;

		public static UserImageLoader Instance { get { return _instance; }}

		private UserImageLoader()
		{
			_loadCompleteEvent = new CompositePresentationEvent<UserImageLoaderItem>();
			_loadCompleteEvent.Subscribe(OnLoadComplete, ThreadOption.UIThread);
		}

		public void AddRequest(UserImageLoaderItem item)
		{
			lock (this)
			{
				CacheItem<BitmapSource> cached;
				if (_cache.TryGetValue(item.Uri, out cached))
				{
					item.ResultHandler(cached.Value);
					return;
				}

				if (!_running)
				{
					_running = true;
					ThreadPool.QueueUserWorkItem(Load, this);
				}

				_que.Enqueue(item);
			}
		}

		private void Load(object obj)
		{
			Debug.WriteLine("Thread starting.");

			UserImageLoaderItem item;
			while (true)
			{
				bool inCache = false;
				lock (this)
				{
					if (_que.Count == 0)
					{
						_running = false;
						break;
					}

					// Get undisposed item from queue
					item = _que.Dequeue();
					if (item.IsDisposed)
					{
						continue;
					}

					// In cache?
					CacheItem<BitmapSource> cached;
					if (_cache.TryGetValue(item.Uri, out cached))
					{
						item.Image = cached.Value;
						inCache = true;
					}
				}

				if (!inCache)
				{
					item.Load();
				}

				lock (this)
				{
					_cache.Set(item.Uri, item.Image);
				}
				_loadCompleteEvent.Publish(item);
			}

			Debug.WriteLine("Thread finished.");

		}

		private void OnLoadComplete(UserImageLoaderItem item)
		{
			item.ResultHandler(item.Image);
		}

	}
}
