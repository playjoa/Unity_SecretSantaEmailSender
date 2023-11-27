using System;
using UI.ModalViews.Abstracts;

namespace UI.ModalViews.Data
{
    public class InformationModalPackageData : ModalPackageData
    {
        public string ButtonText { get; private set; } = "Ok";

        private Action _onConfirmAction;
        
        public override ModalType ModalType => ModalType.Information;

        public InformationModalPackageData() : base()
        {
        }
        
        public void SetButtonText(string buttonText)
        {
            ButtonText = buttonText;
        }

        public void SetConfirmButtonAction(Action onConfirm)
        {
            _onConfirmAction = onConfirm;
        }

        public void InvokeConfirmInformation()
        {
            _onConfirmAction?.Invoke();
        }
    }
}