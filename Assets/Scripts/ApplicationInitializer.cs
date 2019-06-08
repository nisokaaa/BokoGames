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
    }
}
