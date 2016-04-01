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
using EmpireBoxingMembershipSystem.Setup.Group_Corporate;

namespace EmpireBoxingMembershipSystem.Setup.Group
{
    /// <summary>
    /// Interaction logic for Add_Name.xaml
    /// </summary>
    public partial class Add_Name : Window
    {
        OpenGroupList parentForm;
        public Add_Name(OpenGroupList form)
        {
            InitializeComponent();

            parentForm = form;
        }

        private void AddClient()//Add Client to the list
        {
            try
            {
                EmpireBoxingEntities db = new EmpireBoxingEntities();

                var clt = db.CLIENT_PROFILE.Where(r => r.CLT_ID == txtBoxClientID.Text).ToList();

                if (clt.Count == 0) //If invalid ID No.
                {
                    MessageBox.Show("Invalid ID No.", "Error");
                }

                var grpMember = db.GROUP_CORPORATE_MEMBERS.Where(r => r.CLT_ID == txtBoxClientID.Text).ToList().Count;

                if (grpMember > 0) //If member already belong to a Group/Corporate
                {
                    MessageBox.Show("Client already belong to a Group/Corporate Account", "Error on Saving");
                }
                else
                {

                    bool noDuplicate = true;

                    List<OpenGroupList.GroupName> gridItem = parentForm.groupDataGrid.Items.Cast<OpenGroupList.GroupName>().ToList();

                    foreach (var item in gridItem)
                    {
                        if (item.NameID == txtBoxClientID.Text)
                            noDuplicate = false;
                    }

                    if (noDuplicate)
                    {
                        var client = db.CLIENT_PROFILE.FirstOrDefault(r => r.CLT_ID == txtBoxClientID.Text);

                        OpenGroupList.GroupName gName = new OpenGroupList.GroupName
                        {
                            Name = client.FULL_NAME,
                            NameID = client.CLT_ID
                        };

                        parentForm.groupDataGrid.Items.Add(gName);

                        txtBoxClientID.Text = "";
                    }
                    else
                    {
                        MessageBox.Show("Already added on the list");
                    }
                }
            }
            catch(Exception error)
            {
                MessageBox.Show(error.Message, "Error");
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnOkay_Click(object sender, RoutedEventArgs e)
        {
            AddClient();
        }

        private void txtBoxClientID_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                AddClient();
        }
    }
}
