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
using System.Windows.Navigation;
using System.Windows.Shapes;
using MySql.Data.MySqlClient;
using WPF.Dto;
using WPF.Model;

namespace WPF.User_Control
{
    public partial class Member : UserControl
    {
        private MemberModel memberModel;
        private int Id = 0;
        public Member()
        {
            memberModel = new MemberModel();
            InitializeComponent();
            loadTable();
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            saveData();
            clearData();
            loadTable();
        }

        private void saveData()
        {
            Boolean isSaved = memberModel.saveData(new Dto.MemberDto
            {
                Name = txtName.Text.ToString(),
                Email = txtEmail.Text.ToString(),
                Phone = txtPhone.Text.ToString(),
                NIC = txtNIC.Text.ToString(),
                Birthday = Convert.ToDateTime(dpBirthday.Text),
                Address = txtAddress.Text.ToString(),
            });
            if (isSaved)MessageBox.Show("Saved Successfully");
            else MessageBox.Show("Failed to save data");
        }

        private void updateData()
        {
            Boolean isUpdate = memberModel.updateData(new Dto.MemberDto
            {
                ID = this.Id,
                Name = txtName.Text.ToString(),
                Email = txtEmail.Text.ToString(),
                Phone = txtPhone.Text.ToString(),
                NIC = txtNIC.Text.ToString(),
                Birthday = dpBirthday.SelectedDate,
                Address = txtAddress.Text.ToString(),
            });
            if (isUpdate) MessageBox.Show("Updated Successfully");
            else MessageBox.Show("Failed to update data");
        }

        private void loadTable()
        {
            List<MemberDto> list = memberModel.getAllMemberList();
            MembersTable.ItemsSource = list;
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            updateData();
            clearData();
            loadTable();
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            deleteData();
            clearData();
            loadTable();
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            clearData();
            loadTable();
        }

        private void clearData()
        {
            this.Id = 0;
            txtName.Clear();
            txtEmail.Clear();
            txtPhone.Clear();
            txtNIC.Clear();
            dpBirthday.Text = string.Empty;
            txtAddress.Text = string.Empty;

            btnAdd.IsEnabled = true;
            btnUpdate.IsEnabled = false;
            btnDelete.IsEnabled = false;
            btnClear.IsEnabled = false;
        }

        private void deleteData()
        {
            Boolean isUpdate = memberModel.deleteData(new Dto.MemberDto
            {
                ID = this.Id,
            });
            if (isUpdate) MessageBox.Show("Deleted Successfully");
            else MessageBox.Show("Failed to delete data");
        }

        private void ClickMemberTable(object sender, MouseButtonEventArgs e)
        {

            btnAdd.IsEnabled = false;
            btnUpdate.IsEnabled = true;
            btnDelete.IsEnabled = true;
            btnClear.IsEnabled = true;

            var row = ItemsControl.ContainerFromElement(MembersTable, e.OriginalSource as DependencyObject) as DataGridRow;

            if (row != null)
            {
                var item = row.Item as MemberDto;
                Id = Convert.ToInt32(item?.ID);
                txtName.Text = item?.Name;
                txtEmail.Text = item?.Email;
                txtPhone.Text = item?.Phone;
                txtNIC.Text = item?.NIC;
                dpBirthday.Text = item?.Birthday.ToString();
                txtAddress.Text = item?.Address;

            }
        }
    }
}
