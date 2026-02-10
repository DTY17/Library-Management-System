using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
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
            if (ValidateInputs())
            {
                SaveData();
                LoadData();
                ClearData();
            }
        }

        private void UpdateBookBtn_Click(object sender, RoutedEventArgs e)
        {
            if (ValidateInputs())
            {
                UpdateData();
                ClearData();
                LoadData();
            }
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
                Id = Convert.ToInt32(item?.Id);
                BookBookInput.Text = item?.Book;
                BookAuhtorInput.Text = item?.Author;
                BookGenreInput.Text = item?.Genre;
                BookAvailableInput.Text = item?.Available.ToString();
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

        private bool ValidateInputs()
        {
            if (string.IsNullOrWhiteSpace(BookBookInput.Text))
            {
                MessageBox.Show("Book name is required.");
                return false;
            }

            if (string.IsNullOrWhiteSpace(BookAuhtorInput.Text))
            {
                MessageBox.Show("Author name is required.");
                return false;
            }

            if (string.IsNullOrWhiteSpace(BookGenreInput.Text))
            {
                MessageBox.Show("Genre is required.");
                return false;
            }

            if (string.IsNullOrWhiteSpace(BookAvailableInput.Text))
            {
                MessageBox.Show("Availability must be specified (e.g., 0 or 1).");
                return false;
            }

            if (!int.TryParse(BookAvailableInput.Text, out _))
            {
                MessageBox.Show("Availability must be a number.");
                return false;
            }

            return true;
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

            MessageBox.Show(isSaved ? "Saved Successfully" : "Failed to save data");
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

            MessageBox.Show(isUpdate ? "Updated Successfully" : "Failed to update data");
        }

        private void DeleteData()
        {
            var isDeleted = bookModel.deleteData(new BookDto
            {
                Id = this.Id.ToString(),
            });

            MessageBox.Show(isDeleted ? "Deleted Successfully" : "Failed to delete data");
        }
    }
}
