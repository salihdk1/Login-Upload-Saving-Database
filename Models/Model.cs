using System.ComponentModel.DataAnnotations;

namespace LDap.Models
{
    public class Model
    {
        [Key]
        public int SıraNo { get; set; }

        public int TcKimlikNo { get; set; }

        [Required(ErrorMessage = "Adı Soyadı is required.")]
        public required string AdıSoyadı { get; set; }

        [Required(ErrorMessage = "Ünvanı is required.")]
        public required string Ünvanı { get; set; }

        public int ÖrgünEğitimSaat { get; set; }
        public int ÖrgünEğitimTutar { get; set; }
        public int GelirToplamı { get; set; }
        public int DamgaVergisi { get; set; }
        public int GelirVergisi { get; set; }
        public int GelirVergisiİstisnası { get; set; }
        public int DamgaVergisiİstisnası { get; set; }
        public int KesintiToplamı { get; set; }
        public int NetÖdenecekÜcret { get; set; }

        [Required(ErrorMessage = "Ay is required.")]
        public required string Ay { get; set; }
        public int Yıl { get; set; }
    }
}
