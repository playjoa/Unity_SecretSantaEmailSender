using UI.ModalViews.Abstracts;

namespace UI.ModalViews.Data.DataBuilders
{
    public class AuthenticateFromAnonymousModalDataBuilder : ModalDataBuilder<AuthenticateFromAnonymousModalPackageData>
    {
        public AuthenticateFromAnonymousModalDataBuilder() : base(new AuthenticateFromAnonymousModalPackageData())
        {
        }

        public override AuthenticateFromAnonymousModalPackageData Build()
        {
            return modalPackageData;
        }
        
        public AuthenticateFromAnonymousModalDataBuilder AddGoogleLoginAction(System.Action googleLoginAction)
        {
            modalPackageData.SetGoogleLoginChoice(googleLoginAction);
            return this;
        }
        
        public AuthenticateFromAnonymousModalDataBuilder AddAppleLoginAction(System.Action appleLoginAction)
        {
            modalPackageData.SetAppleLoginChoice(appleLoginAction);
            return this;
        }
    }
}