using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseWeaponCtrl : BaseObjCtrl
{

    public string ownerID;
    public int ownerViewID;
    public int minDamage;
    public int maxDamage;

    public bool isHasLifeTime = false;
    public float lifeTime;
    public float TotallifeTime = 5f;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public virtual void AttackStart() { }

    public virtual void AttackEnd() { }

    public override string GetOwnerID()
    {
        return ownerID;
    }

    public override int GetOwnerViewID()
    {
        return ownerViewID;
    }

    public virtual void CheckLifeTime() {
        if (!isHasLifeTime)
            return;

        lifeTime -= Time.deltaTime;
        if (lifeTime <= 0)
        {
            PhotonNetwork.Destroy(gameObject);
        }
    }
}
