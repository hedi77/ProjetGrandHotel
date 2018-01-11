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
        public DbSet<Facture> Factures { get; set; }
        public DbSet<LigneFacture> LignesFactures { get; set; }

        public IList<Client> GetClients()
        {
            Clients.Load();

            return Clients.Local;

        }
        
        //Retourne la liste des factures d'un client à partir d'une date donnée(par défault sur un an glissant)
        public IList<Facture> GetFacture(int idClient,DateTime date)
        {
            DateTime dateMax = date.AddMonths(12);         
            var factures = Factures.Where(f => f.IdClient == idClient && f.DateFacture >= date && f.DateFacture <= dateMax  ).ToList();
            return factures;
        }

        public IList<LigneFacture> LigneFacture(int idFacture)
        {

            var ligne = LignesFactures.Where(l => l.IdFacture == idFacture).ToList();
            return ligne;
        }




        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }

        public GrandHotelContext() : base("GrandHotel.Settings1.GrandHotelChaineConnexion")
        {

        }

        public void AddFacture(Facture fact)
        {
            Factures.Add(fact);
        }
        

        public void AddLigneDeCommande(LigneFacture ligneFact)
        {
            LignesFactures.Add(ligneFact);
        }
        public Facture GetFacture(int numFacture)
        {
            Facture fact = Factures.Where(f => f.Id == numFacture).FirstOrDefault();
            return fact;
        }
        //public int EnregistrerModif()
        //{
        //    return SaveChanges();
        //}
    }
}
