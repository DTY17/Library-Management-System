using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using MySql.Data.MySqlClient;
using WPF.Dto;
using WPF.User_Control;

namespace WPF.Model
{
    internal class BookModel
    {
        private readonly string connectionString = "server=localhost;user id=root;password=Dinan@1234;database=LibraryMS;";

        public BookModel() {}

        public List<BookDto> getBookData()
        {
            List<BookDto> records = new ();

            MySqlConnection conn = new MySqlConnection(connectionString);
            conn.Open();
            string query = "SELECT id, book, author, genre , available FROM books";
            MySqlCommand cmd = new MySqlCommand(query, conn);
            MySqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                records.Add(new BookDto
                {
                    Id = reader.GetInt32("id").ToString(),
                    Book = reader.GetString("book"),
                    Author = reader.GetString("author"),
                    Genre = reader.GetString("genre"),
                    Available = reader.GetInt32("available").ToString()
                });
            }
            return records;
        }

        public Boolean saveData(BookDto book)
        {
            MySqlConnection conn = new MySqlConnection(connectionString);
            conn.Open();

            string query = "INSERT INTO books (book, author, genre, available) VALUES(@book, @author, @genre, @available)";

            MySqlCommand cmd = new MySqlCommand(query, conn);

            cmd.Parameters.AddWithValue("@book", book.Book.ToString());
            cmd.Parameters.AddWithValue("@author", book.Author);
            cmd.Parameters.AddWithValue("@genre", book.Genre);
            cmd.Parameters.AddWithValue("@available", Convert.ToInt32(book.Available));

            int rows = cmd.ExecuteNonQuery();  

            if (rows > 0)
            {
                return true;
            }else
            {
                return false;
            }  
        }

        public int getBookCount()
        {
            int count = 0;
            MySqlConnection conn = new MySqlConnection(connectionString);
            conn.Open();
            string query = "SELECT COUNT(*) FROM books";
            MySqlCommand cmd = new MySqlCommand(query, conn);
            count = Convert.ToInt32(cmd.ExecuteScalar());
            return count;
        }

        public List<BookDto> getBookList()
        {
            List<BookDto> records = new();

            MySqlConnection conn = new MySqlConnection(connectionString);
            conn.Open();
            string query = "SELECT book,id FROM books";
            MySqlCommand cmd = new MySqlCommand(query, conn);
            MySqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                records.Add(new BookDto
                {
                    Book = reader.GetString("book"),
                    Id = reader.GetInt32("id").ToString()
                });
            }
            return records;
        }

        public Boolean updateData(BookDto book)
        {
            MySqlConnection conn = new MySqlConnection(connectionString);
            conn.Open();
            string query = "UPDATE books SET book = @book, author = @author , genre = @genre , available = @available where id = @id";
            MySqlCommand cmd = new MySqlCommand(query, conn);

            cmd.Parameters.AddWithValue("@book", book.Book.ToString());
            cmd.Parameters.AddWithValue("@author", book.Author);
            cmd.Parameters.AddWithValue("@genre", book.Genre);
            cmd.Parameters.AddWithValue("@available", Convert.ToInt32(book.Available));
            cmd.Parameters.AddWithValue("@id", Convert.ToInt32(book.Id));

            return cmd.ExecuteNonQuery() > 0;
        }

        public Boolean deleteData(BookDto book)
        {
            MySqlConnection conn = new MySqlConnection(connectionString);
            conn.Open();
            string query = "DELETE FROM books where id = @id";
            MySqlCommand cmd = new MySqlCommand(query, conn);

            cmd.Parameters.AddWithValue("@id", Convert.ToInt32(book.Id));

            return cmd.ExecuteNonQuery() > 0;
        }
    }
}
