using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldSkillCtrl : BaseSkillCtrl
{
    public ShieldItemCtrl shield;

	// Use this for initialization
	void Start () {
        curSkillCD = 20;
        skillCD = 20;

    }
	
	// Update is called once per frame
	void Update () {
        UpdateSkillTime();
    }

    public override void OnSkillPress()
    {
        base.OnSkillPress();
        if (curSkillCD < skillCD)
            return;
        if (shield)
        {
            shield.hp = shield.maxHp;
        }
        else {
            shield = PhotonNetwork.Instantiate("ShieldItem", playerCtrl.transform.position + playerCtrl.transform.forward * 0.3f,
            playerCtrl.transform.rotation, 0).GetComponent<ShieldItemCtrl>();
            shield.ownerViewID = playerCtrl.GetOwnerViewID();
            shield.isLocalLogic = true;
            shield.playerCtrl = playerCtrl;
            shield.Init();
        }
       
        curSkillCD = 0;
    }
}
