using System;
using UI.ModalViews.Abstracts;

namespace UI.ModalViews.Data.DataBuilders
{
    public class AuthenticationModalDataBuilder : ModalDataBuilder<AuthenticationModalPackageData>
    {
        public AuthenticationModalDataBuilder() : base(new AuthenticationModalPackageData())
        {
        }

        public override AuthenticationModalPackageData Build()
        {
            return modalPackageData;
        }

        public AuthenticationModalDataBuilder AddGuestLoginAction(Action guestLoginAction)
        {
            modalPackageData.SetGuestLoginChoice(guestLoginAction);
            return this;
        }

        public AuthenticationModalDataBuilder AddGoogleLoginAction(Action googleLoginAction)
        {
            modalPackageData.SetGoogleLoginChoice(googleLoginAction);
            return this;
        }

        public AuthenticationModalDataBuilder AddAppleLoginAction(Action appleLoginAction)
        {
            modalPackageData.SetAppleLoginChoice(appleLoginAction);
            return this;
        }
    }
}