using TMPro;
using UI.ModalViews.Abstracts;
using UI.ModalViews.Data;
using UnityEngine;
using UnityEngine.UI;

namespace UI.ModalViews.Components
{
    public class InformationModalView : ModalView<InformationModalPackageData>
    {
        [Header("Texts")] 
        [SerializeField] private TextMeshProUGUI buttonTMP;
        
        [Header("Buttons")] 
        [SerializeField] private Button confirmInformationButton;

        public override ModalType ModalType => ModalType.Information;

        private void OnEnable()
        {
            confirmInformationButton.onClick.AddListener(OnConfirmInformationClickHandler);
        }

        private void OnDisable()
        {
            confirmInformationButton.onClick.RemoveListener(OnConfirmInformationClickHandler);
        }

        protected override void OnInitiated(InformationModalPackageData modalPackageData)
        {
            buttonTMP.text = modalPackageData.ButtonText;
        }

        private void OnConfirmInformationClickHandler()
        {
            CurrentDisplayingData?.InvokeConfirmInformation();
        }
    }
}