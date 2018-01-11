using Outils.TConsole;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrandHotel.Pages
{
    public class Page_GestionDesClients : MenuPage
    {
        public Page_GestionDesClients() : base("GestionDesClients", false)
        {

            Menu.AddOption("1", "Afficher la liste des Clients", AfficherListeClients);
            //Menu.AddOption("2", "Créer un nouveau produit", CreerProduit);
            //Menu.AddOption("3", "Modifier un nouveau produit", ModifierProduit);
            //Menu.AddOption("6", "Enregistrer. ", EnregistrerModifsProduits);


        }

        private void AfficherListeClients()
        {
            /*
            IList<Client> listeClients = GrandHotelApp.DataContext.GetClients();
            ConsoleTable.From(listeClients, "Liste des Clients").Display("Liste des Clients");
            */

        }


    }
}
