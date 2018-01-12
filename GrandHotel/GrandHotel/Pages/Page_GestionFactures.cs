using Outils.TConsole;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrandHotel.Pages
{
    class Page_GestionDesFactures : MenuPage
    {
        private IList<Facture> _listefactures;

        public Page_GestionDesFactures() : base("GestionDesfactures", false)
        {
            Menu.AddOption("1", "Afficher la liste des factures à partir d'un Id Client et d'une date : ", AfficherListeFacture);
            Menu.AddOption("2", "Afficher les lignes d'une facture identifiée par son Id", AfficherLigneFacture);
            Menu.AddOption("3", "Saisir une facture", SaisirFacture);
            Menu.AddOption("4", "Saisir les lignes de facture d'une facture donnée", SaisirLigneFactureDonnee);
            Menu.AddOption("5", "Mettre à jour la date et le mode de paiement d'une facture", ModifierDateEtModePaie);
            Menu.AddOption("6", "Exporter les factures d'un client au format xml\n", ExportFactureClientXml);
            Menu.AddOption("7", "Retour à la page d'Accueil.", () => GrandHotelApp.Instance.NavigateTo(typeof(PageAccueil)));


        }

        //Affiche toute les facture d'un client 
        public void AfficherListeFacture()
        {
            int idclient = Input.Read<int>("Saisir Id du client : ");
            DateTime date = Input.Read<DateTime>("Saisir la date(jj/mm/aaaa) : ");
            var liste = GrandHotelApp.DataContext.GetFactureClient(idclient, date);
            ConsoleTable.From(liste).Display("Liste des factures : ");

        }

        //Affiche les ligne de facture correspondant à une facture donnée
        public void AfficherLigneFacture()
        {
            // charge les factures depuis la BDD et affiche leur nombre
            _listefactures = GrandHotelApp.DataContext.GetToutesFactures();
            Console.WriteLine("Il y a {0} factures.", _listefactures.Count());

            int idFacture = Input.Read<int>("Saisir l'Id d'une facture : ");
            List<LigneFacture> listeDetails = _listefactures.Where(f => f.Id == idFacture).Select(f => f.LigneFactures).FirstOrDefault();
            ConsoleTable.From(listeDetails).Display("Détails Facture : ");
        }
        //Permet de saisir puis Ajouter une facture dans la base
        public void SaisirFacture()
        {
            IList<Client> listeCli = GrandHotelApp.DataContext.GetClients();
            ConsoleTable.From(listeCli, "Liste des Clients").Display("Liste des Clients");
            Facture fact = new Facture();
            fact.IdClient = Input.Read<int>("saisir l'Id du client à facturer : ");
            fact.DateFacture = Input.Read<DateTime>("la date de la facture (jj/mm/aaaa) : ");
            fact.DatePaiement = Input.Read<DateTime>("la date de paiement (jj/mm/aaaa) : ");



            string saisieMoyPaie = null;
            while (saisieMoyPaie != "CHQ" && saisieMoyPaie != "CB" && saisieMoyPaie != "ESP")
            {
                saisieMoyPaie = Input.Read<string>("Mode de paiement ( CB ou ESP ou CHQ ) : ").ToUpper();
            }
            fact.CodeModePaiement = saisieMoyPaie;

            try
            {
                // Enregistrement dans la base

                GrandHotelApp.DataContext.AddFacture(fact);
                Output.WriteLine(ConsoleColor.Green, "Facture ajoutée avec succès.");
                Output.WriteLine("");
            }
            catch (System.Data.Entity.Infrastructure.DbUpdateException ex)
            {
                var innerEx = (ex.InnerException.InnerException as SqlException);
                if (innerEx != null & innerEx.Number == 547)
                    GererErreurSql(innerEx);
            }
            catch (SqlException ex)
            {
                GererErreurSql(ex);
            }

        }
        //Permet de saisir une ligne de facture correspondant à une facture
        public void SaisirLigneFactureDonnee()
        {
            // charge les factures depuis la BDD et affiche leur nombre
            _listefactures = GrandHotelApp.DataContext.GetToutesFactures();
            Console.WriteLine("Il y a {0} factures.", _listefactures.Count());

            int numFacture = Input.Read<int>("Numero de la facture à saisir");
            Facture fact = _listefactures.Where(f => f.Id == numFacture).FirstOrDefault();
            LigneFacture ligne = new LigneFacture();
            int ajout = 1;
            if (fact == null)
                Output.WriteLine(ConsoleColor.Red, "Aucune facture trouvée");
            else
            {
                
                while(ajout ==1)
                {
                    //Affiche les lignes de cette Facture
                    List<LigneFacture> listeDetails = _listefactures.Where(f => f.Id == numFacture).Select(f => f.LigneFactures).FirstOrDefault();
                    ConsoleTable.From(listeDetails).Display("Détails Facture : ");
                    ligne = new LigneFacture();
                    ligne.IdFacture = numFacture;
                    ligne.NumLigne = Input.Read<int>("Ajoutez à la suite une NumLigne de la ligne de commande : ");
                    ligne.Quantite = Input.Read<short>("Quantité des servives facturés(chambres, petits déjeuner ...) ");
                    ligne.MontantHT = Input.Read<Decimal>("Montant HT : ");
                    ligne.TauxTVA = Input.Read<Decimal>("TauxTVA : ");
                    ligne.TauxReduction = Input.Read<Decimal>("TauxReduction : ");
                    GrandHotelApp.DataContext.AddLigneDeCommande(ligne);
                    ajout = Input.ReadIntBetween("Taper 1 pour ajouter une autre ligne, sinon taper 0 : ", 0, 1);
                }
                GrandHotelApp.DataContext.AddLigneDeCommande(ligne);
                Output.WriteLine(ConsoleColor.Green, "les lignes ont été saisies avec succès.");
            }
        }

        public void ModifierDateEtModePaie()
        {
            // charge les factures depuis la BDD et affiche leur nombre
            _listefactures = GrandHotelApp.DataContext.GetToutesFactures();
            Console.WriteLine("Il y a {0} factures.", _listefactures.Count());

            int idFacture = Input.Read<int>("Saisir l'Id d'une facture : ");
            Facture fact ;
            fact = _listefactures.Where(f => f.Id == idFacture).FirstOrDefault();
            string saisieMoyPaie="";
            if (fact == null)
                Output.WriteLine(ConsoleColor.Red, "Aucune facture ne correspond à ce numero : {0}", idFacture);
            else
            {
                fact.DatePaiement = Input.Read<DateTime>("Date de paiement : ", fact.DatePaiement);
                while (saisieMoyPaie != "CHQ" && saisieMoyPaie != "CB" && saisieMoyPaie != "ESP")
                {
                    saisieMoyPaie = Input.Read<string>("Mode de paiement ( CB ou ESP ou CHQ ) : ",fact.CodeModePaiement).ToUpper();
                }
                fact.CodeModePaiement = saisieMoyPaie;
            }
            GrandHotelApp.DataContext.UpdateFacture(fact);
            Output.WriteLine(ConsoleColor.Green, "Modification de la date et du moyen de paiement de la facture, avec succès.");
        }

        public void ExportFactureClientXml()
        {
            // Récupération des factures d'un Client depuis la BDD
            int idclient = Input.Read<int>("Saisir Id du client : ");
            var liste = GrandHotelApp.DataContext.GetFactureClient(idclient);

            GrandHotelApp.DataContext.ExporterXml_Factures_XmlWriter(liste);
            Output.WriteLine(ConsoleColor.Green, "Sérialisation de la liste des factures du client dans le dossier GrandHotel");

        }

        private void GererErreurSql(SqlException ex)
        {
            // à développer en traitant les erreurs remontées
        }


    }
}
