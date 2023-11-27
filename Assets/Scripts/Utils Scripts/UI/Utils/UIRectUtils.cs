using UnityEngine;
using UnityEngine.UI;

namespace Utils.UI.Utils
{
    public static class UIRectUtils
    {
        private static readonly Vector3[] SharedCorners = new Vector3[4];
        
        private static Bounds TransformBoundsTo(this RectTransform source, Transform target)
        {
            var bounds = new Bounds();
            if (source == null) return bounds;
            
            source.GetWorldCorners(SharedCorners);

            var vMin = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
            var vMax = new Vector3(float.MinValue, float.MinValue, float.MinValue);

            var matrix = target.worldToLocalMatrix;
            for (var j = 0; j < 4; j++)
            {
                var v = matrix.MultiplyPoint3x4(SharedCorners[j]);
                vMin = Vector3.Min(v, vMin);
                vMax = Vector3.Max(v, vMax);
            }

            bounds = new Bounds(vMin, Vector3.zero);
            bounds.Encapsulate(vMax);

            return bounds;
        }

        private static float NormalizeScrollDistance(this ScrollRect scrollRect, int axis, float distance)
        {
            var viewport = scrollRect.viewport;
            var viewRect = viewport != null ? viewport : scrollRect.GetComponent<RectTransform>();
            var rect = viewRect.rect;
            var viewBounds = new Bounds(rect.center, rect.size);

            var content = scrollRect.content;
            var contentBounds = content != null ? content.TransformBoundsTo(viewRect) : new Bounds();

            var hiddenLength = contentBounds.size[axis] - viewBounds.size[axis];
            return distance / hiddenLength;
        }
        
        public static float VerticalNormalizedPosition(this ScrollRect scrollRect, RectTransform target)
        {
            var view = scrollRect.viewport != null ? scrollRect.viewport : scrollRect.GetComponent<RectTransform>();
            var viewRect = view.rect;
            var elementBounds = target.TransformBoundsTo(view);
            var offset = viewRect.center.y - elementBounds.center.y;
            var scrollPos = scrollRect.verticalNormalizedPosition - scrollRect.NormalizeScrollDistance(1, offset);
            return scrollPos;
        }

        public static float HorizontalNormalizedPosition(this ScrollRect scrollRect, RectTransform target)
        {
            var view = scrollRect.viewport != null ? scrollRect.viewport : scrollRect.GetComponent<RectTransform>();
            var viewRect = view.rect;
            var elementBounds = target.TransformBoundsTo(view);
            var offset = viewRect.center.x - elementBounds.center.x;
            var scrollPos = scrollRect.horizontalNormalizedPosition - scrollRect.NormalizeScrollDistance(0, offset);
            return scrollPos;
        }
    }
}