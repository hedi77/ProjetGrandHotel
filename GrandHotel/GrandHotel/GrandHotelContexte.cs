using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrandHotel
{
    public class GrandHotelContext : DbContext
    {

        public DbSet<Client> Clients { get; set; }
        //public static IList<string> GetPaysFournisseurs()
        //{
        //    var list = new List<string>();
        //    var cmd = new SqlCommand();
        //    cmd.CommandText = @"select * from Client";

        //    using (var cnx = new SqlConnection(Settings1.Default.GrandHotelConnexion))
        //    {

        //        cmd.Connection = cnx;
        //        cnx.Open();

        //        using (SqlDataReader reader = cmd.ExecuteReader())
        //        {

        //            while (reader.Read())
        //            {
        //                list.Add((string)reader["Nom"]);
        //            }
        //        }
        //    }
        //    return list;
        //}

        public IList<Client> GetClients()
        {
            Clients.Load();

            return Clients.Local;

        }
        

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }

        public GrandHotelContext() : base("GrandHotel.Settings1.GrandHotelChaineConnexion")
        {

        }


    }
}
