using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AStarPathing;

public class MapObjectCtrl : MonoBehaviour {

	public Node node;

	public bool isObstacle;

	// Use this for initialization
	void Start () {
		node = new Node (gameObject.transform.position);
		node.bObstacle = isObstacle;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
