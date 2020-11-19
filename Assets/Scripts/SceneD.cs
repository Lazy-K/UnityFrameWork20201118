
using UnityEngine;

public class SceneD : SceneBehaviour
{
    private static int _count;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SceneScheduler.PopSceneReserve();
            if (0 == _count++ % 2)
            {
                SceneScheduler.PushSceneReserve(
                    new SceneParam {sceneId = SceneId.C, argument = null});
            }
            else
            {
                SceneScheduler.PushSceneReserve(
                    new SceneParam {sceneId = SceneId.A, argument = null});
            }
            SceneScheduler.LoadTopSceneReserve();
        }
    }
}
