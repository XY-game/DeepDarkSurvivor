using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseWeaponNet : BaseObj
{
    BaseWeaponCtrl weaponCtrl;

    private void Awake()
    {
        //photonView = GetComponent<PhotonView>();
        weaponCtrl = GetComponent<BaseWeaponCtrl>();

        //if (PhotonNetwork.isMasterClient) {
        //    weaponCtrl.enabled = true;
        //}
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
            stream.SendNext(weaponCtrl.ownerID);
            stream.SendNext(weaponCtrl.ownerViewID);
        }
        else
        {
            //Network player, receive data
            weaponCtrl.ownerID = (string)stream.ReceiveNext();
            weaponCtrl.ownerViewID = (int)stream.ReceiveNext();
        }
    }
}
