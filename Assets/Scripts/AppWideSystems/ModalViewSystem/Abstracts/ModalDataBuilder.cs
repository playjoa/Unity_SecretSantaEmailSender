using UnityEngine;

namespace UI.ModalViews.Abstracts
{
    public abstract class ModalDataBuilder<TModalPackageData> where TModalPackageData : ModalPackageData
    {
        protected readonly TModalPackageData modalPackageData;
        
        protected ModalDataBuilder(TModalPackageData modalPackageData)
        {
            this.modalPackageData = modalPackageData;
        }
        
        public ModalDataBuilder<TModalPackageData> AddTitle(string title)
        {
            modalPackageData.SetTitle(title);
            return this;
        }

        public ModalDataBuilder<TModalPackageData> AddDescription(string description)
        {
            modalPackageData.SetDescription(description);
            return this;
        }

        public ModalDataBuilder<TModalPackageData> AddSprite(Sprite sprite)
        {
            modalPackageData.SetSprite(sprite);
            return this;
        }

        public abstract TModalPackageData Build();
    }
}