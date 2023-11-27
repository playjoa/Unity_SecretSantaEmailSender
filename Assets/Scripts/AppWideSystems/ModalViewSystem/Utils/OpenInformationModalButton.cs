using UI.ModalViews.Controller;
using UI.ModalViews.Data;
using UI.ModalViews.Data.DataBuilders;
using UnityEngine;
using UnityEngine.UI;

namespace UI.ModalViews.Utils
{
    public class OpenInformationModalButton : MonoBehaviour
    {
        [Header("Target Button")]
        [SerializeField] private Button openModalButton;
        
        [Header("Modal Configuration")]
        [SerializeField] private string modalTitle = "This is the Title";
        [SerializeField] [TextArea] private string modalDesc = "This is the description for this modal";
        [SerializeField] private string modalButtonText = "Ok";
        [SerializeField] private Sprite modalSprite;

        private void OnEnable()
        {
            openModalButton.onClick.AddListener(OnOpenModalClickHandler);
        }

        private void OnDisable()
        {
            openModalButton.onClick.RemoveListener(OnOpenModalClickHandler);
        }

        private void OnOpenModalClickHandler()
        {
            var modalPackageBuilder = new InformationModalDataBuilder()
                .AddButtonText(modalButtonText)
                .AddTitle(modalTitle)
                .AddDescription(modalDesc)
                .AddSprite(modalSprite);
            
            ModalViewController.ME.RequestModalView(modalPackageBuilder.Build());
        }
    }
}