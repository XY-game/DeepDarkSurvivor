using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReChangeSkillCtrl : BaseSkillCtrl
{

	// Use this for initialization
	void Start () {
        curSkillCD = 15;
        skillCD = 15;
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
        curSkillCD = 0;
    }
}
