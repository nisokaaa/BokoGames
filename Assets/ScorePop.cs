using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScorePop : MonoBehaviour
{
    /** アニメーション終了イベントハンドラ */
    public event EventHandler SlideInDoneHandler;
    public event EventHandler SlideOutDoneHandler;
    public event EventHandler SlideDownDoneHandler;

    /** 対象オブジェクト */
    [SerializeField]
    Image m_Target;

    /** 数字テキスト */
    [SerializeField]
    Text m_Text;

    /** アニメーション時間 */
    [SerializeField]
    float m_DurationIn = 10f;
    [SerializeField]
    float m_DurationOut = 10f;
    [SerializeField]
    float m_DurationDown = 5f;

    /** アニメーション開始時間 */
    float m_StartTime;

    /** アニメーション計算時間 */
    float m_Time;

    /** 定位置 */
    Vector3 m_StaticPosition;

    void Start()
    {
        // 初期位置を保存
        m_StaticPosition = m_Target.rectTransform.localPosition;
    }

    /// <summary>
    /// スライドINアニメーションする関数
    /// </summary>
    public IEnumerator SlideIn()
    {
        m_StartTime = Time.timeSinceLevelLoad;
        Vector3 start = m_Target.rectTransform.localPosition;
        Vector3 end = new Vector3(start.x - m_Target.rectTransform.sizeDelta.x, start.y, start.z);

        while (true)
        {
            // 経過時間を計算
            float diff = Time.timeSinceLevelLoad - m_StartTime;

            // 総時間に対する割合を求める
            float rate = diff / (m_DurationIn / 60f);

            // 座標移動
            m_Target.rectTransform.localPosition = Vector3.Lerp(start, end, rate);

            // アニメーションが終了したら
            if (rate >= 1f)
            {
                OnSlideInDone(EventArgs.Empty);

                yield break;
            }

            yield return null;
        }
    }

    /// <summary>
    /// スライドOUTアニメーションする関数
    /// </summary>
    public IEnumerator SlideOut()
    {
        m_StartTime = Time.timeSinceLevelLoad;
        Vector3 start = m_Target.rectTransform.localPosition;
        Vector3 end = new Vector3(start.x + m_Target.rectTransform.sizeDelta.x, start.y, start.z);

        while (true)
        {
            // 経過時間を計算
            float diff = Time.timeSinceLevelLoad - m_StartTime;

            // 総時間に対する割合を求める
            float rate = diff / (m_DurationOut / 60f);

            // 座標移動
            m_Target.rectTransform.localPosition = Vector3.Lerp(start, end, rate);

            // アニメーションが終了したら
            if (rate >= 1f)
            {
                OnSlideOutDone(EventArgs.Empty);

                yield break;
            }

            yield return null;
        }
    }

    /// <summary>
    /// スライドDOWNアニメーションする関数
    /// </summary>
    public IEnumerator SlideDown()
    {
        m_StartTime = Time.timeSinceLevelLoad;
        Vector3 start = m_Target.rectTransform.localPosition;
        Vector3 end = new Vector3(start.x, start.y - m_Target.rectTransform.sizeDelta.y, start.z);

        while (true)
        {
            // 経過時間を計算
            float diff = Time.timeSinceLevelLoad - m_StartTime;

            // 総時間に対する割合を求める
            float rate = diff / (m_DurationDown / 60f);

            // 座標移動
            m_Target.rectTransform.localPosition = Vector3.Lerp(start, end, rate);

            // アニメーションが終了したら
            if (rate >= 1f)
            {
                OnSlideDownDone(EventArgs.Empty);

                yield break;
            }

            yield return null;
        }
    }

    /// <summary>
    /// 座標を生成時の初期位置にセットする関数
    /// </summary>
    public void Reset()
    {
        m_Target.rectTransform.localPosition = m_StaticPosition;
    }

    /// <summary>
    /// テキストに文字列を設定する関数
    /// </summary>
    /// <param name="text"></param>
    public void SetText(string text)
    {
        if(!String.IsNullOrEmpty(text)) m_Text.text = text;
    }

    /// <summary>
    /// アニメーション終了時、各ハンドルに登録したメソッドを処理する
    /// </summary>
    /// <param name="e">イベント引数</param>
    protected virtual void OnSlideInDone(EventArgs e)
    {
        if (SlideInDoneHandler != null)
        {
            SlideInDoneHandler.Invoke(this, e);
        }
    }
    protected virtual void OnSlideOutDone(EventArgs e)
    {
        if (SlideOutDoneHandler != null)
        {
            SlideOutDoneHandler.Invoke(this, e);
        }
    }
    protected virtual void OnSlideDownDone(EventArgs e)
    {
        if (SlideDownDoneHandler != null)
        {
            SlideDownDoneHandler.Invoke(this, e);
        }
    }
}
