
using UnityEngine;

public class SceneC : AppScene
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            AppSceneManager.PopSceneReserve();
            AppSceneManager.PushSceneReserve(new AppSceneManager.SceneReserve{sceneId = SceneId.D, argument = null});
            AppSceneManager.LoadSceneReserve();
        }
    }
}
