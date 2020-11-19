using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class SceneBehaviour : MonoBehaviour
{
    [InitializeOnLoadMethod]
    private static void SetValidateMethod()
    {
        EditorSceneManager.sceneSaved += ValidateScene;
    }

    private static void ValidateScene(Scene scene)
    {
        if (scene.name == "Boot" || scene.name == "Loading")
        {
            return;
        }

        {
            var gos = scene.GetRootGameObjects();
            if (1 != gos.Length)
            {
                Debug.LogError("シーンのルート位置に存在するオブジェクトは、EntryPointのみとしてください。");
            }
            if (1 == gos.Length)
            {
                if ("EntryPoint" != gos[0].name)
                {
                    Debug.LogError("シーンのルート位置に存在するオブジェクトは、EntryPointのみとしてください。");
                }
                if (null == gos[0].GetComponent<SceneBehaviour>())
                {
                    Debug.LogError("シーンのルート位置に存在するオブジェクトには、AppScene継承コンポーネントを付けてください。");
                }
            }

            {
                if (0 != GetObjectCountOfType<EventSystem>(scene))
                {
                    Debug.LogError("シーンにはEventSystemを配置しないでください。");
                }
                if (0 != GetObjectCountOfType<StandaloneInputModule>(scene))
                {
                    Debug.LogError("シーンにはStandaloneInputModuleを配置しないでください。");
                }
            }
        }
    }

    private static int GetObjectCountOfType<T>(Scene scene)
    {
        var count = 0;
        foreach (var go in scene.GetRootGameObjects())
        {
            count += GetObjectCountOfType<T>(go.transform, count);
        }
        return count;
    }    
    
    private static int GetObjectCountOfType<T>(Transform tf, int count = 0)
    {
        if (null != tf.GetComponent<T>()) ++count;
        for (var i = 0; i < tf.childCount; ++i)
        {
            count = GetObjectCountOfType<T>(tf.GetChild(i), count);
        }
        return count;
    }

#if true

    public virtual IEnumerator OnSceneLoadedAsync() { yield break; }
    public virtual IEnumerator OnScenePreUnloadAsync() { yield break; }

    public virtual IEnumerator OnScenePreActivateAsync() { yield break; }
    public virtual IEnumerator OnSceneActivatedAsync() { yield break; }
    
    public virtual IEnumerator OnScenePreDeactivateAsync() { yield break; }
    public virtual IEnumerator OnSceneDeactivatedAsync() { yield break; }

    public virtual IEnumerator OnEnterAsync() { yield return null; }
    public virtual IEnumerator OnLeaveAsync() { yield return null; }

    //public virtual void OnSceneVisible() {}
    //public virtual void OnSceneInvisible() {}
#else
    /// <summary>
    /// シーンに遷移する際の通信処理(プリロード)
    /// </summary>
    public virtual IEnumerator LoadDM() { yield break; } // よく使うメソッド
    
    /// <summary>
    /// Inアニメーションの前に呼ばれます(リソースロードなどを行う)
    /// </summary>
    public virtual IEnumerator OnEnter() { yield break; } // よく使うメソッド
    
    /// <summary>
    /// 初期化処理(イベントハンドラの登録など)
    /// </summary>
    public virtual void Setup() {} // よく使うメソッド
    
    /// <summary>
    /// Viewへ値を適用します
    /// </summary>
    public virtual void Apply() {} // よく使うメソッド
    
    /// <summary>
    /// アプリがサスペンドされたときの処理
    /// </summary>
    public virtual void OnSuspend() {}
    
    /// <summary>
    /// アプリがレジュームされた時、シーン側の具体的な処理
    /// </summary>
    public virtual void OnResume() {}
    
    /// <summary>
    /// Inアニメーションが完了した時の処理
    /// </summary>
    public virtual void OnCompleteInAnimation() {}
    
    /// <summary>
    /// Outアニメーションが開始する時の処理
    /// </summary>
    public virtual void OnBeginOutAnimation() {}

    /// <summary>
    /// オーバーレイ状態には言う際に呼ばれる処理
    /// </summary>
    public virtual IEnumerator OnSuspendOverlayLeave() { yield break; }

    /// <summary>
    /// オーバーレイ状態からレジュームした際に呼ばれる処理
    /// </summary>
    public virtual IEnumerator OnResumeOverlayEnter() { yield break; }

    /// <summary>
    /// シーンを離れると差異の通信処理(保存処理など)
    /// </summary>
    public virtual IEnumerator SaveDM() { yield break; } // よく使うメソッド

    /// <summary>
    /// Outアニメーションが終わった後に呼ばれます
    /// </summary>
    /// <returns></returns>
    public virtual IEnumerator OnLeave() { yield break; } // よく使うメソッド
    
    public virtual void OnHided() {}
#endif
}
