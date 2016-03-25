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
    /// Interaction logic for OpenRates.xaml
    /// </summary>
    public partial class OpenRates : Window
    {
        string name = "";
        public OpenRates(string name)
        {
            InitializeComponent();
            this.name = name;
            GetRateInfo();
            
        }

        private void GetRateInfo() //Get Rate information
        {
            EmpireBoxingEntities db = new EmpireBoxingEntities();
            var rate = db.SESSION_RATE.FirstOrDefault(r => r.SRVC_NAME == name);

            txtBoxRate.Text = rate.RATE.ToString();
            txtBoxSessionCode.Text = rate.SRVC_CODE;
            txtBoxSessionName.Text = rate.SRVC_NAME;
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                EmpireBoxingEntities db = new EmpireBoxingEntities();
                var rate = db.SESSION_RATE.FirstOrDefault(r => r.SRVC_NAME == name);
                rate.RATE = decimal.Parse(txtBoxRate.Text);
                db.SaveChanges();
                this.Close();
            }
            catch
            {
                MessageBox.Show("Error on Updating Rates", "Error");
            }

        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
