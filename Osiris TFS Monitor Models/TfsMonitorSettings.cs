using System.Diagnostics.Contracts;

namespace Osiris.Tfs.Monitor.Models
{
	#region Using

	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Diagnostics;
	using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Windows.Forms;
	using System.Security.Cryptography;

	#endregion // Using
	
	public class TfsMonitorSettings : INotifyPropertyChanged
	{
		#region Fields

		private const int MaxRecentFiles = 10;

		public event PropertyChangedEventHandler PropertyChanged;
		
		string _tfsUri;
		string _tfsDomain;
		string _tfsUser;
		string _tfsPassword;
		bool _tfsUseLocalAccount;
		const string PasswordKey = "1234567890123456"; // 128 bit, 192 bit or 256 bit
		static bool _isPasswordInvalidShown = false;

		#endregion // Fields

		#region Properties

		public bool TfsUseLocalAccount
		{
			get { return _tfsUseLocalAccount; }
			set
			{
				_tfsUseLocalAccount = value;
				RaisePropertyChanged("TfsUseLocalAccount");
				UpdateTfsServiceConnection();
			}
		}

		public string TfsUri 
		{
			get 
			{ 
				return _tfsUri; 
			}
			set
			{
				_tfsUri = value;
				RaisePropertyChanged("TfsUri");
				UpdateTfsServiceConnection();
			}
		}

		public string TfsDomain 
		{
			get
			{
				return _tfsDomain;
			}
			set
			{
				_tfsDomain= value;
				RaisePropertyChanged("TfsDomain");
				UpdateTfsServiceConnection();
			}
		}

		public string TfsUser 
		{
			get
			{
				return _tfsUser;
			}
			set
			{
				_tfsUser = value;
				RaisePropertyChanged("TfsUser");
				UpdateTfsServiceConnection();
			}
		}

		public string TfsPassword 
		{
			get
			{
				return _tfsPassword;
			}
			set
			{
				_tfsPassword = value;
				RaisePropertyChanged("TfsPassword");
				UpdateTfsServiceConnection();
			}
		}

		public MonitorCollection SlideshowMonitors { get; set; }

		public bool SlideshowTurnOffScreenSaver { get; set; }

		public Uri UserProfilesUri { get; set; }

		public List<string> RecentFiles { get; set; }

		#endregion // Properties

		public TfsMonitorSettings()
		{
			this.SlideshowMonitors = new MonitorCollection();
		}

		#region Methods

