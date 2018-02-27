using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseItemNet : BaseObj
{
    public BaseItemCtrl itemCtrl;

    private void Awake()
    {
        itemCtrl = GetComponent<BaseItemCtrl>();
        if (PhotonNetwork.isMasterClient)
        {
            itemCtrl.isLocalLogic = true;
        }
    }

    // Use this for initialization
    void Start () {
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            //We own this player: send the others our data
            stream.SendNext(itemCtrl.ownerViewID);
        }
        else
        {
            //Network player, receive data
            itemCtrl.ownerViewID = (int)stream.ReceiveNext();
            if (itemCtrl.isInit)
            {
                itemCtrl.Init();
                itemCtrl.isInit = false;
            }
        }
       
    }
}
