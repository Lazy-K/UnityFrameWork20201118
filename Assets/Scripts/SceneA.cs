
using System.Collections.Generic;
using UnityEngine;

public class SceneA : SceneBehaviour
{
    private void Awake()
    {
    }
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SceneScheduler.PopSceneReserve();
            SceneScheduler.PushSceneReserve(new SceneParam{sceneId = SceneId.B, argument = null});
            SceneScheduler.LoadTopSceneReserve();
        }
    }
}
