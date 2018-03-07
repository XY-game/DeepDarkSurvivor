using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UILogin : MonoBehaviour {


	bool isJoinInUpdate = false;
    public InputField serverAddress;
    public InputField nickName;

	// Use this for initialization
	void Start () {
		NetClient.GetInstatic ().OnCreatedRoomEvent += JoinedRoomCallBack;
	}

	// Update is called once per frame
	void Update () {
		
	}

	public void JoinSingleGame(int id){
        NetClient.GetInstatic().ConnectToServer();
        StartCoroutine (JoinSingle(id));
	}

	IEnumerator JoinSingle(int id){
		yield return new WaitForSeconds(3f);
        LocalCfg.nickname = nickName.text;
		if (id == 0) {
			LocalCfg.weaponSystem = "MachineGunBulletSystem";
			LocalCfg.skill_1 = "Shield";
			LocalCfg.skill_2 = "Burst";
			LocalCfg.skill_3 = "Jump";
		} else if (id == 1) {
			LocalCfg.weaponSystem = "TankBulletSystem";
			LocalCfg.skill_1 = "Rechange";
			LocalCfg.skill_2 = "Burst";
			LocalCfg.skill_3 = "Jump";
		} else if (id == 2) {
			LocalCfg.weaponSystem = "HeatSeekingMissileSystem";
			LocalCfg.skill_1 = "Shield";
			LocalCfg.skill_2 = "Burst";
			LocalCfg.skill_3 = "Jump";
		}
        NetClient.GetInstatic ().JoinSingleRoom ();
	}

	public void JoinedRoomCallBack(){
		SceneManager.LoadScene ("SurvivorGamePlay");
	}
}
