using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using WPF.Dto;
using WPF.Model;

namespace WPF.User_Control
{
    public partial class Reccords : UserControl
    {
        private RecordModel record;
        private MemberModel member;
        private BookModel book;
        private int id = 0;
        public Reccords()
        {
            record = new RecordModel();
            book = new BookModel();
            member = new MemberModel();
            InitializeComponent();
            loadData();
            loadMemberCombo();
            loadBookCombo();
        }

        private void btnBorrow_Click(object sender, RoutedEventArgs e)
        {
            SavedData();
            loadData();
        }

        private void btnReturn_Click(object sender, RoutedEventArgs e)
        {
            SetReturnedStatus();
            loadData();
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            DeleteData();
            loadData();
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            if (txtSearch.Text != null && txtSearch.Text.Trim() != "")
            {
                loadSearchData();
            }

        }

        private void btnShowAll_Click(object sender, RoutedEventArgs e)
        {
            loadData();
        }

        private void loadData()
        {
            List<RecordDto> records = record.getData();
            RecordsTable.ItemsSource = records;
            ClearData();
        }

        private void loadSearchData()
        {
            List<RecordDto> records = record.getSearchData(txtSearch.Text);
            RecordsTable.ItemsSource = records;
            txtSearch.Clear();
        }

        private void loadMemberCombo()
        {
            List<MemberDto> memberList = member.getMemberList();
            cmbMembers.DisplayMemberPath = "Name";
            cmbMembers.SelectedValuePath = "ID";
            cmbMembers.ItemsSource = memberList;

            //Debug.WriteLine(memberList[0].ID);
        }

        private void loadBookCombo()
        {
            List<BookDto> bookList = book.getBookList();
            cmbBooks.DisplayMemberPath = "Book";
            cmbBooks.SelectedValuePath = "Id";
            cmbBooks.ItemsSource = bookList;

            //Debug.WriteLine(bookList[0].Id);
        }

        private void SavedData()
        {
            DateTime today = DateTime.Today;
            int memberId = Convert.ToInt32(cmbMembers.SelectedValue);
            int bookId = Convert.ToInt32(cmbBooks.SelectedValue);

            Boolean isSaved = record.saveData(new RecordDto
            {
                Member = memberId.ToString(),
                Book = bookId.ToString(),
                BorrowDate = Convert.ToDateTime(dpBorrowDate.Text),
                ReturnDate = Convert.ToDateTime("2000-01-22"),
                DueDate = Convert.ToDateTime(dpDueDate.Text),
                Status = "Not Returned"
            });

            if (isSaved) MessageBox.Show("Saved Successfully");
            else MessageBox.Show("Failed to save data");
        }

        private void SetReturnedStatus()
        {
            DateTime today = DateTime.Today;
            Console.WriteLine(Convert.ToDateTime(today));
            Boolean isSaved = record.SetReturnedStatus(new RecordDto
            {
                Id = this.id,
                ReturnDate = Convert.ToDateTime(today),
                Status = "Returned"
            });
            if (isSaved) MessageBox.Show("Saved Successfully");
            else MessageBox.Show("Failed to save data");
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
                id = Convert.ToInt32(item?.Id);
                cmbMembers.SelectedValue = item?.Member;
                cmbBooks.SelectedValue = item?.Book;
                dpBorrowDate.SelectedDate = item?.BorrowDate;
                dpDueDate.SelectedDate = item?.DueDate;
            }
        }

        private void CancelBtnClick(object sender, RoutedEventArgs e)
        {
            ClearData();
        }

        private void ClearData()
        {
            btnBorrow.IsEnabled = true;
            btnReturn.IsEnabled = false;
            btnDelete.IsEnabled = false;

            cmbMembers.SelectedValue = null;
            cmbBooks.SelectedValue = null;
            dpBorrowDate.SelectedDate = null;
            dpDueDate.SelectedDate = null;
        }

        private void DeleteData()
        {
            Boolean isDelete = record.DeleteData( new RecordDto
            {
                Id = id
            });

            if (isDelete) MessageBox.Show("Deleted Successfully");
            else MessageBox.Show("Failed to delete data");
        }
    }
}
