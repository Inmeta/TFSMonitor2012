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
	using Microsoft.TeamFoundation.WorkItemTracking.Client;
	using System.Windows.Threading;
	using System.ComponentModel;
	using System.Collections.ObjectModel;
	using System.Windows;

	#endregion // Using

	public class TfsRibbonGroupVm : ViewModelBase
	{
		#region Fields

		IEnumerable<SlideElement> _elements;

		#endregion // Fields

		#region Properties

		public List<Node> TreeNodes { get; set; }

		public ObservableNodeList CheckNodes { get; set; }

		#endregion // Properties

		#region Constructors

		public TfsRibbonGroupVm()
		{
			ViewModelEvents.Instance.SlideElementSelected.Subscribe(OnSlideElementSelected);
			ViewModelEvents.Instance.SettingsUpdated.Subscribe(OnSettingsUpdated);

			var items = new List<Node>();
			items.Add(new Node("a"));
			items.Last().Add(new Node(items.Last(), "a.1"));
			items.Last().Add(new Node(items.Last(), "a.2"));
			items.Add(new Node("b"));
			items.Last().Add(new Node(items.Last(), "b.1"));
			items.Last().Add(new Node(items.Last(), "b.2"));
			this.TreeNodes = items;

			var nodes = new ObservableNodeList();
			nodes.Add(new ComboWithCheckBoxNode("test.1"));
			nodes.Add(new ComboWithCheckBoxNode("test.2"));
			this.CheckNodes = nodes;

		}

		#endregion // Constructors

		#region Methods

		/// <summary>
		/// Called when zero or more SlideElements have been selected and we need
		/// to reflect this in UI.
		/// </summary>
		/// <param name="elements"></param>
		private void OnSlideElementSelected(IEnumerable<SlideElement> elements)
		{
			_elements = elements;
			Invalidate();
		}

		void Invalidate()
		{
			// ...
		}

		private void OnSettingsUpdated(TfsMonitorSettings settings)
		{
			Invalidate();
		}

		#endregion // Methods
	}

	public class Node : ComboBoxTreeNode
	{
		#region Fields

		Node _parent;
		List<ComboBoxTreeNode> _children;

		#endregion // Fields

		#region Properties

		public override IEnumerable<ComboBoxTreeNode> Children { get { return _children; } }

		public override string Name { get; set; }

		public override string FullPath
		{
			get
			{
				if (_parent == null)
				{
					return this.Name;
				}
				return _parent.Concat(this.Name);
			}
		}

		#endregion // Properties

		#region Constructors

		public Node(string name) : this(null, name) { }

		public Node(Node parent, string name)
		{
			_parent = parent;
			this.Name = name;
			_children = new List<ComboBoxTreeNode>();
		}

		#endregion // Constructors

		#region Methods

		public void Add(Node node)
		{
			_children.Add(node);
		}

		private string Concat(string path)
		{
			if (_parent == null)
			{
				return this.Name + "/" + path;
			}
			return _parent.Concat(this.Name + "/" + path);
		}

		#endregion // Methods
	}

	public class ObservableNodeList : ObservableCollection<ComboWithCheckBoxNode>
	{

		public ObservableNodeList()
		{

		}

		public override string ToString()
		{

			StringBuilder outString = new StringBuilder();

			foreach (ComboWithCheckBoxNode s in this.Items)
			{

				if (s.IsSelected == true)
				{

					outString.Append(s.Title);

					outString.Append(',');

				}

			}

			return outString.ToString().TrimEnd(new char[] { ',' });

		}

	}

	public class ComboWithCheckBoxNode
	{

		public ComboWithCheckBoxNode(string n) { Title = n; }

		public string Title { get; set; }

		public bool IsSelected { get; set; }

	}


}
