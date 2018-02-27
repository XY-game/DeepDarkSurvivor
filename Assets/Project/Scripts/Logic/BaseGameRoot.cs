using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(PhotonView))]
public class BaseGameRoot : Photon.MonoBehaviour {

    public static BaseGameRoot GetInstance() {
        if (!instance) {
            instance = GameObject.Find("GameRoot").GetComponent<BaseGameRoot>();
        }

        return instance;
    }

    private static BaseGameRoot instance = null;

    //按键事件
    public Joystick joystick;
    public EventTrigger FirenBtnEvent;
    public Button Skill1Btn;
    public Button Skill2Btn;
    public Button Skill3Btn;

    //玩家队列  
    public List<BasePlayerCtrl> playerList;

    //复活点
    public Transform[] reBornPos;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public Vector3 GetRebornPos(){
		return reBornPos[Random.Range(0,reBornPos.Length)].position;
	}

    public virtual void DestroyNetObj(GameObject destroyGO)
    {
        this.photonView.RPC("NetworkDestroy", PhotonTargets.All, destroyGO.GetComponent<PhotonView>().viewID);
    }

    [PunRPC]
    public void NetworkDestroy(int viewID) {
        PhotonView goID = PhotonView.Find(viewID);
        if (goID && goID.isMine)
        {
            PhotonNetwork.Destroy(goID.gameObject);
        }
    }

    public void DoAddPlayer(GameObject player) {
        this.photonView.RPC("AddPlayer", PhotonTargets.All, player.GetComponent<PhotonView>().viewID);
    }

    [PunRPC]
    public void AddPlayer(int viewID)
    {
        PhotonView goID = PhotonView.Find(viewID);
//        if (goID && PhotonNetwork.isMasterClient) {
		if (goID) {
            BasePlayerCtrl bpc = goID.GetComponent<BasePlayerCtrl>();
			if(!playerList.Contains(bpc))
            	playerList.Add(bpc);
        }
    }
    
    public void RemovePlayer(BasePlayerCtrl bpc)
    {
        playerList.Remove(bpc);
    }

    public virtual void RefreshHp(GameObject go,int hp,int damageOwnerViewID){
		this.photonView.RPC("RefreshObjHp", PhotonTargets.All, go.GetComponent<PhotonView>().viewID, hp, damageOwnerViewID);
	}

	[PunRPC]
	public void RefreshObjHp(int viewID,int hp, int damageOwnerViewID) {
		PhotonView goID = PhotonView.Find(viewID);
		if (goID)
		{
            BaseObjCtrl boc = goID.GetComponent<BaseObjCtrl>();

            boc.hp += hp;
            boc.Hurt();
            if (boc.hp > boc.maxHp)
            {
                boc.hp = boc.maxHp;
            }
            else if (boc.hp <= 0) {
                //BaseGameRoot.GetInstance().DoDieReport(nickName);
				if (damageOwnerViewID != -1 && boc.type == ObjType.TANK)
                    BaseGameRoot.GetInstance().DoAddEnergyByKill(damageOwnerViewID);
                boc.Dead();
            }
		}
	}

    public void DoRefreshObjHurt(GameObject go) {
        this.photonView.RPC("RefreshObjHurt", PhotonTargets.All, go.GetComponent<PhotonView>().viewID);
    }

    [PunRPC]
    public void RefreshObjHurt(int viewID)
    {
        PhotonView goID = PhotonView.Find(viewID);
        if (goID)
        {
            BaseObjCtrl boc = goID.GetComponent<BaseObjCtrl>();
            boc.HurtRefresh();
        }
     }

        string showName = "";
    public bool isShowName = false;
    public float showTime = 0;

    public virtual void DoDieReport(string name)
    {
        this.photonView.RPC("DieReport", PhotonTargets.All, name);
    }


    [PunRPC]
    public void DieReport(string name)
    {
        showName = name;
        isShowName = true;
    }

   

    public virtual void DoAddEnergyByKill(int killerViewID)
    {
        this.photonView.RPC("AddEnergyByKill", PhotonTargets.All, killerViewID);
    }

    [PunRPC]
    public void AddEnergyByKill(int killerViewID)
    {
        PhotonView killerGoID = PhotonView.Find(killerViewID);
        if (killerGoID)
        {
            BasePlayerCtrl bpc = killerGoID.GetComponent<BasePlayerCtrl>();
            bpc.AddEnergy(20);
        }

    }

	public GameObject GetPlayerNear(GameObject go){
		if (playerList.Count > 1) {
			playerList.Sort(delegate (BasePlayerCtrl pa, BasePlayerCtrl pb) {
				return Vector3.Distance(pa.transform.position, go.transform.position).CompareTo(
					Vector3.Distance(pb.transform.position, go.transform.position));
			});
			BaseObjCtrl boc = go.GetComponent<BaseObjCtrl>();
			for (int i = 0; i < playerList.Count; i++) {
				if (playerList[i].GetComponent<PhotonView>().viewID != boc.GetOwnerViewID()
					&& !playerList[i].isDead) {
					return playerList[i].gameObject;
				}
			}
		}

		return null;
	}


    private void OnGUI()
    {
        if (isShowName) {
            GUIStyle fontStyle = new GUIStyle();
            fontStyle.normal.background = null;    //设置背景填充  
            fontStyle.normal.textColor = new Color(1, 0, 0);   //设置字体颜色  
            fontStyle.fontSize = 30;       //字体大小  
            GUI.Label(new Rect(Screen.width / 2, Screen.height / 2, 500, 100), showName + "死了！！！！！！！！！！", fontStyle);
        }
    }
}
