using System.Diagnostics.Contracts;

namespace Osiris.Tfs.Monitor.Models
{
	#region Using

	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Diagnostics;
using System.Xml.Serialization;
	using System.IO;
	using System.Windows.Media;
	using Osiris.TFS.Monitor.Common;

	#endregion // Using

	[Serializable()]
	public class Slide
	{
		#region Properties

		public string Name { get; set; }
		public int Duration { get; set; }
		public bool DesignMode { get; set; }
		public List<SlideElementContainer> SlideElements { get; set; }
		public List<Monitor> Monitors { get; set; }

		[XmlIgnore]
		public int Id { get; private set; }

		#endregion // Properties

		#region Constructors

		/// <summary>
		/// Default constructor. 
		/// </summary>
		public Slide() 
		{
			this.SlideElements = new List<SlideElementContainer>();
			this.Monitors = new List<Monitor>();

			// Default values
			this.Duration = 10;
			this.Name = "Untitled";
			this.Id = AutoIdGenerator.GenerateId();
		}

		#endregion // Constructors

		#region Methods

		/// <summary>
		/// Serialize to XML
		/// </summary>
		/// <returns></returns>
		public virtual string XmlSerialize()
		{
			StringBuilder builder = new StringBuilder();
			XmlSerializer ser = new XmlSerializer(typeof(Slide));
			using (StringWriter stringWriter = new StringWriter(builder))
			{
				ser.Serialize(stringWriter, this as Slide);
			}
			return builder.ToString();
		}

		/// <summary>
		/// Deserialize from XML
		/// </summary>
		/// <param name="xml"></param>
		/// <returns></returns>
		public static Slide XmlDeserialize(string xml)
		{
			Contract.Requires(xml != null);

			XmlSerializer ser = new XmlSerializer(typeof(Slide));
			using (StringReader reader = new StringReader(xml))
			{
				return ser.Deserialize(reader) as Slide;
			}
		}

		/// <summary>
		/// Finds next monitor rotating right
		/// </summary>
		public int GetNextMonitor(List<int> available, int monitorPos)
		{
			Contract.Requires(available != null);

			if (monitorPos >= available.Count())
			{
				monitorPos = 0;
			}
			int index = monitorPos;
	
			do
			{
				if (this.Monitors.Count == 0)
				{
					return index;
				}

				if (this.Monitors.Exists(m => m.Number == available[index]))
				{
					return available[index];
				}

				if (++index >= available.Count())
				{
					index = 0;
				}

			} while (index != monitorPos);

			return available[monitorPos];
		}

		#endregion // Methods
	}
}
