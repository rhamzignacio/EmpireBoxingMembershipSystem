using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data.Entity;
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
    /// Interaction logic for ClientProfile.xaml
    /// </summary>
    public partial class ClientProfile : Window
    {
        string cltID = "20151013001"; //Default Value only
        string user = "";
        EmpireBoxingEntities db = new EmpireBoxingEntities();
        public ClientProfile(string cltID, string user)
        {
            InitializeComponent();

            this.cltID = cltID;

            this.user = user;

            GetClientData();

            AllowedService();
        }
        
        private void AllowedService()
        {
            try
            {
                if (lblAge.Content.ToString() != "" && lblAge.Content.ToString() != null)
                {
                    if (int.Parse(lblAge.Content.ToString()) < 17) //Child
                    {
                        btnPack.IsEnabled = false;
                        btnMember.IsEnabled = false;
                        btnDropRate.IsEnabled = false;
                    }
                }
            }
            catch(Exception error)
            {
                MessageBox.Show(error.Message, "Error");
            }
        }

        public string UseSession(int sessionLeft, string sessionName)
        {
            try
            {
                db = new EmpireBoxingEntities();

                if (sessionLeft > 0)
                {
                    var clientProf = db.CLIENT_PROFILE.FirstOrDefault(r => r.CLT_ID == cltID);
                    switch (sessionName)
                    {
                        //Paid Sessions
                        case "Boxing":
                            if (clientProf.SESSION_EXPIRY >= DateTime.Now)
                                clientProf.BOXING = clientProf.BOXING - 1;
                            else
                            {
                                MessageBoxResult result = MessageBox.Show("Boxing Session is expired/nDo you still want to use it?", "Expired Session", MessageBoxButton.YesNo);
                                if (result == MessageBoxResult.Yes)
                                    clientProf.BOXING = clientProf.BOXING - 1;
                                else
                                {
                                    clientProf.BOXING = 0;
                                    db.SaveChanges();
                                    return "N";
                                }
                            }
                            break;
                        case "MuayThai":
                            if (clientProf.SESSION_EXPIRY >= DateTime.Now)
                                clientProf.MUAY_THAI = clientProf.MUAY_THAI - 1;
                            else
                            {
                                MessageBoxResult result = MessageBox.Show("Muay Thai Session is expired\nDo you still want to use it?", "Expired Session", MessageBoxButton.YesNo);
                                if (result == MessageBoxResult.Yes)
                                    clientProf.MUAY_THAI = clientProf.MUAY_THAI - 1;
                                else
                                {
                                    clientProf.MUAY_THAI = 0;
                                    db.SaveChanges();
                                    return "N";
                                }
                            }
                            break;
                        case "MMA":
                            if (clientProf.SESSION_EXPIRY >= DateTime.Now)
                                clientProf.MMA = clientProf.MMA - 1;
                            else
                            {
                                MessageBoxResult result = MessageBox.Show("MMA Session is expired\n Do you still want to use it?", "Expired Session", MessageBoxButton.YesNoCancel);
                                if (result == MessageBoxResult.Yes)
                                    clientProf.MMA = clientProf.MMA - 1;
                                else
                                {
                                    clientProf.MMA = 0;
                                    db.SaveChanges();
                                    return "N";
                                }
                            }
                            break;
                        case "Basscon":
                            if (clientProf.SESSION_EXPIRY >= DateTime.Now)
                                clientProf.BASSCON = clientProf.BASSCON - 1;
                            else
                            {
                                MessageBoxResult result = MessageBox.Show("B@sscon Session is expired\n Do you still want to use it?", "Expired Session", MessageBoxButton.YesNoCancel);
                                if (result == MessageBoxResult.Yes)
                                    clientProf.BASSCON = clientProf.BASSCON - 1;
                                else
                                {
                                    clientProf.BASSCON = 0;
                                    db.SaveChanges();
                                    return "N";
                                }
                            }
                            break;
                        //Free Sessions
                        case "FreeBoxing":
                            if (clientProf.FREE_SESSION_EXPIRY >= DateTime.Now)
                                clientProf.FREE_BOXING = clientProf.FREE_BOXING - 1;
                            else
                            {
                                MessageBoxResult result = MessageBox.Show("Free Boxing Session is expired\n Do you still want to use it?", "Expired Session", MessageBoxButton.YesNo);
                                if (result == MessageBoxResult.Yes)
                                    clientProf.FREE_BOXING = clientProf.FREE_BOXING - 1;
                                else
                                {
                                    clientProf.FREE_BOXING = 0;
                                    db.SaveChanges();
                                    return "N";
                                }
                            }
                            break;
                        case "FreeMuayThai":
                            if (clientProf.FREE_SESSION_EXPIRY >= DateTime.Now)
                                clientProf.FREE_MUAY_THAI = clientProf.FREE_MUAY_THAI - 1;
                            else
                            {
                                MessageBoxResult result = MessageBox.Show("Free Muay Thai Session is expired\n Do you still want to use it?", "Expired Session", MessageBoxButton.YesNo);
                                if (result == MessageBoxResult.Yes)
                                    clientProf.FREE_MUAY_THAI = clientProf.FREE_MUAY_THAI - 1;
                                else
                                {
                                    clientProf.FREE_MUAY_THAI = 0;
                                    db.SaveChanges();
                                    return "N";
                                }
                            }
                            break;
                        case "FreeMMA":
                            if (clientProf.FREE_SESSION_EXPIRY >= DateTime.Now)
                                clientProf.FREE_MMA = clientProf.FREE_MMA - 1;
                            else
                            {
                                MessageBoxResult result = MessageBox.Show("Free MMA Session is expired\n Do you still want to use it?", "Expired Session", MessageBoxButton.YesNo);
                                if (result == MessageBoxResult.Yes)
                                    clientProf.FREE_MMA = clientProf.FREE_MMA - 1;
                                else
                                {
                                    clientProf.FREE_MMA = 0;
                                    db.SaveChanges();
                                    return "N";
                                }
                            }
                            break;
                        case "FreeBasscon":
                            if (clientProf.FREE_SESSION_EXPIRY >= DateTime.Now)
                                clientProf.FREE_BASSCON = clientProf.FREE_BASSCON - 1;
                            else
                            {
                                MessageBoxResult result = MessageBox.Show("Free B@sscon Session is expired\n Do you still want to use it?", "Expired Session", MessageBoxButton.YesNo);
                                if (result == MessageBoxResult.Yes)
                                    clientProf.FREE_BASSCON = clientProf.FREE_BASSCON - 1;
                                else
                                {
                                    clientProf.FREE_BASSCON = 0;
                                    db.SaveChanges();
                                    return "N";
                                }
                            }
                            break;
                    };

                    SESSION_USED_HISTORY sess = new SESSION_USED_HISTORY
                    {
                        CLT_ID = cltID,
                        SRVC_CODE = sessionName.ToUpper(),
                        CRT_DATE = DateTime.Now
                    };

                    db.Entry(sess).State = EntityState.Added;

                    db.SaveChanges();
                    return "Y";
                }
                else
                {
                    MessageBox.Show("Client doesn't have any session left");
                    return "N";
                }
            }
            catch(Exception error)
            {
                MessageBox.Show(error.Message, "Error");
                return "N" ;
            }
        }
        
        public void GetClientData()
        {
            try
            {
                db = new EmpireBoxingEntities();

                var client = db.CLIENT_PROFILE.FirstOrDefault(r => r.CLT_ID == cltID);
                if (client != null)
                {
                    //Client Information
                    lblName.Content = client.FULL_NAME;
                    lblAge.Content = client.AGE;
                    lblGender.Content = client.GENDER;
                    lblAddress.Content = client.ADDRESS;
                    lblContactNo.Content = client.CONTACT_NO;
                    lblEmail.Content = client.EMAIL;
                    lblMedicalHist.Content = client.MED_RECORD;
                    lblMembershipDate.Content = client.MEMBERSHIP_DATE.ToShortDateString();
                    lblMembershipExpiry.Content = client.EXPIRATION_DATE.ToShortDateString();

                    string temp = '*' + client.CLT_ID + '*';

                    LabelCltID.Content = temp; // (*) to be able to read by the barcode

                    if (client.SESSION_EXPIRY != null)
                    {
                        grpBoxAvailService.Header = "Available Sessions [" + client.SESSION_EXPIRY.ToString() + "]";
                    }

                    if (client.FREE_SESSION_EXPIRY != null)
                    {
                        grpBoxFreeService.Header = "Free Sessions [" + client.FREE_SESSION_EXPIRY.ToString() + "]";
                    }

                    //Client Special Rates
                    txtBoxBoxingRate.Text = client.BOXING_RATE.ToString();
                    txtBoxMuayThaiRate.Text = client.MUAY_THAI_RATE.ToString();
                    txtBoxBassconRate.Text = client.BASSCON_RATE.ToString();
                    txtBoxMMARate.Text = client.MMA_RATE.ToString();

                    //Available Sessions
                    btnAvailBoxing.Content = client.BOXING.ToString();
                    btnAvailMuayThai.Content = client.MUAY_THAI.ToString();
                    btnAvailMMA.Content = client.MMA.ToString();
                    btnAvailBasscon.Content = client.BASSCON.ToString();

                    //Free Sessions
                    btnFreeBoxing.Content = client.FREE_BOXING.ToString();
                    btnFreeMuayThai.Content = client.FREE_MUAY_THAI.ToString();
                    btnFreeMMA.Content = client.FREE_MMA.ToString();
                    btnFreeBasscon.Content = client.FREE_BASSCON.ToString();

                    //Unlimited Session
                    if (client.UNLIMITED_EXPIRY != null && client.UNLIMITED_EXPIRY >= DateTime.Now)
                    {
                        btnUnlimited.IsEnabled = true;

                        DateTime tempDate = DateTime.Parse(client.UNLIMITED_EXPIRY.ToString());

                        lblUnlimitedExpiry.Content = tempDate.ToShortDateString();

                        btnUnlimited.Content = client.UNLIMITED_SRVC.ToString();
                    }

                    else//No unlimited session available
                    {
                        btnUnlimited.Content = "N/A";

                        btnUnlimited.IsEnabled = false;
                    }
                    //Client Type Convertions
                    if (client.CLT_TYPE == "O")
                        lblType.Content = "Ordinary";
                    else if (client.CLT_TYPE == "G")
                        lblType.Content = "Group";
                    else if (client.CLT_TYPE == "D")
                        lblType.Content = "Person with Disable";
                    else if (client.CLT_TYPE == "S")
                        lblType.Content = "Student";
                    else if (client.CLT_TYPE == "C")
                        lblType.Content = "Child";

                    //Profile Image
                    if (client.IMAGE_LOC != null && client.IMAGE_LOC != "")
                    {
                        try
                        {
                            BitmapImage logo = new BitmapImage();
                            logo.BeginInit();
                            logo.UriSource = new Uri(client.IMAGE_LOC);
                            logo.EndInit();
                            imageClient.Source = logo;
                        }
                        catch
                        {
                            MessageBox.Show("Can't load image. Please select a valid image format\n (.jpg | .jpeg | .png)", "Incorrect image format");
                        }
                    }
                }
            }
            catch(Exception error)
            {
                MessageBox.Show(error.Message, "Error");
            }
        }

        private void btnPack_Click(object sender, RoutedEventArgs e)
        {
            AddNewPack form = new AddNewPack(cltID);

            form.ShowDialog();

            GetClientData();
        }

        private void btnCash_Click(object sender, RoutedEventArgs e)
        {
            string type = "";
            if (DateTime.Parse(lblMembershipExpiry.Content.ToString())>= DateTime.Now) //Still a member
            {
                type = "MEMBER";
            }
            else //not a member
            {
                type = "";
            }
            AddSingleSession form = new AddSingleSession(cltID,"Cash",type, user);

            form.ShowDialog();

            GetClientData();
        }

        private void btnDropRate_Click(object sender, RoutedEventArgs e)
        {
            AddSingleSession form = new AddSingleSession(cltID, "Drop","", user);

            form.ShowDialog();

            GetClientData();
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog file = new OpenFileDialog();
        }

        private void button_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                OpenFileDialog fileDialog = new OpenFileDialog();

                fileDialog.Filter = "Image files (*.jpg, *.jpeg, *.jpe, *.jfif, *.png) | *.jpg; *.jpeg; *.jpe; *.jfif; *.png";

                fileDialog.Multiselect = false;

                string destinationFolder = @"C:\\EmpireMembershipPhotos";

                if (!Directory.Exists(destinationFolder))
                {
                    Directory.CreateDirectory(destinationFolder);
                }

                if ((bool)fileDialog.ShowDialog())
                {
                    if (fileDialog.OpenFile() != null)
                    {
                        try
                        {
                            EmpireBoxingEntities db = new EmpireBoxingEntities();

                            var clt = db.CLIENT_PROFILE.FirstOrDefault(r => r.CLT_ID == cltID);

                            clt.IMAGE_LOC = fileDialog.FileName;

                            db.SaveChanges();

                            GetClientData();
                        }
                        catch
                        {
                            MessageBox.Show("Can't load image. Please select a valid image format\n (.jpg | .jpeg | .png)", "Incorrect image format");
                        }
                    }
                }
            }
            catch(Exception error)
            {
                MessageBox.Show(error.Message, "Error");
            }
        }

        private void btnFree_Click(object sender, RoutedEventArgs e)
        {
                AddSingleSession form = new AddSingleSession(cltID, "Free", lblType.Content.ToString(), user);

                form.ShowDialog();
        }

        private void btnAvailBoxing_Click(object sender, RoutedEventArgs e)
        {
            if (UseSession(int.Parse(btnAvailBoxing.Content.ToString()), "Boxing") == "Y")
            {
                btnAvailBoxing.Content = (int.Parse(btnAvailBoxing.Content.ToString()) - 1).ToString();

                MessageBox.Show("Boxing Session Used!", "Successfully");
            }
            GetClientData();
        }

        private void btnAvailMuayThai_Click(object sender, RoutedEventArgs e)
        {
            if (UseSession(int.Parse(btnAvailMuayThai.Content.ToString()), "MuayThai") == "Y")
            {
                btnAvailMuayThai.Content = (int.Parse(btnAvailMuayThai.Content.ToString()) - 1).ToString();

                MessageBox.Show("Muay Thai Session Used!", "Successfully");
            }
            GetClientData();
        }

        private void btnAvailBasscon_Click(object sender, RoutedEventArgs e)
        {
            if (UseSession(int.Parse(btnAvailBasscon.Content.ToString()), "Basscon") == "Y")
            {
                btnAvailBasscon.Content = (int.Parse(btnAvailBasscon.Content.ToString()) - 1).ToString();

                MessageBox.Show("B@sscon Session Used!", "Successfully");
            }
            GetClientData();
        }

        private void btnAvailMMA_Click(object sender, RoutedEventArgs e)
        {
            if (UseSession(int.Parse(btnAvailMMA.Content.ToString()), "MMA") == "Y")
            {
                btnAvailMMA.Content = (int.Parse(btnAvailMMA.Content.ToString()) - 1).ToString();

                MessageBox.Show("MMA Session Used!", "Successfully");
            }
            GetClientData();

        }

        private void btnFreeBoxing_Click(object sender, RoutedEventArgs e)
        {
            if (UseSession(int.Parse(btnFreeBoxing.Content.ToString()), "FreeBoxing") == "Y")
            {
                btnFreeBoxing.Content = (int.Parse(btnFreeBoxing.Content.ToString()) - 1).ToString();

                MessageBox.Show("Free Boxing Session Used!", "Successfully");
            }
            GetClientData();
        }

        private void btnMember_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var clt = db.CLIENT_PROFILE.FirstOrDefault(q => q.CLT_ID == cltID);

                string sessCode = "";

                decimal rate;

                int age = 0;

                if (lblAge.Content.ToString() != "")
                    age = int.Parse(lblAge.Content.ToString());
                else
                    age = 30;

                if (age <= 17 ||
                    lblType.Content.ToString() == "Student / Senior Citizen / PWDs")
                {
                    db = new EmpireBoxingEntities();

                    var memFee = db.SESSION_RATE.FirstOrDefault(q => q.SRVC_CODE == "SMEMBERSHIP");

                    sessCode = memFee.SRVC_CODE;

                    rate = memFee.RATE;
                }
                else
                {
                    var memFee = db.SESSION_RATE.FirstOrDefault(q => q.SRVC_CODE == "MEMBERSHIP");

                    sessCode = memFee.SRVC_CODE;

                    rate = memFee.RATE;
                }

                MessageBoxResult result = MessageBox.Show("Membership Fee is PHP " + rate.ToString() + "\n Renew Membership?", "Membership Renewal", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    clt.MEMBERSHIP_DATE = DateTime.Now;

                    clt.EXPIRATION_DATE = DateTime.Now.AddYears(1); //Annual Membership Expiration

                    PAYMENT_HISTORY payment = new PAYMENT_HISTORY
                    {
                        CLT_ID = cltID,
                        CRT_DATE = DateTime.Now,
                        METHOD = "CASH",
                        SESSION_CODE = sessCode,
                        AMOUNT = rate
                    };
                    db.Entry(payment).State = EntityState.Added;
                    db.SaveChanges();
                }
                GetClientData();
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message, "Error");
            }
        }

        private void btnPrint_Click(object sender, RoutedEventArgs e)
        {
            PrintDialog printDialog = new PrintDialog();

            if(printDialog.ShowDialog() == true)
            {
                printDialog.PrintVisual(grid, lblName.Content.ToString());
            }
        }

        private void btnFreeMuayThai_Click(object sender, RoutedEventArgs e)
        {
            if (UseSession(int.Parse(btnFreeMuayThai.Content.ToString()), "FreeMuayThai") == "Y")
            {
                btnFreeMuayThai.Content = (int.Parse(btnFreeMuayThai.Content.ToString()) - 1).ToString();

                MessageBox.Show("Free Muay Thai Session Used!", "Successfully");
            }
            GetClientData();
        }

        private void btnFreeBasscon_Click_1(object sender, RoutedEventArgs e)
        {
            if (UseSession(int.Parse(btnFreeBasscon.Content.ToString()), "FreeBasscon") == "Y")
            {
                btnFreeBasscon.Content = (int.Parse(btnFreeBasscon.Content.ToString()) - 1).ToString();

                MessageBox.Show("Free B@sscon Session Used!", "Successfully");
            }
            GetClientData();
        }

        private void btnFreeMMA_Click(object sender, RoutedEventArgs e)
        {
            if (UseSession(int.Parse(btnFreeMMA.Content.ToString()), "FreeMMA") == "Y")
            {
                btnFreeMMA.Content = (int.Parse(btnFreeMMA.Content.ToString()) - 1).ToString();

                MessageBox.Show("Free MMA Session Used!", "Successfully");
            }
            GetClientData();
        }

        private void btnUnlimited_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                EmpireBoxingEntities db = new EmpireBoxingEntities();

                TIME_RECORD timeRec = new TIME_RECORD
                {
                    CLT_CODE = cltID,
                    TIME_IN = DateTime.Now,
                    SRVC_CODE = btnUnlimited.Content.ToString(),
                    CRT_BY = user
                };

                db.Entry(timeRec).State = EntityState.Added;

                db.SaveChanges();

                MessageBox.Show(btnUnlimited.Content.ToString() + " Used", "Successful");
            }
            catch
            {
                MessageBox.Show("Error on Using " + btnUnlimited.Content.ToString(), "Error");
            }
        }
    }
}
