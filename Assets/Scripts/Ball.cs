using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Ball : MonoBehaviour
{
    /** 一意なID */
    public string m_Id;

    /** 情報 */
    public BallInfo m_Info;

    /** 2DTransform */
    [SerializeField]
    RectTransform m_Rect;

    /** 2DRayCast */
    [SerializeField]
    RayHit2d m_RayCast;

    /** ボールを消せる条件(連結数) */
    public const int LINK_BURST_BALL_COUNT = 3;

    /** 連結時の拡大倍率 */
    public const float LINK_BALL_MAGNIFY = 1.25f;

    /** 連結時の連結数ボーナス拡大倍率 */
    public const float LINK_BALL_MAGNIFY_BONUS = 0.15f;

    public void OnTouch()
    {
        // 結果リスト
        List<Ball> results = new List<Ball>();
        results.Add(this);

        // 走査無視リスト
        List<string> ignores = new List<string>();
        ignores.Add(m_Id);

        // 走査開始
        SearchContact(this, ref ignores, ref results);

        // 条件を満たしていたら
        if (results.Count >= LINK_BURST_BALL_COUNT)
        {
            // タップしたボール以外を消す
            for (int i = 1; i < results.Count; i++)
            {
                results[i].gameObject.SetActive(false);
            }

            // タップした球を大きくする
            results[0].m_Rect.localScale *= LINK_BALL_MAGNIFY * (LINK_BALL_MAGNIFY_BONUS * results.Count) + 1.0f;
        }
    }

    /// <summary>
    /// 同じ色かつ無視リストに載っていないオブジェクトを走査する関数
    /// </summary>
    /// <param name="obj">衝突判定が行われるオブジェクト</param>
    /// <param name="ignores">走査無視リストの参照</param>
    /// <param name="results">結果リストの参照</param>
    void SearchContact(Ball obj, ref List<string> ignores, ref List<Ball> results)
    {
        // 衝突判定があったオブジェクトを取得
        List<Ball> balls = obj.m_RayCast.GetHitBalls(obj);

        if(balls != null)
        {
            foreach (Ball ball in balls)
            {
                // 走査無視リストに載っていたら無視して次へ
                if (ignores.Contains(ball.m_Id))
                {
                    continue;
                }

                // 違う色なら無視して次へ
                if (ball.m_Info.type != m_Info.type)
                {
                    continue;
                }

                // 目的の球を結果リスト・走査無視リストに追加
                results.Add(ball);
                ignores.Add(ball.m_Id);

                // 再帰的に走査
                SearchContact(ball, ref ignores, ref results);
            }
        }
    }

    /// <summary>
    /// 初期化関数
    /// </summary>
    /// <param name="position">初期位置</param>
    /// <param name="info">ボール情報</param>
    public void Init(Vector2 position, BallInfo info)
    {
        // 一意なID設定
        m_Id = Guid.NewGuid().ToString("N").Substring(0, 5);

        // ボール情報をロード
        m_Info = info;

        // Spriteを設定
        GetComponent<Image>().sprite = m_Info.sprite;

        // 2D座標を設定
        m_Rect.position = position;
    }
    
}
