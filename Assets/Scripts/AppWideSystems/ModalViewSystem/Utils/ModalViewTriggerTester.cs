using UI.ModalViews.Controller;
using UI.ModalViews.Data.DataBuilders;
using UnityEngine;

namespace UI.ModalViews.Utils
{
    public class ModalViewTriggerTester : MonoBehaviour
    {
        [Header("Tester State")] 
        [SerializeField] private bool testerActive = false;
        
        [Header("Information Modal Configuration")] 
        [SerializeField] private KeyCode triggerInformationKeyCode = KeyCode.I;
        [SerializeField] private string infoTitle = "Information Modal";
        [SerializeField] private string infoDesc = "This is an informational modal view!";
        [SerializeField] private string infoButtonText = "Ok";
        [SerializeField] private Sprite infoSprite;
        
        [Header("Confirm Cancel Modal Configuration")] 
        [SerializeField] private KeyCode triggerConfirmCancelKeyCode = KeyCode.C;
        [SerializeField] private string confirmTitle = "Confirm or Cancel";
        [SerializeField] private string confirmDesc = "This is a Confirm or Cancel Modal";
        [SerializeField] private string confirmButtonText = "Confirm";
        [SerializeField] private string cancelButtonText = "Cancel";
        [SerializeField] private Sprite confirmSprite;
        
        private void Update()
        {
            if (!testerActive) return;
            
            if (Input.GetKeyDown(triggerInformationKeyCode))
                RequestInformationModal();
            
            if (Input.GetKeyDown(triggerConfirmCancelKeyCode))
                RequestConfirmCancelModal();
        }
        
        private void RequestInformationModal()
        {
            var modalPackageBuilder = new InformationModalDataBuilder()
                .AddButtonText(infoButtonText)
                .AddTitle(infoTitle)
                .AddDescription(infoDesc)
                .AddSprite(infoSprite);
            
            ModalViewController.ME.RequestModalView(modalPackageBuilder.Build());
        }

        private void RequestConfirmCancelModal()
        {
            var modalPackageBuilder = new ConfirmCancelModalPackageDataBuilder()
                .AddConfirmButtonText(confirmButtonText)
                .AddCancelButtonText(cancelButtonText)
                .AddTitle(confirmTitle)
                .AddDescription(confirmDesc)
                .AddSprite(confirmSprite);
            
            ModalViewController.ME.RequestModalView(modalPackageBuilder.Build());
        }
    }
}