using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurstSkillCtrl : BaseSkillCtrl
{

	// Use this for initialization
	void Start () {
		
	}

    // Update is called once per frame
    void Update()
    {
        UpdateSkillTime();
    }

    public override void OnSkillPress()
    {
        base.OnSkillPress();
        if (curSkillCD < skillCD)
            return;
        playerCtrl.rbody.AddForce(gameObject.transform.forward * 600f);
        curSkillCD = 0;
    }
}
