using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AppCore : MonoBehaviour
{
    private static AppCore _instance;
    private static SceneParam _launchSceneParam = new SceneParam
    {
        sceneId = SceneId.A
    };
    
    [RuntimeInitializeOnLoadMethod]
    private static void Reboot()
    {
        if (null != _instance) return;
        var scene = SceneManager.GetActiveScene();
        if (scene.name == "Boot") return;

        _launchSceneParam = new SceneParam
        {
            sceneId = SceneScheduler.SceneNameToSceneId(scene.name)
        };
        SceneManager.LoadScene("Boot");
    }

    public static IEnumerator InitializeAsync()
    {
        {
            var type = typeof(AppCore);
            Debug.Assert(null == FindObjectOfType(type));
            var go = new GameObject(type.Name);
            DontDestroyOnLoad(go);
            _instance = go.AddComponent<AppCore>();
        }

        { 
            yield return AppEventSystem.InitializeAsync();
            yield return SceneScheduler.InitializeAsync();
        }
        {
            SceneScheduler.PushSceneReserve(_launchSceneParam);
            SceneScheduler.LoadTopSceneReserve();
        }
    }
}
