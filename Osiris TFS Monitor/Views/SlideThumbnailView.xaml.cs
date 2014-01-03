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
	using System.Windows.Shapes;
	using System.Collections.ObjectModel;
	using System.Diagnostics;
	using System.Collections.Specialized;
using Osiris.Tfs.Monitor.Models;
	using System.IO;

	#endregion // Using

	public partial class SlideThumbnailView : UserControl, ISlideThumbnailView
	{
		#region Fields

		bool _hasContentView = false;

		#endregion // Fields

		#region Properties

		private SlideThumbnailVm Vm { get { return this.DataContext as SlideThumbnailVm; } }

		#endregion // Properties

		#region Constructors

		public SlideThumbnailView()
		{
			InitializeComponent();
		}

		#endregion // Constructors

		#region Methods

		protected virtual void UserControl_Loaded(object sender, RoutedEventArgs e)
		{
			this.Vm.View = this;
			_thumb.DataContext = GetView();
		}

		public void SetView(Visual elem)
		{
			return;

		}

		private object GetView()
		{
			if (this.Vm == null)
			{
				return null;
			}

            var rc = new Grid();
		    rc.Background = new SolidColorBrush(Colors.Aquamarine);
            

			var view = rc; //new SlideView(new SlideVm(this.Vm.Slide, false, false, true));
			view.Width = 800;
			view.Height = 800;
			_doc.Children.Clear();
			_doc.Children.Add(view);
			_doc.Visibility = Visibility.Hidden;
			return view;
		}

		public void Rename()
		{
			_editName.Text = _name.Text;
			EditName(true);
			_editName.Focus();
			_editName.SelectAll();
		}

		private void Delete_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			ViewModelEvents.Instance.DeleteSlide.Publish(this.Vm.Slide);
		}

		/// <summary>
		/// Called when user is editing name of element.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void EditName_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Enter)
			{
				e.Handled = true;
				this.Vm.Name = _editName.Text;
				EditName(false);
			}

			else if (e.Key == Key.Escape)
			{
				e.Handled = true;
				EditName(false);
			}
		}

		private void EditName_LostFocus(object sender, RoutedEventArgs e)
		{
			e.Handled = true;
			EditName(false);
		}

		private void EditName(bool edit)
		{
			_editName.Visibility = edit ? Visibility.Visible : Visibility.Collapsed;
			_name.Visibility = edit ? Visibility.Collapsed : Visibility.Visible;

			if (!edit)
			{
				this.Focus();
			}
		}

		#endregion // Methods
	}
}
