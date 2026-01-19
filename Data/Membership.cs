using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebVerdandiMedReg.Data
{
    [Table("Medlemsreg")]   // ⚠️ Case sensitive in some collations – use exactly as in SQL
    public class Membership
    {
        [Key]
        [Column("Mednummer")]
        public string Mednummer { get; set; } = default!;

        [Column("clientid")]
        public string? ClientId { get; set; }

        [Column("Distrikt")]
        public string? Distrikt { get; set; }

        [Column("Avdelning")]
        public string? Avdelning { get; set; }

        [Column("Personnr")]
        public string? Personnr { get; set; }

        [Column("Kon")]
        public string? Kon { get; set; }

        [Column("Mednamn")]
        public string? Mednamn { get; set; }

        [Column("Adress1")]
        public string? Adress1 { get; set; }

        [Column("Adress2")]
        public string? Adress2 { get; set; }

        [Column("Postnummer")]
        public string? Postnummer { get; set; }

        [Column("Ort")]
        public string? Ort { get; set; }

        [Column("Telefon")]
        public string? Telefon { get; set; }

        [Column("email")]
        public string? Email { get; set; }

        [Column("Land")]
        public string? Land { get; set; }

        [Column("Kontaktperson")]
        public string? Kontaktperson { get; set; }

        [Column("Medtyp")]
        public string? Medtyp { get; set; }

        [Column("Styrelseuppdrag")]
        public string? Styrelseuppdrag { get; set; }

        [Column("losen")]
        public string? Lösen { get; set; }

        [Column("GSM")]
        public string? GSM { get; set; }

        [Column("URL")]
        public string? URL { get; set; }

        [Column("OBSstatus")]
        public double? OBSstatus { get; set; }

        [Column("ejaktiv")]
        public double? EjAktiv { get; set; }

        [Column("Medlemsinfo")]
        public string? Medlemsinfo { get; set; }

        [Column("RegSedan")]
        public string? RegSedan { get; set; }

        [Column("Faktlevtyp")]
        public string? Faktlevtyp { get; set; }

        [Column("SenastUppdatering")]
        public string? SenastUppdatering { get; set; }

        [Column("SenasteDeb")]
        public string? SenasteDeb { get; set; }
       
        [Column("Personnummer")]
        public string? Personnummer { get; set; }

        [Column("Anstalld")]
        public double? Anstalld { get; set; }

        [Column("AvgiftBet")]
        public string? AvgiftBet { get; set; }

        [Column("Fortroendevald")]
        public double? Fortroendevald { get; set; }



    }
}
