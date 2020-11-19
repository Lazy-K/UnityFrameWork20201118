using System.Collections;
using UnityEngine;

public class SceneBoot : MonoBehaviour
{
    private IEnumerator Start() { yield return AppCore.InitializeAsync(); }
}
