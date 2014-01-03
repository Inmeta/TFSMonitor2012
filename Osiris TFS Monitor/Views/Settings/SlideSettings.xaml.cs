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

	#endregion // Using
	
	public partial class SlideSettings : UserControl
	{
		#region Properties

		private ConsoleVM ViewModel { get { return this.DataContext as ConsoleVM; } }

		#endregion // Properties

		#region Constructors

		public SlideSettings()
		{
			InitializeComponent();
		}

		#endregion // Constructors

		#region Methods

		private void AddSlide_Click(object sender, RoutedEventArgs e)
		{
			var dlg = new SelectSlide(this.ViewModel);
			dlg.Owner = Application.Current.MainWindow;
			dlg.ShowDialog();
		}

		private void EditSlide_Click(object sender, RoutedEventArgs e)
		{
			var dlg = new SlidePropView(this.ViewModel, this.ViewModel.SelectedSlide);
			dlg.Owner = Application.Current.MainWindow;
			dlg.ShowDialog();
		}

		private void Remove_Click(object sender, RoutedEventArgs e)
		{
			this.ViewModel.Model.Slides.Remove(this.ViewModel.SelectedSlide);
		}

		private void List_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			SyncButtons();
		}

		private void SyncButtons()
		{
			if (this.ViewModel.SelectedSlide == null)
			{
				_moveDown.IsEnabled = false;
				_moveUp.IsEnabled = false;
			}
			else
			{
				_moveDown.IsEnabled = (_list.SelectedIndex + 1) < _list.Items.Count;
				_moveUp.IsEnabled = _list.SelectedIndex > 0;
			}
		}

		private void MoveUp_Click(object sender, RoutedEventArgs e)
		{
			this.ViewModel.Model.Slides.Move(_list.SelectedIndex, _list.SelectedIndex - 1);
			SyncButtons();
		}

		private void MoveDown_Click(object sender, RoutedEventArgs e)
		{
			this.ViewModel.Model.Slides.Move(_list.SelectedIndex, _list.SelectedIndex + 1);
			SyncButtons();
		}

		#endregion // Methods

	}
}
