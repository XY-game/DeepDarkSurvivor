using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseObj : Photon.MonoBehaviour
{
    //public PhotonView photonView;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
       
	}

    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        //if (stream.isWriting)
        //{
        //    //We own this player: send the others our data
        //    stream.SendNext(team);
        //    stream.SendNext(hp);
        //}
        //else
        //{
        //    //Network player, receive data
        //    team = (int)stream.ReceiveNext();
        //    hp = (int)stream.ReceiveNext();
        //}
    }
}
