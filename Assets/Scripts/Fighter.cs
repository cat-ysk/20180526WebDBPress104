using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fighter : MonoBehaviour {
    // 弾オブジェクトを保存できる最大数
    int BULLET_MAX = 100;

    // 弾オブジェクトをキャッシュしておくリスト（待機リスト、稼働リスト）
    LinkedList<Bullet> reserveList, activeList;

    // 射撃間隔
    float shotSpan = 3f / 60f;

    // 射撃待ち時間
    float shotWaitTime = 0f;

	// Use this for initialization
	void Start () {
        reserveList = BulletList.GetBulletList(BULLET_MAX, "bullet");
        activeList = new LinkedList<Bullet>();
    }
	
	void Update () {
#if UNITY_EDITOR || UNITY_STANDALONE
        if (Input.GetMouseButton(0))
        {
            MoveByMouse();
        }
        float elapsedTime = Time.deltaTime;
        // 右クリックを押したときの処理
        if (Input.GetMouseButtonDown(1))
        {
            shotWaitTime = elapsedTime;
        }
        // 右クリックを押しているときの処理
        if (Input.GetMouseButton(1))
        {
            shotWaitTime -= elapsedTime;
            if (shotWaitTime < 0f)
            {
                Shoot();
                shotWaitTime += shotSpan;
            }
        }
#else
        if (Input.touchCount > 0)
        {
            MoveByTouch();
        }
#endif
        MoveBullets();
    }

    
    // 自機の移動を行う
    void MoveByMouse()
    {
        Vector3 pos = Input.mousePosition;
        transform.position = GetScreen2WorldPosition(pos);
    }
    // 自機の移動を行う
    void MoveByTouch()
    {
        Touch touch = Input.GetTouch(0);
        Vector3 pos = touch.position;
        transform.position = GetScreen2WorldPosition(pos);
    }

    // 弾を撃つ
    void Shoot()
    {
        var pos = transform.position;
        for (int i = 0; i < 3; ++i)
        {
            // 弾が待機リストにない場合は撃たない
            if (this.reserveList.Count == 0) break;
            // 待機リストから稼働リストへ弾を移動
            var bl = reserveList.First.Value;
            activeList.AddLast(bl);
            reserveList.RemoveFirst();
            // どっぴゅんセレナーデ
            float angle = -10f + (10f * i);
            var rot = Quaternion.Euler(90f, angle, 0f);
            bl.Shoot(pos, rot, 20f);
        }
    }

    void MoveBullets()
    {
        float elapsedTime = Time.deltaTime;
        LinkedListNode<Bullet> node = activeList.First;
        LinkedListNode<Bullet> prevNode = null;
        var index = 0;
        while (node != null)
        {
            var bl = node.Value;
            var active = bl.Run(index, elapsedTime);
            prevNode = node;
            node = node.Next;
            if (!active)
            {
                reserveList.AddLast(bl);
                activeList.Remove(prevNode);
                bl.Vanish();
            }
            ++index;
        }
    }

    // スクリーンにタッチされた座標をワールド座標へ変換する
    Vector3 GetScreen2WorldPosition(Vector3 position)
    {
        Camera cam = Camera.main;
        Transform camTrans = cam.transform;
        position.z = camTrans.position.y;
        return cam.ScreenToWorldPoint(position);
    }

    private void OnTriggerEnter(Collider other)
    {
        gameObject.SetActive(false);
    }
}
