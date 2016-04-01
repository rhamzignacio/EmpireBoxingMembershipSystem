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


namespace EmpireBoxingMembershipSystem.Setup
{
    /// <summary>
    /// Interaction logic for NewUser.xaml
    /// </summary>
    public partial class NewUser : Window
    {
        string user = "";
        string userName = "";
        public NewUser(string _user, string _userName)
        {
            InitializeComponent();

            user = _user;

            userName = _userName;

            if (_userName != "")
            { //Update Info
                GetExistingData(_userName);
               
            }
        }
        
        private void GetExistingData(string _userName)
        {
            try
            {
                EmpireBoxingEntities db = new EmpireBoxingEntities();

                var user = db.USER_ACCOUNT.FirstOrDefault(r => r.USERNAME == _userName);

                txtBoxUsername.Text = user.USERNAME;

                txtBoxFirstName.Text = user.FIRST_NAME;

                txtBoxLastName.Text = user.LAST_NAME;

                txtBoxMiddleInitial.Text = user.MIDDLE_INITIAL;

                txtBoxPassword.Password = user.PASSWORD;

                if (user.ACCESS_LVL.ToLower() == "admin")
                    checkBoxAdmin.IsChecked = true;

                txtBoxUsername.IsEnabled = false; //Cannot change username
            }
            catch(Exception error)
            {
                MessageBox.Show(error.Message, "Error");
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            EmpireBoxingEntities db = new EmpireBoxingEntities();
            string message = "";

            string isAdmin = "";

            if (checkBoxAdmin.IsChecked == true)
                isAdmin = "admin";
            else
                isAdmin = "user";

            if (txtBoxFirstName.Text == "")
                message += "First name is required\n";

            if (txtBoxLastName.Text == "")
                message += "Last name is required\n";

            if (txtBoxUsername.Text == "")
                message += "Username is required\n";

            if (txtBoxPassword.Password == "")
                message += "Password is required\n";

            if (txtBoxConfirmPassword.Password == "")
                message += "Confirm Password is required\n";

            if (txtBoxConfirmPassword.Password != txtBoxPassword.Password)
                message += "Password not match\n";


            var ifExistingUsername = db.USER_ACCOUNT.FirstOrDefault(r => r.USERNAME == txtBoxUsername.Text);

            if (userName == "")
            { 
                if (ifExistingUsername == null)
                {
                    message += "Username already used";
                }
            }

            if(message == "")
            {
                if (txtBoxPassword.Password != txtBoxConfirmPassword.Password)
                    MessageBox.Show("Password not match", "Error on Password");
                else
                {
                    if(userName == "")
                    {
                        try
                        {
                            USER_ACCOUNT newUser = new USER_ACCOUNT
                            {
                                USERNAME = txtBoxUsername.Text,
                                PASSWORD = txtBoxPassword.Password,
                                CRT_BY = user,
                                ACCESS_LVL = isAdmin,
                                FIRST_NAME = txtBoxFirstName.Text,
                                LAST_NAME = txtBoxLastName.Text,
                                MIDDLE_INITIAL = txtBoxMiddleInitial.Text,
                                CRT_DATE = DateTime.Now
                            };

                            db.Entry(newUser).State = EntityState.Added;

                            db.SaveChanges();

                            MessageBox.Show("New User Saved", "Saved Successfully");

                            this.Close();
                        }
                        catch (Exception error)
                        {
                            MessageBox.Show(error.Message, "Error on Saving"); //Error on saving to Database
                        }                           

                        
                    }
                    else{
                        try {
                            ifExistingUsername.PASSWORD = txtBoxPassword.Password;
                            ifExistingUsername.FIRST_NAME = txtBoxFirstName.Text;
                            ifExistingUsername.LAST_NAME = txtBoxLastName.Text;
                            ifExistingUsername.MIDDLE_INITIAL = txtBoxMiddleInitial.Text;
                            ifExistingUsername.CRT_BY = user;
                            ifExistingUsername.CRT_DATE = DateTime.Now;
                            ifExistingUsername.ACCESS_LVL = isAdmin;

                            db.Entry(ifExistingUsername).State = EntityState.Modified;

                            db.SaveChanges();

                            MessageBox.Show("User Info Updated", "Updated Successfully");

                            this.Close();
                        }
                        catch(Exception error)
                        {
                            MessageBox.Show(error.Message, "Error on updating User Info"); //Error on updating to database
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show(message, "Error Saving"); //Throws error if there are missing fields
            }
        }
    }
}