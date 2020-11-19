using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum SceneId
{
    None,
    A,
    B,
    C,
    D,
    Length
}

public class SceneParam
{
    public SceneId sceneId;
    public Dictionary<string, string> argument;
}

public class SceneScheduler : MonoBehaviour
{
    private struct SceneGroup
    {
        public SceneId[] sceneIds;
    }
    private static readonly SceneGroup[] _sceneGroups =
    {
        new SceneGroup {sceneIds = new[] {SceneId.A}},
        new SceneGroup {sceneIds = new[] {SceneId.B, SceneId.C}},
    };
    private static int FindSceneGroupIndex(SceneId sceneId)
    {
        for (var i = 0; i < _sceneGroups.Length; ++i)
        {
            if (Array.Exists(_sceneGroups[i].sceneIds, element => element == sceneId))
            {
                return i;
            }
        }
        return -1;
    }
    
    private static int _loadedSceneGroupIndex = -1;
    
    private static SceneScheduler _instance;
    private static bool _isLateInitialized;
    private static readonly List<SceneId> _loadedSceneIds = new List<SceneId>();

    private static readonly Stack<SceneParam> _stackSceneReserve = new Stack<SceneParam>();
    private static SceneParam _currentSceneParam;
    private static SceneParam _previousSceneParam;

    private static SceneBehaviour _loadingSceneBehaviour;
    
    public static IEnumerator InitializeAsync()
    {
        {
            var type = typeof(SceneScheduler);
            Debug.Assert(null == FindObjectOfType(type));
            var go = new GameObject(type.Name);
            DontDestroyOnLoad(go);
            _instance = go.AddComponent<SceneScheduler>();
        }
        {
            yield return SceneManager.LoadSceneAsync("Loading", LoadSceneMode.Additive);
            var scene = SceneManager.GetSceneByName("Loading");
            _loadingSceneBehaviour = GetSceneBehaviour(scene);
            yield return _loadingSceneBehaviour.OnSceneLoadedAsync();
            _loadingSceneBehaviour.gameObject.SetActive(false);
        }
    }

    public static SceneId SceneNameToSceneId(string sceneName)
    {
        for (var i = 0; i < (int)SceneId.Length; ++i)
        {
            var sceneId = (SceneId)i;
            if (sceneId.ToString() == sceneName) return sceneId;
        }
        return SceneId.None;
    }

    private static SceneBehaviour GetSceneBehaviour(Scene scene) { return scene.GetRootGameObjects()[0].GetComponent<SceneBehaviour>(); }
    
    public static SceneParam GetCurrentSceneParam() { return _currentSceneParam; }
    public static SceneParam GetPreviousSceneParam() { return _previousSceneParam; }


    public static void PushSceneReserve(SceneParam param) { _stackSceneReserve.Push(param); }
    public static SceneParam PopSceneReserve() { return _stackSceneReserve.Pop(); }
    public static void LoadTopSceneReserve() { _instance.StartCoroutine(LoadTopSceneReserveAsync()); }

    private static IEnumerator LoadTopSceneReserveAsync()
    {
        _previousSceneParam = _currentSceneParam;
        _currentSceneParam = _stackSceneReserve.Last();

        if (null == _previousSceneParam)
        {
            var scene = SceneManager.GetSceneByName("Boot");
            yield return SceneManager.UnloadSceneAsync(scene, UnloadSceneOptions.None);
        }
        else
        {
            yield return LeaveSceneAsync(_previousSceneParam.sceneId);
        }
        var sceneGroupIndex = FindSceneGroupIndex(_currentSceneParam.sceneId);
        if (-1 != sceneGroupIndex)
        {
            if (_loadedSceneGroupIndex != sceneGroupIndex)
            {
                for (var i = 0; i < _loadedSceneIds.Count; ++i)
                {
                    var sceneName = _loadedSceneIds[i].ToString();
                    Debug.LogFormat("Unload Scene: {0}", sceneName);
                    var scene = SceneManager.GetSceneByName(sceneName);
                    var sceneBehaviour = GetSceneBehaviour(scene);
                    yield return sceneBehaviour.OnScenePreUnloadAsync();
                    yield return SceneManager.UnloadSceneAsync(scene, UnloadSceneOptions.None);
                }
                _loadedSceneIds.Clear();
                
                for (var i = 0; i < _sceneGroups[sceneGroupIndex].sceneIds.Length; ++i)
                {
                    var sceneId = _sceneGroups[sceneGroupIndex].sceneIds[i];
                    yield return LoadSceneAsync(sceneId);
                }
                _loadedSceneGroupIndex = sceneGroupIndex;
            }
        }
        else
        {
            if (!_loadedSceneIds.Exists(element => element == _currentSceneParam.sceneId))
            {
                yield return LoadSceneAsync(_currentSceneParam.sceneId);
            }
        }
        yield return EnterSceneAsync(_currentSceneParam.sceneId);
    }

    private static IEnumerator LoadSceneAsync(SceneId sceneId)
    {
        var sceneName = sceneId.ToString();
        Debug.LogFormat("Load Scene: {0}", sceneName);
        var asyncOp = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        while (!asyncOp.isDone) yield return null;
        _loadedSceneIds.Add(sceneId);
            
        var scene = SceneManager.GetSceneByName(sceneName);
        var sceneBehaviour = GetSceneBehaviour(scene);
        yield return sceneBehaviour.OnSceneLoadedAsync();
        sceneBehaviour.gameObject.SetActive(false);
    }

    private static IEnumerator LeaveSceneAsync(SceneId sceneId)
    {
        var sceneName = sceneId.ToString();
        Debug.LogFormat("LeaveSceneAsync: {0}", sceneName);
        var scene = SceneManager.GetSceneByName(sceneName);
        var sceneBehaviour = GetSceneBehaviour(scene);
        {
            yield return _loadingSceneBehaviour.OnScenePreActivateAsync();
            _loadingSceneBehaviour.gameObject.SetActive(true);
            yield return _loadingSceneBehaviour.OnSceneActivatedAsync();

            var coroutine1 = sceneBehaviour.OnLeaveAsync();
            var coroutine2 = _loadingSceneBehaviour.OnEnterAsync();
            yield return coroutine1;
            yield return coroutine2;
        }
        yield return sceneBehaviour.OnScenePreDeactivateAsync();
        sceneBehaviour.gameObject.SetActive(false);
        yield return sceneBehaviour.OnSceneDeactivatedAsync();
    }

    private static IEnumerator EnterSceneAsync(SceneId sceneId)
    {
        var sceneName = sceneId.ToString();
        var scene = SceneManager.GetSceneByName(sceneName);
        SceneManager.SetActiveScene(scene);
        var sceneBehaviour = GetSceneBehaviour(scene);
        yield return sceneBehaviour.OnScenePreActivateAsync();
        yield return sceneBehaviour.OnSceneActivatedAsync();
        {
            var coroutine1 = _loadingSceneBehaviour.OnLeaveAsync();
            var coroutine2 = sceneBehaviour.OnEnterAsync();
            yield return coroutine1;
            yield return coroutine2;
            
            sceneBehaviour.gameObject.SetActive(true);
            
            yield return _loadingSceneBehaviour.OnScenePreDeactivateAsync();
            _loadingSceneBehaviour.gameObject.SetActive(false);
            yield return _loadingSceneBehaviour.OnSceneDeactivatedAsync();
        }
    }
}
