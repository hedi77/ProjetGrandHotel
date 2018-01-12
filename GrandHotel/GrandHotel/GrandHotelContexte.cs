using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace GrandHotel
{
    public class GrandHotelContext : DbContext
    {

        public DbSet<Client> Clients { get; set; }
        public DbSet<Facture> Factures { get; set; }
        public DbSet<LigneFacture> LignesFactures { get; set; }
        public DbSet<Telephone> Telephones { get; set; }
        public DbSet<Email> Emails { get; set; }
        public DbSet<Adresse> Adresses { get; set; }

        #region page client

        public IList<Client> GetClients()
        {
            return Clients.AsNoTracking().ToList();
        }

        public IList<Client> GetCoordonneesClient()
        {
            return Clients.AsNoTracking().Include(c => c.Emails).ToList();
        }

        public void AjouterClient(Client client)
        {
            Clients.Add(client);
            SaveChanges();
        }

        public void AjouterAdresse(Adresse adresse)
        {
            Adresses.Add(adresse);
            SaveChanges();
        }

        public void AjouterTeleOuMail(Telephone tele, Email mail, bool? a)
        {
            // ajout Telephone
            if (a == true)      
            {
                Telephones.Add(tele);
                SaveChanges();
            }
            // ajout email
            else 
            {
                Emails.Add(mail);
                SaveChanges();
            }

        }

        public void SupprimerClient(int id)
        {
            Client clie = Clients.Find(id);
            Adresse adrClie = Adresses.Find(id);
            List<Telephone> listeTelClie = Telephones.Where( t => t.IdClient == id).ToList();
            List<Email> listeMailClie = Emails.Where(em => em.IdClient == id).ToList();

            if (clie != null)
            {
                Clients.Remove(clie);
                Adresses.Remove(adrClie);
                foreach( Telephone tele in listeTelClie)
                {
                    Telephones.Remove(tele);
                }
                foreach (Email mail in listeMailClie)
                {
                    Emails.Remove(mail);
                }

                SaveChanges();
            }

        }

        public void ExporterXml_XmlWriter(IEnumerable<Client> listeclient)
        {
            // paramètres pour l'indentation du flux xml généré
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.IndentChars = "\t";

            using (XmlWriter writer = XmlWriter.Create(@"..\..\Export_Liste_Clients.xml", settings))
            {
                writer.WriteStartDocument();
                writer.WriteStartElement("ListedeClients");

                // On parcourt la liste des clients triés par id
                foreach (Client cli in listeclient.OrderBy(cl => cl.Id))
                {
                    writer.WriteStartElement("Client");

                    writer.WriteAttributeString("Id", cli.Id.ToString());
                    writer.WriteAttributeString("Civilite", cli.Civilite.ToString());
                    writer.WriteAttributeString("Nom", cli.Nom.ToString());

                    // Ecriture du premier telephone et du premier mail du client
                    writer.WriteStartElement("Contact");
                    writer.WriteAttributeString("tel", cli.Telephones.Select(t => t.Numero).FirstOrDefault());
                    writer.WriteAttributeString("mail", cli.Emails.Select(em => em.Adresse).FirstOrDefault());
                    writer.WriteEndElement();

                    writer.WriteEndElement();
                }
                writer.WriteEndElement();
                // Ecriture de la balise fermante de l'élément racine et fin du document
                writer.WriteEndDocument();
            }
        }

        #endregion

        #region Facture
        //Retourne la liste des factures d'un client à partir d'une date donnée(par défault sur un an glissant)
        public IList<Facture> GetFactureClient(int idClient,DateTime date)
        {
            DateTime dateMax = date.AddMonths(12);         
            var factures = Factures.Where(f => f.IdClient == idClient && f.DateFacture >= date && f.DateFacture <= dateMax  ).ToList();
            return factures;
        }

        //Retourne la liste des factures d'un client à partir d'une date donnée(par défault sur un an glissant)
        public IList<Facture> GetFactureClient(int idClient)
        {
            var factures = Factures.Where(f => f.IdClient == idClient).ToList();
            return factures;
        }

        // charge toutes les factures (et lignes) et retourne le nombre de factures
        public List<Facture> GetToutesFactures()
        {
            return Factures.ToList();
        }
        public IList<LigneFacture> LigneFacture(int idFacture)
        {

            var ligne = LignesFactures.Where(l => l.IdFacture == idFacture).ToList();
            return ligne;
        }
        // ajout d'une facture
        public void AddFacture(Facture fact)
        {
            Factures.Add(fact);
            SaveChanges();
        }
        // ajout de lignes de facture
        public void AddLigneDeCommande(LigneFacture ligneFact)
        {
            LignesFactures.Add(ligneFact);
            SaveChanges();
        }

        //Modifie Paiement et Date facture existante
        public void UpdateFacture(Facture fact)
        {
            var facture = Factures.Find(fact.Id);
            facture.DatePaiement = fact.DatePaiement;
            facture.CodeModePaiement = fact.CodeModePaiement;
            SaveChanges();
        }

        public void ExporterXml_Factures_XmlWriter(IEnumerable<Facture> listefactclient)
        {
            // paramètres pour l'indentation du flux xml généré
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.IndentChars = "\t";
            decimal resultMontant =0;
            using (XmlWriter writer = XmlWriter.Create(@"..\..\Export_Liste_Factures_Client.xml", settings))
            {
                writer.WriteStartDocument();
                writer.WriteStartElement("Client");

                // On parcourt la liste des clients triés par id
                foreach (Facture fact in listefactclient.OrderBy(lf => lf.Id))
                {
                    writer.WriteStartElement("Facture");

                    writer.WriteAttributeString("Id", fact.Id.ToString());
                    var montant = fact.LigneFactures.Select(lf => lf.MontantHT).ToList();
                    foreach (var m in montant)
                        resultMontant += m;

                    writer.WriteAttributeString("Montant", resultMontant.ToString());
                    resultMontant = 0;

                    writer.WriteEndElement();
                }
                writer.WriteEndElement();
                // Ecriture de la balise fermante de l'élément racine et fin du document
                writer.WriteEndDocument();
            }
        }

//retourne une facture 
//public Facture GetFacture(int numFacture)
//{
//    Facture fact = Factures.Where(f => f.Id == numFacture).FirstOrDefault();
//    return fact;
//}

#endregion


protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
        public GrandHotelContext() : base("GrandHotel.Settings1.GrandHotelChaineConnexion")
        {

        }
    }
}


 

