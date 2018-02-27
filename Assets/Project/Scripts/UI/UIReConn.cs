using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIReConn : MonoBehaviour {

	public GameObject ui;

	// Use this for initialization
	void Start () {
		NetClient.GetInstatic ().OnDisConnEvent += DisConnedCallBack;
		NetClient.GetInstatic ().OnRejoinRoomEvent += RejoinCallBack;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void DisConnedCallBack(){
		ui.SetActive (true);
	}

	public void RejoinCallBack(){
		ui.SetActive (false);
	}

	public void OnReConnBtnClick(){
//		ui.SetActive (false);
		NetClient.GetInstatic ().ConnectToServer();
	}


}
