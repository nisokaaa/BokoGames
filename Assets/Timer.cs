using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Timer : MonoBehaviour
{
    /** 計測終了イベントハンドラ */
    public event EventHandler OnDoneMeasureHandler;

    /** 計測開始時間 */
    float m_StartTime;

    /** 経過時間 */
    float m_Time;

    /** 計測時間 */
    float m_Duration = 0f;

    /** 残りループ回数 */
    int m_LoopCount;

    /** 計測終了後ループするかどうか */
    bool m_IsLoop = false;

    /** 途中停止させられたかどうか */
    bool m_IsStop = false;

	void FixedUpdate ()
    {
        // 一時停止中なら
        if (m_IsStop)
        {
            // 開始時間を調節する
            m_StartTime += Time.deltaTime;
            return;
        }

        // 駆動中なら
        if (IsWork())
        {
            // 経過時間を求める
            m_Time = Time.timeSinceLevelLoad - m_StartTime;

            // 経過時間が計りたい時間を超えたら
            if (m_Duration <= m_Time)
            {
                // ループ設定されていたら
                if (m_IsLoop)
                {
                    // ループ残り回数が１回なら
                    if (m_LoopCount == 1)
                    {
                        // 以下を実行してループを抜ける
                        StartTimer(m_Duration);
                    }
                    else
                    {
                        // 再度セットする
                        StartTimer(m_Duration, true, m_LoopCount);
                    }
                }
                else
                {
                    // 駆動を終了する
                    m_Duration = 0f;

                    // 計測終了イベントを実行する
                    OnDoneMeasure(EventArgs.Empty);
                    
                    // 全パラメータをリセットする
                    Break();
                }
            }
        }
    }

    /// <summary>
    /// 計測が完了したら呼ばれるイベント
    /// </summary>
    /// <param name="e">イベント引数</param>
    protected virtual void OnDoneMeasure(EventArgs e)
    {
        if(OnDoneMeasureHandler != null)
        {
            OnDoneMeasureHandler.Invoke(this, e);
        }
    }

    /// <summary>
    /// 計測を開始する関数
    /// </summary>
    /// <param name="duration">計測したい時間</param>
    /// <param name="loop">ループするかどうか</param>
    /// <param name="count">ループ回数</param>
    /// <returns>設定に成功したどうか</returns>
    public bool StartTimer(float duration, bool loop = false, int count = 0)
    {
        /** 計測時間が０、ループ回数が負、ループ設定で回数が負 */
        if (duration <= 0f || count < 0 || (!loop && count > 0)) return false;

        m_StartTime = Time.timeSinceLevelLoad;
        m_Duration = duration;
        m_LoopCount = count != 0 ? count-1 : 0;
        m_IsLoop = count != 1 ? loop : false;

        return true;
    }

    /// <summary>
    /// 駆動中か調べる関数
    /// </summary>
    /// <returns>駆動中かどうか</returns>
    public bool IsWork()
    {
        return m_Duration > 0 ? true : false;
    }

    /// <summary>
    /// 停止中か調べる関数
    /// </summary>
    /// <returns>停止中かどうか</returns>
    public bool IsStop()
    {
        return m_IsStop;
    }

    /// <summary>
    /// 計測を一時停止させる関数
    /// </summary>
    public void Stop()
    {
        m_IsStop = true;
    }

    /// <summary>
    /// 計測を再開する関数
    /// </summary>
    public void Resume()
    {
        m_IsStop = false;
    }

    /// <summary>
    /// 始めから再度計測を開始する関数
    /// </summary>
    /// <returns></returns>
    public bool ReStart()
    {
        // 停止中に再度再開はできない
        if(IsStop()) return false;
        
        m_StartTime = Time.timeSinceLevelLoad;
        return true;
    }

    /// <summary>
    /// 計測を中断する関数
    /// </summary>
    public void Break()
    {
        m_Time = 0f;
        m_StartTime = 0f;
        m_Duration = 0f;
        m_LoopCount = 0;
        m_IsStop = false;
        m_IsLoop = false;
    }

    /// <summary>
    /// 計測開始からの経過時間を取得する関数
    /// </summary>
    /// <returns>経過時間</returns>
    public float GetTime()
    {
        // 駆動中でないなら０を返す
        return IsWork() ? m_Time : 0f;
    }

    /// <summary>
    /// 計測開始からの経過時間(秒)を取得する関数
    /// </summary>
    /// <returns>経過時間(秒)</returns>
    public int GetTimeSec()
    {
        // 駆動中なら秒、そうでないなら０を返す
        return IsWork() ? Mathf.FloorToInt(m_Time) : 0;
    }
}