using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace EmpireBoxingMembershipSystem.Setup.Rates
{
    /// <summary>
    /// Interaction logic for RatesList.xaml
    /// Edit Rates Only
    /// Not allowed to Add new Session Rate
    /// </summary>
    public partial class RatesList : Window
    {
        public RatesList()
        {
            InitializeComponent();
            GetColumns();
            GetRateList();
        }

        private class RatesHeader
        {
            public string SessionName { get; set; }
            public string SessionRate { get; set; }
            public string Category { get; set; }
        }

        private void GetColumns()
        {
            DataGridTextColumn column = null;

            column = new DataGridTextColumn();
            column.Header = "Session Name";
            column.Width = 400;
            column.Binding = new Binding("SessionName");
            ratesGrid.Columns.Add(column);

            column = new DataGridTextColumn();
            column.Header = "Price";
            column.Width = 130;
            column.Binding = new Binding("SessionRate");
            ratesGrid.Columns.Add(column);

            column = new DataGridTextColumn();
            column.Header = "Category";
            column.Width = 130;
            column.Binding = new Binding("Category");
            ratesGrid.Columns.Add(column);
        }

        private void GetRateList()
        {
            ratesGrid.Items.Clear();
            EmpireBoxingEntities db = new EmpireBoxingEntities();
            var list = db.SESSION_RATE.OrderBy(r => r.CATEGORY).ToList();
            foreach(var item in list)
            {
                RatesHeader header = new RatesHeader
                {
                    SessionName = item.SRVC_NAME,
                    SessionRate = item.RATE.ToString(),
                    Category = item.CATEGORY
                };
                ratesGrid.Items.Add(header);
            }
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            RatesHeader header = (RatesHeader)ratesGrid.SelectedItem;
            if (header != null)
            {
                OpenRates form = new OpenRates(header.SessionName.ToString());
                form.ShowDialog();
                GetRateList();
            }
            else
                MessageBox.Show("Please select item", "Error");
        }

        private void ratesGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void ratesGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if(sender == null)
                return;

            if (e.ButtonState != MouseButtonState.Pressed)//only react on pressed
                return;

            var dataGrid = sender as DataGrid;

            if (dataGrid == null || dataGrid.SelectedItems == null)
                return;

            RatesHeader header = (RatesHeader)ratesGrid.SelectedItem;
            if (header != null)
            {
                OpenRates form = new OpenRates(header.SessionName.ToString());
                form.ShowDialog();
                GetRateList();
            }
            else
                MessageBox.Show("Please select item", "Error");
        }
    }
}
