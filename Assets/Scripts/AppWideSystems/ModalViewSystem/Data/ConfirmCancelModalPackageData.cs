using System;
using UI.ModalViews.Abstracts;

namespace UI.ModalViews.Data
{
    public class ConfirmCancelModalPackageData : ModalPackageData
    {
        public string ConfirmButtonText { get; private set; } = "Confirm";
        public string CancelButtonText { get; private set; } = "Cancel";

        private Action _onConfirm;
        private Action _onCancel;
        
        public override ModalType ModalType => ModalType.ConfirmCancel;

        public void SetConfirmButtonText(string text)
        {
            ConfirmButtonText = text;
        }
        
        public void SetCancelButtonText(string text)
        {
            CancelButtonText = text;
        }

        public void SetConfirmAction(Action onConfirm)
        {
            _onConfirm = onConfirm;
        }
        
        public void SetCancelAction(Action onCancel)
        {
            _onCancel = onCancel;
        }
        
        public void InvokeConfirmAction()
        {
            _onConfirm?.Invoke();
        }
        
        public void InvokeCancelAction()
        {
            _onCancel?.Invoke();
        }
    }
}