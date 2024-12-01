using System.Text;
using AppWideSystems.SecretSantaSystem.Data;

namespace AppWideSystems.SecretSantaSystem.Util
{
    public static class SecretSantaUtils
    {
        public const string SECRET_SANTA_CREATE_ASSET_PATH = "SecretSanta/";
        public const string SECRET_SANTA_DATA_CREATE_ASSET_PATH = SECRET_SANTA_CREATE_ASSET_PATH + "Data";

        // HTML Keys
        public const string HTML_USERNAME_KEY = "{UserName}";
        public const string HTML_SECRETSANTA_USERNAME_KEY = "{SecretSantaName}";
        public const string HTML_SECRETSANTA_EMAIL_KEY = "{SecretSantaEmail}";
        public const string HTML_SECRETSANTA_PHONENUMBER_KEY = "{SecretSantaPhone}";
        
        public static string GenerateEmailSubject(this SecretSantaResult secretSantaResult)
        {
            return $"Sorteio do Amigo Secreto da Família Castro Natal 2023 - {secretSantaResult.User.UserName}";
        }

        public static string GenerateHtmlEmailBody(this SecretSantaResult secretSantaResult, EmailTemplateData emailTemplateData)
        {
            if (emailTemplateData == null || emailTemplateData.EmailHtmlTemplate == null)
            {
                return string.Empty;
            }

            var emailBody = emailTemplateData.EmailHtmlTemplate.text;

            emailBody = emailBody.Replace(HTML_USERNAME_KEY, secretSantaResult.User.UserName);
            emailBody = emailBody.Replace(HTML_SECRETSANTA_USERNAME_KEY, secretSantaResult.DrawnedUser.UserName);
            emailBody = emailBody.Replace(HTML_SECRETSANTA_EMAIL_KEY, secretSantaResult.DrawnedUser.UserEmail);
            emailBody = emailBody.Replace(HTML_SECRETSANTA_PHONENUMBER_KEY, secretSantaResult.DrawnedUser.UserPhoneNumber);

            return emailBody;
        }
    }
}