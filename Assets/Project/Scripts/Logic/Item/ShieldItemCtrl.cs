using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldItemCtrl : BaseItemCtrl
{
    public BasePlayerCtrl playerCtrl;
    public Transform followTrans;

    // Use this for initialization
    void Start () {
        hp = 50;
        maxHp = 50;
         
	}
	
	// Update is called once per frame
	void Update () {
        if (followTrans) {
            gameObject.transform.position = followTrans.position + followTrans.forward * 0.8f;
            gameObject.transform.rotation = followTrans.rotation;
        }
           
    }

    public override void Dead()
    {
        base.Dead();
        if (isLocalLogic)
        {
            PhotonNetwork.Destroy(gameObject);
        }
    }

    public override void Init()
    {
        base.Init();
        GameObject go = PhotonView.Find(GetOwnerViewID()).gameObject;
        if (go) {
            followTrans = go.transform;
        }
      
    }
}
