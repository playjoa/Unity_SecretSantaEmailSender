using AppWideSystems.SecretSantaSystem.Util;
using UnityEngine;
using Utils.UniqueId.Components;

namespace AppWideSystems.SecretSantaSystem.Data
{
    [CreateAssetMenu(menuName = SecretSantaUtils.SECRET_SANTA_DATA_CREATE_ASSET_PATH + nameof(SecretSantaUserData), fileName = "SecretSantaUser")]
    public class SecretSantaUserData : ScriptableObjectWithId
    {
        [Header("User Data")]
        [SerializeField] private string userName = "John";
        [SerializeField] private string userEmail = "john@gmail.com";
        [SerializeField] private string userPhoneNumber = "+ 55 99999-99999";

        public string UserName => userName;
        public string UserEmail => userEmail;
        public string UserPhoneNumber => userPhoneNumber;
    }
}