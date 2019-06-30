using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplicationInitializer
{
    [RuntimeInitializeOnLoadMethod]
    static void Initialize()
    {
        // ボール情報読み込み
        BallBundleInfo.Load();

        // AudioManager生成
        GameObject prefab = (GameObject)Resources.Load("Prefabs/AudioManager");
        GameObject.Instantiate(prefab, Vector3.zero, Quaternion.identity);
    }
}
