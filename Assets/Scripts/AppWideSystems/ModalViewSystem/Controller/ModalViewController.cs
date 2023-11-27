using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AppWide.AppInitialization.Controller;
using AppWideSystems.AppInitialization.Interfaces;
using UI.ModalViews.Abstracts;
using UI.ModalViews.Data;
using UnityEngine;
using Utils_Scripts.Patterns;

namespace UI.ModalViews.Controller
{
    public class ModalViewController : MonoBehaviourSingleton<ModalViewController>, IAppWideSystem
    {
        [Header("Modal Windows")]
        [SerializeField] private ModalView[] modalViews;
        
        public static ModalViewController ME { get; private set; }
        
        public string AppSystemName => "UI Modals";

        private readonly Queue<ModalPackageData> _modalViewsPackageQueue = new Queue<ModalPackageData>(); 
        private Dictionary<ModalType, ModalView> _availableModalViews;
        private Coroutine _displayModalViewsCoroutine;
        private ModalPackageData _currentModalPackageData;
        private ModalView _currentModalViewInDisplay;
        
        private bool HasModalViewsToShow => _modalViewsPackageQueue.Any();
        
        protected override void OnAwaken()
        {
            foreach (var modalView in modalViews)
            {
                modalView.ToggleModal(false);
            }
        }

        public IEnumerator Initiate(AppInitializationController appInitializationController)
        {
            _availableModalViews = new Dictionary<ModalType, ModalView>();

            foreach (var modalView in modalViews)
            {
                if (_availableModalViews.ContainsKey(modalView.ModalType))
                {
                    Debug.LogWarning($"There are more than one of the ModalView Type {modalView.ModalType}. Could not add {modalView.gameObject.name}");
                    continue;
                }

                _availableModalViews.Add(modalView.ModalType, modalView);
            }

            yield return true;
        }
        
        public void UpdateChildModules()
        {
            if (!Application.isEditor) return;

            modalViews = GetComponentsInChildren<ModalView>(true);
        }

        public void RequestModalView(ModalPackageData modalPackageData)
        {
            _modalViewsPackageQueue.Enqueue(modalPackageData);
            
            if (_displayModalViewsCoroutine != null) return;
            _displayModalViewsCoroutine = StartCoroutine(ShowModalViewsCoroutine());
        }
        
        private IEnumerator ShowModalViewsCoroutine()
        {
            while (HasModalViewsToShow)
            {
                var modalViewPackageToDisplay = _modalViewsPackageQueue.Dequeue();
                
                if (DisplayModalView(modalViewPackageToDisplay))
                    yield return new WaitUntil(() => !_currentModalViewInDisplay.Active);
            }

            _displayModalViewsCoroutine = null;
        }

        private bool DisplayModalView(ModalPackageData modalPackageData)
        {
            if (!TryGetModalViewToDisplay(modalPackageData.ModalType, out var modalView)) return false;
            
            DisablePreviousModal();
            
            modalView.Initiate(modalPackageData);
            modalView.ToggleModal(true);

            _currentModalPackageData = modalPackageData;
            _currentModalViewInDisplay = modalView;
            return true;
        }

        private void DisablePreviousModal()
        {
            if (_currentModalViewInDisplay != null)
            {
                _currentModalViewInDisplay.ToggleModal(false);
            }
        }

        private bool TryGetModalViewToDisplay(ModalType modalType, out ModalView modalView)
        {
            return _availableModalViews.TryGetValue(modalType, out modalView);
        }
    }
}