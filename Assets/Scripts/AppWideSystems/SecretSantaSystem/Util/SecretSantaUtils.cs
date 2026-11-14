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
        
        public static string GenerateEmailSubject(string recipientName)
        {
            return $"Your Secret Santa draw - {recipientName}";
        }

        public static string GenerateEmailSubject(this SecretSantaResult secretSantaResult)
        {
            return GenerateEmailSubject(secretSantaResult.User.UserName);
        }

        public static string FillEmailHtmlTemplate(string htmlTemplate, string userName, string secretSantaName, string secretSantaEmail, string secretSantaPhone)
        {
            if (string.IsNullOrEmpty(htmlTemplate))
            {
                return string.Empty;
            }

            userName ??= string.Empty;
            secretSantaName ??= string.Empty;
            secretSantaEmail ??= string.Empty;
            secretSantaPhone ??= string.Empty;

            return htmlTemplate
                .Replace(HTML_USERNAME_KEY, userName)
                .Replace(HTML_SECRETSANTA_USERNAME_KEY, secretSantaName)
                .Replace(HTML_SECRETSANTA_EMAIL_KEY, secretSantaEmail)
                .Replace(HTML_SECRETSANTA_PHONENUMBER_KEY, secretSantaPhone);
        }

        public static string GenerateHtmlEmailBody(this SecretSantaResult secretSantaResult, EmailTemplateData emailTemplateData)
        {
            if (emailTemplateData == null || emailTemplateData.EmailHtmlTemplate == null)
            {
                return string.Empty;
            }

            return FillEmailHtmlTemplate(
                emailTemplateData.EmailHtmlTemplate.text,
                secretSantaResult.User.UserName,
                secretSantaResult.DrawnedUser.UserName,
                secretSantaResult.DrawnedUser.UserEmail,
                secretSantaResult.DrawnedUser.UserPhoneNumber);
        }
    }
}