		public void Load(ISettings settings)
		{
			// Defaults
			if (string.IsNullOrEmpty(settings.TfsUseLocalAccount))
			{
				settings.TfsUseLocalAccount = true.ToString();
			}
			if (string.IsNullOrEmpty(settings.SlideshowTurnOffScreenSaver))
			{
				settings.SlideshowTurnOffScreenSaver = false.ToString();
			}

			this.TfsUri = settings.TfsUri;
			this.TfsDomain = settings.TfsDomain;
			this.TfsUser = settings.TfsUser; 
			this.TfsPassword = string.IsNullOrEmpty(settings.TfsPassword) ? null : DecryptPassword(settings.TfsPassword);
			this.TfsUseLocalAccount = bool.Parse(settings.TfsUseLocalAccount);


			if (!string.IsNullOrEmpty(settings.SlideshowMonitors))
			{
				try
				{
					this.SlideshowMonitors = new MonitorCollection(settings.SlideshowMonitors.Split(';').Select(m => new Monitor(int.Parse(m))));
				}
				catch (Exception)
				{
					MessageBox.Show(null, "The settings for monitors provided in the configuration file is in invalid format.\r\n" +
						"The settings for monitors has been reset.", "Osiris TFS Monitor", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				}
			}
			this.SlideshowTurnOffScreenSaver = bool.Parse(settings.SlideshowTurnOffScreenSaver);
	
			
			this.UserProfilesUri = string.IsNullOrEmpty(settings.UserProfilesUri) ? null : new Uri(settings.UserProfilesUri);

			// Parse recent files ("<file1>;<file2>")
			this.RecentFiles = new List<string>(settings.RecentFiles.Split(';').Where(s => !string.IsNullOrEmpty(s)).Take(MaxRecentFiles));
			UpdateTfsServiceConnection();
		}


		private string EncryptPassword(string passwd)
		{
			if (passwd == null)
			{
				return null;
			}

			byte[] keyArray;
			byte[] toEncryptArray = Encoding.UTF8.GetBytes(passwd);
			keyArray = Encoding.UTF8.GetBytes(PasswordKey);
			var tdes = new TripleDESCryptoServiceProvider();
			tdes.Key = keyArray;
			tdes.Mode = CipherMode.ECB;
			tdes.Padding = PaddingMode.PKCS7;
			ICryptoTransform cTransform = tdes.CreateEncryptor();
			byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
			tdes.Clear();
			return Convert.ToBase64String(resultArray, 0, resultArray.Length);
		}

		private string DecryptPassword(string encodedPasswd)
		{
			Contract.Requires(encodedPasswd != null);

			try
			{
				byte[] keyArray;
				byte[] toEncryptArray = Convert.FromBase64String(encodedPasswd);
				keyArray = UTF8Encoding.UTF8.GetBytes(PasswordKey);
				var tdes = new TripleDESCryptoServiceProvider();
				tdes.Key = keyArray;
				tdes.Mode = CipherMode.ECB;
				tdes.Padding = PaddingMode.PKCS7;
				ICryptoTransform cTransform = tdes.CreateDecryptor();
				byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
				tdes.Clear();
				return (resultArray == null) ? "" : Encoding.UTF8.GetString(resultArray);
			}
			catch (Exception)
			{
				if (!_isPasswordInvalidShown)
				{
					_isPasswordInvalidShown = true;
					MessageBox.Show(null, "The password provided in the configuration file is invalid.\r\n" +
						"The passord has been reset to blank." +
						"\r\n\r\nTo change the password, open application settings, re-enter the password and save settings.", "Osiris TFS Monitor", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				}
				return "";		
			}
		}

		/// <summary>
		/// When connection settings change, update tfs-service.
		/// </summary>
		private void UpdateTfsServiceConnection()
		{
			if (this.TfsUseLocalAccount)
			{
				TfsService.Instance.SetConnectionInfo(this.TfsUri);
			}
			else
			{
				TfsService.Instance.SetConnectionInfo(this.TfsUri, this.TfsDomain, this.TfsUser, this.TfsPassword);
			}
		}

		public void Save(ISettings settings)
		{
			settings.TfsUri = this.TfsUri;
			settings.TfsDomain = this.TfsDomain;
			settings.TfsUser = this.TfsUser;
			settings.TfsPassword = string.IsNullOrEmpty(this.TfsPassword) ? null : EncryptPassword(this.TfsPassword);
			settings.TfsUseLocalAccount = this.TfsUseLocalAccount.ToString();
			settings.SlideshowMonitors = this.SlideshowMonitors.Select(m => m.Number.ToString()).Aggregate("", (current, number) => current + (((current == "") ? "" : ";") + number));
			settings.SlideshowTurnOffScreenSaver = this.SlideshowTurnOffScreenSaver.ToString();
			settings.UserProfilesUri = this.UserProfilesUri == null ? null : this.UserProfilesUri.OriginalString;
			settings.RecentFiles = this.RecentFiles.Take(MaxRecentFiles).Aggregate("", (current, file) => current + (((current == "") ? "" : ";") + file));
			settings.Save();
			UpdateTfsServiceConnection();
		}

		/// <summary>
		/// Signal to framework that one of the properties has changed.
		/// </summary>
		/// <param name="propertyName"></param>
		public void RaisePropertyChanged(string propName)
		{
			if (this.PropertyChanged != null)
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
			}
		}

		private void AddRecentFile(string fileName)
		{
			Contract.Requires(this.RecentFiles != null);

			// Remove equals
			var equals = this.RecentFiles.Where(f => f == fileName).ToList();
			foreach (var equal in equals)
			{
				this.RecentFiles.Remove(equal);
			}
			
			// Move file to top of recent files
			this.RecentFiles.Insert(0, fileName);

			// Remove items if list over limit
			if (this.RecentFiles.Count > MaxRecentFiles)
			{
				this.RecentFiles.RemoveRange(10, this.RecentFiles.Count - MaxRecentFiles);
			}
		}

		public static void AddRecentFile(ISettings settings, string fileName)
		{
			var s = new TfsMonitorSettings();
			s.Load(settings);
			s.AddRecentFile(fileName);
			s.Save(settings);
		}
	
		#endregion // Methods
	}
}
