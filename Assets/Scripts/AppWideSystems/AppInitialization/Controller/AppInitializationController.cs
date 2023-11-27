using System;
using System.Collections;
using System.Collections.Generic;
using AppWideSystems.AppInitialization.Interfaces;
using AppWideSystems.AppInitializationSceneLoader.Controller;
using AppWideSystems.EmailSystem.Controller;
using AppWideSystems.SecretSantaSystem.Controller;
using UI.ModalViews.Controller;
using UnityEngine;
using Utils.SceneLoader.Controller;
using Random = UnityEngine.Random;

namespace AppWide.AppInitialization.Controller
{
    public class AppInitializationController : MonoBehaviour
    {
        [Header("UI Systems")] 
        [SerializeField] private ModalViewController modalViewController;
        [SerializeField] private SceneLoaderController sceneLoaderController;
        
        [Header("Core Systems")]
        [SerializeField] private EmailSystemController emailSystemController;
        [SerializeField] private SecretSantaController secretSantaController;
        [SerializeField] private AppSceneInitiatorController sceneInitiatorController;
        
        public static AppInitializationController ME { get; private set; }
        
        public static event Action<AppInitializationController> OnAppInitiated;
        public static event Action<IAppWideSystem> OnAppSystemStartedLoading;
        public static event Action<IAppWideSystem, string> OnAppInitializationFailed;

        private readonly List<IAppWideSystem> _appWideSystems = new();
        private readonly List<IAppWideLogInSystem> _appWideLogInSystems = new();
        private IAppWideSystem _currentSystemToInitiate;

        private void Awake()
        {
            if (ME != null)
            {
                Destroy(gameObject);
                return;
            }

            ME = this;
            DontDestroyOnLoad(this);
        }
        
        private void Start()
        {            
            SetUpSystemsInitializationOrder();
            StartCoroutine(InitializationCoroutine(_appWideSystems));
        }

        public void InvokeAppInitializationFail(IAppWideSystem appWideSystem, string errorMessage)
        {
            OnAppInitializationFailed?.Invoke(appWideSystem, errorMessage);
        }
        
        public void InitializeLogInSystems()
        {
            StartCoroutine(InitializationCoroutine(_appWideLogInSystems));
        }
        
        public static void QuitGameApp()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }

        private IEnumerator InitializationCoroutine(List<IAppWideSystem> appWideSystems)
        {
            yield return new WaitForEndOfFrame();

            foreach (var gameSystem in appWideSystems)
            {
                OnAppSystemStartedLoading?.Invoke(gameSystem);
                yield return gameSystem.Initiate(this);
            }

            OnAppInitiated?.Invoke(this);
        }
        
        private IEnumerator InitializationCoroutine(List<IAppWideLogInSystem> appWideSystems)
        {
            yield return new WaitForEndOfFrame();

            foreach (var gameSystem in appWideSystems)
            {
                OnAppSystemStartedLoading?.Invoke(gameSystem);
                yield return gameSystem.Initiate(this);
            }

            OnAppInitiated?.Invoke(this);
        }

        private WaitForSeconds RandomWaitTime()
        {
            var randomTime = Random.Range(0.1f, 0.35f);
            return new WaitForSeconds(randomTime);
        }

        private void SetUpSystemsInitializationOrder()
        {
            // UI Systems
            QueueSystemInitialization(modalViewController);
            QueueSystemInitialization(sceneLoaderController);
            
            // Core Systems
            QueueSystemInitialization(emailSystemController);
            QueueSystemInitialization(secretSantaController);
            QueueSystemInitialization(sceneInitiatorController);
        }

        private void QueueSystemInitialization(IAppWideSystem systemToInitialize)
        {
            _appWideSystems.Add(systemToInitialize);

            if (systemToInitialize is IAppWideLogInSystem logOutSystem)
            {
                _appWideLogInSystems.Add(logOutSystem);
                Debug.Log($"Registered LogOut System: {logOutSystem.AppSystemName}");
            }
        }
    }
}