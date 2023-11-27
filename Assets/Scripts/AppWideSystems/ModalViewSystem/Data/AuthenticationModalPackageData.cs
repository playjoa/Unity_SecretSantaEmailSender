using System;
using UI.ModalViews.Abstracts;

namespace UI.ModalViews.Data
{
    public class AuthenticationModalPackageData : ModalPackageData
    {
        public override ModalType ModalType => ModalType.Authentication;

        private Action _onGuestLoginChoice;
        private Action _onGoogleLoginChoice;
        private Action _onAppleLoginChoice;
        
        public void SetGuestLoginChoice(Action onConfirm)
        {
            _onGuestLoginChoice = onConfirm;
        }
        
        public void SetGoogleLoginChoice(Action onConfirm)
        {
            _onGoogleLoginChoice = onConfirm;
        }
        
        public void SetAppleLoginChoice(Action onConfirm)
        {
            _onAppleLoginChoice = onConfirm;
        }
        
        public void InvokeGuestLoginAction()
        {
            _onGuestLoginChoice?.Invoke();
        }
        
        public void InvokeGoogleLoginAction()
        {
            _onGoogleLoginChoice?.Invoke();
        }
        
        public void InvokeAppleLoginAction()
        {
            _onAppleLoginChoice?.Invoke();
        }
    }
}