using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AppWide.AppInitialization.Controller;
using AppWideSystems.AppInitialization.Interfaces;
using AppWideSystems.EmailSystem.Controller;
using AppWideSystems.EmailSystem.Data;
using AppWideSystems.SecretSantaSystem.Data;
using AppWideSystems.SecretSantaSystem.Util;
using UnityEngine;
using Utils_Scripts.Patterns;
using Utils.Extensions;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace AppWideSystems.SecretSantaSystem.Controller
{
    public class SecretSantaController : MonoBehaviourSingleton<SecretSantaController>, IAppWideSystem
    {
        [Header("Data")]
        [SerializeField] private EmailTemplateData emailTemplateData;
        [SerializeField] private SecretSantaCandidatesData currentSecretSantaCandidates;
        [SerializeField] private SecretSantaResultData currentSecretSantaResultData;

        public event Action<SecretSantaResultData> OnUsersDrawn;
        public event Action<SecretSantaResultData> OnNotifiedUsers;
        
        public string AppSystemName => "Secret Santa Controller";

        private EmailSystemController EmailSystem => EmailSystemController.ME;
        
        public IEnumerator Initiate(AppInitializationController appInitializationController)
        {
            yield return true;
        }

        public void GenerateDraw()
        {
            if (currentSecretSantaCandidates == null || currentSecretSantaResultData == null)
                return;

            var candidates = currentSecretSantaCandidates.SecretSantaCandidatesList;
            if (candidates == null || candidates.Count < 2)
            {
                Debug.LogError("Not enough candidates for a valid Secret Santa draw!");
                return;
            }

            List<SecretSantaUserData> shuffledCandidates;
            var maxRetries = 100;
            bool validDraw;

            do
            {
                shuffledCandidates = new List<SecretSantaUserData>(candidates);
                shuffledCandidates.Shuffle();
                validDraw = true;
                
                for (var i = 0; i < candidates.Count; i++)
                {
                    if (candidates[i].Id == shuffledCandidates[i].Id)
                    {
                        validDraw = false;
                        Debug.LogWarning("Not a valid Secret Santa draw! Retrying...");
                        break;
                    }
                }

                maxRetries--;
                if (maxRetries <= 0)
                {
                    Debug.LogError("Failed to generate a valid Secret Santa draw after multiple retries.");
                    return;
                }
            } while (!validDraw);

            var resultsList = new List<SecretSantaResult>();
            for (var i = 0; i < candidates.Count; i++)
            {
                resultsList.Add(new SecretSantaResult(candidates[i], shuffledCandidates[i]));
            }
            
            currentSecretSantaResultData.SetResult(resultsList);

#if UNITY_EDITOR
            EditorUtility.SetDirty(currentSecretSantaResultData);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
#endif

            Debug.Log("Secret Santa draw generated successfully!");
            OnUsersDrawn?.Invoke(currentSecretSantaResultData);
        }

        private SecretSantaUserData DrawUser(SecretSantaUserData targetUser, List<SecretSantaUserData> usersPool)
        {
            var poolWithoutTarget = usersPool.Where(u => !u.Id.Equals(targetUser.Id)).ToList();
            var randomUserDrawn = poolWithoutTarget.RandomElement();

            usersPool.Remove(randomUserDrawn);
            return randomUserDrawn;
        }

        public void NotifyResultUsers()
        {
            if (currentSecretSantaResultData == null) return;
            if (!currentSecretSantaResultData.SecretSantaResultList.Any()) return;

            foreach (var secretSantaResult in currentSecretSantaResultData.SecretSantaResultList)
            {
                if (secretSantaResult.NotifiedUser) continue;

                var emailSubject = secretSantaResult.GenerateEmailSubject();
                var emailBody = secretSantaResult.GenerateHtmlEmailBody(emailTemplateData);

                var emailContent = new EmailContentData()
                {
                    EmailTargets = new List<string> { secretSantaResult.User.UserEmail },
                    IsHtmlBody = true,
                    EmailSubject = emailSubject,
                    EmailBody = emailBody
                };

                EmailSystem.SendEmail(emailContent, () => { secretSantaResult.SetNotifiedUser(); });
            }
            
#if UNITY_EDITOR
            EditorUtility.SetDirty(currentSecretSantaResultData);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();       
#endif
            
            OnNotifiedUsers?.Invoke(currentSecretSantaResultData);
        }

        public void ResetNotifiedUsers()
        {
            if (currentSecretSantaResultData == null) return;
            if (!currentSecretSantaResultData.SecretSantaResultList.Any()) return;

            foreach (var secretSantaResult in currentSecretSantaResultData.SecretSantaResultList)
            {
                secretSantaResult.ResetNotifiedUser();
            }
        }
    }
}