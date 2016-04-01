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

namespace EmpireBoxingMembershipSystem
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        public Login()
        {
            InitializeComponent();
            if (CheckConnection() == false)
            {
                lblDBConnection.Content = "Disconnected...";

                this.IsEnabled = false;

                MessageBox.Show("Cannot connect to database...", "Error");
            }
        }

        private bool CheckConnection()
        {
            try
            {
                EmpireBoxingEntities db = new EmpireBoxingEntities();

                db.Database.Connection.Open();

                return true;
            }
            catch //Database Connection Issue
            {
                return false;
            }
        }


        private void UserLogin()
        {
            try
            {
                EmpireBoxingEntities db = new EmpireBoxingEntities();
                var user = db.USER_ACCOUNT.FirstOrDefault(r => r.USERNAME == txtBoxUsername.Text);

                if (txtBoxPassword.Password == user.PASSWORD)
                {
                    MainWindow form = new MainWindow(user.USERNAME, this);
                    form.Show();
                    txtBoxPassword.Password = "";
                    txtBoxUsername.Text = "";
                    this.Hide();
                }
            }
            catch
            {
                MessageBox.Show("Invalid Username/Password", "Error");
            }
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            UserLogin();
        }

        private void txtBoxPassword_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                UserLogin();
        }
    }
}