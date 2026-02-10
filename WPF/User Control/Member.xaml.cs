using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
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
            if (ValidateInputs())
            {
                saveData();
                clearData();
                loadTable();
            }
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (ValidateInputs())
            {
                updateData();
                clearData();
                loadTable();
            }
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

        private bool ValidateInputs()
        {
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("Name is required.");
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtEmail.Text))
            {
                MessageBox.Show("Email is required.");
                return false;
            }

            if (!txtEmail.Text.Contains("@"))
            {
                MessageBox.Show("Invalid email format.");
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtPhone.Text))
            {
                MessageBox.Show("Phone number is required.");
                return false;
            }

            if (!long.TryParse(txtPhone.Text, out _))
            {
                MessageBox.Show("Phone number must be numeric.");
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtNIC.Text))
            {
                MessageBox.Show("NIC is required.");
                return false;
            }

            if (dpBirthday.SelectedDate == null)
            {
                MessageBox.Show("Birthday must be selected.");
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtAddress.Text))
            {
                MessageBox.Show("Address is required.");
                return false;
            }

            return true;
        }

        private void saveData()
        {
            bool isSaved = memberModel.saveData(new MemberDto
            {
                Name = txtName.Text,
                Email = txtEmail.Text,
                Phone = txtPhone.Text,
                NIC = txtNIC.Text,
                Birthday = dpBirthday.SelectedDate,
                Address = txtAddress.Text
            });

            MessageBox.Show(isSaved ? "Saved Successfully" : "Failed to save data");
        }

        private void updateData()
        {
            bool isUpdate = memberModel.updateData(new MemberDto
            {
                ID = this.Id,
                Name = txtName.Text,
                Email = txtEmail.Text,
                Phone = txtPhone.Text,
                NIC = txtNIC.Text,
                Birthday = dpBirthday.SelectedDate,
                Address = txtAddress.Text
            });

            MessageBox.Show(isUpdate ? "Updated Successfully" : "Failed to update data");
        }

        private void deleteData()
        {
            bool isDeleted = memberModel.deleteData(new MemberDto
            {
                ID = this.Id
            });

            MessageBox.Show(isDeleted ? "Deleted Successfully" : "Failed to delete data");
        }

        private void loadTable()
        {
            List<MemberDto> list = memberModel.getAllMemberList();
            MembersTable.ItemsSource = list;
        }

        private void clearData()
        {
            this.Id = 0;
            txtName.Clear();
            txtEmail.Clear();
            txtPhone.Clear();
            txtNIC.Clear();
            dpBirthday.SelectedDate = null;
            txtAddress.Clear();

            btnAdd.IsEnabled = true;
            btnUpdate.IsEnabled = false;
            btnDelete.IsEnabled = false;
            btnClear.IsEnabled = false;
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
                if (item == null) return;

                Id = (int)item.ID;
                txtName.Text = item.Name;
                txtEmail.Text = item.Email;
                txtPhone.Text = item.Phone;
                txtNIC.Text = item.NIC;
                dpBirthday.SelectedDate = item.Birthday;
                txtAddress.Text = item.Address;
            }
        }
    }
}
