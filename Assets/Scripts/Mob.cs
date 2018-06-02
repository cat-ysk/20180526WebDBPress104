using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mob : MonoBehaviour {

    public GameObject explosion = null;

    public enum MOVE
    {
        DOWN,
        HOMING,
        TURN_LEFT,
        TURN_RIGHT
    }

    public MOVE moveType = MOVE.DOWN;

    float MOVE_SPEED = 3f;
    float TURN_SPEED = 60f;
    Vector3 direct = Vector3.back;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        Move();
    }

    private void Move()
    {
        var elapsedTime = Time.deltaTime;
        var tr = transform;
        var vup = Vector3.up;
        var turnAngle = TURN_SPEED * elapsedTime;
        Quaternion rot;
        switch(moveType)
        {
            case MOVE.DOWN:
                direct = Vector3.back;
                break;
            case MOVE.HOMING:
                direct = CalcHomingDirect(elapsedTime);
                break;
            case MOVE.TURN_LEFT:
                rot = Quaternion.AngleAxis(turnAngle, vup);
                direct = rot * direct;
                break;
            case MOVE.TURN_RIGHT:
                rot = Quaternion.AngleAxis(-turnAngle, vup);
                direct = rot * direct;
                break;
        }
        var move = direct * (MOVE_SPEED * elapsedTime);
        var pos = tr.position + move;
        var fwd = Vector3.forward;
        rot = Quaternion.FromToRotation(fwd, direct);
        tr.SetPositionAndRotation(pos, rot);
    }
    // ホーミング処理
    Vector3 CalcHomingDirect(float time)
    {
        var vup = Vector3.up;
        // 自機オブジェクトをタグから取得
        var pl = GameObject.FindWithTag("Player");
        if (pl == null)
            return direct;
        var tr = transform;
        var plTr = pl.transform;
        // 自機方向を取得
        var target = plTr.position - tr.position;
        target = Vector3.Normalize(target);
        // 現在方向と自機方向の間の角度を計算
        var angle = Vector3.SignedAngle(direct, target, vup);
        var homingAngle = TURN_SPEED * time;
        Quaternion rot;
        if (angle < -homingAngle)
        {
            rot = Quaternion.AngleAxis(-homingAngle, vup);
            return rot * direct;
        }
        else if (angle > homingAngle)
        {
            rot = Quaternion.AngleAxis(homingAngle, vup);
            return rot * direct;
        }
        else
        {
            return target;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Object.Instantiate(explosion, transform.position, Quaternion.identity);
        Object.Destroy(gameObject);
        gameObject.SetActive(false);
    }


}
