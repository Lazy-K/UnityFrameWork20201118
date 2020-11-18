
using UnityEngine;

public class SceneA : AppScene
{
    private void Awake()
    {
    }
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            AppSceneManager.PopSceneReserve();
            AppSceneManager.PushSceneReserve(new AppSceneManager.SceneReserve{sceneId = SceneId.B, argument = null});
            AppSceneManager.LoadSceneReserve();
        }
    }
}
