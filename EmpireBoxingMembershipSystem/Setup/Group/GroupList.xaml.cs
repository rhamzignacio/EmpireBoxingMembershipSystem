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
    /// Interaction logic for GroupList.xaml
    /// </summary>
    public partial class GroupList : Window
    {
        public GroupList()
        {
            InitializeComponent();

            GenerateColumns();

            GetData();
        }

        private class GroupNameClass
        {
            public string GroupName { get; set; }
        }

        private void GenerateColumns()
        {
            DataGridTextColumn column = null;

            column = new DataGridTextColumn();
            column.Header = "Group/Corporate Name";
            column.Width = 400;
            column.Binding = new Binding("GroupName");
            groupDataGrid.Columns.Add(column);
        }

        private void GetData()
        {
            try
            {
                groupDataGrid.Items.Clear();

                EmpireBoxingEntities db = new EmpireBoxingEntities();

                var groupList = db.GROUP_CORPORATE_PROFILE.ToList();

                foreach (var item in groupList)
                {
                    GroupNameClass header = new GroupNameClass
                    {
                        GroupName = item.NAME
                    };

                    groupDataGrid.Items.Add(header);
                }
            }
            catch(Exception error)
            {
                MessageBox.Show(error.Message, "Error");
            }
        }

        private void btnNew_Click(object sender, RoutedEventArgs e)
        {
            OpenGroupList form = new OpenGroupList(""); //new

            form.ShowDialog();

            GetData();
        }

        private void btnView_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                GroupNameClass header = (GroupNameClass)groupDataGrid.SelectedItem;

                OpenGroupList form = new OpenGroupList(header.GroupName); //update

                form.ShowDialog();

                GetData();
            }
            catch //If no data selected on Grid
            {
                MessageBox.Show("Select Data from Grid", "Error");
            }
        }
    }
}
