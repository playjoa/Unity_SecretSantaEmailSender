using System;
using UI.ModalViews.Abstracts;

namespace UI.ModalViews.Data.DataBuilders
{
    public class ConfirmCancelModalPackageDataBuilder : ModalDataBuilder<ConfirmCancelModalPackageData>
    {
        public ConfirmCancelModalPackageDataBuilder() : base(new ConfirmCancelModalPackageData())
        {
        }

        public ConfirmCancelModalPackageDataBuilder AddConfirmButtonText(string text)
        {
            modalPackageData.SetConfirmButtonText(text);
            return this;
        }

        public ConfirmCancelModalPackageDataBuilder AddCancelButtonText(string text)
        {
            modalPackageData.SetCancelButtonText(text);
            return this;
        }

        public ConfirmCancelModalPackageDataBuilder AddConfirmAction(Action onConfirm)
        {
            modalPackageData.SetConfirmAction(onConfirm);
            return this;
        }

        public ConfirmCancelModalPackageDataBuilder AddCancelAction(Action onCancel)
        {
            modalPackageData.SetCancelAction(onCancel);
            return this;
        }

        public override ConfirmCancelModalPackageData Build()
        {
            return modalPackageData;
        }
    }
}