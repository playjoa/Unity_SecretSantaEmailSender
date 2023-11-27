using System.Collections.Generic;
using AppWideSystems.SecretSantaSystem.Util;
using UnityEngine;

namespace AppWideSystems.SecretSantaSystem.Data
{
    [CreateAssetMenu(menuName = SecretSantaUtils.SECRET_SANTA_DATA_CREATE_ASSET_PATH + nameof(SecretSantaResultData), fileName = "SecretSantaResultData")]
    public class SecretSantaResultData : ScriptableObject
    {
        [Header("Results")]
        [SerializeField] private List<SecretSantaResult> secretSantaResultList;

        public List<SecretSantaResult> SecretSantaResultList => secretSantaResultList;

        public void SetResult(List<SecretSantaResult> secretSantaResults)
        {
            secretSantaResultList = new List<SecretSantaResult>(secretSantaResults);
        }
    }
}