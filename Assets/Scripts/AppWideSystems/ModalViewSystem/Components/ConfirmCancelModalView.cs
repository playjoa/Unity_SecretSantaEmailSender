using TMPro;
using UI.ModalViews.Abstracts;
using UI.ModalViews.Data;
using UnityEngine;
using UnityEngine.UI;

namespace UI.ModalViews.Components
{
    public class ConfirmCancelModalView : ModalView<ConfirmCancelModalPackageData>
    {
        [Header("Buttons")] 
        [SerializeField] private Button confirmButton;
        [SerializeField] private Button cancelButton;
        
        [Header("Texts")] 
        [SerializeField] private TextMeshProUGUI confirmTMP;
        [SerializeField] private TextMeshProUGUI cancelTMP;

        public override ModalType ModalType => ModalType.ConfirmCancel;
        
        private void OnEnable()
        {
            confirmButton.onClick.AddListener(OnConfirmButtonClickHandler);
            cancelButton.onClick.AddListener(OnCancelButtonClickHandler);
        }

        private void OnDisable()
        {
            confirmButton.onClick.RemoveListener(OnConfirmButtonClickHandler);
            cancelButton.onClick.RemoveListener(OnCancelButtonClickHandler);
        }

        protected override void OnInitiated(ConfirmCancelModalPackageData modalPackageData)
        {
            confirmTMP.text = modalPackageData.ConfirmButtonText;
            cancelTMP.text = modalPackageData.CancelButtonText;
        }

        private void OnConfirmButtonClickHandler()
        {
            CurrentDisplayingData?.InvokeConfirmAction();
        }
        
        private void OnCancelButtonClickHandler()
        {
            CurrentDisplayingData?.InvokeCancelAction();
        }
    }
}