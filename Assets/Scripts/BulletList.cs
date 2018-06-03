using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletList : MonoBehaviour {

    // 弾丸リストを初期化して返す
    public static LinkedList<Bullet> GetBulletList(int max, string textureName)
    {
        var bulletList = new LinkedList<Bullet>();
        GameObject pr = Resources.Load<GameObject>(textureName);
        for (var i = 0; i < max; ++i)
        {
            GameObject obj = Object.Instantiate(pr);
            obj.SetActive(false);
            var bl = obj.GetComponent<Bullet>();
            bulletList.AddLast(bl);
        }
        return bulletList;
    }

}
