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

    
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        _instance = this;
    }

    public static IEnumerator InitializeAsync()
    {
        yield break;
    }

    public static void SetInputEnable(bool isEnable)
    {
        _instance._eventSystem.enabled = isEnable;
    }
}
