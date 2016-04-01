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

namespace EmpireBoxingMembershipSystem.Client
{
    /// <summary>
    /// Interaction logic for ClientList.xaml
    /// </summary>
    public partial class ClientList : Window
    {
        string user = "";

        public ClientList(string user)
        {
            InitializeComponent();

            this.user = user;

            GetColumns();

            GetClientList("");
        }

        private class Clients
        {
            private string _status;
            public string Status
            {
                get
                {
                    if (_status == "Y")
                        return "Active";
                    else
                        return "Inactive";
                }
                set { _status = value; }
            }
            public string ClientID { get; set; }
            public string FullName { get; set; }
            public string Age { get; set; }
            public string ExpirationDate { get; set; }
            public string ContactNo { get; set; }
            public string GroupName { get; set; }
        }

        private void GetColumns()//Generate Columns
        {
            DataGridTextColumn column = null;

            column = new DataGridTextColumn();
            column.Header = "Status";
            column.Width = 100;
            column.Binding = new Binding("Status");
            clientGrid.Columns.Add(column);

            column = new DataGridTextColumn();
            column.Header = "ID";
            column.Width = 150;
            column.Binding = new Binding("ClientID");
            clientGrid.Columns.Add(column);

            column = new DataGridTextColumn();
            column.Header = "Name";
            column.Width = 300;
            column.Binding = new Binding("FullName");
            clientGrid.Columns.Add(column);

            column = new DataGridTextColumn();
            column.Header = "Group/Corporate Name";
            column.Width = 200;
            column.Binding = new Binding("GroupName");
            clientGrid.Columns.Add(column);

            column = new DataGridTextColumn();
            column.Header = "Age";
            column.Width = 80;
            column.Binding = new Binding("Age");
            clientGrid.Columns.Add(column);

            column = new DataGridTextColumn();
            column.Header = "Contact No";
            column.Width = 100;
            column.Binding = new Binding("ContactNo");
            clientGrid.Columns.Add(column);
        }

        private void GetClientList(string searchKey)
        {
            try
            {
                clientGrid.Items.Clear(); //Clear Data Grid

                EmpireBoxingEntities db = new EmpireBoxingEntities();

                var list = db.CLIENT_PROFILE.OrderBy(r => r.FULL_NAME).ToList();

                if (searchKey != "")
                    list = list.AsQueryable().Where(r => r.FULL_NAME.ToLower()
                    .Contains(searchKey.ToLower())).ToList();

                foreach (var item in list)
                {
                    string temp = "";

                    if (item.CORP_GROUP_NAME != null)
                    {
                        temp = db.GROUP_CORPORATE_PROFILE.FirstOrDefault
                            (r => r.ID == item.CORP_GROUP_NAME).NAME;
                    }
                    else
                        temp = "None";

                    Clients clt = new Clients
                    {
                        Status = item.STATUS,
                        ClientID = item.CLT_ID,
                        FullName = item.FULL_NAME,
                        Age = item.AGE,
                        ExpirationDate = item.EXPIRATION_DATE.ToShortDateString(),
                        ContactNo = item.CONTACT_NO,
                        GroupName = temp
                    };

                    clientGrid.Items.Add(clt);
                }
            }
            catch(Exception error)
            {
                MessageBox.Show(error.Message, "Error");
            }
        }

        private void btnView_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Clients header = (Clients)clientGrid.SelectedItem;

                if (header != null)
                {
                    if (header.Status == "Active")
                    {
                        ClientProfile form = new ClientProfile(header.ClientID.ToString(), user);
                        form.ShowDialog();
                    }
                    else
                        MessageBox.Show("Client is Inactive", "Error");
                }
                else
                    MessageBox.Show("Select Data from Grid", "Error");
            }
            catch(Exception error)
            {
                MessageBox.Show(error.Message, "Error");
            }
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            GetClientList(txtBoxSearch.Text); 
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            try
            { 
                Clients header = (Clients)clientGrid.SelectedItem;
                if (header.Status == "Active")
                {
                    NewClient form = new NewClient(header.ClientID.ToString());
                    form.ShowDialog();

                    GetClientList("");
                }
                else
                    MessageBox.Show("Client is Inactive", "Error");
            }
            catch
            {
                MessageBox.Show("Please select data from list", "Error");
            }
        }

        private void btnNewClient_Click(object sender, RoutedEventArgs e)
        {
            NewClient form = new NewClient("");
            form.ShowDialog();

            GetClientList("");
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string type = "";

                Clients header = (Clients)clientGrid.SelectedItem;

                var db = new EmpireBoxingEntities();

                var client = db.CLIENT_PROFILE.FirstOrDefault(r=>r.CLT_ID == header.ClientID);
                if (client.STATUS == "Y") //Deactivate
                {
                    client.STATUS = "N";
                    type = "Deactivate";
                }
                else //Activate
                {
                    client.STATUS = "Y";
                    type = "Activate";
                }

                MessageBoxResult result = MessageBox.Show("Are you sure you want to " + type, type +
                    " Confirmation", MessageBoxButton.YesNo);

                if (result == MessageBoxResult.Yes)
                {
                    db.Entry(client).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    GetClientList("");
                }          
            }
            catch
            {
                MessageBox.Show("Please select data from list", "Error");
            }
        }

        private void btnNew_Click_1(object sender, RoutedEventArgs e)
        {
            NewClient form = new NewClient("");
            form.ShowDialog();

            GetClientList("");
        }
    }
}
