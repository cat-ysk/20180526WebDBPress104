using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

    private bool isRender = false;

    private Vector3 direct = Vector3.forward;
    private float speed = 20f;

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
        float time = Time.deltaTime;
        var move = direct * (speed * time);
        transform.Translate(move, Space.World);

        if (!isRender)
        {
            Object.Destroy(gameObject);
        }
        isRender = false;
	}

    public void Shoot(Quaternion rot, float spd)
    {
        direct = rot * Vector3.up;
        speed = spd;
        transform.rotation = rot;
    }
}
