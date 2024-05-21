// LdapSettings.cs
using System.ComponentModel.DataAnnotations;

namespace LDap.Models
{
    public class LdapSettings
    {
        [Required(ErrorMessage = "LdapServer is required.")]
        public string LdapServer { get; set; }

        [Required(ErrorMessage = "LdapPort is required.")]
        public int LdapPort { get; set; }

        [Required(ErrorMessage = "LdapBaseDn is required.")]
        public string LdapBaseDn { get; set; }
    }
}