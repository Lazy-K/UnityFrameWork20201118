
using UnityEngine;

public class SceneB : AppScene
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            AppSceneManager.PopSceneReserve();
            AppSceneManager.PushSceneReserve(new AppSceneManager.SceneReserve{sceneId = SceneId.C, argument = null});
            AppSceneManager.LoadSceneReserve();
        }
    }
}
