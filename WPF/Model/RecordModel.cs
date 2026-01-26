using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic;
using MySql.Data.MySqlClient;
using WPF.Dto;
using WPF.User_Control;

namespace WPF.Model
{
    internal class RecordModel
    {
        private string connectionString = "server=localhost;user id=root;password=Dinan@1234;database=LibraryMS";
        public RecordModel() { }
        public int getNotReturnBookCount()
        {
            DateTime today = DateTime.Today;

            int count = 0;
            MySqlConnection conn = new MySqlConnection(connectionString);
            conn.Open();
            string query = "SELECT COUNT(*) FROM record where duedate = @duedate";
            MySqlCommand cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@duedate", today.ToString("yyyy-MM-dd"));
            count = Convert.ToInt32(cmd.ExecuteScalar());
            return count;
        }

        public List<RecordDto> getData()
        {
            List<RecordDto> records = new();

            MySqlConnection conn = new MySqlConnection(connectionString);
            conn.Open();
            string query = "SELECT * FROM record";
            MySqlCommand cmd = new MySqlCommand(query, conn);
            MySqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                int col = reader.GetOrdinal("returndate");
                records.Add(new RecordDto
                {
                    Id = reader.GetInt32("id"),
                    Member = reader.GetInt32("member").ToString(),
                    Book = reader.GetInt32("book").ToString(),
                    BorrowDate = reader.GetDateTime("borrowdate"),
                    DueDate = reader.GetDateTime("duedate"),
                    ReturnDate = reader.IsDBNull(col) ? (DateTime?)null : reader.GetDateTime(col),
                    Status = reader.GetString("status")
                });
                
            }
            conn.Close();
            return records;
        }

        public Boolean saveData(RecordDto record)
        {
            Debug.WriteLine(">>> MEMBER SENT TO SQL = " + record.Member);
            Debug.WriteLine(">>> BOOK SENT TO SQL = " + record.Book);
            Debug.WriteLine("Save data record ");
            MySqlConnection conn = new MySqlConnection(connectionString);
            conn.Open();

            string query = "INSERT INTO record (member, book, borrowdate, duedate, returndate, status) VALUES (@member, @book, @borrowdate, @duedate, @returndate, @status)";
            MySqlCommand cmd = new MySqlCommand(query, conn);

            cmd.Parameters.AddWithValue("@member", Convert.ToInt32(record.Member));
            cmd.Parameters.AddWithValue("@book", Convert.ToInt32(record.Book));
            cmd.Parameters.AddWithValue("@borrowdate", record.BorrowDate);
            cmd.Parameters.AddWithValue("@duedate", record.DueDate);
            cmd.Parameters.AddWithValue("@returndate", record.ReturnDate); 
            cmd.Parameters.AddWithValue("@status", record.Status);

            int rows = cmd.ExecuteNonQuery();
            conn.Close();
            return rows > 0;
        }

        public Boolean MarkReturned()
        {
            return true;
        }

        public Boolean SetReturnedStatus(RecordDto record)
        {
            MySqlConnection conn = new MySqlConnection(connectionString);
            conn.Open();
            string query = "UPDATE record SET returndate = @returndate, status = @status WHERE id = @id";
            MySqlCommand cmd = new MySqlCommand(query, conn);

            cmd.Parameters.AddWithValue("@returndate", record.ReturnDate);
            cmd.Parameters.AddWithValue("@status", record.Status);
            cmd.Parameters.AddWithValue("@id", Convert.ToInt32(record.Id));

            return cmd.ExecuteNonQuery() > 0;
        }

        public Boolean DeleteData(RecordDto record)
        {
            MySqlConnection conn = new MySqlConnection(connectionString);
            conn.Open();
            string query = "DELETE FROM record WHERE id = @id";
            MySqlCommand cmd = new MySqlCommand(query, conn);

            cmd.Parameters.AddWithValue("@id", Convert.ToInt32(record.Id));

            return cmd.ExecuteNonQuery() > 0;
        }
        

        public List<RecordDto> getSearchData(string search)
        {
            List<RecordDto> records = new();

            MySqlConnection conn = new MySqlConnection(connectionString);
            conn.Open();
            string query = "SELECT * FROM record join members where member = @member.name";
            MySqlCommand cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@member", search);
            MySqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                int col = reader.GetOrdinal("returndate");
                records.Add(new RecordDto
                {
                    Id = reader.GetInt32("id"),
                    Member = reader.GetInt32("member").ToString(),
                    Book = reader.GetInt32("book").ToString(),
                    BorrowDate = reader.GetDateTime("borrowdate"),
                    DueDate = reader.GetDateTime("duedate"),
                    ReturnDate = reader.IsDBNull(col) ? (DateTime?)null : reader.GetDateTime(col),
                    Status = reader.GetString("status")
                });

            }
            conn.Close();
            return records;
        }
    }
}
