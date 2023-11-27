using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Utils.GameTools
{
    [RequireComponent(typeof(Canvas))]
    public class SafeArea : MonoBehaviour
    {
        [Header("Base Components")]
        [SerializeField] private Canvas canvas; 
        [SerializeField] private RectTransform rectTransform;
        
        [Header("Target Safe Area")]
        [SerializeField] private RectTransform safeAreaRectTransform;
        
        public static event Action OnOrientationChange;
        public static event Action OnResolutionChange; 
        public static bool InLandscape { get; private set; }

        private static readonly List<SafeArea> Helpers = new List<SafeArea>();

        private static bool _screenChangeVarsInitialized = false;
        private static ScreenOrientation _lastOrientation = ScreenOrientation.Portrait;
        private static Vector2 _lastResolution = Vector2.zero;
        private static Rect _lastSafeArea = Rect.zero;

        private const string SAFE_OBJECT_NAME = "SafeArea";

        private void OnValidate()
        {
            if (!canvas) canvas = GetComponent<Canvas>();
            if (!rectTransform) rectTransform = GetComponent<RectTransform>();
            if (!safeAreaRectTransform) safeAreaRectTransform = transform.Find(SAFE_OBJECT_NAME) as RectTransform;
        }

        private void Awake()
        {
            if (!Helpers.Contains(this))
                Helpers.Add(this);
            
            if (safeAreaRectTransform == null)
            {
                Debug.LogError($"Could not find SafeArea Holder in object: {gameObject.name}. Please add a SafeArea named object to hold objects!");
                return;
            }

            if (_screenChangeVarsInitialized) return;
            _lastOrientation = Screen.orientation;
            _lastResolution.x = Screen.width;
            _lastResolution.y = Screen.height;
            _lastSafeArea = Screen.safeArea;

            _screenChangeVarsInitialized = true;
        }

        private void Start()
        {
            ApplySafeArea();
        }

        private void Update()
        {
            if (Helpers[0] != this)
                return;

            if (Application.isMobilePlatform)
            {
                if (Screen.orientation != _lastOrientation)
                    OrientationChanged();

                if (Screen.safeArea != _lastSafeArea)
                    SafeAreaChanged();
            }
            else
            {
                if (Screen.width != _lastResolution.x || Screen.height != _lastResolution.y)
                    ResolutionChanged();
            }
        }

        private void ApplySafeArea()
        {
            if (safeAreaRectTransform == null)
                return;

            var safeArea = Screen.safeArea;

            var anchorMin = safeArea.position;
            var anchorMax = safeArea.position + safeArea.size;
            var pixelRect = canvas.pixelRect;
            anchorMin.x /= pixelRect.width;
            anchorMin.y /= pixelRect.height;
            anchorMax.x /= pixelRect.width;
            anchorMax.y /= pixelRect.height;

            safeAreaRectTransform.anchorMin = anchorMin;
            safeAreaRectTransform.anchorMax = anchorMax;
        }

        private void OnDestroy()
        {
            if (Helpers != null && Helpers.Contains(this))
                Helpers.Remove(this);
        }

        private static void OrientationChanged()
        {
            _lastOrientation = Screen.orientation;
            _lastResolution.x = Screen.width;
            _lastResolution.y = Screen.height;

            InLandscape = _lastOrientation == ScreenOrientation.LandscapeLeft ||
                          _lastOrientation == ScreenOrientation.LandscapeRight;
            
            OnOrientationChange?.Invoke();
        }

        private static void ResolutionChanged()
        {
            if (_lastResolution.x == Screen.width && _lastResolution.y == Screen.height)
                return;

            _lastResolution.x = Screen.width;
            _lastResolution.y = Screen.height;

            InLandscape = Screen.width > Screen.height;
            
            OnResolutionChange?.Invoke();
        }

        private static void SafeAreaChanged()
        {
            if (_lastSafeArea == Screen.safeArea)
                return;

            _lastSafeArea = Screen.safeArea;

            foreach (var t in Helpers)
                t.ApplySafeArea();
        }

        private static Vector2 GetCanvasSize()
        {
            return Helpers[0].rectTransform.sizeDelta;
        }

        public static Vector2 GetSafeAreaSize()
        {
            foreach (var t in Helpers.Where(t => t.safeAreaRectTransform != null))
            {
                return t.safeAreaRectTransform.sizeDelta;
            }

            return GetCanvasSize();
        }
    }
}