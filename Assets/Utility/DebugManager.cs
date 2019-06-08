using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * @brief   デバッグ管理クラス.
 */
public class DebugManager : SingletonMonobehaviour<DebugManager>
{
    /**
     * @brief ログ
     */
    private DebugLogger logger = null;
    public DebugLogger Logger
    {
        get
        {
            if (logger == null)
            {
                // クラスnew, スクリプトをアタッチ
                logger = this.gameObject.AddComponent<DebugLogger>();
            }
            return logger;
        }
    }
    
    /**
     * @brief   ログを表示.
     * @param   text   出力文字列.
     * @param   color   出力色.
     */
    public static void Log(string text)
    {
        Instance.Logger.AddText(text);
    }
    public static void Log(string text, Color color)
    {
        Instance.Logger.AddText(text, color);
    }

    /**
     * @brief   GUI関連.
     */
    private void OnGUI()
    {
#if UNITY_EDITOR
        // ログGUI
        Logger.OnGUI_ShowLog();
#endif
    }
}
