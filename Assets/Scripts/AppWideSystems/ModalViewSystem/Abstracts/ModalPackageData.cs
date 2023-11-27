using System;
using UI.ModalViews.Data;
using UnityEngine;

namespace UI.ModalViews.Abstracts
{
    public abstract class ModalPackageData
    {
        public string Title { get; private set; }
        public string Description { get; private set; }
        public Sprite Sprite { get; private set; }

        public event Action OnModalDisplayed;
        public event Action OnModalClosed;

        public abstract ModalType ModalType { get; }

        protected ModalPackageData()
        {
        }

        public void InvokeDisplayedModalEvent()
        {
            OnModalDisplayed?.Invoke();
        }

        public void InvokeModalClosedEvent()
        {
            OnModalClosed?.Invoke();
        }
        
        public void SetTitle(string title)
        {
            Title = title;
        }
        
        public void SetDescription(string description)
        {
            Description = description;
        }
        
        public void SetSprite(Sprite sprite)
        {
            Sprite = sprite;
        }
    }
}