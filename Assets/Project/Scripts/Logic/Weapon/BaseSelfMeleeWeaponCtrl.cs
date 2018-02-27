using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Collections;

public class BaseSelfMeleeWeaponCtrl : BaseWeaponCtrl
{
    bool isAttack = false;
    List<BasePlayerCtrl> hurtedList = new List<BasePlayerCtrl>();

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public override void AttackStart() {
        isAttack = true;
        hurtedList.Clear();
    }

    public override void AttackEnd()
    {
        isAttack = false;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (isLocalLogic && isAttack && collision.transform.root.GetComponent<BasePlayerCtrl>())
        {
            BasePlayerCtrl bpc = collision.transform.root.GetComponent<BasePlayerCtrl>();
            if (bpc.GetOwnerViewID() != GetOwnerViewID() && !hurtedList.Contains(bpc))
            {
                Debug.Log(collision.gameObject.name);
                hurtedList.Add(bpc);
                bpc.TakeDamage(10, GetOwnerID(), GetOwnerViewID());
            }

        }
    }
}
