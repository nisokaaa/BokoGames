using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ball : MonoBehaviour
{
    /** 情報 */
    BallInfo m_Info;

    /** 2DTransform */
    RectTransform m_Rect;

    void Start ()
    {
		
	}
	
	void Update ()
    {
    }

    public void Init(Vector2 position, BallInfo info)
    {
        // ボール情報をロード
        m_Info = info;

        // Spriteを設定
        GetComponent<Image>().sprite = m_Info.sprite;

        // 2D座標を設定
        m_Rect = GetComponent<RectTransform>();
        m_Rect.position = position;
    }
}
