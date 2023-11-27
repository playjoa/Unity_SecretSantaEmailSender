using System;
using System.Collections;
using System.Collections.Generic;
using AppWide.AppInitialization.Controller;
using AppWideSystems.AppInitialization.Interfaces;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utils.Extensions;
using Utils.SceneLoader.Data;

namespace Utils.SceneLoader.Controller
{
    public class SceneLoaderController : MonoBehaviour, IAppWideSystem
    {
        public static SceneLoaderController ME { get; private set; }

        public delegate void SceneLoaderHandler(LoadingSceneData sceneData);
        public event SceneLoaderHandler OnStartedToLoadScene;
        public event SceneLoaderHandler OnSceneLoaded;
        
        public static GameScene CurrentGameScene { get; private set; }
        
        public string AppSystemName => "Scene Loader UI";
        public string ProgressPercentage { get; private set; }
        public float Progress { get; private set; }
        public bool LoadingAScene { get; private set; }
        
        private readonly List<string> _scenesAvailable = new List<string>();
        private readonly WaitForSeconds _startUpDelay = new WaitForSeconds(0.5f);

        private void Awake()
        {
            if (ME == null)
            {
                ME = this;
                DontDestroyOnLoad(this);
                return;
            }
            DestroyImmediate(this);
        }
        
        public IEnumerator Initiate(AppInitializationController appInitializationController)
        {
            GetAvailableScenes();
            yield return true;
        }

        private void GetAvailableScenes()
        {
            for (var i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
            {
                var scenePath = SceneUtility.GetScenePathByBuildIndex(i);
                var lastSlash = scenePath.LastIndexOf("/", StringComparison.Ordinal);
                _scenesAvailable.Add(scenePath.Substring(lastSlash + 1, scenePath.LastIndexOf(".", StringComparison.Ordinal) - lastSlash - 1));
            }
        }

        private static float ProgressClamped(float progress) => Mathf.Clamp01(progress / .9f);

        private void ResetValues()
        {
            ProgressPercentage = "0%";
            Progress = 0f;
        }

        public void LoadScene(GameScene targetGameScene, LoadingScreenType loadingScreenType = LoadingScreenType.Standard)
        {
            if (LoadingAScene) return;

            var targetSceneToLoad = targetGameScene.Equals(GameScene.ReloadCurrent) ? CurrentGameScene : targetGameScene;
            var sceneId = LoadingScenes.GetSceneId(targetSceneToLoad);

            if (sceneId == string.Empty || !_scenesAvailable.Contains(sceneId))
            {
                Debug.LogError($"Scene Id: {sceneId} and/or GameScene {targetSceneToLoad} is not valid!");
                return;
            }

            ResetValues();
            StartCoroutine(LoadSceneASync(sceneId, new LoadingSceneData(targetSceneToLoad, loadingScreenType)));
        }

        private IEnumerator LoadSceneASync(string targetScene, LoadingSceneData sceneData)
        {
            OnStartedToLoadScene?.Invoke(sceneData);
            LoadingAScene = true;

            yield return _startUpDelay;

            var loadingSceneOperation = SceneManager.LoadSceneAsync(targetScene);

            while (!loadingSceneOperation.isDone)
            {
                var progress = ProgressClamped(loadingSceneOperation.progress);

                ProgressPercentage = progress.FloatToPercentage();
                Progress = progress;
                yield return null;
            }

            Debug.Log($"Loaded Scene: {sceneData.GameScene} Previous Scene Was: {CurrentGameScene}");
            LoadingAScene = false;
            CurrentGameScene = sceneData.GameScene;

            OnSceneLoaded?.Invoke(sceneData);
        }
    }
}