using Outils.TConsole;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrandHotel.Pages
{
    class Page_GestionDesFactures : MenuPage
    {
        public Page_GestionDesFactures() : base("GestionDesfactures", false)
        {
            Menu.AddOption("1", "Afficher la liste des factures à partir d'une date", AfficherListeFacture);
            Menu.AddOption("2", "Afficher les lignes d'une facture identifiée par son Id", AfficherLigneFacture);
            Menu.AddOption("3", "Saisir une facture", SaisirFacture);
            Menu.AddOption("4", "Saisir les lignes de facture d'une facture donnée", SaisirLigneFactureDonnee);
            Menu.AddOption("5", "Mettre à jour la date et le mode de paiement d'une facture", SaisirDateEtModePaiement);
            Menu.AddOption("6", "Exporter les factures d'un client au format xml", ExportFactureClientXml);
            Menu.AddOption("7", "enregistrer Modification", EnregistrerChangement);


        }


        public void AfficherListeFacture()
        {
            int idclient = Input.Read<int>("Saisir Id du client");
            DateTime date = Input.Read<DateTime>("Saisir la date(jj/mm/aaaa) ");
            var liste = GrandHotelApp.DataContext.GetFacture(idclient, date);
            ConsoleTable.From(liste).Display("Liste des factures");

        }

        public void AfficherLigneFacture()
        {
            int idFacture = Input.Read<int>("Saisir Id de la facture");
            var listeDetails = GrandHotelApp.DataContext.LigneFacture(idFacture);
            ConsoleTable.From(listeDetails).Display("Détails Facture");

        }
        public void SaisirFacture()
        {
            Facture fact = new Facture();
            fact.IdClient = Input.Read<int>("Id du client ");
            fact.DateFacture = Input.Read<DateTime>("la date de la facture (jj/mm/aaaa) ");
            fact.DatePaiement = Input.Read<DateTime>("la date de paiement (jj/mm/aaaa) ");
            fact.CodeModePaiement = Input.Read<string>("Mode de paiement").ToUpper();
            GrandHotelApp.DataContext.AddFacture(fact);
            
        }

        public void SaisirLigneFactureDonnee()
        {
            int numFacture = Input.Read<int>("Numero de la facture à saisir");
            Facture fact = GrandHotelApp.DataContext.GetFacture(numFacture);
            LigneFacture ligne = new LigneFacture();
            int ajout = 1;
            if (fact == null)
                Output.WriteLine(ConsoleColor.Red, "Aucune facture trouvée");
            else
            {
                ligne.IdFacture = numFacture;
                do
                {
                    
                    ligne.NumLigne = Input.Read<int>("id de la ligne de commande");
                    ligne.Quantite = Input.Read<short>("Quantité des servives facturés(chambres, petits déjeuner ...) ");
                    ligne.MontantHT = Input.Read<Decimal>("Montant HT : ");
                    ligne.TauxTVA = Input.Read<Decimal>("TauxTVA : ");
                    ligne.TauxReduction = Input.Read<Decimal>("TauxReduction : ");
                    GrandHotelApp.DataContext.AddLigneDeCommande(ligne);
                    ajout = Input.ReadIntBetween("Taper 1 pour ajouter une autre ligne \n Sinon taper 0", 0, 1);
                } while (ajout == 1); 
            }
            
        }

        public void SaisirDateEtModePaiement()
        {
            
        }

        public void ExportFactureClientXml()
        {
           
        }
        
        public void EnregistrerChangement()
        {
            if (GrandHotelApp.DataContext.SaveChanges() > 0)
                Output.WriteLine(ConsoleColor.Green, "Modification effectué avec succes");
            else
                Output.WriteLine(ConsoleColor.Red, "Echec");
        }


    }
}
