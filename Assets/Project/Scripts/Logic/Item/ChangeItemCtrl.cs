using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeItemCtrl : BaseItemCtrl
{
    public string weaponSystemPrefab = "";

    private Transform tr;
    private Rigidbody rb;

    // Use this for initialization
    void Start () {
        type = ObjType.ITEM;

        tr = GetComponent<Transform>();
        rb = GetComponent<Rigidbody>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider collider)
    {
		if (PhotonNetwork.isMasterClient && collider.gameObject.GetComponent<BasePlayerCtrl>())
        {
            BasePlayerCtrl bpc = collider.gameObject.GetComponent<BasePlayerCtrl>();
            PhotonNetwork.Destroy(this.gameObject);
        }

    }
}
