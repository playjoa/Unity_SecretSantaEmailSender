using DG.Tweening;
using UnityEngine;

namespace Utils.DOTweens.Components
{
    public class DOTweenPingPongAnim : MonoBehaviour
    {
        [Header("Anim Direction")]
        [SerializeField] private PingPongDirection pingPongDirection = PingPongDirection.UpDown;
        
        [Header("Animation Configuration")]
        [SerializeField] private float distanceToPingPong = 35;
        [SerializeField] private float pingPongDuration = 0.75f;
        [SerializeField] private Ease easeType = Ease.InOutSine;
        
        [Header("Animation Configuration")] 
        [SerializeField] private bool activateOnEnable = true;
        [SerializeField] private float delayRange = 0;

        private bool _alreadyStarted;
        private Vector3 _localStartingPosition;

        private void Awake() => _localStartingPosition = transform.localPosition;

        private void OnEnable()
        {
            if (!activateOnEnable) return;
            
            if (delayRange <= 0)
            {
                StartAnimation();
            }
            else
            {
                Invoke(nameof(StartAnimation), Random.Range(0, delayRange));
            }

            _alreadyStarted = true;
        }

        private void OnDisable() => StopAnimation();

        private void ResetTransform()
        {
            if (_alreadyStarted) return;
            
            switch (pingPongDirection)
            {
                case PingPongDirection.UpDown:
                    transform.localPosition = new Vector3(_localStartingPosition.x,
                        _localStartingPosition.y - distanceToPingPong / 2f, _localStartingPosition.z);
                    break;
                case PingPongDirection.LeftRight:
                    transform.localPosition = new Vector3(_localStartingPosition.x - distanceToPingPong / 2f,
                        _localStartingPosition.y, _localStartingPosition.z);
                    break;
                case PingPongDirection.FrontBack:
                    transform.localPosition = new Vector3(_localStartingPosition.x,
                        _localStartingPosition.y, _localStartingPosition.z - distanceToPingPong / 2f);
                    break;
                default:
                    Debug.LogError("Ping Ping Type Not registered");
                    break;
            }
        }

        public void StartAnimation()
        {
            ResetAnimation();

            switch (pingPongDirection)
            {
                case PingPongDirection.UpDown:
                    transform.DOLocalMoveY(transform.localPosition.y + distanceToPingPong, pingPongDuration)
                        .SetLoops(-1, LoopType.Yoyo).SetEase(easeType);
                    break;
                case PingPongDirection.LeftRight:
                    transform.DOLocalMoveX(transform.localPosition.x + distanceToPingPong, pingPongDuration)
                        .SetLoops(-1, LoopType.Yoyo).SetEase(easeType);
                    break;
                case PingPongDirection.FrontBack:
                    transform.DOLocalMoveZ(transform.localPosition.z + distanceToPingPong, pingPongDuration)
                        .SetLoops(-1, LoopType.Yoyo).SetEase(easeType);
                    break;
                default:
                    Debug.LogError("Ping Ping Type Not registered");
                    break;
            }
        }

        private void ResetAnimation()
        {
            StopAnimation();
            ResetTransform();
        }

        public void StopAnimation() => transform.DOKill();
    }

    public enum PingPongDirection
    {
        UpDown,
        LeftRight,
        FrontBack
    }
}