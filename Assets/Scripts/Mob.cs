using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mob : MonoBehaviour {

    public GameObject explosion = null;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

    }

    private void OnTriggerEnter(Collider other)
    {
        Object.Instantiate(explosion, transform.position, Quaternion.identity);
        Object.Destroy(gameObject);
        gameObject.SetActive(false);
    }
}
