using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AppEventSystem : MonoBehaviour
{
    [SerializeField]
    private EventSystem _eventSystem = null;

    private static AppEventSystem _instance;

    public static IEnumerator InitializeAsync()
    {
        var request = Resources.LoadAsync<AppEventSystem>("AppEventSystem");
        while (!request.isDone) yield return null;
        _instance = Instantiate(request.asset) as AppEventSystem;
        DontDestroyOnLoad(_instance);
    }

    public static void SetInputEnable(bool isEnable)
    {
        _instance._eventSystem.enabled = isEnable;
    }
}
