
using UnityEngine;

public class SceneC : AppScene
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            AppSceneManager.PopSceneReserve();
            AppSceneManager.PushSceneReserve(new AppSceneManager.SceneReserve{sceneId = SceneId.A, argument = null});
            AppSceneManager.LoadSceneReserve();
        }
    }
}
