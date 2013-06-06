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
	using Osiris.Tfs.Report;
	using System.Diagnostics;
	using Osiris.TFS.Monitor.Common;

	#endregion // Using

	public partial class BurndownChartView : UserControl, IBurndownChartView
	{
		#region Properties

		BurndownChartVm ViewModel { get { return this.DataContext as BurndownChartVm; } }

		#endregion // Properties

		#region Constructors

		public BurndownChartView(BurndownChartVm vm)
		{
			InitializeComponent();

            this.DataContext = vm;
		}

		#endregion // Constructors

		#region Methods

		public void UpdateChart(SprintBurndown data)
		{

			_docChart.Children.Clear();

			if (data == null)
			{
				return;
			}


			var chart = new LineChart();

			// Estimate
			LineChartLine estimate = null;
			if (data.SprintBurndownType.DisplayEstimate)
			{
				estimate = new LineChartLine(false, "Estimate", Color.FromRgb(196, 252, 0));
			}
	
			// Remaining
			LineChartLine remaining = new LineChartLine(true, data.SprintBurndownType.Name, Color.FromRgb(0, 196, 252));

			// X-axis dates
			List<string> xValues = new List<string>();

			// Text under x-axis
			for (var time = data.StartDate.Date; time <= data.EndDate.Date; time = time.AddDays(1))
			{
				if (this.ViewModel.ExcludeWeekEnds && (time.DayOfWeek == DayOfWeek.Saturday || time.DayOfWeek == DayOfWeek.Sunday))
				{
					continue;
				}
				xValues.Add(String.Format("{0}/{1}", time.Day, time.Month));
			}

			foreach (BurndownData x in data)
			{
				if (this.ViewModel.ExcludeWeekEnds && (x.Time.DayOfWeek == DayOfWeek.Saturday || x.Time.DayOfWeek == DayOfWeek.Sunday))
				{
					continue;
				}

				double xValue = x.Time.SubtractEx(data.StartDate, this.ViewModel.ExcludeWeekEnds).TotalHours;

				bool future = (x.Time > DateTime.Now);
				bool highlight = (x.Time == x.Time.Date);
				bool nowDate = (x.Time == data.NowDate);

				Color? fillColor = null; //nowDate ? Colors.Red : (Color?)null;

				remaining.Add(new LineChartPoint(xValue, (double)x.Remaining, !future, highlight || nowDate, fillColor));
				if (estimate != null)
				{
					estimate.Add(new LineChartPoint(xValue, (double)x.Estimate, true, highlight, null));
				}
			}

			//this.Chart.Clear();
			chart.SetXValues(xValues);
			chart.AddLine(remaining);
			if (estimate != null)
			{
				chart.AddLine(estimate);
			}
			chart.RefreshGraph();
			_docChart.Children.Add(chart);
		}

		private void UserControl_Loaded(object sender, RoutedEventArgs e)
		{
			this.ViewModel.View = this;
		}

		private void UserControl_Unloaded(object sender, RoutedEventArgs e)
		{
            if (this.ViewModel != null)
            {
                this.ViewModel.View = null;
            }
		}

		private void UserControl_SizeChanged(object sender, SizeChangedEventArgs e)
		{
			this.ViewModel.SizeChanged(e.NewSize.Width, e.NewSize.Height);
			e.Handled = true;
		}

		public void UpdateDataContext()
		{
			var dc = this.ViewModel;
			this.DataContext = null;
			this.DataContext = dc;
		}

		#endregion // Methods

	}
}
