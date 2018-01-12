using Outils.TConsole;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrandHotel.Pages
{
    public class Page_GestionDesClients : MenuPage
    {
        private IList<Client> _listeClients;

        public Page_GestionDesClients() : base("GestionDesClients", false)
        {
            Menu.AddOption("1", "Afficher la liste des Clients.", AfficherListeClients);
            Menu.AddOption("2", "Afficher les Coordonnées d'un Client.", AfficherCoordonneesClient);
            Menu.AddOption("3", "Saisir un nouveau Client.", SaisirNouveauClientAdresse);
            Menu.AddOption("4", "Ajouter un numéro de téléphone Client ou un email Client.", AjouterTelephoneOuEmail);
            Menu.AddOption("5", "Supprimer un Client.", SupprimerClient);
            Menu.AddOption("6", "Exporter la liste des Clients.", ExporterListeClient);
            Menu.AddOption("7", "Retour à la page d'Accueil.", () => GrandHotelApp.Instance.NavigateTo(typeof(PageAccueil)));
        }

        public void AfficherListeClients()
        {
            _listeClients = GrandHotelApp.DataContext.GetClients();
            ConsoleTable.From(_listeClients, "Liste des Clients").Display("Liste des Clients");
        }

        private void AfficherCoordonneesClient()
        {
            // Affichage de la liste des clients - avec récupération des eventuels ajouts dans la BDD
            AfficherListeClients();

            // Affichage des téléphones et emails du client sélectionné
            int id = Input.Read<int>("De quel client souhaitez-vous afficher l'adresse postale, les emails et les n° de téléphones ? ");


            var adresseDuClient = _listeClients.Where(c => c.Id == id).Select(c => c.Adresse).FirstOrDefault();
            Console.WriteLine("Adresse : {0} {1} CP : {2} ville : {3}", adresseDuClient.Rue, adresseDuClient.Complement,
                                                                        adresseDuClient.CodePostal, adresseDuClient.Ville);

            var telephones = _listeClients.Where(c => c.Id == id).Select(c => c.Telephones).FirstOrDefault();
            ConsoleTable.From(telephones).Display("Telephones");

            var emails = _listeClients.Where(c => c.Id == id).Select(c => c.Emails).FirstOrDefault();
            ConsoleTable.From(emails).Display("Emails");
        }

        private void SaisirNouveauClientAdresse()
        {
            // Saisie des infos du client
            Output.WriteLine("Saisissez les informations du client : ");
            Client cli = new Client();
            cli.Civilite = Input.Read<string>("Civilité : ");
            cli.Nom = Input.Read<string>("Nom : ");
            cli.Prenom = Input.Read<string>("Prenom: ");
            cli.CarteFidelite = Input.Read<bool>("Carte de Fidelité (tapez true OU false) : ");
            cli.Societe = Input.Read<string>("Société : ");

            try
            {
                // Enregistrement du client dans la base
                GrandHotelApp.DataContext.AjouterClient(cli);
                Output.WriteLine(ConsoleColor.Green, "Client créé avec succès");
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


            //saisie de l'adresse du client
            Output.WriteLine("Saisissez l'adresse de ce client : ");
            Adresse adre = new Adresse();
            adre.IdClient = cli.Id;
            adre.Rue = Input.Read<string>("Rue : ");
            adre.Complement = Input.Read<string>("Complement : ");
            adre.CodePostal = Input.Read<string>("Code Postal : ");
            adre.Ville = Input.Read<string>("Ville : ");

            try
            {
                // Enregistrement dans la base
                GrandHotelApp.DataContext.AjouterAdresse(adre);
                Output.WriteLine(ConsoleColor.Green, "L'Adresse du client a été créée avec succès");
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

        private void AjouterTelephoneOuEmail()
        {
            Telephone tel = new Telephone();
            Email mail = new Email();
            bool? operation = null;

            // Affichage de la liste des clients - avec récupération des eventuels ajouts dans la BDD
            AfficherListeClients();

            // Récupère le client dont l'id a été saisi
            int id = Input.Read<int>("Veuillez saisir l'Id du client : ");
            Client cli = _listeClients.Where(cl => cl.Id == id).FirstOrDefault();
            int choix = Input.ReadIntBetween("Vous voulez saisir un téléphone (tapez 1) ou un email (tapez 2) : ", 1,2);

            // saisie telephone
            if (choix == 1)
            {
                operation = true;

                tel.IdClient = id;
                tel.Numero = Input.Read<string>("Veuillez saisir le numéro du client : ");
                string saisieCodeType = "A";
                while(saisieCodeType != "M" && saisieCodeType != "F")
                {
                    saisieCodeType = Input.Read<string>("Veuillez saisir le CodeType du téléphone ( M pour Mobile ) ou ( F pour Fixe) : ");
                }
                tel.CodeType = saisieCodeType;
                int? saisiePro = null;
                while (saisiePro != 1 && saisiePro != 2)
                {
                    saisiePro = Input.Read<int>("Est-ce un téléphone Professionnel (oui : tapez 1) ou (non : tapez 2) : ");
                }
                if (saisiePro == 1) tel.Pro = true;
                else tel.Pro = false;
            }
            // saisie email
            if (choix == 2)
            {
                operation = false;

                mail.IdClient = id;
                mail.Adresse = Input.Read<string>("Veuillez saisir l'email du client : ");
                int? saisiePro = null;
                while (saisiePro != 1 && saisiePro != 2)
                {
                    saisiePro = Input.Read<int>("Est-ce un email Professionnel (oui : tapez 1) ou (non : tapez 2) : ");
                }
                if (saisiePro == 1) mail.Pro = true;
                else mail.Pro = false;
            }


            try
            {
                // Enregistrement dans la base
                GrandHotelApp.DataContext.AjouterTeleOuMail(tel, mail, operation);
                if (operation == true) Output.WriteLine(ConsoleColor.Green, "téléphone ajouté avec succès");
                else Output.WriteLine(ConsoleColor.Green, "email ajouté avec succès");
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

        private void SupprimerClient()
        {
            // Affichage de la liste des clients - avec récupération des eventuels ajouts dans la BDD
            AfficherListeClients();

            int id = Input.Read<int>("Id du Client à supprimer :");
            try
            {
                GrandHotelApp.DataContext.SupprimerClient(id);
                Output.WriteLine(ConsoleColor.Green, "Client +(adresse postale, emails et téléphones) effacés avec succès");
                Output.WriteLine("");
            }
            catch (SqlException e)
            {
                GererErreurSql(e);
            }
        }

        private void GererErreurSql(SqlException ex)
        {
            if (ex.Number == 547)
                Output.WriteLine(ConsoleColor.Red,
                    "Le client ne peut pas être supprimé car il est associé à une facture ou à l'occupation d'une chambre.");
            else
                throw ex;
        }

        private void ExporterListeClient()
        {
            // Récupération de la liste des Clients depuis la BDD
            _listeClients = GrandHotelApp.DataContext.GetClients();
            GrandHotelApp.DataContext.ExporterXml_XmlWriter(_listeClients);
            Output.WriteLine(ConsoleColor.Green, "Sérialisation de la liste des clients avec coordonnées dans le dossier GrandHotel");
        }

    }
}
