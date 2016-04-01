using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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
    /// Interaction logic for NewClient.xaml
    /// </summary>
    public partial class NewClient : Window
    {
        string cltID;
        string imageLoc = "";
        EmpireBoxingEntities db = new EmpireBoxingEntities();
        public NewClient(string cltID)
        {
            InitializeComponent();            

            LoadDropDownData();

            this.cltID = cltID;

            if (cltID != "")
            {       
                GetExistingData();
            }
            else
            {
                datePickerMembershipDate.Text = DateTime.Now.AddMonths(-1).ToString();
                datePickerMembershipExpiry.Text = DateTime.Now.AddMonths(-1).ToString();
            }
        }

        private void GetExistingData()
        {
            try
            {
                db = new EmpireBoxingEntities();

                var cltInfo = db.CLIENT_PROFILE.FirstOrDefault(r => r.CLT_ID == cltID);

                txtBoxAddress.Text = cltInfo.ADDRESS;

                txtBoxAge.Text = cltInfo.AGE;

                txtBoxBassconRate.Text = cltInfo.BASSCON_RATE.ToString();

                txtBoxBoxingRate.Text = cltInfo.BOXING_RATE.ToString();

                txtBoxContactNo.Text = cltInfo.CONTACT_NO;

                txtBoxEmail.Text = cltInfo.EMAIL;

                txtBoxFirstName.Text = cltInfo.FIRST_NAME;

                txtBoxLastName.Text = cltInfo.LAST_NAME;

                txtBoxMedHistory.Text = cltInfo.MED_RECORD;

                txtBoxMiddleInitial.Text = cltInfo.MIDDLE_INITIAL;

                txtBoxMma.Text = cltInfo.MMA_RATE.ToString();

                txtBoxMuayThaiRate.Text = cltInfo.MUAY_THAI_RATE.ToString();

                cmbBoxGender.Text = cltInfo.GENDER;

                var temp = "";

                if (cltInfo.TYPE == "O")
                    temp = "Ordinary";
                else
                    temp = "Student / Senior Citizen / PWDs";

                cmbBoxType.Text = temp;

                datePickerMembershipDate.Text = cltInfo.MEMBERSHIP_DATE.ToString();

                datePickerMembershipExpiry.Text = cltInfo.EXPIRATION_DATE.ToString();
            }
            catch(Exception error)
            {
                MessageBox.Show(error.Message, "Error");
            }
        }

        private void LoadDropDownData()
        {
            try
            {
                //Gender
                ComboBoxItem cmbItem = new ComboBoxItem
                {
                    Tag = "Male",
                    Content = "Male"
                };
                cmbBoxGender.Items.Add(cmbItem);

                cmbItem = new ComboBoxItem
                {
                    Tag = "Female",
                    Content = "Female"
                };
                cmbBoxGender.Items.Add(cmbItem);

                //Type
                cmbItem = new ComboBoxItem
                {
                    Tag = "O",
                    Content = "Ordinary"
                };
                cmbBoxType.Items.Add(cmbItem);

                cmbItem = new ComboBoxItem
                {
                    Tag = "S",
                    Content = "Student / Senior Citizen / PWDs"
                };
                cmbBoxType.Items.Add(cmbItem);
            }
            catch(Exception error)
            {
                MessageBox.Show(error.Message, "Error");
            }
        }

        private string GetCltID()
        {
            try
            {
                string id = "";

                string year = DateTime.Now.Year.ToString();

                string month = DateTime.Now.Month.ToString();

                string day = DateTime.Now.Day.ToString();

                db = new EmpireBoxingEntities();

                while (month.Length != 2)
                    month = "0" + month;

                var count = db.CLIENT_PROFILE.Where(r => r.CLT_ID.Contains(year + month + day)).ToList().Count + 1;

                string ctr = count.ToString();

                while (ctr.Length != 3)
                    ctr = "0" + ctr;

                id = year + month + day + ctr;
                return id;
            }
            catch(Exception error)
            {
                MessageBox.Show(error.Message, "Error");
                return "";
            }
        }


        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            db = new EmpireBoxingEntities();
            string errMessage = "";

            //Error handlers
            if (txtBoxFirstName.Text == "")
                errMessage += "First Name is required\n";
            if (txtBoxLastName.Text == "")
                errMessage += "Last Name is required\n";
            if (cmbBoxType.Text == "")
                errMessage += "Client type is required\n";
            if (cmbBoxGender.Text == "")
                errMessage += "Gender is required\n";

            if (errMessage == "")
            {
                if (cltID == "") //New Client
                {
                    CLIENT_PROFILE clt = new CLIENT_PROFILE
                    {
                        CLT_ID = GetCltID(),
                        CLT_TYPE = ((ComboBoxItem)cmbBoxType.SelectedItem).Tag.ToString(),
                        STATUS = "Y",
                        FIRST_NAME = txtBoxFirstName.Text,
                        LAST_NAME = txtBoxLastName.Text,
                        MIDDLE_INITIAL = txtBoxMiddleInitial.Text,
                        FULL_NAME = txtBoxFirstName.Text + " " + txtBoxMiddleInitial.Text + " " + txtBoxLastName.Text,
                        MEMBERSHIP_DATE = DateTime.Parse(datePickerMembershipDate.Text),
                        EXPIRATION_DATE = DateTime.Parse(datePickerMembershipExpiry.Text),
                        MED_RECORD = txtBoxMedHistory.Text,
                        AGE = txtBoxAge.Text,
                        GENDER = ((ComboBoxItem)cmbBoxGender.SelectedItem).Content.ToString(),
                        IMAGE_LOC = imageLoc,
                        CONTACT_NO = txtBoxContactNo.Text,
                        EMAIL = txtBoxEmail.Text,
                        ADDRESS = txtBoxAddress.Text,
                        //CORP_GROUP_NAME = int.Parse(((ComboBoxItem)cmbBoxGroupCorpName.SelectedItem).Tag.ToString()),
                        TYPE = ((ComboBoxItem)cmbBoxType.SelectedItem).Tag.ToString(),

                        //Session Pack
                        BOXING = 0,
                        MUAY_THAI = 0,
                        BASSCON = 0,
                        MMA = 0,
                        SESSION_EXPIRY = DateTime.Now,
                        //Free Session Pack
                        FREE_BASSCON = 0,
                        FREE_BOXING = 0,
                        FREE_MMA = 0,
                        FREE_MUAY_THAI = 0,
                        FREE_SESSION_EXPIRY = DateTime.Now,
                        //Special Rate
                        BOXING_RATE = decimal.Parse(txtBoxBoxingRate.Text),
                        MUAY_THAI_RATE = decimal.Parse(txtBoxMuayThaiRate.Text),
                        MMA_RATE = decimal.Parse(txtBoxMma.Text),
                        BASSCON_RATE = decimal.Parse(txtBoxBassconRate.Text)
                    };
                    try
                    {
                        db.Entry(clt).State = System.Data.Entity.EntityState.Added;
                        db.SaveChanges();
                        MessageBox.Show("Client #: " + clt.CLT_ID + "\nName: " + clt.FULL_NAME + "\nAge: " + clt.AGE + " Gender: " + clt.GENDER, "Successfully Saved");
                        this.Close();
                    }
                    catch
                    {
                        MessageBox.Show("There was some kind of error", "Error on saving");
                    }
                }
                else //Update Client
                {
                    var clt = db.CLIENT_PROFILE.FirstOrDefault(r => r.CLT_ID == cltID);

                    clt.CLT_TYPE = ((ComboBoxItem)cmbBoxType.SelectedItem).Tag.ToString();
                    clt.STATUS = "Y";
                    clt.FIRST_NAME = txtBoxFirstName.Text;
                    clt.LAST_NAME = txtBoxLastName.Text;
                    clt.MIDDLE_INITIAL = txtBoxMiddleInitial.Text;
                    clt.FULL_NAME = txtBoxFirstName.Text + " " + txtBoxMiddleInitial.Text + " " + txtBoxLastName.Text;
                    clt.MEMBERSHIP_DATE = DateTime.Parse(datePickerMembershipDate.Text);
                    clt.EXPIRATION_DATE = DateTime.Parse(datePickerMembershipExpiry.Text);
                    clt.MED_RECORD = txtBoxMedHistory.Text;
                    clt.AGE = txtBoxAge.Text;
                    clt.GENDER = ((ComboBoxItem)cmbBoxGender.SelectedItem).Content.ToString();
                    clt.IMAGE_LOC = imageLoc;
                    clt.CONTACT_NO = txtBoxContactNo.Text;
                    clt.EMAIL = txtBoxEmail.Text;
                    clt.ADDRESS = txtBoxAddress.Text;
                    //CORP_GROUP_NAME = int.Parse(((ComboBoxItem)cmbBoxGroupCorpName.SelectedItem).Tag.ToString()),
                    clt.TYPE = ((ComboBoxItem)cmbBoxType.SelectedItem).Tag.ToString();

                    //Session Pack
                    clt.BOXING = 0;
                    clt.MUAY_THAI = 0;
                    clt.BASSCON = 0;
                    clt.MMA = 0;
                    clt.SESSION_EXPIRY = DateTime.Now;

                    //Free Session Pack
                    clt.FREE_BASSCON = 0;
                    clt.FREE_BOXING = 0;
                    clt.FREE_MMA = 0;
                    clt.FREE_MUAY_THAI = 0;
                    clt.FREE_SESSION_EXPIRY = DateTime.Now;

                    //Special Rate
                    clt.BOXING_RATE = decimal.Parse(txtBoxBoxingRate.Text);
                    clt.MUAY_THAI_RATE = decimal.Parse(txtBoxMuayThaiRate.Text);
                    clt.MMA_RATE = decimal.Parse(txtBoxMma.Text);
                    clt.BASSCON_RATE = decimal.Parse(txtBoxBassconRate.Text);

                    try
                    {
                        db.SaveChanges();

                        MessageBox.Show("Sucessfully Saved");

                        this.Close();
                    }
                    catch
                    {
                        MessageBox.Show("There was some kind of error", "Error on updating");
                    }
                }
            }
            else
            {
                MessageBox.Show(errMessage, "Error on Saving");
            }
        }

        private void btnUploadImage_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                OpenFileDialog fileDialog = new OpenFileDialog();
                fileDialog.Multiselect = false;

                string destinationFolder = @"c:\\EmpireMembershipPhotos";
                if (!Directory.Exists(destinationFolder))
                    Directory.CreateDirectory(destinationFolder);

                if ((bool)fileDialog.ShowDialog())
                {
                    if (fileDialog.OpenFile() != null)
                    {
                        try
                        {
                            imageLoc = fileDialog.FileName;

                            BitmapImage logo = new BitmapImage();
                            logo.BeginInit();
                            logo.UriSource = new Uri(imageLoc);
                            logo.EndInit();
                            image.Source = logo;
                        }
                        catch
                        {
                            MessageBox.Show("Can't load image", "Incorrect image format");
                        }
                    }
                }
            }
            catch(Exception error)
            {
                MessageBox.Show(error.Message, "Error");
            }
        }
    }
}
