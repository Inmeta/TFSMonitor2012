using System.Diagnostics.Contracts;
using Twitterizer;

namespace Osiris.Tfs.Monitor.Models
{
	#region Using

	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
using System.Windows.Threading;
	using System.Windows.Forms;
	using System.Diagnostics;
	using System.Xml.Serialization;

	#endregion // Using

	public class Twitter : SlideElement, IEquatable<Twitter>
	{
		#region Fields

		int _rows;
		int _columns;
		List<Tweet> _tweets = new List<Tweet>();

		#endregion // Fields
	
		#region Properties

		public string Title { get; set; }

		[XmlIgnore]
		public int MaxTweets { get { return this.Columns * this.Rows; } }

		[XmlIgnore]
		public IEnumerable<Tweet> Tweets { get { return _tweets; } }

		public int Rows
		{
			get { return _rows; }
			set
			{
				_rows = value;
			}
		}

		public int Columns 
		{
			get { return _columns; }
			set
			{
				_columns = value;
			}
		}

		public string Query { get; set; }
		
		#endregion // Properties

		#region Constructors

		public Twitter()
		{
			// Default values
			this.Title = "Untitled";
			this.Rows = 10;
			this.Columns = 1;
			this.UpdateInterval = 60 * 5; // Five minutes;
		}

		#endregion // Constructors

		public bool Equals(Twitter other)
		{
			return false;
		}

		/// <summary>
		/// Note: Active record pattern... Move to service later.
		/// </summary>
		public void LoadTweets()
		{
			_tweets = new List<Tweet>();
			//var query = "#Inmeta";
			

			var options = new SearchOptions()
			{
				PageNumber = 1,
				NumberPerPage = this.MaxTweets
			};

			var searchResult = TwitterSearch.Search(this.Query, options);

			if (searchResult.Result == RequestResult.Success)
			{
				foreach (var tweet in searchResult.ResponseObject)
				{
					if (!_tweets.Any(t => t.Equals(tweet)))
					{
						_tweets.Add(new Tweet(tweet));
					}
				}
			}


		}
	}

	public class Tweet : IEquatable<TwitterSearchResult>
	{
		private readonly TwitterSearchResult _twitterSearchResult;

		public string From { get { return _twitterSearchResult.FromUserScreenName; }  }
		public string Text { get { return _twitterSearchResult.Text; } }
		public DateTime Date { get { return _twitterSearchResult.CreatedDate; } }

		internal Tweet(TwitterSearchResult twitterSearchResult)
		{
			_twitterSearchResult = twitterSearchResult;
		}

		public bool Equals(TwitterSearchResult other)
		{
			return this.From == other.FromUserScreenName && this.Text == other.Text;
		}
	}
}
