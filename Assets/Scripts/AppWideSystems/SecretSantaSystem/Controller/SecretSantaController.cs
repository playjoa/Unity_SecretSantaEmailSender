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
            if (currentSecretSantaCandidates == null) return;
            if (currentSecretSantaResultData == null) return;
            if (!currentSecretSantaCandidates.SecretSantaCandidatesList.Any()) return;

            var candidatesPoolList = currentSecretSantaCandidates.SecretSantaCandidatesList.ToList();
            var resultsList = new List<SecretSantaResult>();

            foreach (var secretSantaCandidate in currentSecretSantaCandidates.SecretSantaCandidatesList)
            {
                var drawnedUser = DrawUser(secretSantaCandidate, candidatesPoolList);
                var secretSantaResult = new SecretSantaResult(secretSantaCandidate, drawnedUser);

                if (drawnedUser == null)
                {
                    Debug.LogError($"User: {secretSantaCandidate.UserName} could not have a drawned participant. Please try again!");
                    return;
                }

                resultsList.Add(secretSantaResult);
                Debug.Log($"User: {secretSantaCandidate.UserName} Drawned: {drawnedUser.UserName}");
            }
            
            currentSecretSantaResultData.SetResult(resultsList);

#if UNITY_EDITOR
            EditorUtility.SetDirty(currentSecretSantaResultData);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();       
#endif
            
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
                var emailBody = secretSantaResult.GenerateHtmlEmailBody();

                var emailContent = new EmailContentData()
                {
                    EmailTargets = new List<string> { secretSantaResult.User.UserEmail },
                    IsHtmlBody = true,
                    EmailSubject = emailSubject,
                    EmailBody = emailBody
                };

                EmailSystem.SendEmail(emailContent, () => { secretSantaResult.SetNotifiedUser(); });
            }
            
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