using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SurviveGameRoot : BaseGameRoot {


    public GameObject localPlayer;
	public List<GameObject> npcs = new List<GameObject>();
	public int npcCount = 0;


    // Use this for initialization
    void Start () {
		InitScene ();
		NetClient.GetInstatic ().OnRejoinRoomEvent += ReJoinPlayer;
	}
	
	// Update is called once per frame
	void Update () {
        if (isShowName) {
            showTime += Time.deltaTime;
            if (showTime > 5f) {
                showTime = 0;
                isShowName = false;
            }
        }
	}

	void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
		if (stream.isWriting)
		{
			//We own this player: send the others our data
			//stream.SendNext(npcCount);
		}
		else
		{
			//Network player, receive data
			//npcCount = (int)stream.ReceiveNext();
		}
	}

	void InitScene(){
        InitPlayer();
    }

	void OnMasterClientSwitched(PhotonPlayer newMasterClient){
		if (newMasterClient.IsLocal) {
			Debug.Log ("Change me");
            playerList = new List<BasePlayerCtrl>(FindObjectsOfType<BasePlayerCtrl>());
        }
	}

	[PunRPC]
	void AddNpc(PhotonMessageInfo mi){
		GameObject newPlayerObject = PhotonNetwork.InstantiateSceneObject( "NPCTEST", Vector3.zero, Quaternion.identity, 0, null );
        npcs.Add(newPlayerObject);
        npcCount++;
	}
    
    void InitPlayer() {
		localPlayer = PhotonNetwork.Instantiate("Character2D", new Vector3(Random.Range(-2,2),
            0, Random.Range(-2, 2)), Quaternion.identity, 0);
        localPlayer.name = PhotonNetwork.playerName;

        DoAddPlayer(localPlayer);

        BasePlayerCtrl playerCtrl = localPlayer.GetComponent<BasePlayerCtrl>();
        playerCtrl.isLocalLogic = true;

        playerCtrl.selfMeleeWeaponCtrl = playerCtrl.handLBone.GetComponent<BaseSelfMeleeWeaponCtrl>();
        playerCtrl.selfMeleeWeaponCtrl.isLocalLogic = true;
        playerCtrl.selfMeleeWeaponCtrl.ownerViewID = localPlayer.GetComponent<PhotonView>().viewID;
    }
    

	bool isInitPlayer = true;
	void LateUpdate(){
		if (!isInitPlayer)
			return;
		playerList = new List<BasePlayerCtrl>(FindObjectsOfType<BasePlayerCtrl>());
		isInitPlayer = false;
	}

	public void ReJoinPlayer(){
		playerList = new List<BasePlayerCtrl>(FindObjectsOfType<BasePlayerCtrl>());
		InitPlayer ();
	}
}
