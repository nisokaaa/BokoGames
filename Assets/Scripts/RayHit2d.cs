using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayHit2d : MonoBehaviour
{
    class Ray
    {
        public Ray(Vector2 origin, float theta)
        {
            this.origin = origin;

            // オブジェクトを中心とした2DRayの向きを計算
            this.direction = (origin - Vector2.zero).normalized;

            this.theta = theta;
        }

        /** Rayの開始位置 */
        public Vector2 origin;

        /** Rayの向き */
        public Vector2 direction;

        /** シータ */
        public float theta;
    }

    /** Rayリスト */
    List<Ray> m_RayList = new List<Ray>();

    /** Rayの数 */
    [SerializeField]
    int m_Amount = 25;

    /** Rayの長さ */
    [SerializeField]
    float m_Length = 25f;

    /** 中心座標からRay開始座標までの半径R */
    [SerializeField]
    float m_OriginOffsetR = 30f;

    /** 親の2DTransforｍ */
    [SerializeField]
    RectTransform m_ParentRect;

    /** HitしたBallのリスト */
    List<Ball> m_HitBalls = new List<Ball>();

    void Start()
    {
        for (int i = 0; i < m_Amount; i++)
        {
            // 度数をラジアン角に変換
            float theta = (360f / m_Amount * i) * Mathf.PI / 180f;

            // オブジェクトの中心から半径Rだけ離れた2DRayの開始座標を求める
            Vector2 origin = new Vector2(m_OriginOffsetR * Mathf.Cos(theta),
                                      m_OriginOffsetR * Mathf.Sin(theta));

            m_RayList.Add(new Ray(origin, theta));
        }
    }

    void Update()
    {
        foreach(Ray ray in m_RayList)
        {
            // オブジェクトの中心から半径Rだけ離れた2DRayの開始座標を求める
            ray.origin = new Vector2(m_OriginOffsetR * Mathf.Cos(ray.theta),
                                      m_OriginOffsetR * Mathf.Sin(ray.theta));

            Debug.DrawRay(new Vector2(ray.origin.x + m_ParentRect.position.x, ray.origin.y + m_ParentRect.position.y), ray.direction * m_Length, Color.green, 0.01f);
        }
    }

    /// <summary>
    /// RayにHitしたBallのリストを返す
    /// </summary>
    /// <returns>Ballのリスト</returns>
    public List<Ball> GetHitBalls(Ball ignore)
    {
        List<string> m_IgnoreIds = new List<string>();

        m_HitBalls.Clear();

        foreach (Ray ray in m_RayList)
        {
            RaycastHit2D info = Physics2D.Raycast(new Vector2(ray.origin.x + m_ParentRect.position.x,
                                                              ray.origin.y + m_ParentRect.position.y),
                                                  ray.direction, m_Length);

            // RayCastが衝突物を検知し、それが壁ではない場合
            if (info.collider != null && info.collider.tag != "wall")
            {
                Ball ball = info.collider.GetComponent<Ball>();

                if(ball.m_Id == ignore.m_Id)
                {
                    continue;
                }

                // 無視リストが空なら
                if (m_IgnoreIds.Count == 0)
                {
                    // 走査無視リストに追加
                    m_IgnoreIds.Add(ball.m_Id);

                    // 衝突リストに追加
                    m_HitBalls.Add(ball);

                    continue;
                }

                for(int i = 0; i < m_IgnoreIds.Count; i++)
                {
                    // 衝突物が無視リストに含まれていないなら
                    if (!m_IgnoreIds.Contains(ball.m_Id))
                    {
                        // 走査無視リストに追加
                        m_IgnoreIds.Add(ball.m_Id);

                        // 衝突リストに追加
                        m_HitBalls.Add(ball);
                    }
                }    
            }
        }

        //// デバッグ表示
        //foreach(Ball ball in m_HitBalls)
        //{
        //    DebugManager.Log(ball.m_Info.type.ToString());
        //}
        //DebugManager.Log("------------------------------", Color.red);

        return m_HitBalls.Count > 0 ? m_HitBalls : null;
    }
}
