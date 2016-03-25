using System.Windows;
using EmpireBoxingMembershipSystem.Setup;
using EmpireBoxingMembershipSystem.Client;
using EmpireBoxingMembershipSystem.Setup.Group;
using EmpireBoxingMembershipSystem.Setup.Rates;
using EmpireBoxingMembershipSystem.Setup.User;
using EmpireBoxingMembershipSystem.History;
using System.Linq;
using System.Windows.Input;
using System.Windows.Controls.Primitives;

namespace EmpireBoxingMembershipSystem
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Login login;

        string user = "";

        public MainWindow(string user, Login login)
        {
            InitializeComponent();

            this.login = login;

            this.user = user;

            UserRights(user); //Disable or Enable button depends on Access Level

            Grid.Focus();
        }

        private void UserRights(string user)
        {
            EmpireBoxingEntities db = new EmpireBoxingEntities();

            var userInfo = db.USER_ACCOUNT.FirstOrDefault(r => r.USERNAME == user);

            string accessLvl = userInfo.ACCESS_LVL.ToUpper();

            if (accessLvl.ToUpper() == "ADMIN")
            {
                //Have all Access
            }
            else if(accessLvl.ToUpper() == "USER")
            {
                //Limited Access Only
                btnNewUser.IsEnabled = false;

                btnRates.IsEnabled = false;

                btnUserList.IsEnabled = false;

                btnGroup.IsEnabled = false;
            }
        }

        private void btnNewUser_Click(object sender, RoutedEventArgs e)
        {
            NewUser form = new NewUser(user,"");

            form.ShowDialog();
        }

        private void btnNewClient_Click(object sender, RoutedEventArgs e)
        {
            NewClient newClient = new NewClient("");

            newClient.ShowDialog();
        }

        private void btnClientProfile_Click(object sender, RoutedEventArgs e)
        {
            ClientList form = new ClientList(user);

            form.ShowDialog();
        }

        private void btnGroup_Click(object sender, RoutedEventArgs e)
        {
            GroupList form = new GroupList();

            form.ShowDialog();
        }

        private void btnRates_Click(object sender, RoutedEventArgs e)
        {
            RatesList form = new RatesList();

            form.ShowDialog();
        }

        private void btnLogout_Click(object sender, RoutedEventArgs e)
        {
            login.Show();

            this.Close();
        }

        private void btnTimeIn_Click(object sender, RoutedEventArgs e)
        {
            Time_In form = new Time_In(user);

            form.ShowDialog();
        }

        private void Window_Closed(object sender, System.EventArgs e)
        {
            login.Show();
        }

        private void btnUserList_Click(object sender, RoutedEventArgs e)
        {
            UserList form = new UserList(user);

            form.Show();
        }

        private void Grid_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            
        }

        private void Grid_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.C)
                btnTimeIn.RaiseEvent(new RoutedEventArgs(ButtonBase.ClickEvent));
            else if (e.Key == Key.N)
                btnNewClient.RaiseEvent(new RoutedEventArgs(ButtonBase.ClickEvent));
        }

        private void btnSessionHistory_Click(object sender, RoutedEventArgs e)
        {
            SessionHistory form = new SessionHistory();
            form.ShowDialog();
        }

        private void btnPaymentHistory_Click(object sender, RoutedEventArgs e)
        {
            PaymentHistory form = new PaymentHistory();
            form.ShowDialog();
        }
    }
}
