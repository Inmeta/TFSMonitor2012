namespace Osiris.Tfs.Monitor
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
	using System.Diagnostics;

	#endregion // Using

	public partial class DocumentExplorerView : UserControl, IDocumentExplorerView
	{
	    DocumentExplorerVm _vm;

	    public DocumentExplorerView()
		{
			InitializeComponent();
			this.DataContext = new DocumentExplorerVm(this);
		}

	    private void UserControl_Loaded(object sender, RoutedEventArgs e)
		{
		}

		private void Grid_SizeChanged(object sender, SizeChangedEventArgs e)
		{
			AdjustItemsSizes(e.NewSize.Width);
		}

		public void SlideInserted(SlideThumbnailVm vm)
		{
			AdjustItemsSizes(_thumbGrid.ActualWidth);
		}

		private void AdjustItemsSizes(double width)
		{
			ScrollViewer sv = _listView.FindVisualChild<ScrollViewer>();
			var w = Math.Max(0, width - 8);
			var totalHeight = _listView.Items.Count * w;

			//Debug.WriteLine("gridheight: " + _thumbGrid.ActualHeight + ", itemsheight: " + totalHeight);

			if (totalHeight >= _thumbGrid.ActualHeight)
			{
				// Adjust for scrollbar...
				//sv.VerticalScrollBarVisibility = ScrollBarVisibility.Visible;
				//w = sv.ViewportWidth - 8;
				//w -= _listView.Items.Count * 
			}
			else
			{
				//sv.VerticalScrollBarVisibility = ScrollBarVisibility.Disabled;
			}

			foreach (var item in _listView.Items)
			{
				var thumbVm = item as SlideThumbnailVm;

				if (thumbVm.View != null)
				{
					var obj = _listView.ContainerFromElement(thumbVm.View as DependencyObject);
					if (obj != null)
					{
						ListViewItem lvi = obj as ListViewItem;
						lvi.HorizontalAlignment = HorizontalAlignment.Left;
						lvi.Width = w + 5;
						lvi.Height = w + 5;
					}
				}

				thumbVm.Width = w;
				thumbVm.Height = w;
			}
		}
	}

}
