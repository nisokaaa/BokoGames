using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * @brief   デバッグログ管理クラス.
 */
public class DebugLogger : MonoBehaviour
{
    /**
     * @brief   ログデータ.
     */
    private class LogData
    {
        /**
         * @brief テキスト.
         */
        public string text { get; set; }
        
        /**
         * @brief テキストカラー.
         */
        public Color color { get; set; }

        /**
         * @brief   コンストラクタ.
         * @param   Text   テキストデータ
         */
        public LogData(string Text)
        {
            text = Text;
            color = Color.white;
        }
    }

    /**
     * @brief 出力文字列.
     */
    private List<LogData> logList = new List<LogData>();

    /**
     * @brief フォントサイズ.
     */
    public int fontSize = 15;

    /**
     * @brief デバッグGUI幅.
     */
    public int width = 350;

    /**
     * @brief デバッグGUI高さ.
     */
    public int height = 150;

    /**
     * @brief デバッグGUIとフレームの隙間.
     */
    private int margin = 15;

    /**
     * @brief スクロール値.
     */
    Vector2 scrollPosition = Vector2.zero;

    /**
     * @brief   出力文字列に格納.
     * @param   text   出力文字列.
     * @param   color   出力色.
     */
    public void AddText(string text)
    {
        // 末尾に追加
        logList.Insert(logList.Count, new LogData(text + "\r\n"));

        // スクロール位置を最下部に移動
        scrollPosition.y = Mathf.Infinity;
    }
    public void AddText(string text, Color color)
    {
        // 末尾に追加
        this.AddText(text);

        // 色を設定
        logList[logList.Count-1].color = color;
    }

    /**
     * @brief   GUI表示.
     */
    public void OnGUI_ShowLog()
    {
        // スタイル設定
        GUIStyle style = new GUIStyle();
        style.margin = new RectOffset(15, 0, -fontSize, -fontSize);
        style.fontSize = fontSize;
        style.normal = new GUIStyleState() { textColor = Color.gray };

        // デフォルトで右下に表示
        Rect rect = new Rect((Screen.width - width) - margin,
                             (Screen.height - height) - margin,
                              width, height);

        GUILayout.BeginArea(rect, GUI.skin.box);

        scrollPosition = GUILayout.BeginScrollView(scrollPosition);
        
        // テキストを表示
        for(int i = 0; i < logList.Count; i++)
        {
            style.normal.textColor = logList[i].color;
            GUILayout.Label(logList[i].text, style);
        }

        GUILayout.EndScrollView();

        GUILayout.EndArea();
    }
}
