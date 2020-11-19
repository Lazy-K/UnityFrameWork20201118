
using UnityEngine;

public class SceneB : SceneBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SceneScheduler.PopSceneReserve();
            SceneScheduler.PushSceneReserve(new SceneParam{sceneId = SceneId.C, argument = null});
            SceneScheduler.LoadTopSceneReserve();
        }
    }
}
