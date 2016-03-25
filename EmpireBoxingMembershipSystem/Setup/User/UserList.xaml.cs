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

namespace EmpireBoxingMembershipSystem.Setup.User
{
    /// <summary>
    /// Interaction logic for UserList.xaml
    /// </summary>
    public partial class UserList : Window
    {
        string user = "";

        public UserList(string _user)
        {
            InitializeComponent();

            GenerateColumns();

            GetAllUser();

            user = _user;
        }

        private class UserHeader
        {
            public string ACCESS_LVL { get; set; }
            public string USERNAME { get; set; }
            public string FIRST_NAME { get; set; }
            public string LAST_NAME { get; set; }
        }

        private void GenerateColumns()
        {
            DataGridTextColumn column = null;

            column = new DataGridTextColumn();
            column.Header = "Access Level";
            column.Width = 200;
            column.Binding = new Binding("ACCESS_LVL");
            dataGridUser.Columns.Add(column);

            column = new DataGridTextColumn();
            column.Header = "Username";
            column.Width = 200;
            column.Binding = new Binding("USERNAME");
            dataGridUser.Columns.Add(column);

            column = new DataGridTextColumn();
            column.Header = "First Name";
            column.Width = 200;
            column.Binding = new Binding("FIRST_NAME");
            dataGridUser.Columns.Add(column);

            column = new DataGridTextColumn();
            column.Header = "Last Name";
            column.Width = 200;
            column.Binding = new Binding("LAST_NAME");
            dataGridUser.Columns.Add(column);
        }

        private void GetAllUser()
        {
            dataGridUser.Items.Clear(); //Remove all item in Data Grid

            EmpireBoxingEntities db = new EmpireBoxingEntities();

            var userList = db.USER_ACCOUNT.OrderBy(r => r.USERNAME).ToList();

            foreach (var item in userList)
            {
                UserHeader header = new UserHeader
                {
                    ACCESS_LVL = item.ACCESS_LVL,
                    USERNAME = item.USERNAME,
                    FIRST_NAME = item.FIRST_NAME,
                    LAST_NAME = item.LAST_NAME
                };

                dataGridUser.Items.Add(header);
            }
        }

        private void btnNewUser_Click(object sender, RoutedEventArgs e)
        {
            NewUser form = new NewUser(user, "");

            form.ShowDialog();
        }

        private void btnView_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                UserHeader header = (UserHeader)dataGridUser.SelectedItem;

                NewUser form = new NewUser(user, header.USERNAME);

                form.ShowDialog();
            }
            catch
            {
                MessageBox.Show("Please select data from list", "Error");
            }
        }
    }
}
