﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fighter : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        float elapsedTime = Time.deltaTime;
#if UNITY_EDITOR || UNITY_STANDALONE
        if (Input.GetMouseButton(0))
        {
            Vector3 pos = Input.mousePosition;
            transform.position = GetScreen2WorldPosition(pos); ;
        }
#else
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            Vector3 pos = touch.position;
            transform.position = GetScreen2WorldPosition(pos);
        }
#endif
    }

    Vector3 GetScreen2WorldPosition(Vector3 position)
    {
        Camera cam = Camera.main;
        Transform camTrans = cam.transform;
        position.z = camTrans.position.y;
        return cam.ScreenToWorldPoint(position);
    }
}
