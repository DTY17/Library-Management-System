using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using WPF.Dto;
using WPF.Utils;

namespace WPF.Repository
{
    internal class RecordRepository
    {
        public int GetNotReturnBookCount(DateTime today)
        {
            object result = DB.ExecuteScalar(
                "SELECT COUNT(*) FROM record WHERE duedate = @duedate",
                new MySqlParameter("@duedate", today.ToString("yyyy-MM-dd"))
            );
            return result != null ? Convert.ToInt32(result) : 0;
        }

        public List<RecordDto> GetAllRecords()
        {
            var records = new List<RecordDto>();
            using (var reader = DB.ExecuteReader("SELECT r.id , m.name , b.book, r.borrowdate, r.duedate, r.returndate, r.status FROM record AS r JOIN members AS m ON r.member = m.id JOIN books AS b ON r.book = b.id;"))
            {
                while (reader.Read())
                {
                    int col = reader.GetOrdinal("returndate");
                    records.Add(new RecordDto
                    {
                        Id = reader.GetInt32("id"),
                        Member = reader.GetString("name").ToString(),
                        Book = reader.GetString("book").ToString(),
                        BorrowDate = reader.GetDateTime("borrowdate"),
                        DueDate = reader.GetDateTime("duedate"),
                        ReturnDate = reader.IsDBNull(col) ? (DateTime?)null : reader.GetDateTime(col),
                        Status = reader.GetString("status")
                    });
                }
            }
            return records;
        }

        public bool SaveRecord(RecordDto record)
        {
            return DB.ExecuteNonQuery(
                "INSERT INTO record (member, book, borrowdate, duedate, returndate, status) VALUES (@member, @book, @borrowdate, @duedate, @returndate, @status)",
                new MySqlParameter("@member", Convert.ToInt32(record.Member)),
                new MySqlParameter("@book", Convert.ToInt32(record.Book)),
                new MySqlParameter("@borrowdate", record.BorrowDate),
                new MySqlParameter("@duedate", record.DueDate),
                new MySqlParameter("@returndate", record.ReturnDate ?? (object)DBNull.Value),
                new MySqlParameter("@status", record.Status)
            ) > 0;
        }

        public bool UpdateReturnStatus(RecordDto record)
        {
            return DB.ExecuteNonQuery(
                "UPDATE record SET returndate=@returndate, status=@status WHERE id=@id",
                new MySqlParameter("@returndate", record.ReturnDate ?? (object)DBNull.Value),
                new MySqlParameter("@status", record.Status),
                new MySqlParameter("@id", record.Id)
            ) > 0;
        }

        public bool DeleteRecord(int id)
        {
            return DB.ExecuteNonQuery(
                "DELETE FROM record WHERE id=@id",
                new MySqlParameter("@id", id)
            ) > 0;
        }

        public List<RecordDto> SearchRecords(string keyword)
        {
            var records = new List<RecordDto>();
            using (var reader = DB.ExecuteReader(
                "SELECT * FROM record WHERE member LIKE @keyword OR book LIKE @keyword",
                new MySqlParameter("@keyword", $"%{keyword}%")))
            {
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
            }
            return records;
        }
    }
}
