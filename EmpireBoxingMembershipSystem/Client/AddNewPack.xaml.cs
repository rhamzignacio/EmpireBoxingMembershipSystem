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
using System.Data.Entity;
using System.Text.RegularExpressions;

namespace EmpireBoxingMembershipSystem.Client
{
    /// <summary>
    /// Interaction logic for AddNewPack.xaml
    /// </summary>
    public partial class AddNewPack : Window
    {
        string cltID = "";

        Regex numericRegex = new Regex("[^0-9]+");

        Regex alphabet = new Regex("[A-Za-z]");

        public AddNewPack(string cltID)
        {
            InitializeComponent();

            this.cltID = cltID;

            ToggleCustomPackage(false);

            GetPackageDropdown();
        }

        public void ToggleCustomPackage(bool _input)
        {
            if (_input == true)
            {
                txtBoxBasscon.IsEnabled = true;
                txtBoxBoxing.IsEnabled = true;
                txtBoxMma.IsEnabled = true;
                txtBoxMuayThai.IsEnabled = true;
                txtBoxPrice.IsReadOnly = false;
            }
            else
            {
                txtBoxBasscon.Text = "0";
                txtBoxBoxing.Text = "0";
                txtBoxMma.Text = "0";
                txtBoxMuayThai.Text = "0";
                txtBoxBasscon.IsEnabled = false;
                txtBoxBoxing.IsEnabled = false;
                txtBoxMma.IsEnabled = false;
                txtBoxMuayThai.IsEnabled = false;
                txtBoxPrice.IsReadOnly = true;
            }
        }

        public void ToggleFreePackage()
        {
            txtBoxBasscon.IsEnabled = true;
            txtBoxBoxing.IsEnabled = true;
            txtBoxMma.IsEnabled = true;
            txtBoxMuayThai.IsEnabled = true;
        }

        private void GetPackageDropdown()
        {
            try
            {
                EmpireBoxingEntities db = new EmpireBoxingEntities();

                cmbBoxPackage.Items.Clear();

                var ifGroup = db.GROUP_CORPORATE_MEMBERS.Where(r => r.CLT_ID == cltID);

                var package = db.SESSION_RATE.Where(r => r.TYPE == "P");



                var listPackage = package.ToList();

                if (ifGroup.ToList().Count > 0)
                {
                    var group = db.SESSION_RATE.Where(r => r.SRVC_CODE.Contains("GC") && r.SRVC_NAME.Contains("Pack")).ToList();

                    foreach (var item in group)
                    {
                        ComboBoxItem cbxItem = new ComboBoxItem
                        {
                            Tag = item.SRVC_CODE,
                            Content = item.SRVC_NAME
                        };

                        cmbBoxPackage.Items.Add(cbxItem);
                    }
                }

                foreach (var item in listPackage)
                {
                    ComboBoxItem cbxItem = new ComboBoxItem
                    {
                        Tag = item.SRVC_CODE,
                        Content = item.SRVC_NAME
                    };

                    cmbBoxPackage.Items.Add(cbxItem);
                }
            }
            catch(Exception error)
            {
                MessageBox.Show(error.Message, "Error");
            }
            
        }

        private void cmbBoxPackage_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (((ComboBoxItem)cmbBoxPackage.SelectedItem).Tag.ToString() == "CUSTOM")
                ToggleCustomPackage(true);
            else if (((ComboBoxItem)cmbBoxPackage.SelectedItem).Tag.ToString() == "FREE")
                ToggleFreePackage();
            else
                ToggleCustomPackage(false);

            //Get Rate
            EmpireBoxingEntities db = new EmpireBoxingEntities();

            var rates = db.SESSION_RATE.FirstOrDefault(r => r.SRVC_CODE == ((ComboBoxItem)cmbBoxPackage.SelectedItem).Tag.ToString());

            txtBoxPrice.Text = rates.RATE.ToString();
        }

