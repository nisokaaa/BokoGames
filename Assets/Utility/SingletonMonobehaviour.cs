using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * @brief   シングルトンテンプレート.
 */
public abstract class SingletonMonobehaviour<T> : MonoBehaviour where T : MonoBehaviour
{
    /**
     * @brief インスタンス.
     */
    private static T instance;

    /**
     * @brief インデクサ.
     */
    protected static T Instance
    {
        get
        {
            if(instance == null)
            {
                GameObject obj = new GameObject(typeof(T).Name);
                instance = obj.gameObject.AddComponent<T>();
            }
            return instance;
        }
    }

    /**
     * @brief   オブジェクト生成 コールバック.
     */
    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    /**
     * @brief   オブジェクト破棄 コールバック.
     */
    protected virtual void OnDestroy()
    {
        Destroy(this.gameObject);    
    }
}
