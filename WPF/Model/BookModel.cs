using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using MySql.Data.MySqlClient;
using WPF.Dto;
using WPF.Repository;
using WPF.User_Control;

namespace WPF.Model
{
    internal class BookModel
    {
        private BookRepository bookRepo;

        public BookModel() {

            this.bookRepo = new();
        }

        public List<BookDto> getBookData()
        {
            return bookRepo.getBookDataRepo();
        }

        public Boolean saveData(BookDto book)
        {
            return bookRepo.saveDataRepo(book);
        }

        public int getBookCount()
        {
            return bookRepo.getBookCountRepo();
        }

        public List<BookDto> getBookList()
        {
            return bookRepo.getBookListRepo();
        }

        public Boolean updateData(BookDto book)
        {
            return bookRepo.updateDataRepo(book);
        }

        public Boolean deleteData(BookDto book)
        {
            return bookRepo.deleteDataRepo(book);
        }
    }
}