        private void button_Click(object sender, RoutedEventArgs e) //Save Button
        {
            try
            {
                string errMessage = "";

                if (expirationDate.Text == "")
                    errMessage += "Expiration Date is Required";

                if (errMessage == "")
                {
                    int tempBasscon = 0, tempBoxing = 0, tempMMA = 0, tempMuayThai = 0;

                    try
                    {
                        tempBasscon = int.Parse(txtBoxBasscon.Text);

                        tempBoxing = int.Parse(txtBoxBoxing.Text);

                        tempMMA = int.Parse(txtBoxMma.Text);

                        tempMuayThai = int.Parse(txtBoxMuayThai.Text);

                    }
                    catch
                    {
                        if (tempBasscon == 0)
                            txtBoxBasscon.Text = "0";

                        if (tempBoxing == 0)
                            txtBoxBoxing.Text = "0";

                        if (tempMMA == 0)
                            txtBoxMma.Text = "0";

                        if (tempMuayThai == 0)
                            txtBoxMuayThai.Text = "0";
                    }
                    string message = "";
                    EmpireBoxingEntities db = new EmpireBoxingEntities();
                    var client = db.CLIENT_PROFILE.FirstOrDefault(r => r.CLT_ID == cltID);
                    if (((ComboBoxItem)cmbBoxPackage.SelectedItem).Tag.ToString() == "CUSTOM") //Custom Package
                    {
                        //Update Client Profile
                        client.BOXING += int.Parse(txtBoxBoxing.Text);

                        client.MUAY_THAI += int.Parse(txtBoxMuayThai.Text);

                        client.MMA += int.Parse(txtBoxMma.Text);

                        client.BASSCON += int.Parse(txtBoxBasscon.Text);

                        message = "Boxing: " + txtBoxBoxing.Text +
                            "\nMuay Thai: " + txtBoxMuayThai.Text +
                            "\nMMA: " + txtBoxMma.Text +
                            "\nB@sscon: " + txtBoxBasscon.Text;
                    }
                    else if (((ComboBoxItem)cmbBoxPackage.SelectedItem).Tag.ToString() == "FREE") //Free Package
                    {
                        //Update Client Profile
                        client.FREE_BOXING += int.Parse(txtBoxBoxing.Text);

                        client.FREE_MUAY_THAI += int.Parse(txtBoxMuayThai.Text);

                        client.FREE_MMA += int.Parse(txtBoxMma.Text);

                        client.FREE_BASSCON += int.Parse(txtBoxBasscon.Text);

                        message = "Boxing: " + txtBoxBoxing.Text +
                            "\nMuay Thai: " + txtBoxMuayThai.Text +
                            "\nMMA: " + txtBoxMma.Text +
                            "\nB@sscon: " + txtBoxBasscon.Text;

                        //Add Expiration Date
                        client.FREE_SESSION_EXPIRY = DateTime.Parse(expirationDate.Text);
                    }
                    else //Fixed Package
                    {
                        switch (((ComboBoxItem)cmbBoxPackage.SelectedItem).Tag.ToString())
                        {
                            case "10PBASSCON":
                                client.BASSCON += 10;
                                break;
                            case "10PMMA":
                                client.MMA += 10;
                                break;
                            case "12PBOXING":
                                client.BOXING += 12;
                                break;
                            case "12MUAYTHAI":
                                client.MUAY_THAI += 12;
                                break;
                            case "3PBOXING":
                                client.BOXING += 3;
                                break;
                            case "3PMUAYTHAI":
                                client.MUAY_THAI += 3;
                                break;
                            //Group Rate
                            case "GC10PBASSCON":
                                client.BASSCON += 10;
                                break;

                            case "GC10PMMA":
                                client.MMA += 10;
                                break;

                            case "GC12PBOXING":
                                client.BOXING += 12;
                                break;

                            case "GC12PMUAYTHAI":
                                client.MUAY_THAI += 12;
                                break;
                        }
                        message = ((ComboBoxItem)cmbBoxPackage.SelectedItem).Content.ToString();
                    }
                    client.SESSION_EXPIRY = DateTime.Parse(expirationDate.Text);
                    //Add Payment Transaction
                    PAYMENT_HISTORY payment = new PAYMENT_HISTORY
                    {
                        CLT_ID = cltID,
                        METHOD = "CASH",
                        AMOUNT = decimal.Parse(txtBoxPrice.Text),
                        SESSION_CODE = ((ComboBoxItem)cmbBoxPackage.SelectedItem).Tag.ToString(),
                        CRT_DATE = DateTime.Now
                    };
                    db.Entry(payment).State = EntityState.Added;
                    try
                    {
                        db.SaveChanges();
                        message += "\n\nPrice: " + txtBoxPrice.Text;
                        MessageBox.Show(message, "Saved");
                        this.Close();
                    }
                    catch (Exception er)
                    {
                        MessageBox.Show(er.Message, "Error on Saving");
                    }
                }
                else
                {
                    MessageBox.Show(errMessage, "Error on Saving");
                }
            }
            catch(Exception error)
            {
                MessageBox.Show(error.Message, "Error");
            }
        }

        private void button1_Click(object sender, RoutedEventArgs e) //Cancek Button
        {
            this.Close();
        }

        private void txtBoxBoxing_KeyUp(object sender, KeyEventArgs e)
        {
            
        }

        private void txtBoxBoxing_TextChanged(object sender, TextChangedEventArgs e)
        {           
            if (e.Handled = numericRegex.IsMatch(txtBoxBoxing.Text) == true)
                txtBoxBoxing.Text = "0";           
        }

        private void txtBoxMma_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (e.Handled = numericRegex.IsMatch(txtBoxMma.Text) == true)
                txtBoxMma.Text = "0";
        }

        private void txtBoxMuayThai_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (e.Handled = numericRegex.IsMatch(txtBoxMuayThai.Text) == true)
                txtBoxMuayThai.Text = "0";
        }

        private void txtBoxBasscon_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (e.Handled = numericRegex.IsMatch(txtBoxBasscon.Text) == true)
                txtBoxBasscon.Text = "0";
        }

        private void txtBoxPrice_TextChanged(object sender, TextChangedEventArgs e)
        {
          
            if (e.Handled = alphabet.IsMatch(txtBoxPrice.Text) == true)
                txtBoxPrice.Text = "0.00";
        }
    }
}
