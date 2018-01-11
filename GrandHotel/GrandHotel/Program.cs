using GrandHotel.Pages;
using Outils.TConsole;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrandHotel
{
    public class Program
    {
        public static void Main(string[] args)
        {
            GrandHotelApp app = GrandHotelApp.Instance;
            app.Title = "GrandHotel";

            // Ajout des pages
            Page accueil = new PageAccueil();
            app.AddPage(accueil);
            Page gestionDesClients = new Page_GestionDesClients();
            app.AddPage(gestionDesClients);
            Page gestionDesFactures = new Page_GestionDesFactures();
            app.AddPage(gestionDesFactures);
            Page resultatsHotel = new Page_ResultatsHotel();
            app.AddPage(resultatsHotel);



            // Affichage de la page d'accueil
            app.NavigateTo(accueil);
            app.Run();
        }
    }
}
