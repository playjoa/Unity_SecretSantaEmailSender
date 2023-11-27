using AppWideSystems.SecretSantaSystem.Controller;
using UnityEngine;
using UnityEngine.UI;

namespace AppWideSystems.SecretSantaSystem.UI
{
    public class SecretSantaUIPanel : MonoBehaviour
    {
        [Header("UI Components")]
        [SerializeField] private Button drawUsersButton;
        [SerializeField] private Button notifyUsersButton;
        [SerializeField] private Button resetNotifyUsersButton;

        private SecretSantaController SecretSanta => SecretSantaController.ME; 
        
        private void Awake()
        {
            drawUsersButton.onClick.AddListener(OnDrawUsersClickHandler);
            notifyUsersButton.onClick.AddListener(OnNotifyUsersClickHandler);
            resetNotifyUsersButton.onClick.AddListener(OnResetNotifiedUsersClickHandler);
        }

        private void OnDestroy()
        {
            drawUsersButton.onClick.RemoveListener(OnDrawUsersClickHandler);
            notifyUsersButton.onClick.RemoveListener(OnNotifyUsersClickHandler);
            resetNotifyUsersButton.onClick.RemoveListener(OnResetNotifiedUsersClickHandler);
        }
        
        private void OnDrawUsersClickHandler()
        {
            SecretSanta.GenerateDraw();
        }
        
        private void OnNotifyUsersClickHandler()
        {
            SecretSanta.NotifyResultUsers();
        }
        
        private void OnResetNotifiedUsersClickHandler()
        {
            SecretSanta.ResetNotifiedUsers();
        }
    }
}