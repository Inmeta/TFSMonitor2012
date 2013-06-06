namespace Osiris.Tfs.Monitor
{
	#region Using

	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using Microsoft.Windows.Controls.Ribbon;
	using System.Windows.Controls;
	using System.Globalization;
	using System.Windows.Data;
	using System.Windows;
	using System.Collections;
	using System.Collections.Specialized;
	using System.Diagnostics;

	#endregion // Using

	/// <summary>
	/// Extensionwrapper on Microsfts RibbonComboBox which supports binding of ItemSourceEx,
	/// DisplayMemberPathEx and SelectedValueEx.
	/// 
	/// Microsofts version requires RibbonComboBoxItem as a child item, but this wrapper
	/// behaves like a normmal wpf ComboBox without the need for binding throught ComboBoxItem.
	/// </summary>
	public class RibbonComboBoxEx : Microsoft.Windows.Controls.Ribbon.RibbonComboBox
	{
		#region Fields

		public static readonly DependencyProperty ItemsSourceExProperty = DependencyProperty.Register(
		  "ItemsSourceEx",
		  typeof(IEnumerable),
		  typeof(RibbonComboBoxEx),
		  new PropertyMetadata(null, OnItemsSourceExChangedCallback));

		public static readonly DependencyProperty DisplayMemberPathExProperty = DependencyProperty.Register(
		  "DisplayMemberPathEx",
		  typeof(string),
		  typeof(RibbonComboBoxEx),
		  new PropertyMetadata(null));

		public static readonly DependencyProperty SelectedValueExProperty = DependencyProperty.Register(
		  "SelectedValueEx",
		  typeof(object),
		  typeof(RibbonComboBoxEx),
		  new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnSelectedValueExChangedCallback)
		);

		#endregion // Fields

		#region Properties

		public IEnumerable ItemsSourceEx
		{
			get { return (IEnumerable)this.GetValue(ItemsSourceExProperty); }
			set { this.SetValue(ItemsSourceExProperty, value); 	}
		}

		public string DisplayMemberPathEx
		{
			get { return (string)this.GetValue(DisplayMemberPathExProperty); }
			set { this.SetValue(DisplayMemberPathExProperty, value); }
		}

		public object SelectedValueEx
		{
			get { return (string)this.GetValue(SelectedValueExProperty); }
			set { this.SetValue(SelectedValueExProperty, value); }
		}

		#endregion Properties

		#region Constructors

		public RibbonComboBoxEx() : base()
		{
		}

		#endregion // Constructors

		#region Methods

		private static void OnSelectedValueExChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			RibbonComboBoxEx rcb = d as RibbonComboBoxEx;
			if (e.NewValue != null)
			{
				foreach (var item in rcb.Items)
				{
					var cbItem = item as RibbonComboBoxItem;
					if (cbItem.DataContext == e.NewValue)
					{
						rcb.SelectedValue = cbItem;
						break;
					}
				}
			}
		}

		private static void OnItemsSourceExChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			RibbonComboBoxEx rcb = d as RibbonComboBoxEx;
			rcb.Items.Clear();
			rcb.InsertItems(e.NewValue as IEnumerable);

			if (e.NewValue is INotifyCollectionChanged)
			{
				var notify = e.NewValue as INotifyCollectionChanged;
				notify.CollectionChanged += new NotifyCollectionChangedEventHandler(rcb.ItemSourceEx_CollectionChanged);
			}
		}

		private void InsertItems(IEnumerable items)
		{
			if (items == null)
			{
				return;
			}

			foreach (var item in items)
			{
				var b = new Binding(this.DisplayMemberPathEx);
				var cbItem = new RibbonComboBoxItem();
				cbItem.DataContext = item;
				cbItem.SetBinding(RibbonComboBoxItem.ContentProperty, b);
				this.Items.Add(cbItem);
			}
		}

		private void ItemSourceEx_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			if (e.Action == NotifyCollectionChangedAction.Reset)
			{
				this.Items.Clear();
			}
			else if (e.Action == NotifyCollectionChangedAction.Add)
			{
				InsertItems(e.NewItems);
			}
			else if (e.Action == NotifyCollectionChangedAction.Move || e.Action == NotifyCollectionChangedAction.Remove
				|| e.Action == NotifyCollectionChangedAction.Replace)
			{
				throw new NotSupportedException("Notify actions: Move, Remove and Replace is not supported in RibbonComboBoxEx.");
			}
			
		}

		protected override void OnSelectionChanged(SelectionChangedEventArgs e)
		{
			if (e.AddedItems == null || e.AddedItems.Count <= 0)
			{
				this.SelectedValueEx = null;
			}
			else
			{
				var cbItem = e.AddedItems[0] as RibbonComboBoxItem;
				this.SelectedValueEx = cbItem.DataContext;
			}

			base.OnSelectionChanged(e);
		}

		#endregion // Methods
	}
}
