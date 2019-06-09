using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallSpawner : MonoBehaviour
{
    static BallSpawner m_Instance;
    public static BallSpawner Instance
    {
        get { return m_Instance != null ? m_Instance : null; }
    }

    /** 初期プール数 */
    const int MAX_BALL_COUNT = 75;

    /** 2DTransform */
    [SerializeField]
    RectTransform m_Rect;

    /** 生成するキャンバス */
    [SerializeField]
    Canvas m_Canvas;

    /** ボールPrefab */
    [SerializeField]
    GameObject m_BallPrefab;

    /** ボールリスト */
    List<Ball> m_Balls = new List<Ball>();
    
    /** 生成範囲(半径) */
    [SerializeField]
    int m_CreateRange = 50;

    /** 生成個数 */
    [SerializeField]
    int m_CreateAmount = 10;

    /** 生成間隔 */
    [SerializeField]
    float m_CreateInterval = 0.25f;

    /** ランダムインスタンス */
    System.Random m_Rand = new System.Random();

    void Start()
    {
        // 開始時までにインスペクタに置く前提
        m_Instance = this;

        for (int i = 0; i < MAX_BALL_COUNT; i++)
        {
            Ball ball = Instantiate(m_BallPrefab, gameObject.transform.position, Quaternion.identity).GetComponent<Ball>();

            if(ball != null)
            {
                // 親をCanavasに設定
                ball.gameObject.transform.SetParent(m_Canvas.transform, false);

                // アクティブ化
                ball.gameObject.SetActive(false);

                // リストに追加
                m_Balls.Add(ball);
            }
        }
    }

    void Update()
    {

#if UNITY_EDITOR
        //** 以下デバッグコード
        if (Input.GetKeyDown(KeyCode.E))
        {
            StartCoroutine(CreateBall(m_CreateAmount, m_CreateInterval, m_CreateRange));
        }
#endif

    }

    /// <summary>
    /// ボールを生成する
    /// </summary>
    /// <param name="amount">生成する量</param>
    /// <param name="interval">生成間隔</param>
    /// <param name="range">生成座標範囲(半径)</param>
    IEnumerator CreateBall(int amount, float interval, int range)
    {
        List<BallInfo> ballInfoList = BallBundleInfo.GetInfoList();

        for (int i = 0; i < amount; i++)
        {
            // 非アクティブのボールをひとつ取得
            Ball ball = m_Balls.Find(s => !s.gameObject.activeSelf);

            if (ball != null)
            {
                // アクティブ化
                ball.gameObject.SetActive(true);

                // 生成座標
                int randPosition = m_Rand.Next(-range, range);
                Vector2 position = new Vector2((int)m_Rect.position.x + randPosition,
                                               (int)m_Rect.position.y + randPosition);
                
                // 初期化
                ball.Init(position, ballInfoList[m_Rand.Next(ballInfoList.Count)]);
            }

            yield return new WaitForSeconds(interval);
        }

        yield break;
    }
}