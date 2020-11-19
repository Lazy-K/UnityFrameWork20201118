using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum LoadingType
{
    Loading1,
    Loading2,
    Loading3,
    Default = Loading1
}

public class SceneLoading : SceneBehaviour
{
    [SerializeField]
    private Text _text = null;
    
    private void Awake()
    {
        gameObject.SetActive(false);
    }

    public override IEnumerator OnSceneLoadedAsync()
    {
        yield break;
    }

    private LoadingType GetLoadingType(SceneId previousSceneId, SceneId currentSceneId)
    {
        return LoadingType.Default;
    }

    public override IEnumerator OnSceneActivatedAsync()
    {
        var loadingType = GetLoadingType(SceneScheduler.GetPreviousSceneParam()?.sceneId ?? SceneId.None,
            SceneScheduler.GetCurrentSceneParam().sceneId);
        _text.text = loadingType.ToString();

        yield return new WaitForSeconds(1);
    }
}
