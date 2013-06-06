using System.Diagnostics.Contracts;

namespace Osiris.Tfs.Monitor.Models
{
	#region Using

	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Xml.Serialization;
	using System.IO;
	using Osiris.TFS.Monitor.Common;

	#endregion // Using

	/// <summary>
	/// Document containing slides (if any)
	/// </summary>
	[Serializable()]
	[XmlInclude(typeof(Slide))]
	public class TfsMonitorDocument
	{
		#region Fields

		string _fileName;

		#endregion // Fields

		#region Properties

		public string FileName { get { return _fileName; } }

		public List<Slide> Slides { get; set; }

		public bool HasFileName { get { return !string.IsNullOrEmpty(_fileName); } }

		#endregion // Properties

		#region Constructors

		//[Obsolete("Used only by serialization.")]
		public TfsMonitorDocument()
		{
			this.Slides = new List<Slide>();
		}

		#endregion // Constructors

		#region Methods

		public void Save(ISettings settings)
		{
			if (_fileName == null)
			{
				throw new Exception("Filename not specified.");
			}

			SaveAs(settings, _fileName);
		}

		public void SaveAs(ISettings settings, string fileName)
		{
			FileStream fs = new FileStream(fileName, FileMode.Create);
			SaveAs(settings, fs, fileName);
		}

		public void SaveAs(ISettings settings, Stream stream, string fileName)
		{
			Contract.Requires(stream != null);

			_fileName = fileName;
			using (var sw = new StreamWriter(stream))
			{
				sw.Write(XmlSerialize());
				TfsMonitorSettings.AddRecentFile(settings, fileName);
			}
		}

		public static TfsMonitorDocument Open(ISettings settings, string fileName)
		{
			FileStream fs = new FileStream(fileName, FileMode.Open);
			using (StreamReader sr = new StreamReader(fs))
			{
				string xml = sr.ReadToEnd();
				var doc = XmlDeserialize(xml);
				doc._fileName = fileName;
				TfsMonitorSettings.AddRecentFile(settings, fileName);
				return doc;
			}
		}

		/// <summary>
		/// Serialize to XML
		/// </summary>
		/// <returns></returns>
		private string XmlSerialize()
		{
			StringBuilder builder = new StringBuilder();
			XmlSerializer ser = new XmlSerializer(typeof(TfsMonitorDocument));
			using (StringWriter stringWriter = new StringWriter(builder))
			{
				ser.Serialize(stringWriter, this as TfsMonitorDocument);
			}
			return builder.ToString();
		}

		/// <summary>
		/// Deserialize from XML
		/// </summary>
		/// <param name="xml"></param>
		/// <returns></returns>
		private static TfsMonitorDocument XmlDeserialize(string xml)
		{
			Contract.Requires(xml != null);

			var ser = new XmlSerializer(typeof(TfsMonitorDocument));
			using (var reader = new StringReader(xml))
			{
				return ser.Deserialize(reader) as TfsMonitorDocument;
			}
		}

		#endregion // Methods
	}
}
