using DG.Tweening;
using UnityEngine;

namespace Utils.Animations
{
    public class PulseAnimation : MonoBehaviour
    {
        [Header("Target")]
        [SerializeField] private Transform targetTransform;
        
        [Header("Animation Configuration")]
        [SerializeField] private float baseSize = 1;
        [SerializeField] private float velocity = 5.5f;

#if UNITY_EDITOR
        private void OnValidate() => Awake();
#endif
        
        private void Awake() => targetTransform ??= GetComponent<Transform>();

        private void OnDisable()
        {
            targetTransform.localScale = Vector3.one * baseSize;
        }

        private void Update()
        {
            Animate();
        }

        private void Animate()
        {
            if (DOTween.IsTweening(gameObject.transform)) return;

            var anim = baseSize + Mathf.Sin(Time.time * velocity) * baseSize / 30f;
            transform.localScale = Vector3.one * anim;
        }
    }
}
