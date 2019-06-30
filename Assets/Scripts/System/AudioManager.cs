using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 音声管理
/// </summary>
public class AudioManager : MonoBehaviour
{
    static AudioManager m_Instance = null;
    public static AudioManager Instance
    {
        get
        {
            return m_Instance != null ? m_Instance : null;
        }
    }

    /** BGM,SE読み込みディレクトリPATH */
    public const string BGM_LOAD_PATH = "Sounds/BGM";
    public const string SE_LOAD_PATH = "Sounds/SE";

    /** BGMAudioSource */
    [SerializeField]
    AudioSource m_BgmAudioSource;

    /** BGMAudioSource */
    [SerializeField]
    AudioSource m_SeAudioSource;

    /** フェード時間(フレーム数) */
    [SerializeField]
    float m_FadeTimeRateFrame = 30f;

    /** フェード中フラグ */
    static string m_NextBgm = "";

    /** 全トラックのAudioClip辞書配列 */
    public Dictionary<string, AudioClip> m_BgmDic, m_SeDic;

    void Start ()
    {
        m_Instance = this;

        DontDestroyOnLoad(this.gameObject);

        // 音声ファイルを読み込み、辞書配列に格納する
        m_BgmDic = new Dictionary<string, AudioClip>();
        m_SeDic = new Dictionary<string, AudioClip>();

        object[] bgmList = Resources.LoadAll(BGM_LOAD_PATH);
        object[] seList = Resources.LoadAll(SE_LOAD_PATH);

        foreach(AudioClip bgm in bgmList)
        {
            m_BgmDic[bgm.name] = bgm;
        }

        foreach (AudioClip se in seList)
        {
            m_SeDic[se.name] = se;
        }
    }

    /// <summary>
    /// SEを再生する
    /// </summary>
    /// <param name="seName">ファイル名</param>
    /// <param name="callback">再生後のコールバック処理(オプション)</param>
    public static void PlaySE(string seName, FunctionType callback = null)
    {
        // Instanceが生成されているか
        if (Instance == null)
            return;

        // SEが存在するかどうか調べる
        if(!Instance.m_SeDic.ContainsKey(seName))
        {
            Debug.Log(seName + "is not found.");
            return;
        }
        else
        {
            // SE再生
            Instance.m_SeAudioSource.PlayOneShot(Instance.m_SeDic[seName] as AudioClip);

            // コールバック実行
            if(callback != null)
            {
                IEnumerator coroutine = Instance.PlayAfterCallback(callback);
                Instance.StartCoroutine(coroutine);
            }
        }
    }

    /// <summary>
    /// SE再生後にコールバック処理を実行する
    /// </summary>
    public delegate void FunctionType();
    private IEnumerator PlayAfterCallback(FunctionType callback)
    {
        while(true)
        {
            yield return new WaitForFixedUpdate();

            // 再生が終了したら
            if(!Instance.m_SeAudioSource.isPlaying)
            {
                callback();
                break;
            }
        }
    }

    /// <summary>
    /// BGMを再生する
    /// </summary>
    /// <param name="bgmName">ファイル名</param>
    public static void PlayBGM(string bgmName)
    {
        // Instanceが生成されているか
        if (Instance == null)
            return;

        // BGMが存在するかどうか調べる
        if (!Instance.m_BgmDic.ContainsKey(bgmName))
        {
            Debug.Log(bgmName + "is not found.");
            return;
        }

        // BGMが流れていないなら
        if(!Instance.m_BgmAudioSource.isPlaying)
        {
            Instance.m_BgmAudioSource.clip = Instance.m_BgmDic[bgmName] as AudioClip;
            Instance.m_BgmAudioSource.Play();
        }
        // 既に別ファイルのBGMを再生中だったら
        else if (Instance.m_BgmAudioSource.clip.name != bgmName)
        {
            // フェード中だったら要求を棄却
            if (!string.IsNullOrEmpty(m_NextBgm))
                return;

            // フェードアウトさせた後にBGMを変える
            m_NextBgm = bgmName;
            IEnumerator coroutine = Instance.FadeOutPlayBGM(m_NextBgm);
            Instance.StartCoroutine(coroutine);
        }
    }

    /// <summary>
    /// フェードアウトさせた後に別のBGMを再生する
    /// </summary>
    /// <param name="nextBgmName">次のBGMファイル名</param>
    IEnumerator FadeOutPlayBGM(string nextBgmName)
    {
        // 開始時間と初期音量を保存
        float startTime = Time.timeSinceLevelLoad;
        float defaultVolume = m_BgmAudioSource.volume;

        while (true)
        {
            // 経過時間と割合を求める
            float diff = Time.timeSinceLevelLoad - startTime;
            float rate = diff / (m_FadeTimeRateFrame / 60f);

            // 1～0の間で補完する
            m_BgmAudioSource.volume = Mathf.Lerp(1, 0, rate);

            // 音量が0になったら
            if(m_BgmAudioSource.volume <= 0f)
            {
                // 再生停止
                m_BgmAudioSource.Stop();
                m_BgmAudioSource.volume = defaultVolume;

                if (!string.IsNullOrEmpty(nextBgmName))
                {
                    // 次のBGMを再生する
                    PlayBGM(nextBgmName);
                    m_NextBgm = null;
                }

                yield break;
            }

            yield return null;
        }
    }
}
