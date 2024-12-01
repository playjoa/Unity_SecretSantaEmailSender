using System;
using System.Collections;
using System.Net;
using System.Net.Mail;
using AppWide.AppInitialization.Controller;
using AppWideSystems.AppInitialization.Interfaces;
using AppWideSystems.EmailSystem.Data;
using UnityEngine;
using Utils_Scripts.Patterns;

namespace AppWideSystems.EmailSystem.Controller
{
    public class EmailSystemController : MonoBehaviourSingleton<EmailSystemController>, IAppWideSystem
    {
        [Header("Provider Data")]
        [SerializeField] private EmailProviderData emailProviderData;
        
        public string AppSystemName => "E-mail System";

        private SmtpClient _emailClient;
        
        public IEnumerator Initiate(AppInitializationController appInitializationController)
        {
            InitiateEmailProvider(emailProviderData);
            yield return true;
        }

        private void InitiateEmailProvider(EmailProviderData providerData)
        {
            Debug.Log($"Creating Email Provider! Provider: {providerData.EmailProvider} E-mail: {providerData.AdminEmail} Password: {providerData.AdminPassword}");
            
            _emailClient = new SmtpClient(providerData.EmailProvider, 587);
            
            _emailClient.UseDefaultCredentials = false;
            _emailClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            _emailClient.Credentials = new NetworkCredential(providerData.AdminEmail, providerData.AdminPassword) as ICredentialsByHost;
            _emailClient.EnableSsl = true;
                
            ServicePointManager.ServerCertificateValidationCallback = (s, certificate, chain, sslPolicyErrors) => true;
        }

        public void SendEmail(EmailContentData emailContentData, Action onEmailSent = null, Action onEmailFailedSent = null)
        {
            try
            {
                var mailMessage = new MailMessage();
                mailMessage.From = new MailAddress
                (
                    emailProviderData.AdminEmail,
                    emailContentData.EmailDisplayName,
                    System.Text.Encoding.UTF8
                );

                foreach (var emailTarget in emailContentData.EmailTargets)
                {
                    mailMessage.To.Add(new MailAddress(emailTarget));
                }

                mailMessage.Subject = emailContentData.EmailSubject;
                mailMessage.Body = emailContentData.EmailBody;
                mailMessage.IsBodyHtml = emailContentData.IsHtmlBody;
                
                _emailClient.Send(mailMessage);
                onEmailSent?.Invoke();
                foreach (var mail in mailMessage.To)
                {
                    Debug.Log($"Email Sent to {mail.Address} with subject: {mailMessage.Subject}");
                }
            }
            catch (SmtpException ex)
            {
                onEmailFailedSent?.Invoke();
                Debug.LogError($"Email Failed to send: {ex.Message}");
            }
            catch (Exception ex)
            {
                onEmailFailedSent?.Invoke();
                Debug.LogError($"An unexpected error occurred: {ex.Message}");
            }
        }
    }
}