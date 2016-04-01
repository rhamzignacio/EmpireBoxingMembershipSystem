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
    /// Interaction logic for Time_In.xaml
    /// </summary>
    public partial class Time_In : Window
    {
        string user = "";
        public Time_In(string user)
        {
            InitializeComponent();

            this.user = user;

            txtBoxClientNo.Focus();
        }

        private void Logged()
        {
            try
            {
                EmpireBoxingEntities db = new EmpireBoxingEntities();
                var clt = db.CLIENT_PROFILE.Where(r => r.CLT_ID == txtBoxClientNo.Text);
                if (clt.ToList().Count > 0)
                {
                    var temp = clt.FirstOrDefault(r => r.CLT_ID == txtBoxClientNo.Text);
                    if (temp.STATUS == "Y")//Active Client
                    {
                        ClientProfile form = new ClientProfile(temp.CLT_ID, user);
                        form.Show();
                        Close();
                    }
                    else //Inactive Client
                        MessageBox.Show("Client is Inactive", "Error");
                }
                else
                {
                    MessageBox.Show("Invalid ID", "Error");
                }
            }
            catch(Exception error)
            {
                MessageBox.Show(error.Message, "Error");
            }
        }

        private void btnOkay_Click(object sender, RoutedEventArgs e)
        {
            Logged();
        }

        private void txtBoxClientNo_KeyUp(object sender, KeyEventArgs e) //For Barcode Scanner
        {
            if (e.Key == Key.Enter)
                Logged();
        }
    }
}