using Outils.TConsole;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrandHotel.Pages
{
    public class PageAccueil : MenuPage
    {
        public PageAccueil() : base("Accueil", false)
        {

            Menu.AddOption("0", "Quitter l'application",
                    () => Environment.Exit(0));
            Menu.AddOption("1", "Page Gestion des Clients",
                    () => GrandHotelApp.Instance.NavigateTo(typeof(Page_GestionDesClients)));
            Menu.AddOption("2", "Page Gestion des Factures",
                    () => GrandHotelApp.Instance.NavigateTo(typeof(Page_GestionDesFactures)));
            Menu.AddOption("3", "Page Resultats Grand Hotel",
                    () => GrandHotelApp.Instance.NavigateTo(typeof(Page_ResultatsHotel)));

        }
    }
}
