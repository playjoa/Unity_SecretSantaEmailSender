using System.Collections.Generic;
using AppWideSystems.SecretSantaSystem.Util;
using UnityEngine;
using UnityEngine.Serialization;

namespace AppWideSystems.SecretSantaSystem.Data
{
    [CreateAssetMenu(menuName = SecretSantaUtils.SECRET_SANTA_DATA_CREATE_ASSET_PATH + nameof(SecretSantaCandidatesData), fileName = "SecretSantaCandidatesData")]
    public class SecretSantaCandidatesData : ScriptableObject
    {
        [FormerlySerializedAs("secretSantaResultList")]
        [Header("Results")]
        [SerializeField] private List<SecretSantaUserData> secretSantaCandidatesList;

        public List<SecretSantaUserData> SecretSantaCandidatesList => secretSantaCandidatesList;

    }
}