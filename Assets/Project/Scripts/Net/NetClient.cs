using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void OnCreatedRoomDelegate();
public delegate void OnLeftRoomDelegate();
public delegate void OnJoinRoomDelegate();
public delegate void OnRejoinRoomDelegate();
public delegate void OnDisConnDelegate();
public delegate void OnMasterClientSwitchedDelegate(PhotonPlayer newMasterClient);

public class NetClient : MonoBehaviour {

	private static NetClient instatic;

	public static NetClient GetInstatic(){
		if (!instatic) {
			if (!GameObject.Find ("NetClient")) {
				GameObject netClient = new GameObject ("NetClient");
				instatic = netClient.AddComponent<NetClient> ();
			} else {
				GameObject netClient = GameObject.Find ("NetClient");
				instatic = netClient.GetComponent<NetClient> ();
			}
		}
		return instatic;
	}

	public event OnCreatedRoomDelegate OnCreatedRoomEvent;
    public event OnLeftRoomDelegate OnLeftRoomEvent;
	public event OnJoinRoomDelegate OnJoinRoomEvent;
	public event OnRejoinRoomDelegate OnRejoinRoomEvent;
	public event OnDisConnDelegate OnDisConnEvent;
    public event OnMasterClientSwitchedDelegate OnMasterClientSwitchedEvent;

	private bool isReJoin = false;

	void Awake(){
		DontDestroyOnLoad (this.gameObject);
		//ConnectToServer ();

	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void ConnectToServer(){
		PhotonNetwork.automaticallySyncScene = true;

		//set version
		if (PhotonNetwork.connectionStateDetailed == ClientState.PeerCreated)  
		{
            //PhotonNetwork.ConnectUsingSettings (LocalCfg.VERSION);
            PhotonNetwork.SwitchToProtocol(ExitGames.Client.Photon.ConnectionProtocol.Udp);
            PhotonNetwork.ConnectToMaster(LocalCfg.serverAddress, 5055,
                "ea30a92a-abe2-4af8-9486-5f26118d8867", LocalCfg.VERSION);
        }  
	
		//unique playername
		#if UNITY_EDITOR
		PhotonNetwork.playerName = "player" + Random.Range(0,9999);
		#else
		PhotonNetwork.playerName = "player" + SystemInfo.deviceUniqueIdentifier;
		#endif
	}

	public void OnJoinedLobby(){
		PhotonNetwork.autoCleanUpPlayerObjects = true;
		if (isReJoin) {
//			isReJoin = false;
			JoinRoom (LocalCfg.curRoom);
		}
	}

	public void OnConnectedToMaster()
	{
		PhotonNetwork.JoinLobby();
	}

	public void OnCreatedRoom()
	{
		Debug.Log("OnCreatedRoom ");
		if (OnCreatedRoomEvent != null) {	
			OnCreatedRoomEvent ();
		}
	}

	public void OnJoinedRoom()
	{
		Debug.Log("OnCreatedRoom ");
		if (OnJoinRoomEvent != null) {	
			OnJoinRoomEvent ();
		}

		if (isReJoin) {
			if (OnRejoinRoomEvent != null) {
				OnRejoinRoomEvent ();
			}
			isReJoin = false;
		}

	
	}

    public void OnLeftRoom()
    {
        Debug.Log("OnLeftRoom");
        if (OnLeftRoomEvent != null)
        {
            OnLeftRoomEvent();
        }
    }

    public void CreatRoom(string roomName){
		PhotonNetwork.CreateRoom (roomName,new RoomOptions{MaxPlayers = LocalCfg.MAX_PLAYER},new TypedLobby());
	}

	public void JoinOrCreatRoom(string roomName){
		PhotonNetwork.JoinOrCreateRoom (roomName,new RoomOptions{MaxPlayers = LocalCfg.MAX_PLAYER},new TypedLobby());
		LocalCfg.curRoom = roomName;
	}

	public void JoinRoom(string roomName){
		PhotonNetwork.JoinRoom (roomName);
	}

	public void ReJoinRoom(string roomName){
		PhotonNetwork.ReJoinRoom (roomName);
	}

	public void RandomRoom(string roomName){
		PhotonNetwork.JoinRandomRoom ();
	}

	public void JoinSingleRoom(){
		JoinOrCreatRoom (LocalCfg.SINGLE_ROOM_NAME);
	}

	public RoomInfo[] GetRoomlist(){
		return PhotonNetwork.GetRoomList();
	}

	public void LeaveRoom(){
		PhotonNetwork.LeaveRoom();
	}

	public void OnDisconnectedFromPhoton()
	{
		Debug.Log("Disconnected from Photon.");
//		ConnectToServer ();

		if (OnDisConnEvent != null) {
			OnDisConnEvent ();
		}

		isReJoin = true;
	}

	public void OnFailedToConnectToPhoton(object parameters)
	{
		Debug.Log("OnFailedToConnectToPhoton. StatusCode: " + parameters + " ServerAddress: " + PhotonNetwork.ServerAddress);
	
	}

	public void OnPhotonCreateRoomFailed()
	{
		Debug.Log("OnPhotonCreateRoomFailed got called. This can happen if the room exists (even if not visible). Try another room name.");
	}

	public void OnPhotonJoinRoomFailed(object[] cause)
	{
		Debug.Log("OnPhotonJoinRoomFailed got called. This can happen if the room is not existing or full or closed.");
	}

	public void OnPhotonRandomJoinFailed()
	{
		Debug.Log("OnPhotonRandomJoinFailed got called. Happens if no room is available (or all full or invisible or closed). JoinrRandom filter-options can limit available rooms.");
	}

	public void OnApplicationQuit(){
        if (PhotonNetwork.inRoom) {
            if (OnLeftRoomEvent != null)
            {
                OnLeftRoomEvent();
            }
            LeaveRoom();
        }
	}

	public void OnMasterClientSwitched(PhotonPlayer newMasterClient){
		Debug.Log("OnMasterClientSwitched");
		if (OnMasterClientSwitchedEvent != null) {	
			OnMasterClientSwitchedEvent (newMasterClient);
		}
	}


}
