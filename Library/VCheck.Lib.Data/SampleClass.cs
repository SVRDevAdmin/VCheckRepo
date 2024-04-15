using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;

namespace VCheck.Lib.Data
{
    public class SampleClass
    {
        private readonly Microsoft.Extensions.Configuration.IConfiguration config;
        public SampleClass(Microsoft.Extensions.Configuration.IConfiguration config)
        {
            this.config = config;
        }

        private MySqlConnection Connection
        {
            get
            {
                String? connectionString = this.config.GetConnectionString("DefaultConnection");
                return new MySqlConnection(connectionString);
            }
        }

        public List<Album> GetData()
        {
            List<Album> sList = new List<Album>();

            using (MySqlConnection conn = this.Connection)
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("Select Id, column2 from testingtbl", conn);

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        sList.Add(new Album()
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            column2 = reader["column2"].ToString()
                        });
                    }
                }
            }

            return sList;
        }
    }
    public class Album
    {
        public int Id { get; set; }
        public String? column2 { get; set; }
    }
}
