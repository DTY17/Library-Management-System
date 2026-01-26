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
using WPF.Dto;
using WPF.Model;

namespace WPF.User_Control
{
    public partial class Books : UserControl
    {
        private BookModel bookModel;
        private int Id = 0;
        public Books()
        {
            bookModel = new BookModel();
            InitializeComponent();
            LoadData();
        }

        private void AddBookBtn_Click(object sender, RoutedEventArgs e)
        {
            SaveData();
            LoadData();
            ClearData();
        }

        private void UpdateBookBtn_Click(object sender, RoutedEventArgs e)
        {
            UpdateData();
            ClearData();
            LoadData();
        }

        private void DeleteBookBtn_Click(object sender, RoutedEventArgs e)
        {
            DeleteData();
            ClearData();
            LoadData();
        }
        private void CancelBookBtn_Click(object sender, RoutedEventArgs e)
        {
            ClearData();
            LoadData();
        }

        private void ClickRowTable(object sender, MouseButtonEventArgs e)
        {
            AddBookBtn.IsEnabled = false;
            UpdateBookBtn.IsEnabled = true;
            DeleteBookBtn.IsEnabled = true;
            CancelBookBtn.IsEnabled = true;

            var row = ItemsControl.ContainerFromElement(BooksTable, e.OriginalSource as DependencyObject) as DataGridRow;

            if (row != null)
            {
                var item = row.Item as BookDto;
                Id = Convert.ToInt32( item?.Id );
                BookBookInput.Text = item?.Book;
                BookAuhtorInput.Text = item?.Author;
                BookGenreInput.Text = item?.Genre;
                BookAvailableInput.Text = item?.Available;
            }
        }

        private void LoadData()
        {
            List<BookDto> bookList = bookModel.getBookData();
            BooksTable.ItemsSource = bookList;
            ClearData();
        }

        private void ClearData()
        {
            BookBookInput.Clear();
            BookAuhtorInput.Clear();
            BookGenreInput.Clear();
            BookAvailableInput.Clear();
            this.Id = 0;

            AddBookBtn.IsEnabled = true;
            UpdateBookBtn.IsEnabled = false;
            DeleteBookBtn.IsEnabled = false;
            CancelBookBtn.IsEnabled = false;
        }

        private void SaveData()
        {
            var isSaved = bookModel.saveData(new BookDto
            {
                Book = BookBookInput.Text,
                Author = BookAuhtorInput.Text,
                Genre = BookGenreInput.Text,
                Available = BookAvailableInput.Text
            });

            if (isSaved) MessageBox.Show("Saved Successfully");
            else MessageBox.Show("Failed to save data");
 
        }

        private void UpdateData()
        {
            var isUpdate = bookModel.updateData(new BookDto
            {
                Id = this.Id.ToString(),
                Book = BookBookInput.Text,
                Author = BookAuhtorInput.Text,
                Genre = BookGenreInput.Text,
                Available = BookAvailableInput.Text

            });

            if (isUpdate) MessageBox.Show("Updated Successfully");
            else MessageBox.Show("Failed to update data");
        }

        private void DeleteData()
        {
            var isUpdate = bookModel.deleteData(new BookDto
            {
                Id = this.Id.ToString()
            });

            if (isUpdate) MessageBox.Show("Delete Successfully");
            else MessageBox.Show("Failed to delete data");
        }
    }
}
