using AppWideSystems.EmailSystem.Utils;
using UnityEngine;

namespace AppWideSystems.EmailSystem.Data
{
    [CreateAssetMenu(menuName = EmailSystemUtils.EMAIL_SYSTEM_CREATE_DATA_ASSET_PATH + nameof(EmailProviderData), fileName = "EmailProviderData")]
    public class EmailProviderData : ScriptableObject
    {
        [Header("Provider Info")] 
        [SerializeField] private string emailProvider = "smtp.gmail.com";

        [Header("Account Info")] 
        [SerializeField] private string adminEmail = "admin@google.com";
        [SerializeField] private string adminPassword = "P4ssw0rd";
        
        public string EmailProvider => emailProvider;
        public string AdminEmail => adminEmail;
        public string AdminPassword => adminPassword;
    }
}