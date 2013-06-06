using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics;

namespace TfsMonitor.Controls
{
	#region Using

	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Windows;
	using System.Windows.Controls;
	using System.Windows.Data;
	using System.Windows.Documents;
	using System.Windows.Input;
	using System.Windows.Media;
	using System.Windows.Media.Imaging;
	using System.Windows.Navigation;
	using System.Windows.Shapes;

	#endregion // Using

	public partial class MultiColumnListBox : UserControl
	{
		public IEnumerable ItemsSource
		{
			get { return (IEnumerable)GetValue(ItemsSourceProperty); }
			set { SetValue(ItemsSourceProperty, value); }
		}

		public static readonly DependencyProperty ItemsSourceProperty =
			DependencyProperty.Register("ItemsSource", typeof(IEnumerable), typeof(MultiColumnListBox), new UIPropertyMetadata(null, OnItemSourceChanged));

		public int Columns
		{
			get { return (int)GetValue(ColumnsProperty); }
			set { SetValue(ColumnsProperty, value); }
		}

		public static readonly DependencyProperty ColumnsProperty =
			DependencyProperty.Register("Columns", typeof(int), typeof(MultiColumnListBox), new UIPropertyMetadata(0, OnColumnsChanged));

		public int Rows
		{
			get { return (int)GetValue(RowsProperty); }
			set { SetValue(RowsProperty, value); }
		}

		public static readonly DependencyProperty RowsProperty =
			DependencyProperty.Register("Rows", typeof(int), typeof(MultiColumnListBox), new UIPropertyMetadata(10, OnRowsChanged));

		public double ItemHeight
		{
			get { return (double)GetValue(ItemHeightProperty); }
			set { SetValue(ItemHeightProperty, value); }
		}

		public static readonly DependencyProperty ItemHeightProperty =
			DependencyProperty.Register("ItemHeight", typeof(double), typeof(MultiColumnListBox), new UIPropertyMetadata(double.NaN));


		public MultiColumnListBox()
		{
			InitializeComponent();
		}

		private static void OnItemSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			var cb = d as MultiColumnListBox;
			if (cb._observableItemsSource != null)
			{
				cb._observableItemsSource.CollectionChanged -= cb.OnCollectionChanged;
			}

			cb.UpdateItemsSource();
			cb._observableItemsSource = cb.ItemsSource as INotifyCollectionChanged;
			if (cb._observableItemsSource != null)
			{
				cb._observableItemsSource.CollectionChanged += cb.OnCollectionChanged;
			}
		}

		private INotifyCollectionChanged _observableItemsSource;

		private void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			UpdateItemsSource();
		}

		List<ObservableCollection<object>> _columnSources = new List<ObservableCollection<object>>();

		private static void OnRowsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			var cb = d as MultiColumnListBox;
			cb.UpdateItemsSource();
			
		}

		private static void OnColumnsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			var cb = d as MultiColumnListBox;

			cb._columnSources.Clear();
			cb._mainGrid.Children.Clear();
			cb._mainGrid.ColumnDefinitions.Clear();

			for (int i = 0; i < (int)e.NewValue; i++)
			{
				var itemsSource = new ObservableCollection<object>();
				cb._columnSources.Add(itemsSource);
				cb._mainGrid.ColumnDefinitions.Add(new ColumnDefinition());
				var lb = new ListBox();
				VirtualizingStackPanel.SetIsVirtualizing(lb, true);
				VirtualizingStackPanel.SetVirtualizationMode(lb, VirtualizationMode.Recycling);
				ScrollViewer.SetCanContentScroll(lb, true);
				Grid.SetColumn(lb, i);
				lb.Padding = new Thickness(0);
				lb.BorderThickness = new Thickness(0);
				lb.Background = Brushes.Transparent;
				ScrollViewer.SetHorizontalScrollBarVisibility(lb, ScrollBarVisibility.Hidden);
				ScrollViewer.SetVerticalScrollBarVisibility(lb, ScrollBarVisibility.Hidden);
				cb._mainGrid.Children.Add(lb);
				lb.ItemsSource = itemsSource;
				
			}

			cb.UpdateItemsSource();

		}

		private void UpdateItemsSource()
		{
			if (this.Columns <= 0 || this.ItemsSource == null)
			{
				return;
			}

			int currCol = 0;
			_columnSources.ForEach(s => s.Clear());
			foreach (var item in this.ItemsSource)
			{
				_columnSources[currCol].Add(item);
				if (++currCol >= this.Columns)
				{
					currCol = 0;
				}
			}

			this.ItemHeight = (this.ActualHeight - 3) / this.Rows;
		}

		private void OnLoaded(object sender, RoutedEventArgs e)
		{
			if (_observableItemsSource == null)
			{
				_observableItemsSource = ItemsSource as INotifyCollectionChanged;
				if (_observableItemsSource != null)
				{
					_observableItemsSource.CollectionChanged += OnCollectionChanged;
				}
			}
		}

		private void OnUnloaded(object sender, RoutedEventArgs e)
		{
			if (_observableItemsSource != null)
			{
				_observableItemsSource.CollectionChanged -= OnCollectionChanged;
				_observableItemsSource = null;
				Debug.WriteLine("Disposing MultiColumnListBox");
			}
		}

	}
}
