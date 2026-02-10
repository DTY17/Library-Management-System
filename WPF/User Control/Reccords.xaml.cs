using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using WPF.Dto;
using WPF.Model;

namespace WPF.User_Control
{
    public partial class Reccords : UserControl
    {
        private readonly RecordModel recordModel;
        private readonly MemberModel memberModel;
        private readonly BookModel bookModel;
        private int currentRecordId = 0;

        public Reccords()
        {
            InitializeComponent();
            recordModel = new RecordModel();
            memberModel = new MemberModel();
            bookModel = new BookModel();

            LoadData();
            LoadMemberCombo();
            LoadBookCombo();
        }

        private void btnBorrow_Click(object sender, RoutedEventArgs e)
        {
            if (ValidateInputs())
            {
                SaveData();
                LoadData();
            }
        }

        private void btnReturn_Click(object sender, RoutedEventArgs e)
        {
            SetReturnedStatus();
            LoadData();
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            DeleteData();
            LoadData();
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtSearch.Text))
            {
                LoadSearchData();
            }
            else
            {
                MessageBox.Show("Please enter a search keyword.");
            }
        }

        private void btnShowAll_Click(object sender, RoutedEventArgs e)
        {
            LoadData();
        }

        private void LoadData()
        {
            List<RecordDto> records = recordModel.GetData();
            RecordsTable.ItemsSource = records;
            ClearData();
            txtRecordCount.Text = $"{records.Count} records";
        }

        private void LoadSearchData()
        {
            List<RecordDto> records = recordModel.GetSearchData(txtSearch.Text);
            RecordsTable.ItemsSource = records;
            txtSearch.Clear();
            txtRecordCount.Text = $"{records.Count} records";
        }

        private void LoadMemberCombo()
        {
            List<MemberDto> memberList = memberModel.getMemberList();
            cmbMembers.ItemsSource = memberList;
            cmbMembers.DisplayMemberPath = "Name";
            cmbMembers.SelectedValuePath = "ID";
        }

        private void LoadBookCombo()
        {
            List<BookDto> bookList = bookModel.getBookList();
            cmbBooks.ItemsSource = bookList;
            cmbBooks.DisplayMemberPath = "Book";
            cmbBooks.SelectedValuePath = "Id";
        }

        private bool ValidateInputs()
        {
            if (cmbMembers.SelectedValue == null)
            {
                MessageBox.Show("Please select a member.");
                return false;
            }

            if (cmbBooks.SelectedValue == null)
            {
                MessageBox.Show("Please select a book.");
                return false;
            }

            if (dpBorrowDate.SelectedDate == null)
            {
                MessageBox.Show("Please select a borrow date.");
                return false;
            }

            if (dpDueDate.SelectedDate == null)
            {
                MessageBox.Show("Please select a due date.");
                return false;
            }

            return true;
        }

        private void SaveData()
        {
            int memberId = Convert.ToInt32(cmbMembers.SelectedValue);
            int bookId = Convert.ToInt32(cmbBooks.SelectedValue);

            bool isSaved = recordModel.SaveData(new RecordDto
            {
                Member = memberId.ToString(),
                Book = bookId.ToString(),
                BorrowDate = dpBorrowDate.SelectedDate ?? DateTime.Today,
                DueDate = dpDueDate.SelectedDate ?? DateTime.Today.AddDays(7),
                ReturnDate = null,
                Status = "Not Returned"
            });

            MessageBox.Show(isSaved ? "Saved Successfully" : "Failed to save data");
        }

        private void SetReturnedStatus()
        {
            bool isUpdated = recordModel.SetReturnedStatus(new RecordDto
            {
                Id = currentRecordId,
                ReturnDate = DateTime.Today,
                Status = "Returned"
            });

            MessageBox.Show(isUpdated ? "Marked as Returned" : "Failed to update status");
        }

        private void DeleteData()
        {
            bool isDeleted = recordModel.DeleteData(new RecordDto
            {
                Id = currentRecordId
            });

            MessageBox.Show(isDeleted ? "Deleted Successfully" : "Failed to delete data");
        }

        private void ClickRowRecordTable(object sender, MouseButtonEventArgs e)
        {
            btnBorrow.IsEnabled = false;
            btnReturn.IsEnabled = true;
            btnDelete.IsEnabled = true;

            var row = ItemsControl.ContainerFromElement(RecordsTable, e.OriginalSource as DependencyObject) as DataGridRow;
            if (row != null)
            {
                var item = row.Item as RecordDto;
                if (item == null) return;

                currentRecordId = (int)item.Id;
                cmbMembers.SelectedItem = item.Member;
                cmbBooks.SelectedItem = item.Book;
                dpBorrowDate.SelectedDate = item.BorrowDate;
                dpDueDate.SelectedDate = item.DueDate;
                Debug.WriteLine(item.Member);
            }
        }

        private void CancelBtnClick(object sender, RoutedEventArgs e)
        {
            ClearData();
        }

        private void ClearData()
        {
            currentRecordId = 0;
            cmbMembers.SelectedValue = null;
            cmbBooks.SelectedValue = null;
            dpBorrowDate.SelectedDate = null;
            dpDueDate.SelectedDate = null;

            btnBorrow.IsEnabled = true;
            btnReturn.IsEnabled = false;
            btnDelete.IsEnabled = false;
        }
    }
}
