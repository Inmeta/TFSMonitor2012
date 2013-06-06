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
	using Microsoft.Windows.Controls.Ribbon;
	using System.Diagnostics;

	#endregion // Using

	public partial class ApplicationRibbonView : UserControl, IApplicationRibbonView
	{
		#region Constructors

		public ApplicationRibbonView()
		{
			InitializeComponent();

			this.DataContext = new ApplicationRibbonVm(this);

			/*#if DEBUG
			_tfsTab.Visibility = Visibility.Visible;
			#endif*/
		}

		#endregion // Constructors

		#region Methods

		private void ApplicationInfo_MouseDown(object sender, MouseButtonEventArgs e)
		{
			var dlg = new ApplicationInfoView(Application.Current.MainWindow);
			dlg.ShowDialog();
		}

		private IEnumerable<ICustomTab> GetCustomTabs(object type)
		{
			var customTabs = new List<ICustomTab>();

			// Hmm factory or virtual method? ....
			if (type is BurndownChartMenuVm)
			{
				customTabs.Add(new TfsTabView());
				customTabs.Add(new BurndownChartMenuView(type as BurndownChartMenuVm));
			}
			else if (type is WebPageMenuVm)
			{
				customTabs.Add(new WebPageMenuView(type as WebPageMenuVm));
			}
			else if (type is BuildMonitorMenuVm)
			{
				customTabs.Add(new BuildMonitorMenuView(type as BuildMonitorMenuVm));
			}
			else if (type is TaskManagerMenuVm)
			{
				customTabs.Add(new TaskManagerMenuView(type as TaskManagerMenuVm));
			}
			else if (type is TwitterMenuVm)
			{
				customTabs.Add(new TwitterMenuView(type as TwitterMenuVm));
			}

			return customTabs;

		}

		public void SetCustomTab(object type)
		{
			var customTabs = GetCustomTabs(type);

			// Remove those not used
			var remove = new List<RibbonTab>();
			foreach (ICustomTab ribbonTab in ribbon.Tabs.Where(t => t is ICustomTab))
			{
				if ((ribbonTab.IsCommon && !customTabs.Any(ct => ct.GetType() == ribbonTab.GetType())) || !ribbonTab.IsCommon)
				{
					remove.Add(ribbonTab as RibbonTab);
				}
			}
			remove.ForEach(ct => ribbon.Tabs.Remove(ct));

			// Add new
			foreach (var ribbonTab in customTabs)
			{
				if (!ribbon.Tabs.Any(ct => ct.GetType() == ribbonTab.GetType()))
				{
					ribbon.Tabs.Add(ribbonTab as RibbonTab);
				}
			}

			if (ribbon.SelectedTab != null)
			{
				var tab = ribbon.Tabs.SingleOrDefault(t => t.GetType() == ribbon.SelectedTab.GetType());
				ribbon.SelectedTab = tab ?? GetBestSelection(ribbon.SelectedTab);
			}

		}

		private RibbonTab GetBestSelection(RibbonTab selectedTab)
		{
			if (selectedTab is ICustomTab)
			{
				return ribbon.Tabs.OfType<ICustomTab>().Where(t => !t.IsCommon).SingleOrDefault() as RibbonTab;
			}
			return ribbon.Tabs.First();
		}

		#endregion // Methods

	}

	public class RibbonComboBox : ComboBox, IRibbonControl
	{

	}

}
