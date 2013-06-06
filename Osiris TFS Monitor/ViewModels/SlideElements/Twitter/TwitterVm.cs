using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Threading;
using System.Windows.Media.Imaging;

namespace Osiris.Tfs.Monitor
{



	#region Using

	using System;
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
	using System.Windows;
	using Osiris.TFS.Monitor.Common;
	using Microsoft.TeamFoundation.Build.Client;
	using Microsoft.Practices.Composite.Presentation.Events;

	#endregion // Using


	public interface ITwitterView
	{
	}

	public class TwitterVm : SlideElementVm
	{
		#region Fields

		ITwitterView _view;
		Twitter _model;
		string _infoText;
		Scale2d _scale2d = new Scale2d();
		int _rows = 0;
		int _columns = 0;
		bool _enablePolling;
		TwitterQuery _pollingQuery;
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

		public ITwitterView View 
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

		public ObservableCollection<TweetVm> Tweets { get; private set; }

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

		public TwitterVm(Twitter model, bool enablePolling, bool autoUnload = true)
		{
			_autoUnload = autoUnload;
			_model = model;
			_enablePolling = enablePolling;
			this.Title = model.Title;
			this.Tweets = new ObservableCollection<TweetVm>();
		}

		#endregion // Constructors

		#region Methods

		private void Load()
		{
			if (!_isLoaded)
			{
				Debug.WriteLine("Loading TwitterVm");

				_isLoaded = true;
				_pollingQuery = new TwitterQuery(_model.Id, _enablePolling ? _model.UpdateInterval : (int?) null, _model, OnTwitterQueryCompleted);
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

			var se = model as Twitter;
			if (se == null)
			{
				return;
			}

			this.Title = se.Title;
			this.Rows = se.Rows;
			this.Columns = se.Columns;
		}

		private void OnTwitterQueryCompleted(TwitterQuery qry)
		{
			//Debug.WriteLine("OnBuildQueryCompleted()");

			if (_view == null || !_isLoaded)
			{
				return;
			}

			if (qry.Exception == null)
			{
				UpdateTweets(qry.Results);
			}
			else
			{
				InfoText = "Requesting TFS data failed: '" + qry.Exception.Message + "'";
			}
		}

		private void UpdateTweets(IEnumerable<Tweet> results)
		{
			this.Tweets.Clear();
			results.ToList().ForEach(t => this.Tweets.Add(new TweetVm(t)));

			this.Rows = _model.Rows;
			this.Columns = _model.Columns;

			Debug.WriteLine("Number of tweets in Tweets: {0}", this.Tweets.Count());

		}

		Scale2d _panelScale2d = new Scale2d(15);

		public void SizeChanged(double width, double height)
		{
			_scale2d.SetSize(width, height);
			_panelScale2d.SetSize(width, height / this.Rows);
			RaisePropertyChanged(() => Scale2d);
		}

		public override void Unload()
		{
			//this.Builds.ToList().ForEach(b => b.Dispose());
			//this.Builds.Clear();


			if (!_isLoaded)
			{
				return;
			}

			if (_pollingQuery != null)
			{
				ViewModelEvents.Instance.TwitterQueryCompleted.Unsubscribe(OnTwitterQueryCompleted);
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


	public class TweetVm : ViewModelBase
	{
		private readonly Tweet _tweet;
		Scale2d _scale2d;
		public Scale2d Scale2d { get { return _scale2d; } private set { if (value != _scale2d) { _scale2d = value; RaisePropertyChanged(() => Scale2d); } } }

		public string Text { get { return _tweet.Text; }}
		public string From { get { return _tweet.From; } }
		public string Month { get { return _tweet.Date.ToString("MMMM"); } }
		public int Day { get { return _tweet.Date.Day; } }

		public TweetVm(Tweet tweet)
		{
			_tweet = tweet;
			this.Scale2d = new Scale2d(15);
		}

		public void SizeChanged(double width, double height)
		{
			this.Scale2d.SetSize(width, height);
			RaisePropertyChanged(() => Scale2d);
		}

	}
}
