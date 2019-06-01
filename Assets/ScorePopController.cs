using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ScorePopController : MonoBehaviour
{
    /** タスクデータ */
    public class Task
    {
        public Task(int score){
            this.score = score;
        }
        public int score { get; private set; }
    };

    /** タスクキュー */
    Queue<Task> m_TaskQueue = new Queue<Task>();

    /** ポップリスト */
    [SerializeField]
    List<ScorePop> m_PopList = new List<ScorePop>();

    /** ポップ表示時間(待機) */
    [SerializeField]
    float m_WaitDuration = 1.5f;
    
    void Start()
    {
        // すべて非アクティブにする
        m_PopList.ForEach(t => t.gameObject.SetActive(false));
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.E))
        {
        }

        // キューにタスクが存在する
        if(m_TaskQueue.Count > 0)
        {
            ScorePop pop = null;
            if (pop = m_PopList.Find(t => !t.gameObject.activeSelf))
            {
                pop.SetText(m_TaskQueue.Peek().score.ToString());
                StartCoroutine(SlideInOut(pop));
                m_TaskQueue.Dequeue();
            }
        }
    }

    /// <summary>
    /// ポップを出す要求を受理する関数
    /// </summary>
    /// <param name="score">得点</param>
    public void SetPop(int score)
    {
        // 要求をキューへ追加
        m_TaskQueue.Enqueue(new Task(score));
    }

    /// <summary>
    /// スライドIN、OUTするアニメーション処理
    /// </summary>
    /// <param name="pop">対象オブジェクト</param>
    IEnumerator SlideInOut(ScorePop pop)
    {
        pop.gameObject.SetActive(true);

        yield return StartCoroutine(SlideDownOther(pop));

        yield return StartCoroutine(pop.SlideIn());

        yield return new WaitForSeconds(m_WaitDuration);

        yield return StartCoroutine(pop.SlideOut());

        pop.Reset();

        pop.gameObject.SetActive(false);

        yield return null;
    }

    /// <summary>
    /// 既出のポップを下にスライドDOWNさせる処理
    /// </summary>
    /// <param name="me">スライドINさせるポップ</param>
    IEnumerator SlideDownOther(ScorePop me)
    {
        foreach (ScorePop other in m_PopList)
        {
            // アクティブかつ既出のポップをすべて下げる
            if (other.gameObject.activeSelf && me != other)
            {
                yield return StartCoroutine(other.SlideDown());
            }
        }

        yield return null;
    }
}
