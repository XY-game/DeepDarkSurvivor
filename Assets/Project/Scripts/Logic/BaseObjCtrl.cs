using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ObjType {
    TANK = 0,
    WEAPON = 1,
    ITEM=2
}

public class BaseObjCtrl : MonoBehaviour {

    public int team = 0;
    public int hp = 100;
    public int maxHp = 100;

    public int energy = 100;
    public int maxEnergy = 100;

    public ObjType type;
    public bool isLocalLogic = false;

    public bool isDead = false;
    public float curDeadCount = 0;

    public float curHurtColdDown = 0.5f;
    public bool isHurt = false;

    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }

    public virtual void TakeDamage(int damage, string damageOwnerID,int viewID)
    {
		BaseGameRoot.GetInstance ().RefreshHp (gameObject, -damage, viewID);
    }

    float energyTimeCount = 0;
    public virtual void EnergyLost()
    {
        if (energyTimeCount > 1) {
            energy--;
            energyTimeCount = 0;
        }

        if (energy <= 0) {
            Dead();
        }

        energyTimeCount += Time.deltaTime;
    }

    public virtual void AddEnergy(int add)
    {
        energy += add;
        if (energy > maxEnergy) {
            energy = maxEnergy;
        }
    }

    public virtual void Dead() {

    }
    public virtual void Hurt()
    {
    }

    public virtual void HurtRefresh()
    {
    }

    public virtual void GetTarget()
    {
        
    }

//    public virtual void GetTargetCallBack(GameObject result)
//    {
//    }

    public virtual string GetOwnerID() {
        return "NONE_OWNER";
    }


    public virtual int GetOwnerViewID()
    {
        return -1;
    }
}
