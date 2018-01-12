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
        [XmlIgnore]
        public string Prenom { get; set; }
        [XmlIgnore]
        public bool CarteFidelite { get; set; }
        [XmlIgnore]
        public string Societe { get; set; }

        [XmlIgnore]
        [Display(ShortName = "None")]
        public virtual List<Telephone> Telephones { get; set; }
        [XmlIgnore]
        [Display(ShortName = "None")]
        public virtual List<Email> Emails { get; set; }
        [XmlIgnore]
        [Display(ShortName = "None")]
        public virtual Adresse Adresse { get; set; }
    }

    public class Telephone
    {
        [Key]
        [XmlText]
        public string Numero { get; set; }
        [ForeignKey("Client")]
        [XmlIgnore]
        public int IdClient { get; set; }
        [XmlIgnore]
        public string CodeType { get; set; }
        [XmlIgnore]
        public bool Pro { get; set; }
        [XmlIgnore]
        [Display(ShortName = "None")]
        public virtual Client Client { get; set; }
    }
    
    public class Email
    {
        [Key]
        [XmlText]
        public string Adresse { get; set; }
        [XmlIgnore]
        public bool Pro { get; set; }
        [XmlIgnore]
        [ForeignKey("Client")]
        public int IdClient { get; set; }
        [XmlIgnore]
        [Display(ShortName = "None")]
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
        [XmlIgnore]
        [Display(ShortName = "None")]
        public virtual Client Client { get; set; }
    }


}
