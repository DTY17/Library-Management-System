using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using WPF.Dto;
using WPF.User_Control;
using WPF.Utils;

namespace WPF.Repository
{
    internal class BookRepository
    {
        public List<BookDto> getBookDataRepo()
        {
            List<BookDto> records = new();
            using (var reader = DB.ExecuteReader("SELECT id, book, author, genre, available FROM books"))
            {
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
            }
            return records;
        }
        public bool saveDataRepo(BookDto book)
        {
            bool isSaved = false;

            int rowsAffected = DB.ExecuteNonQuery(
                "INSERT INTO books (book, author, genre, available) VALUES(@book, @author, @genre, @available)",
                new MySqlParameter("@book", book.Book),
                new MySqlParameter("@author", book.Author),
                new MySqlParameter("@genre", book.Genre),
                new MySqlParameter("@available", Convert.ToInt32(book.Available))
            );

            if (rowsAffected > 0)
            {
                isSaved = true;
            }
            return isSaved;
        }

        public int getBookCountRepo()
        {
            int count = 0;
            object result = DB.ExecuteScalar(
                "SELECT COUNT(*) FROM books"
            );

            if (result != null && int.TryParse(result.ToString(), out int parsed))
            {
                count = parsed;
            }
            return count;
        }

        public List<BookDto> getBookListRepo() {
            List<BookDto> records = new();
            using (var reader = DB.ExecuteReader("SELECT book,id FROM books"))
            {
                while (reader.Read())
                {
                    records.Add(new BookDto
                    {
                        Id = reader.GetInt32("id").ToString(),
                        Book = reader.GetString("book"),
                    });
                }
            }
            return records;
        }

        public Boolean updateDataRepo(BookDto book)
        {
            bool isSaved = false;

            int rowsAffected = DB.ExecuteNonQuery(
                "UPDATE books SET book = @book, author = @author , genre = @genre , available = @available where id = @id",
                new MySqlParameter("@book", book.Book),
                new MySqlParameter("@author", book.Author),
                new MySqlParameter("@genre", book.Genre),
                new MySqlParameter("@available", Convert.ToInt32(book.Available)),
                new MySqlParameter("@id", book.Id)
            );

            if (rowsAffected > 0)
            {
                isSaved = true;
            }
            return isSaved;
        }

        public Boolean deleteDataRepo(BookDto book)
        {
            bool isSaved = false;

            int rowsAffected = DB.ExecuteNonQuery(
                "DELETE FROM books where id = @id",
                new MySqlParameter("@id", book.Id)
            );

            if (rowsAffected > 0)
            {
                isSaved = true;
            }
            return isSaved;
        }
    }
}
