
using UnityEngine;

public class SceneD : AppScene
{
    private static int _count;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            AppSceneManager.PopSceneReserve();
            if (0 == _count++ % 2)
            {
                AppSceneManager.PushSceneReserve(new AppSceneManager.SceneReserve {sceneId = SceneId.C, argument = null});
            }
            else
            {
                AppSceneManager.PushSceneReserve(new AppSceneManager.SceneReserve {sceneId = SceneId.A, argument = null});
            }
            AppSceneManager.LoadSceneReserve();
        }
    }
}
