using TMPro;
using UI.ModalViews.Data;
using UnityEngine;
using UnityEngine.UI;

namespace UI.ModalViews.Abstracts
{
    public abstract class ModalView : MonoBehaviour
    {
        public abstract ModalType ModalType { get; }
        public virtual bool Active => gameObject.activeSelf;

        public abstract void Initiate(ModalPackageData modalPackageData);

        public virtual void ToggleModal(bool value)
        {
            gameObject.SetActive(value);
        }
    }

    public abstract class ModalView<TModalData> : ModalView where TModalData : ModalPackageData
    {
        [Header("Default Modal Texts")] 
        [SerializeField] private TextMeshProUGUI modalWindowTitle;
        [SerializeField] private TextMeshProUGUI modalWindowDescription;

        [Header("Default Modal Images")] 
        [SerializeField] private Image defaultImage;
        
        public TModalData CurrentDisplayingData { get; private set; }

        public override void Initiate(ModalPackageData modalPackageData)
        {
            if (modalPackageData is not TModalData data)
            {
                Debug.LogError($"Loading modal window with wrong data type ({modalPackageData.ModalType}) at {gameObject.name}");
                return;
            }

            CurrentDisplayingData = data;
            SetTitleText(modalPackageData.Title);
            SetDescriptionText(modalPackageData.Description);
            SetDefaultImage(modalPackageData.Sprite);

            OnInitiated(CurrentDisplayingData);
        }

        protected abstract void OnInitiated(TModalData modalPackageData);

        public override void ToggleModal(bool value)
        {
            base.ToggleModal(value);

            if (value)
            {
                CurrentDisplayingData?.InvokeDisplayedModalEvent();
            }
            else
            {
                CurrentDisplayingData?.InvokeModalClosedEvent();
            }
        }

        protected void SetTitleText(string titleText)
        {
            if (modalWindowTitle) modalWindowTitle.text = titleText;
        }

        protected void SetDescriptionText(string descriptionText)
        {
            if (modalWindowDescription) modalWindowDescription.text = descriptionText;
        }

        protected void SetDefaultImage(Sprite splashArtSprite)
        {
            if (!defaultImage) return;
            
            if (splashArtSprite == null)
            {
                defaultImage.gameObject.SetActive(false);
                return;
            }
            
            defaultImage.overrideSprite = splashArtSprite;
            defaultImage.gameObject.SetActive(true);
        }
    }
}