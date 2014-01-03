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
using Osiris.Tfs.Monitor.Models;
using Osiris.Tfs.Report;
	using System.Diagnostics;
	using System.IO;

	#endregion // Using

	public interface ISlideView : IDisposable
	{
		UserControl UserControl { get; }
	}

	public partial class BurndownSlideView : UserControl, IBurndownView, ISlideView
	{
		#region Fields

		//BurndownSlideVM _vm;

		#endregion // Fields

		#region Constructors

		public BurndownSlideView()
		{
			InitializeComponent();
		}

		public BurndownSlideView(BurndownSlide slide)
		{
			InitializeComponent();

			this.DataContext = slide;

			//_vm = new BurndownSlideVM(slide, this);

			//Init();

			//this.DataContext = _vm.Slide;
		}

		#endregion // Constructors

		#region Methods

		/*private void Init()
		{
			if (_vm.Slide.BackgroundColor == null && !string.IsNullOrEmpty(_vm.Slide.BackgroundImage))
			{
				if (File.Exists(_vm.Slide.BackgroundImage))
				{
					var source = new Uri(_vm.Slide.BackgroundImage);
					if (source != null)
					{
						_bkImg.Source = new BitmapImage(source);
					}
				}
			}
			else
			{
				this.Background = new SolidColorBrush(_vm.Slide.BackgroundColor.Value);
			}

		}*/

		public void UpdateChart(SprintBurndown data)
		{
			if (this.Loading.Visibility == Visibility.Visible)
			{
				this.Loading.Visibility = Visibility.Collapsed;
				this.Chart.Visibility = Visibility.Visible;
			}

			// Remaining
			LineChartLine remaining = new LineChartLine(true, "Remaining work", Color.FromRgb(0, 196, 252));

			// Actual
			LineChartLine actual = new LineChartLine(false, "Actual", Color.FromRgb(196, 252, 0));

			// X-axis dates
			List<string> xValues = new List<string>();
			
			foreach (BurndownChartData<DateTime> x in data)
			{
				bool future = (x.XAxis > DateTime.Now);
				bool highlight = (x.XAxis == x.XAxis.Date);

				remaining.Add(new LineChartPoint((double)x.Remaining, !future, highlight));
				actual.Add(new LineChartPoint((double)x.Actual, true, highlight));
				
				// Add dates for highlighted
				if (highlight)
				{
					xValues.Add(String.Format("{0}/{1}", x.XAxis.Day, x.XAxis.Month));
				}
			}

			this.Chart.Clear();
			this.Chart.SetXValues(xValues);
			this.Chart.AddLine(remaining);
			this.Chart.AddLine(actual);

			this.Chart.RefreshGraph();
		}

		#endregion // Methods

		#region ISlideView Members

		public new UserControl UserControl { get { return this; } }

		#endregion // ISlideView Members

		#region IDisposable Members

		public void Dispose()
		{
			//_vm.Dispose();
		}

		#endregion // IDisposable Members

		private void UserControl_Loaded(object sender, RoutedEventArgs e)
		{
			/*if (this.DataContext != null)
			{
				if (this.DataContext is BurndownSlideVM)
				{
					_vm = new BurndownSlideVM(((BurndownSlideVM)this.DataContext).Slide, this);
					this.DataContext = _vm.Slide;
				}
			}*/
		}

	}
}
