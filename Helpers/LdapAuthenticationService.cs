using Microsoft.Extensions.Options;
using LDap.Models;
using System;
using System.DirectoryServices;
using Microsoft.Extensions.Logging;

namespace LDap.Helpers
{
    public class LdapAuthenticationService
    {
        private readonly string _ldapBaseDn;
        private readonly string _ldapServer;
        private readonly int _ldapPort;
        private readonly ILogger<LdapAuthenticationService> _logger;

        public LdapAuthenticationService(IOptions<LdapSettings> ldapSettings, ILogger<LdapAuthenticationService> logger)
        {
            _ldapBaseDn = ldapSettings.Value.LdapBaseDn;
            _ldapServer = ldapSettings.Value.LdapServer;
            _ldapPort = ldapSettings.Value.LdapPort;
            _logger = logger;
        }

        public bool ValidateCredentials(string username, string password)
        {
            try
            {
                if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                {
                    _logger.LogError("Kullanıcı adı veya şifre boş olamaz.");
                    return false;
                }

                // Dizin bağlantısı oluşturma
                using (var entry = new DirectoryEntry($"LDAP://{_ldapServer}:{_ldapPort}/{_ldapBaseDn}", username, password, AuthenticationTypes.Secure))
                {
                    // Bağlantıyı test et
                    object nativeObject = entry.NativeObject;
                }

                // Bağlantıda bir hata olmadıysa, doğrulama başarılıdır
                return true;
            }
            catch (DirectoryServicesCOMException ex)
            {
                _logger.LogError($"LDAP bağlantı hatası: {ex.Message}");
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Bir hata oluştu: {ex.Message}");
                return false;
            }
        }
    }
}
