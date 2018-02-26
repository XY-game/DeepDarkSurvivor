using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseItemCtrl : BaseObjCtrl
{

    public int ownerViewID;

    public bool isInit = true;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public override int GetOwnerViewID()
    {
        return ownerViewID;
    }

    public virtual void Init() { }
}
