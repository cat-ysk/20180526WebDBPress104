using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

    private bool isRender = false;

	// Use this for initialization
	void Start () {

    }

    void OnWillRenderObject()
    {
        Camera cam = Camera.current;
        isRender |= (cam.cameraType == CameraType.Game);
    }

    // Update is called once per frame
    void Update () {
        Vector3 move = new Vector3(0f, 0f, 1f);
        transform.Translate(move, Space.World);

        if (!isRender)
        {
            Object.Destroy(gameObject);
        }
        isRender = false;
	}

}
