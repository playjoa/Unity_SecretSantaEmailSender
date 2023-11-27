using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Utils.UI.Utils;

namespace Utils.UI
{
    public class ScrollRectFocusAnimation : MonoBehaviour
    {
        [Header("Target Content Rect")]
        [SerializeField] private ScrollRect targetScrollRect;

        [Header("Focus Configuration")] 
        [SerializeField] private float focusDelay = 0.05f;
        [SerializeField] private float focusDuration = 0.5f;
        [SerializeField] private Ease focusEase = Ease.InExpo;
        
        private bool CanFocus => targetScrollRect != null;

        private void Awake()
        {
            targetScrollRect.onValueChanged.AddListener(OnScrollRectValueChangedHandler);
        }

        private void OnDestroy()
        {
            targetScrollRect.onValueChanged.RemoveListener(OnScrollRectValueChangedHandler);
        }

        public void FocusOnContent(RectTransform focusContent)
        {
            if (!CanFocus) return;
            if (focusContent == null) return;

            if (targetScrollRect.vertical)
            {
                VerticalFocusAnimation(focusContent);
                return;
            }
            else
            {
                HorizontalFocusAnimation(focusContent);
                return;
            }
        }

        private void VerticalFocusAnimation(RectTransform focusContent)
        {
            var verticalPosition = Mathf.Clamp(targetScrollRect.VerticalNormalizedPosition(focusContent), 0f, 1f);
           
            targetScrollRect.DOKill();
            targetScrollRect.DOVerticalNormalizedPos(verticalPosition, focusDuration)
                .SetDelay(focusDelay)
                .SetEase(focusEase);
        }

        private void HorizontalFocusAnimation(RectTransform focusContent)
        {
            var horizontalPosition = Mathf.Clamp(targetScrollRect.HorizontalNormalizedPosition(focusContent), 0f, 1f);
            
            targetScrollRect.DOKill();
            targetScrollRect.DOHorizontalNormalizedPos(horizontalPosition, focusDuration)
                .SetDelay(focusDelay)
                .SetEase(focusEase);
        }

        private void OnScrollRectValueChangedHandler(Vector2 newValue)
        {
            if (!CanFocus) return;
            
            targetScrollRect.DOKill();
        }
    }
}