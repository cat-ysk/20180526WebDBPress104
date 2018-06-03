using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour {
    public GameObject hit = null;
    public GameObject explosion = null;
    float shotWaitTime = 0f;
    int shotCount = 0;
    int BULLET_MAX = 500;
    LinkedList<Bullet> reserveList, activeList;
    AudioSource[] audioEffect;

	// Use this for initialization
	void Start ()
    {
        reserveList = BulletList.GetBulletList(BULLET_MAX, "en_bullet");
        activeList = new LinkedList<Bullet>();
        audioEffect = GetComponents<AudioSource>();
    }
    
    // Update is called once per frame
    void Update () {
        var elapsedTime = Time.deltaTime;
        RapidProc(elapsedTime);
        LinkedListNode<Bullet> node = activeList.First;
        LinkedListNode<Bullet> prevNode = null;
        var index = 0;
        while (node != null)
        {
            var bl = node.Value;
            var act = bl.Run(index, elapsedTime);
            prevNode = node;
            node = node.Next;
            if (!act)
            {
                reserveList.AddLast(bl);
                activeList.Remove(prevNode);
                bl.Vanish();
            }
            ++index;
        }
	}

    const int WAY_COUNT = 8;
    const float WAY_ANGLE = 360f;
    float SHOT_SPAN = 12f / 60f; // 連射間隔
    float SHOT_SPEED = 5f;
    float RAPID_ANGLE = 5.5f;
    Quaternion WAY_ROT = Quaternion.Euler(0f, WAY_ANGLE, 0f);

    void RapidProc(float elapsedTime)
    {
        shotWaitTime -= elapsedTime;
        if (shotWaitTime > 0f)
            return;
        var tr = transform;
        Bullet bl = null;
        float angle = RAPID_ANGLE * shotCount;
        var rot = Quaternion.Euler(90f, angle, 0f);
        for (int i = 0; i < WAY_COUNT; ++i)
        {
            bl = PickupBullet();
            if (bl == null)
                break;
            bl.Shoot(tr.position, rot, SHOT_SPEED);
            rot = WAY_ROT * rot;
        }
        rot = Quaternion.Euler(90f, -angle, 0f);
        for (int i = 0; i < WAY_COUNT; ++i)
        {
            bl = PickupBullet();
            if (bl == null)
                break;
            bl.Shoot(tr.position, rot, SHOT_SPEED);
            rot = WAY_ROT * rot;
        }
        ++shotCount;
        shotWaitTime += SHOT_SPAN;
    }

    Bullet PickupBullet()
    {
        if (reserveList.Count == 0)
            return null;
        var bl = reserveList.First.Value;
        activeList.AddLast(bl);
        reserveList.RemoveFirst();
        return bl;
    }

    public int endurance = 500;

    private void OnTriggerEnter(Collider other)
    {
        var pos = transform.position;
        var rot = Quaternion.identity;
        Object.Instantiate(hit, pos, rot);

        --endurance;
        if (endurance < 1)
        {
            var s0 = audioEffect[0];
            s0.PlayOneShot(s0.clip);
            var s1 = audioEffect[1];
            s1.PlayOneShot(s1.clip);
            while(activeList.Count > 0)
            {
                var bl = activeList.First.Value;
                bl.Vanish();
                reserveList.AddLast(bl);
                activeList.RemoveFirst();
            }
            Object.Destroy(gameObject);
            Object.Instantiate(explosion, pos, rot);
        }
    }
}
