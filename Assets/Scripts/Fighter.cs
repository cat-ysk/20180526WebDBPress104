using System.Collections;
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
            transform.position = GetScreen2WorldPosition(pos);
        }
        if (Input.GetMouseButtonDown(1))
        {
            ThreeWayShot();
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

    void NormalShot()
    {
        GameObject pr = Resources.Load<GameObject>("bullet");
        Vector3 pos = transform.position;
        Quaternion rot = Quaternion.Euler(90f, 0f, 0f);
        Object.Instantiate(pr, pos, rot);
    }

    void ThreeWayShot()
    {
        var pr = Resources.Load<GameObject>("bullet");
        var pos = transform.position;
        for (int i = 0; i < 3; ++i)
        {
            var rot = Quaternion.identity;
            var obj = Object.Instantiate(pr, pos, rot);
            float angle = -10f + (10f * i);
            rot = Quaternion.Euler(90f, angle, 0f);

            var bl = obj.GetComponent<Bullet>();
            bl.Shoot(rot, 20f);
        }
    }

    Vector3 GetScreen2WorldPosition(Vector3 position)
    {
        Camera cam = Camera.main;
        Transform camTrans = cam.transform;
        position.z = camTrans.position.y;
        return cam.ScreenToWorldPoint(position);
    }
}
