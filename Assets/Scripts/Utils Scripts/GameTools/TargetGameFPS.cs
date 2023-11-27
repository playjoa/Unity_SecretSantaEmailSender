using UnityEngine;

namespace Utils.GameTools
{
    public class TargetGameFPS : MonoBehaviour
    {
        private void Start()
        {
            var nativeRefreshRate = Screen.currentResolution.refreshRateRatio;
            
            Debug.Log($"Native Refresh Rate: {nativeRefreshRate.value}");
            Debug.Log($"Native Resolution Width: {Screen.currentResolution.width} Height: {Screen.currentResolution.height}");
            
            Application.targetFrameRate = 60;
            
            Debug.Log($"Current Target: {Application.targetFrameRate}");
        }
    }
}