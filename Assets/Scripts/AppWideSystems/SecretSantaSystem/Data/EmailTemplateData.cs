using AppWideSystems.SecretSantaSystem.Util;
using UnityEngine;

namespace AppWideSystems.SecretSantaSystem.Data
{
    [CreateAssetMenu(menuName = SecretSantaUtils.SECRET_SANTA_DATA_CREATE_ASSET_PATH + nameof(EmailTemplateData), fileName = "EmailTemplateData")]
    public class EmailTemplateData : ScriptableObject
    {
        [Header("HTML")]
        [SerializeField] private TextAsset emailHtmlTemplate;
        
        public TextAsset EmailHtmlTemplate => emailHtmlTemplate;
    }
}