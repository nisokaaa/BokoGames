using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(
    fileName = "BallInfo",
    menuName = "ScriptableObject/BallInfo",
    order = 0)
]
public class BallBundleInfo : ScriptableObject
{
    /** ロード先 */
    public const string PATH = "ScriptableObject/BallInfo";

    static BallBundleInfo m_Instance;
    public static BallBundleInfo Instance
    {
        get { return m_Instance != null ? m_Instance : null; }
    }

    /// <summary>
    /// リソースをロードする関数
    /// アプリ起動時に一度呼ばれる
    /// </summary>
    /// <returns>ロード結果(成功/失敗)</returns>
    public static bool Load()
    {
        m_Instance = Resources.Load<BallBundleInfo>(PATH);
        return m_Instance != null ? true : false;
    }
    
    public static BallInfo GetInfo(int idx)
    {
        return Instance.m_Info[idx];
    }

    public static List<BallInfo> GetInfoList()
    {
        return Instance.m_Info;
    }

    /** 情報リスト */
    public List<BallInfo> m_Info;
}

/** 情報 */
[System.Serializable]
public class BallInfo
{
    public int id;
    public BALL_TYPE type;
    public Sprite sprite;
}

/** 種別 */
public enum BALL_TYPE
{
    ORANGE = 0,
    GREEN,
    BLUE,
    RED,
}