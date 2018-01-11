using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrandHotel
{
    //public class Client
    //{
    //    public int Id { get; set; }
    //    public string Civilite { get; set; }
    //    public string Nom { get; set; }
    //    public string Prenom { get; set; }
    //    public bool CarteFidelite { get; set; }
    //    public string Societe { get; set; }
    //}
    public class Facture
    {
        public int FactureId { get; set; }
        public int IdClient { get; set; }
        public DateTime DateFacture { get; set; }
        public DateTime DatePaiement { get; set; }
        public string CodeModePaiement { get; set; }

        public virtual List<LigneFacture> LigneFactures { get;set }

    }

    public class LigneFacture
    {

    }
}
