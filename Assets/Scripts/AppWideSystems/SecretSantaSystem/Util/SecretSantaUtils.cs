using System.Text;
using AppWideSystems.SecretSantaSystem.Data;

namespace AppWideSystems.SecretSantaSystem.Util
{
    public static class SecretSantaUtils
    {
        public const string SECRET_SANTA_CREATE_ASSET_PATH = "SecretSanta/";
        public const string SECRET_SANTA_DATA_CREATE_ASSET_PATH = SECRET_SANTA_CREATE_ASSET_PATH + "Data";

        public static string GenerateEmailSubject(this SecretSantaResult secretSantaResult)
        {
            return $"Sorteio do Amigo Secreto da Família Castro Natal 2023 - {secretSantaResult.User.UserName}";
        }

        public static string GenerateHtmlEmailBody(this SecretSantaResult secretSantaResult)
        {
            var emailBodyStringBuilder = new StringBuilder();

            emailBodyStringBuilder.AppendLine("<!DOCTYPE html>");
            emailBodyStringBuilder.AppendLine("<html lang=\"pt-br\">");
            emailBodyStringBuilder.AppendLine("<head>");
            emailBodyStringBuilder.AppendLine("  <meta charset=\"UTF-8\">");
            emailBodyStringBuilder.AppendLine("  <meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\">");
            emailBodyStringBuilder.AppendLine("  <title>Resultado Amigo Secreto</title>");
            emailBodyStringBuilder.AppendLine("  <style>");
            emailBodyStringBuilder.AppendLine("    body {");
            emailBodyStringBuilder.AppendLine("      font-family: 'Arial', sans-serif;");
            emailBodyStringBuilder.AppendLine("      line-height: 1.6;");
            emailBodyStringBuilder.AppendLine("      margin: 20px;");
            emailBodyStringBuilder.AppendLine("      padding: 20px;");
            emailBodyStringBuilder.AppendLine("      background-color: #f7f7f7;");
            emailBodyStringBuilder.AppendLine("    }");
            emailBodyStringBuilder.AppendLine("");
            emailBodyStringBuilder.AppendLine("    p {");
            emailBodyStringBuilder.AppendLine("      font-size: 16px;");
            emailBodyStringBuilder.AppendLine("      color: #333;");
            emailBodyStringBuilder.AppendLine("    }");
            emailBodyStringBuilder.AppendLine("");
            emailBodyStringBuilder.AppendLine("    h2 {");
            emailBodyStringBuilder.AppendLine("      color: #e60712;");
            emailBodyStringBuilder.AppendLine("    }");
            emailBodyStringBuilder.AppendLine("");
            emailBodyStringBuilder.AppendLine("    span.friend {");
            emailBodyStringBuilder.AppendLine("      font-size: 22px;");
            emailBodyStringBuilder.AppendLine("      font-weight: bold;");
            emailBodyStringBuilder.AppendLine("      color: #1e90ff;");
            emailBodyStringBuilder.AppendLine("    }");
            emailBodyStringBuilder.AppendLine("");
            emailBodyStringBuilder.AppendLine("    span.contact {");
            emailBodyStringBuilder.AppendLine("      font-weight: bold;");
            emailBodyStringBuilder.AppendLine("      color: #0eb525;");
            emailBodyStringBuilder.AppendLine("    }");
            emailBodyStringBuilder.AppendLine("");
            emailBodyStringBuilder.AppendLine("    br {");
            emailBodyStringBuilder.AppendLine("      margin-bottom: 10px;");
            emailBodyStringBuilder.AppendLine("    }");
            emailBodyStringBuilder.AppendLine("  </style>");
            emailBodyStringBuilder.AppendLine("</head>");
            emailBodyStringBuilder.AppendLine("<body>");
            emailBodyStringBuilder.AppendLine("  <h2>Resultado do Amigo Secreto da Família</h2>");
            emailBodyStringBuilder.AppendLine("  <p>Querida Família,</p>");
            emailBodyStringBuilder.AppendLine("  <p>É com grande alegria que revelamos os resultados do nosso tão aguardado sorteio de Amigo Secreto da família deste ano! 🎁✨</p>");
            emailBodyStringBuilder.AppendLine($"  <p><span class=\"friend\">{secretSantaResult.User.UserName}</span> - Seu Amigo Secreto é: <span class=\"friend\">{secretSantaResult.DrawnedUser.UserName}</span></p>");
            emailBodyStringBuilder.AppendLine($"  <p>Se você quiser contactar o seu amigo(a) secreto(a), aqui está o contato(a) dele(a). E-mail: <span class=\"contact\">{secretSantaResult.DrawnedUser.UserEmail}</span> | WhatsApp: <span class=\"contact\">{secretSantaResult.DrawnedUser.UserPhoneNumber}</span></p>");
            emailBodyStringBuilder.AppendLine("  <p>Lembrem-se, o espírito do Amigo Secreto é mais do que apenas presentes; é sobre espalhar amor, carinho e criar memórias duradouras. 🎄🤗</p>");
            emailBodyStringBuilder.AppendLine("  <p>Então, preparem-se para surpreender seus Amigos Secretos com gestos carinhosos e presentes cuidadosamente escolhidos.</p>");
            emailBodyStringBuilder.AppendLine("  <p>Que este Natal seja repleto de risos, amor e momentos preciosos compartilhados entre todos nós!</p>");
            emailBodyStringBuilder.AppendLine("</body>");
            emailBodyStringBuilder.AppendLine("</html>");
            
            return emailBodyStringBuilder.ToString();
        }
    }
}