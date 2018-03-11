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
		if (id == 0) {
			NetClient.GetInstatic().ConnectToCloudServer();
		} else if (id == 1) {
			NetClient.GetInstatic().ConnectToServer();
		}
     
        StartCoroutine (JoinSingle(id));
	}

	IEnumerator JoinSingle(int id){
		yield return new WaitForSeconds(3f);
        LocalCfg.nickname = nickName.text;
        NetClient.GetInstatic ().JoinSingleRoom ();
	}

	public void JoinedRoomCallBack(){
		SceneManager.LoadScene ("SurvivorGamePlay");
	}
}
