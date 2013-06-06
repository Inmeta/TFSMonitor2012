namespace Osiris.Tfs.Monitor.Models
{
	#region Using

	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;

	#endregion // Using

	public abstract class SlideType
	{
		#region Properties

		/// <summary>
		/// Name of slide type
		/// </summary>
		public string Name { get; private set; }

		/// <summary>
		/// Description of slide type
		/// </summary>
		public string Description { get; private set; }

		#endregion // Properties

		#region Constrcutors

		public SlideType(string name, string description)
		{
			this.Name = name;
			this.Description = description;
		}

		#endregion // Constrcutors

		#region Methods

		/// <summary>
		/// Enumerates all slide types
		/// </summary>
		/// <returns></returns>
		public static IEnumerable<SlideType> EnumTypes()
		{
			List<SlideType> slideTypes = new List<SlideType>();
			slideTypes.Add(new BurndownSlideType());
			return slideTypes;
		}

		public abstract Slide CreateSlide(string name);

		#endregion // Methods
	}
}
