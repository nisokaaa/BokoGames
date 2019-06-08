using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    /**  */

	void Start ()
    {
		
	}
	
	void Update ()
    {
		
	}

    public void Init(Vector2 position)
    {
        transform.position = new Vector3(position.x, position.y, 0f);
    }
}
