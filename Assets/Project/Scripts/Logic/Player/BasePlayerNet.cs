using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasePlayerNet : BaseObj
{
    ThirdPersonCameraCtrl cameraCtrl;
    BasePlayerCtrl playerCtrl;

    private void Awake()
    {
		playerCtrl = GetComponent<BasePlayerCtrl>();
        Init();
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
            stream.SendNext(playerCtrl.team);
            stream.SendNext(playerCtrl.hp);
            stream.SendNext(playerCtrl.energy);
            stream.SendNext(playerCtrl.playerID);
            stream.SendNext(playerCtrl.viewID);
            stream.SendNext(playerCtrl.nickName);
            stream.SendNext(playerCtrl.isDead);
            stream.SendNext(playerCtrl.isHurt);
            stream.SendNext(playerCtrl.isLeft);
        }
        else
        {
            //Network player, receive data
            playerCtrl.team = (int)stream.ReceiveNext();
            playerCtrl.hp = (int)stream.ReceiveNext();
            playerCtrl.energy = (int)stream.ReceiveNext();
            playerCtrl.playerID = (string)stream.ReceiveNext();
            playerCtrl.viewID = (int)stream.ReceiveNext();
            playerCtrl.nickName = (string)stream.ReceiveNext();
			playerCtrl.isDead = (bool)stream.ReceiveNext();
            playerCtrl.isHurt = (bool)stream.ReceiveNext();
            playerCtrl.isLeft = (bool)stream.ReceiveNext();
        }
    }

    void Init() {
        if (photonView.isMine) {
//            cameraCtrl = Camera.main.GetComponent<ThirdPersonCameraCtrl>();
//            cameraCtrl.follow = this.gameObject.transform;
//            cameraCtrl.enabled = true;

            playerCtrl.enabled = true;
			playerCtrl.isLocalLogic = true;
            playerCtrl.playerID = PhotonNetwork.playerName;
            playerCtrl.viewID = photonView.viewID;
            playerCtrl.nickName = LocalCfg.nickname;
            BaseSkillCtrl skill_1 = SkillSimpleFactory.GetSkill(playerCtrl, LocalCfg.skill_1);
            playerCtrl.skillList.Add(skill_1);
            BaseSkillCtrl skill_2 = SkillSimpleFactory.GetSkill(playerCtrl, LocalCfg.skill_2);
            playerCtrl.skillList.Add(skill_2);
            BaseSkillCtrl skill_3 = SkillSimpleFactory.GetSkill(playerCtrl, LocalCfg.skill_3);
            playerCtrl.skillList.Add(skill_3);
        }
    }

}
