using System;
using UI.ModalViews.Abstracts;

namespace UI.ModalViews.Data
{
    public class AuthenticateFromAnonymousModalPackageData : ModalPackageData
    {
        public override ModalType ModalType => ModalType.AuthenticationFromAnonymous;
        
        private Action _onGoogleLoginChoice;
        private Action _onAppleLoginChoice;
        
        public void SetGoogleLoginChoice(Action onConfirm)
        {
            _onGoogleLoginChoice = onConfirm;
        }
        
        public void SetAppleLoginChoice(Action onConfirm)
        {
            _onAppleLoginChoice = onConfirm;
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