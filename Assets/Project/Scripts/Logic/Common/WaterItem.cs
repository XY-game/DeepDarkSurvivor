using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterItem : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<BaseObjCtrl>()) {
            BaseObjCtrl boc = collision.gameObject.GetComponent<BaseObjCtrl>();
            if (boc.isLocalLogic) {
                boc.TakeDamage(boc.maxHp, "water",-1);
            }
        }
    }
}
