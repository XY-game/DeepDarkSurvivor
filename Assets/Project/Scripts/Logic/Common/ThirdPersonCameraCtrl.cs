using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCameraCtrl : MonoBehaviour {

    public float distanceAway = -5;          // distance from the back of the craft  
    public float distanceUp = 10;            // distance above the craft  
    public float smooth = 1;                // how smooth the camera movement is  
    private GameObject hovercraft;      // to store the hovercraft  
	public Vector3 targetPosition;     // the position the camera is trying to be in  
    public Transform follow;

    bool isShake = false;
    /// <summary>  
    /// 相机震动方向  
    /// </summary>  
    public Vector3 shakeDir = new Vector3(0.5f,0f,0.5f);
    /// <summary>  
        /// 相机震动时间  
        /// </summary>  
    public float shakeTime = 1.0f;

    private float currentTime = 0.0f;
    private float totalTime = 0.0f;

    private float orginY = 0;

    // Use this for initialization
    void Start () {
        

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Init() {
        orginY = follow.position.y;
    }

    public void BeginShake() {
        Trigger();
        isShake = true;
    }

    void LateUpdate()
    {
        if (!follow)
            return;

        if (isShake)
        {
            UpdateShake();
        }
        else {
            // setting the target position to be the correct offset from the hovercraft  
            //targetPosition = follow.position + Vector3.up* distanceUp + follow.forward* distanceAway;  
            targetPosition = follow.position + Vector3.forward * distanceAway;
            // making a smooth transition between it's current position and the position it wants to be in  
            //transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime* smooth);  
            transform.position = Vector3.Lerp(transform.position, targetPosition, smooth);
            // make sure the camera is looking the right way!  
            //transform.LookAt(follow);
        }
    }



    public void Trigger()
    {
        totalTime = shakeTime;
        currentTime = shakeTime;
    }

    public void Stop()
    {
        isShake = false;
        currentTime = 0.0f;
        totalTime = 0.0f;
    }

    public void UpdateShake()
    {
        if (currentTime > 0.0f && totalTime > 0.0f)
        {
            float percent = currentTime / totalTime;

            Vector3 shakePos = Vector3.zero;
            shakePos.x = UnityEngine.Random.Range(-Mathf.Abs(shakeDir.x) * percent, Mathf.Abs(shakeDir.x) * percent);
            shakePos.y = UnityEngine.Random.Range(-Mathf.Abs(shakeDir.y) * percent, Mathf.Abs(shakeDir.y) * percent);
            shakePos.z = UnityEngine.Random.Range(-Mathf.Abs(shakeDir.z) * percent, Mathf.Abs(shakeDir.z) * percent);

            Camera.main.transform.position += shakePos;

            currentTime -= Time.deltaTime;
        }
        else
        {
            Stop();
            currentTime = 0.0f;
            totalTime = 0.0f;
        }
    }
}
