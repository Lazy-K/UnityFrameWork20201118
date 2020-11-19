
using UnityEngine;

public class SceneC : SceneBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SceneScheduler.PopSceneReserve();
            SceneScheduler.PushSceneReserve(new SceneParam{sceneId = SceneId.D, argument = null});
            SceneScheduler.LoadTopSceneReserve();
        }
    }
}
