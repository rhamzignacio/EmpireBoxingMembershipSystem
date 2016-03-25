using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using EmpireBoxingMembershipSystem.Setup.Group;
using System.Data.Entity;

namespace EmpireBoxingMembershipSystem.Setup.Group_Corporate
{
    /// <summary>
    /// Interaction logic for OpenGroupList.xaml
    /// </summary>
    public partial class OpenGroupList : Window
    {
        public string NameID = "";
        public string method = "EDIT";
        public bool isDelete = false;

        public class GroupName
        {
            public string Name { get; set; }
            public string NameID { get; set; }
        }

        public OpenGroupList(string name)

        {
            InitializeComponent();
            CreateTable();
            if (name == "") //New
            {
                method = "ADD";
                isDelete = false;
            }
            else //Edit
            {
                GetExistingData(name);
                method = "EDIT";
                isDelete = true;
            }
        }

        private void GetExistingData(string name)
        {
            EmpireBoxingEntities db = new EmpireBoxingEntities();
            var record = db.GROUP_CORPORATE_PROFILE.FirstOrDefault(r => r.NAME == name);
            var member = db.GROUP_CORPORATE_MEMBERS.Where(r => r.CORP_ID == record.ID);

            txtBoxName.Text = record.NAME;
            groupDataGrid.Items.Clear();
            foreach(var item in member)
            {
                var fName = db.CLIENT_PROFILE.FirstOrDefault(r => r.CLT_ID == item.CLT_ID);
                GroupName items = new GroupName
                {
                    NameID = item.CLT_ID,
                    Name = fName.FULL_NAME
                };
                groupDataGrid.Items.Add(items);
            }
        }

        private void CreateTable()
        {
            DataGridTextColumn column = null;

            column = new DataGridTextColumn();
            column.Header = "ID No.";
            column.Width = 120;
            column.Binding = new Binding("NameID");
            groupDataGrid.Columns.Add(column);

            column = new DataGridTextColumn();
            column.Header = "Name";
            column.Width = 300;
            column.Binding = new Binding("Name");
            groupDataGrid.Columns.Add(column);
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            Add_Name form = new Add_Name(this);

            form.ShowDialog();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void SaveItems() //Save Member from DataGrid
        {
            EmpireBoxingEntities db = new EmpireBoxingEntities();

            List<GroupName> gridItem = groupDataGrid.Items.Cast<GroupName>().ToList();

            var corpID = db.GROUP_CORPORATE_PROFILE.FirstOrDefault(q => q.NAME.ToUpper() == txtBoxName.Text.ToUpper());

            foreach (var item in gridItem)
            {
                var ifCltExist = db.GROUP_CORPORATE_MEMBERS.Where(r => r.CLT_ID == item.NameID);

                if (ifCltExist.ToList().Count == 0)
                {
                    GROUP_CORPORATE_MEMBERS member = new GROUP_CORPORATE_MEMBERS
                    {
                        CLT_ID = item.NameID,
                        CORP_ID = corpID.ID
                    };
                    db.Entry(member).State = EntityState.Added;
                }
                else
                {
                    MessageBox.Show(item.Name + "Already belong to a Group/Corporate");
                }
            }
            db.SaveChanges();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            EmpireBoxingEntities db = new EmpireBoxingEntities();

            List<GroupName> gridItem = groupDataGrid.Items.Cast<GroupName>().ToList();

            if(method == "ADD")
            { //Add New
                try {
                    if (gridItem.Count() != 0)
                    {
                        //Save Group Header
                        var temp = db.GROUP_CORPORATE_PROFILE.Where(q => q.NAME.ToUpper() == txtBoxName.Text.ToUpper()).ToList();

                        if (temp.Count == 0)
                        {
                            GROUP_CORPORATE_PROFILE gProfile = new GROUP_CORPORATE_PROFILE
                            {
                                NAME = txtBoxName.Text,
                                TYPE = "G",
                                CRT_DATE = DateTime.Now
                            };

                            db.Entry(gProfile).State = EntityState.Added;

                            db.SaveChanges();

                            SaveItems(); //Save Members
                        }
                        else
                        {
                            MessageBox.Show("Group/Corporate Name already exist!", "Error on Saving");
                            return;
                        }
                    }
                }
                catch(Exception error)
                {
                    MessageBox.Show(error.Message, "Error on Saving");
                    return;
                }        
            }
            else //Edit
            {
                try {
                    db = new EmpireBoxingEntities();

                    if (isDelete)    //Delete All existing Members
                    {
                        var corpID = db.GROUP_CORPORATE_PROFILE.FirstOrDefault(r => r.NAME == txtBoxName.Text); //Get Corporate ID

                        var deleteMember = db.GROUP_CORPORATE_MEMBERS.Where(r=>r.CORP_ID == corpID.ID).ToList();//Delete all member

                        foreach (var mem in deleteMember)
                        {
                            db.Entry(mem).State = EntityState.Deleted;
                        }

                        db.SaveChanges();
                    }

                    SaveItems();  //Save Members
                }
                catch(Exception error)
                {
                    MessageBox.Show(error.Message, "Error on Saving");
                    return;
                }
            }
            MessageBox.Show("Succesfully Saved");

            this.Close();
        }

        private void btnRemove_Click(object sender, RoutedEventArgs e)
        {
            isDelete = true;

            groupDataGrid.Items.Remove(groupDataGrid.SelectedItem); //Remove selected data
        }
    }
}
