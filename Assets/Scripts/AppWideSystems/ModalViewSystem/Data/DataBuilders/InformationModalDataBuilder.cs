using System;
using UI.ModalViews.Abstracts;

namespace UI.ModalViews.Data.DataBuilders
{
    public class InformationModalDataBuilder : ModalDataBuilder<InformationModalPackageData>
    {
        public InformationModalDataBuilder() : base(new InformationModalPackageData())
        {
        }

        public InformationModalDataBuilder AddButtonText(string buttonText)
        {
            modalPackageData.SetButtonText(buttonText);
            return this;
        }
        
        public InformationModalDataBuilder AddConfirmAction(Action onConfirmAction)
        {
            modalPackageData.SetConfirmButtonAction(onConfirmAction);
            return this;
        }

        public override InformationModalPackageData Build()
        {
            return modalPackageData;
        }
    }
}