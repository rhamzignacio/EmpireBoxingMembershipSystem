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
    /// Interaction logic for AddSingleSession.xaml
    /// </summary>
    public partial class AddSingleSession : Window
    {
        string cltID = "";

        string type = "";

        string member = "";

        string user = "";

        bool ifUnlimited = false;

        public AddSingleSession(string cltID, string type, string member, string user)
        {
            InitializeComponent();

            this.cltID = cltID;

            this.type = type;

            this.member = member;

            this.user = user;

            GetSessionDropDown(type);
        }

        private void TimeIn(string serviceCode)
        {
            try
            {
                EmpireBoxingEntities db = new EmpireBoxingEntities();
                TIME_RECORD record = new TIME_RECORD
                {
                    CLT_CODE = cltID,
                    TIME_IN = DateTime.Now,
                    SRVC_CODE = serviceCode,
                    CRT_BY = user
                };
                db.Entry(record).State = EntityState.Added;
                db.SaveChanges();
            }
            catch(Exception error)
            {
                MessageBox.Show(error.Message, "Error");
            }

        }

        public void GetSessionDropDown(string type)
        {
            try
            {
                cmbBoxSession.Items.Clear();
                EmpireBoxingEntities db = new EmpireBoxingEntities();
                string temp = "";
                if (type == "Cash" || type == "Free")
                    temp = "S";
                else
                    temp = "D";

                if (member == "Child")
                {
                    temp = "C";
                    member = "";
                }
                else if (member == "Kids")
                {
                    temp = "K";
                    member = "";
                }

                if (member == "")
                {
                    var session = db.SESSION_RATE.Where(r => r.TYPE == temp).ToList();
                    foreach (var item in session)
                    {
                        if (item.SRVC_NAME.Contains("Member "))
                        {
                        }
                        else
                        {
                            ComboBoxItem cbxItem = new ComboBoxItem
                            {
                                Tag = item.SRVC_CODE,
                                Content = item.SRVC_NAME
                            };
                            cmbBoxSession.Items.Add(cbxItem);
                        }
                    }
                }
                else
                {
                    var ifGroup = db.GROUP_CORPORATE_MEMBERS.Where(r => r.CLT_ID == cltID);

                    var sessionMember = db.SESSION_RATE.Where(r => r.SRVC_NAME.Contains("Member "));

                    var sessionList = sessionMember.ToList();

                    foreach (var item in sessionList)
                    {
                        ComboBoxItem cbxItem = new ComboBoxItem
                        {
                            Tag = item.SRVC_CODE,
                            Content = item.SRVC_NAME
                        };
                        cmbBoxSession.Items.Add(cbxItem);
                    }

                    //Add Corporate Session Rate
                    if (ifGroup.ToList().Count > 0)
                    {
                        var sessionGroup = db.SESSION_RATE.Where(r => r.SRVC_CODE.Contains("GC") && !r.SRVC_NAME.Contains("Pack"));

                        foreach (var item in sessionGroup)
                        {
                            ComboBoxItem cbxItem = new ComboBoxItem
                            {
                                Tag = item.SRVC_CODE,
                                Content = item.SRVC_NAME
                            };
                            cmbBoxSession.Items.Add(cbxItem);
                        }
                    }

                    //Add Unlimited Session
                    var unlimited = db.SESSION_RATE.Where(r => r.SRVC_NAME.ToLower().Contains("unlimited"));

                    foreach (var item in unlimited)
                    {
                        ComboBoxItem cbxItem = new ComboBoxItem
                        {
                            Tag = item.SRVC_CODE,
                            Content = item.SRVC_NAME
                        };
                        cmbBoxSession.Items.Add(cbxItem);
                    }
                }
            }
            catch(Exception error)
            {
                MessageBox.Show(error.Message, "Error");
            }
        }

        private void cmbBoxSession_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            EmpireBoxingEntities db = new EmpireBoxingEntities();
            var rates = db.SESSION_RATE.FirstOrDefault(r => r.SRVC_CODE == ((ComboBoxItem)cmbBoxSession.SelectedItem).Tag.ToString());
            if(rates != null)
            {
                if (type != "Free")
                    txtBoxPrice.Text = rates.RATE.ToString();
                else
                    txtBoxPrice.Text = "0.00";

                if (rates.SRVC_NAME.ToLower().Contains("unlimited"))
                    ifUnlimited = true;
            }
            
        }

        private void btnOkay_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                EmpireBoxingEntities db = new EmpireBoxingEntities();

                PAYMENT_HISTORY payment = new PAYMENT_HISTORY();

                if (ifUnlimited)
                {
                    payment = new PAYMENT_HISTORY
                    {
                        METHOD = "Cash",
                        AMOUNT = decimal.Parse(txtBoxPrice.Text),
                        SESSION_CODE = ((ComboBoxItem)cmbBoxSession.SelectedItem).Tag.ToString(),
                        CRT_DATE = DateTime.Now,
                        CLT_ID = cltID
                    };

                    var clientProfile = db.CLIENT_PROFILE.FirstOrDefault(r => r.CLT_ID == cltID);

                    var days = db.SESSION_RATE.FirstOrDefault(r => r.SRVC_CODE
                        == ((ComboBoxItem)cmbBoxSession.SelectedItem).Tag.ToString());

                    clientProfile.UNLIMITED_EXPIRY = DateTime.Now.AddDays(double.Parse(days.VALID_DAYS.ToString()));

                    clientProfile.UNLIMITED_SRVC = ((ComboBoxItem)cmbBoxSession.SelectedItem).Tag.ToString();

                    db.Entry(clientProfile).State = EntityState.Modified;

                    db.SaveChanges();
                }
                else
                {
                    if (type == "Cash")
                    {
                        payment = new PAYMENT_HISTORY
                        {
                            METHOD = "Cash",
                            AMOUNT = decimal.Parse(txtBoxPrice.Text),
                            SESSION_CODE = ((ComboBoxItem)cmbBoxSession.SelectedItem).Tag.ToString(),
                            CRT_DATE = DateTime.Now,
                            CLT_ID = cltID
                        };
                    }
                    else if (type == "Free")
                    {
                        payment = new PAYMENT_HISTORY
                        {
                            METHOD = "Free",
                            AMOUNT = 0,
                            SESSION_CODE = ((ComboBoxItem)cmbBoxSession.SelectedItem).Tag.ToString(),
                            CRT_DATE = DateTime.Now,
                            CLT_ID = cltID
                        };
                    }
                }

                db.Entry(payment).State = EntityState.Added;

                db.SaveChanges();
                MessageBox.Show("Session: " + ((ComboBoxItem)cmbBoxSession.SelectedItem).Content.ToString() + "\nPrice: " + txtBoxPrice.Text, "Saved");

                TimeIn(((ComboBoxItem)cmbBoxSession.SelectedItem).Tag.ToString());

                this.Close();
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message, "Error");
            }
        }

        private void txtBoxPrice_TextChanged(object sender, TextChangedEventArgs e)
        {
            //Accepts only Numbers
            //Regex numericRegex = new Regex("[^0-9]+");

            //if (e.Handled = numericRegex.IsMatch(txtBoxPrice.Text) == true)
            //    txtBoxPrice.Text = "0";
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
