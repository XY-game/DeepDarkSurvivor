using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpSkillCtrl : BaseSkillCtrl
{

	// Use this for initialization
	void Start () {
		
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
        playerCtrl.rbody.AddForce(Vector3.up * 300f + gameObject.transform.forward * 400f);
        curSkillCD = 0;
    }
}
