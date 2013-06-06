using System.Collections;
using System.Diagnostics.Contracts;

namespace Osiris.Tfs.Monitor.Models
{
	#region Using

	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
using System.Xml.Serialization;
	using Osiris.Tfs.Report;

	#endregion // Using

	public class SlideElementCollection : IEnumerable<SlideElement> // List<SlideElement>
	{
		private readonly List<SlideElement> _elements = new List<SlideElement>();

		public SlideElementCollection(SlideElement element)
		{
			_elements.Add(element);
		}

		public SlideElementCollection(IEnumerable<SlideElement> elements)
		{
			Contract.Requires(elements != null);

			_elements.AddRange(elements);
		}

		public TfsWorkItemFilter TfsWorkItemFilter()
		{
			TfsWorkItemFilter filter = null;
			foreach (var elem in this)
			{
				if (elem.TfsWorkItemFilter == null)
				{
					return null;
				}
				if (filter == null)
				{
					filter = new TfsWorkItemFilter(elem.TfsWorkItemFilter);
				}
				else
				{
					// To do: Compare
					return null;
				}
			}
			return filter;
		}
		
		public IEnumerator<SlideElement> GetEnumerator()
		{
			return _elements.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

	}
}
