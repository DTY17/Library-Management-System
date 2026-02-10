using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using WPF.Dto;
using WPF.Utils;

namespace WPF.Repository
{
    internal class MemberRepository
    {
        public int getMemberCountRepo()
        {
            return Convert.ToInt32(DB.ExecuteScalar("SELECT COUNT(*) FROM members"));
        }

        public List<MemberDto> getMemberListRepo()
        {
            var records = new List<MemberDto>();
            using (var reader = DB.ExecuteReader("SELECT name, id FROM members"))
            {
                while (reader.Read())
                {
                    records.Add(new MemberDto
                    {
                        ID = reader.GetInt32("id"),
                        Name = reader.GetString("name")
                    });
                }
            }
            return records;
        }

        public bool saveDataRepo(MemberDto member)
        {
            return DB.ExecuteNonQuery(
                "INSERT INTO members (name, email, phone, nic, birthday, address) VALUES(@name, @email, @phone, @nic, @birthday, @address)",
                new MySqlParameter("@name", member.Name),
                new MySqlParameter("@email", member.Email),
                new MySqlParameter("@phone", member.Phone),
                new MySqlParameter("@nic", member.NIC),
                new MySqlParameter("@birthday", member.Birthday),
                new MySqlParameter("@address", member.Address)
            ) > 0;
        }

        public List<MemberDto> getAllMemberListRepo()
        {
            var records = new List<MemberDto>();
            using (var reader = DB.ExecuteReader("SELECT * FROM members"))
            {
                while (reader.Read())
                {
                    records.Add(new MemberDto
                    {
                        ID = reader.GetInt32("id"),
                        Name = reader.GetString("name"),
                        Email = reader.GetString("email"),
                        Phone = reader.GetString("phone"),
                        NIC = reader.GetString("nic"),
                        Birthday = reader.GetDateTime("birthday"),
                        Address = reader.GetString("address")
                    });
                }
            }
            return records;
        }

        public bool updateDataRepo(MemberDto member)
        {
            return DB.ExecuteNonQuery(
                "UPDATE members SET name=@name, email=@email, phone=@phone, nic=@nic, birthday=@birthday, address=@address WHERE id=@id",
                new MySqlParameter("@name", member.Name),
                new MySqlParameter("@email", member.Email),
                new MySqlParameter("@phone", member.Phone),
                new MySqlParameter("@nic", member.NIC),
                new MySqlParameter("@birthday", member.Birthday),
                new MySqlParameter("@address", member.Address),
                new MySqlParameter("@id", member.ID)
            ) > 0;
        }

        public bool deleteDataRepo(MemberDto member)
        {
            return DB.ExecuteNonQuery(
                "DELETE FROM members WHERE id=@id",
                new MySqlParameter("@id", member.ID)
            ) > 0;
        }
    }
}
