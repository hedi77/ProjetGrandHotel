using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrandHotel
{
    public class Client
    {
        public int Id { get; set; }
        public string Civilite { get; set; }
        public string Nom { get; set; }
        public string Prenom { get; set; }
        public bool CarteFidelite { get; set; }
        public string Societe { get; set; }
      //  [Display(ShortName ="none")]
        public virtual List<Facture> ListeFactures { get; set; }


    }
    public class Facture
    {
        
        public int Id { get; set; }
        [ForeignKey("Client")]
        public int IdClient { get; set; }
        public DateTime DateFacture { get; set; }
        public DateTime DatePaiement { get; set; }
        [StringLength (3)]
        public string CodeModePaiement { get; set; }

        [Display(ShortName = "none")]
        public virtual List<LigneFacture> LigneFactures { get; set; }
        [Display(ShortName = "none")]
        public virtual Client Client { get; set; }

    }

    public class LigneFacture
    {
        [ForeignKey("Facture")]
        public int IdFacture { get; set; }
        [Key]
        public int NumLigne { get; set; }
        public short Quantite { get; set; }
        public Decimal MontantHT { get; set; }
        public Decimal TauxTVA { get; set; }
        public Decimal TauxReduction { get; set; }

        [Display(ShortName = "none")]
        public virtual Facture Facture { get; set; }
    }
}
