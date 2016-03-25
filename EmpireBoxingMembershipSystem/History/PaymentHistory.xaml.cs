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
    /// Interaction logic for PaymentHistory.xaml
    /// </summary>
    public partial class PaymentHistory : Window
    {
        public PaymentHistory()
        {
            InitializeComponent();

            CreateTableColumn();

            GetPaymentHistory();

        }

        private class PaymentGrid
        {
            public string ClientName { get; set; }
            public string Method { get; set; }
            public string Amount { get; set; }
            public string Session { get; set; }
            public string CreatedDate { get; set; }
            public string CreatedBy { get; set; }
        }

        private void CreateTableColumn()
        {
            DataGridTextColumn column = null;

            column = new DataGridTextColumn();
            column.Header = "Client Name";
            column.Width = 300;
            column.Binding = new Binding("ClientName");
            dataGridPayment.Columns.Add(column);

            column = new DataGridTextColumn();
            column.Header = "Payment Method";
            column.Width = 130;
            column.Binding = new Binding("Method");
            dataGridPayment.Columns.Add(column);

            column = new DataGridTextColumn();
            column.Header = "Session";
            column.Width = 130;
            column.Binding = new Binding("Session");
            dataGridPayment.Columns.Add(column);

            column = new DataGridTextColumn();
            column.Header = "Amount";
            column.Width = 130;
            column.Binding = new Binding("Amount");
            dataGridPayment.Columns.Add(column);

            column = new DataGridTextColumn();
            column.Header = "Created Date";
            column.Width = 130;
            column.Binding = new Binding("CreatedDate");
            dataGridPayment.Columns.Add(column);

            column = new DataGridTextColumn();
            column.Header = "Created By";
            column.Width = 130;
            column.Binding = new Binding("CreatedBy");
            dataGridPayment.Columns.Add(column);
        }

        private void GetPaymentHistory(DateTime? FromDate = null, DateTime? ToDate = null)
        {
            using(var db = new EmpireBoxingEntities())
            {
                dataGridPayment.Items.Clear();

                var payments = db.PAYMENT_HISTORY.AsQueryable();

                if (FromDate != null && ToDate != null)
                    payments = payments.Where(r => r.CRT_DATE >= FromDate && r.CRT_DATE <= ToDate);

                payments.OrderByDescending(r=>r.CRT_DATE).ToList().ForEach(item =>
                {
                    var client = db.CLIENT_PROFILE.FirstOrDefault(r => r.CLT_ID == item.CLT_ID);
                    if (client != null)
                    {
                        var sess = db.SESSION_RATE.FirstOrDefault(r => r.SRVC_CODE == item.SESSION_CODE);

                        PaymentGrid payment = new PaymentGrid
                        {
                            ClientName = client.FIRST_NAME + " " + client.MIDDLE_INITIAL + " " + client.LAST_NAME,
                            Method = item.METHOD,
                            Amount = string.Format("{0:0.00}", item.AMOUNT),
                            Session = sess.SRVC_NAME,
                            CreatedDate = item.CRT_DATE.ToString(),
                            CreatedBy = item.CRT_BY != null ? item.CRT_BY : ""
                        };

                        dataGridPayment.Items.Add(payment);
                    }
                });
            }
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            if (FromDate.Text != "" && ToDate.Text != "")
                GetPaymentHistory(DateTime.Parse(FromDate.Text), DateTime.Parse(ToDate.Text));
            else
                MessageBox.Show("Please select date range", "Error");
        }
    }
}
