using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace GrandHotel
{
    public class Client
    {
        [XmlAttribute]
        public int Id { get; set; }
        [XmlText]
        public string Civilite { get; set; }
        [XmlText]
        public string Nom { get; set; }
        [XmlText]
        public string Prenom { get; set; }
        [XmlIgnore]
        public bool CarteFidelite { get; set; }
        [XmlText]
        public string Societe { get; set; }

        [Display(ShortName = "None")]
        [XmlIgnore]
        public virtual List<Telephone> Telephones { get; set; }
        [Display(ShortName = "None")]
        [XmlIgnore]
        public virtual List<Email> Emails { get; set; }
        [Display(ShortName = "None")]
        [XmlIgnore]
        public virtual Adresse Adresse { get; set; }
    }

    public class Telephone
    {
        [Key][XmlText]
        public string Numero { get; set; }
        [ForeignKey("Client")]
        [XmlIgnore]
        public int IdClient { get; set; }
        [XmlIgnore]
        public string CodeType { get; set; }
        [XmlIgnore]
        public bool Pro { get; set; }
        [Display(ShortName = "None")][XmlIgnore]
        public virtual Client Client { get; set; }
    }

    public class Email
    {
        [Key]
        [XmlText]
        public string Adresse { get; set; }
        [XmlIgnore]
        public bool Pro { get; set; }
        [ForeignKey("Client")]
        [XmlIgnore]
        public int IdClient { get; set; }
        [Display(ShortName = "None")]
        [XmlIgnore]
        public virtual Client Client { get; set; }
    }

    public class Adresse
    {
        [Key, ForeignKey("Client")]
        [XmlIgnore]
        public int IdClient { get; set; }
        [XmlText]
        public string Rue { get; set; }
        [XmlText]
        public string Complement { get; set; }
        [XmlText]
        public string CodePostal { get; set; }
        [XmlText]
        public string Ville { get; set; }
        [Display(ShortName = "None")]
        [XmlIgnore]
        public virtual Client Client { get; set; }
    }

    public class Facture
    {
        [XmlAttribute]
        public int Id { get; set; }
        [ForeignKey("Client")]
        [XmlAttribute]
        public int IdClient { get; set; }
        [XmlIgnore]
        public DateTime DateFacture { get; set; }
        [XmlIgnore]
        public DateTime DatePaiement { get; set; }
        [XmlIgnore]
        [StringLength (3)]
        public string CodeModePaiement { get; set; }

        [XmlIgnore]
        [Display(ShortName = "none")]
        public virtual List<LigneFacture> LigneFactures { get; set; }
        [XmlIgnore]
        [Display(ShortName = "none")]
        public virtual Client Client { get; set; }

    }

    public class LigneFacture
    {
        [XmlIgnore]
        [Column(Order =0), Key, ForeignKey("Facture")]
        public int IdFacture { get; set; }
        [XmlIgnore]
        [Column(Order = 1),Key]
        public int NumLigne { get; set; }
        [XmlIgnore]
        public short Quantite { get; set; }
        [XmlAttribute]
        public Decimal MontantHT { get; set; }
        [XmlIgnore]
        public Decimal TauxTVA { get; set; }
        [XmlIgnore]
        public Decimal TauxReduction { get; set; }

        [XmlIgnore]
        [Display(ShortName = "none")]
        public virtual Facture Facture { get; set; }
    }
}
