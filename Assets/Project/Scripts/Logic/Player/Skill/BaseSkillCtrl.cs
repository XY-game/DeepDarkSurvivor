using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseSkillCtrl : MonoBehaviour {

    public BasePlayerCtrl playerCtrl;

    public string ownerID;
    public int ownerViewID;

    public float curSkillCD = 5f;
    public float skillCD = 5f;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public virtual void OnSkillPress() {

    }

    public void UpdateSkillTime() {
        if (curSkillCD < skillCD)
            curSkillCD += Time.deltaTime;
    }
}
