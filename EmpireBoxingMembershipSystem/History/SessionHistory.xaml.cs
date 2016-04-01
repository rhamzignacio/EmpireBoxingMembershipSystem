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

namespace EmpireBoxingMembershipSystem.History
{
    /// <summary>
    /// Interaction logic for SessionHistory.xaml
    /// </summary>
    public partial class SessionHistory : Window
    {
        public SessionHistory()
        {
            InitializeComponent();

            CreateTableColumns();
            GetSessionHistory();
        }

        private class SessionGrid
        {
            public string ClientName { get; set; }
            public string Service { get; set; }
            public string CreatedDate { get; set; }
            public string CreatedBy { get; set; }
        }

        private void CreateTableColumns()
        {
            DataGridTextColumn column = null;

            column = new DataGridTextColumn();
            column.Header = "Client Name";
            column.Width = 300;
            column.Binding = new Binding("ClientName");
            dataGridSession.Columns.Add(column);

            column = new DataGridTextColumn();
            column.Header = "Service";
            column.Width = 150;
            column.Binding = new Binding("Service");
            dataGridSession.Columns.Add(column);

            column = new DataGridTextColumn();
            column.Header = "Created Date";
            column.Width = 200;
            column.Binding = new Binding("CreatedDate");
            dataGridSession.Columns.Add(column);

            column = new DataGridTextColumn();
            column.Header = "Created By";
            column.Width = 100;
            column.Binding = new Binding("CreatedBy");
            dataGridSession.Columns.Add(column);
        }

        private void GetSessionHistory(DateTime? fromDate = null, DateTime? toDate = null)
        {
            try
            {
                using (var db = new EmpireBoxingEntities())
                {
                    dataGridSession.Items.Clear(); //Clear data in the list

                    var session = db.SESSION_USED_HISTORY.AsQueryable();

                    if (fromDate != null && toDate != null)
                        session = session.Where(r => r.CRT_DATE >= fromDate && r.CRT_DATE <= toDate);

                    session.OrderByDescending(r => r.CRT_DATE).ToList().ForEach(item =>
                    {
                        var client = db.CLIENT_PROFILE.FirstOrDefault(r => r.CLT_ID == item.CLT_ID);
                        if (client != null)
                        {
                            SessionGrid grid = new SessionGrid
                            {
                                ClientName = client.FIRST_NAME + " " + client.MIDDLE_INITIAL + " " + client.LAST_NAME,
                                Service = item.SRVC_CODE,
                                CreatedBy = item.CRT_BY != null ? item.CRT_BY : "",
                                CreatedDate = item.CRT_DATE.ToString()
                            };

                            dataGridSession.Items.Add(grid);
                        }
                    });
                }
            }
            catch(Exception error)
            {
                MessageBox.Show(error.Message, "Error");
            }
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            if (ToDate.Text != "" && FromDate.Text != "")
                GetSessionHistory(DateTime.Parse(FromDate.Text), DateTime.Parse(ToDate.Text));
            else
                MessageBox.Show("Please select date range", "Error");
        }
    }
}
