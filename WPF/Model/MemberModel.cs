using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using WPF.Dto;

namespace WPF.Model
{
    internal class MemberModel
    {
        private readonly string connectionString = "server=localhost;user id=root;password=Dinan@1234;database=LibraryMS;";
        public MemberModel() { }
        public int getMemberCount()
        {
            int count = 0;
            MySqlConnection conn = new MySqlConnection(connectionString);
            conn.Open();
            string query = "SELECT COUNT(*) FROM Members";
            MySqlCommand cmd = new MySqlCommand(query, conn);
            count = Convert.ToInt32(cmd.ExecuteScalar());
            return count;
        }

        public List<MemberDto> getMemberList()
        {
            List<MemberDto> records = new();

            MySqlConnection conn = new MySqlConnection(connectionString);
            conn.Open();
            string query = "SELECT name,id FROM members";
            MySqlCommand cmd = new MySqlCommand(query, conn);
            MySqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                records.Add(new MemberDto
                {
                    Name = reader.GetString("name"),
                    ID = reader.GetInt32("id")
                });
            }
            return records;
        }

        public Boolean saveData(MemberDto member)
        {
            MySqlConnection conn = new MySqlConnection(connectionString);
            conn.Open();

            string query = "INSERT INTO members (name, email, phone, nic, birthday, address) VALUES(@name, @email, @phone, @nic, @birthday, @address)";
            MySqlCommand cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@name", member.Name.ToString());
            cmd.Parameters.AddWithValue("@email", member.Email);
            cmd.Parameters.AddWithValue("@phone", member.Phone);
            cmd.Parameters.AddWithValue("@nic", member.NIC);
            cmd.Parameters.AddWithValue("@birthday", member.Birthday);
            cmd.Parameters.AddWithValue("@address", member.Address);

            int rows = cmd.ExecuteNonQuery();

            if (rows > 0)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        public List<MemberDto> getAllMemberList()
        {
            List<MemberDto> records = new();

            MySqlConnection conn = new MySqlConnection(connectionString);
            conn.Open();
            string query = "SELECT * FROM members";
            MySqlCommand cmd = new MySqlCommand(query, conn);
            MySqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                records.Add(new MemberDto{
                    ID = reader.GetInt32("id"),
                    Name = reader.GetString("name"),
                    Email = reader.GetString("email"),
                    Phone = reader.GetString("phone"),
                    NIC = reader.GetString("NIC"),
                    Birthday = reader.GetDateTime("birthday")
                });
            }
            return records;
        }

        public Boolean updateData(MemberDto member)
        {
            MySqlConnection conn = new MySqlConnection(connectionString);
            conn.Open();

            string query = "UPDATE members SET name = @name, email = @email, phone = @phone, nic = @nic, birthday = @birthday where id = @id";
            MySqlCommand cmd = new MySqlCommand(query, conn);

            cmd.Parameters.AddWithValue("@name", member.Name);
            cmd.Parameters.AddWithValue("@email", member.Email);
            cmd.Parameters.AddWithValue("@phone", member.Phone);
            cmd.Parameters.AddWithValue("@nic", member.NIC);
            cmd.Parameters.AddWithValue("@birthday", Convert.ToDateTime(member.Birthday));
            cmd.Parameters.AddWithValue("@id", member.ID);

            return cmd.ExecuteNonQuery() > 0;
        }

        public Boolean deleteData(MemberDto member)
        {
            MySqlConnection conn = new MySqlConnection(connectionString);
            conn.Open();

            string query = "DELETE FROM members where id = @id";
            MySqlCommand cmd = new MySqlCommand(query, conn);

            cmd.Parameters.AddWithValue("@id", member.ID);

            return cmd.ExecuteNonQuery() > 0;
        }
    }
}
