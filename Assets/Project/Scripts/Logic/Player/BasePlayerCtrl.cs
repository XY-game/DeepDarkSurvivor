using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using Anima2D;

public class BasePlayerCtrl : BaseObjCtrl
{
    public string playerID;
    public int viewID;
    public string nickName;

    public Joystick joystick;

    // 移动变量 
    public float moveSpeed = 0f;
    public float accSpeed = 0.2f;
    public float maxSpeed = 8f;

    public float curAttackCD = 1f;
    public float attackCD = 1f;
    public float attackStartTime = 0.1f;
    public float curAttackAbleTime = 0.3f;
    public float attackAbleTime = 0.3f;
    public bool isAttack = false;
    public bool isAttackAble = false;

    public Rigidbody rbody;
    public Transform tr;

    //位置和旋转信息变量设置初始值  
    public Vector3 currPos = Vector3.zero;

    public Vector3 curSpeed;

    //结构
    public Animator animator;

    //身体骨骼
    public Bone2D bodyBone;

    public Bone2D headBone;

    public Bone2D handLBone;

    public Bone2D handRBone;

    public Bone2D legLBone;

    public Bone2D legRBone;

    public Bone2D headWearBone;

    //身体sprite
    public SpriteMeshInstance bodySprite;
    public SpriteMeshInstance headSprite;
    public SpriteMeshInstance handLSprite;
    public SpriteMeshInstance handRSprite;
    public SpriteMeshInstance legLSprite;
    public SpriteMeshInstance legRSprite;
    public SpriteMeshInstance headWearSprite;

    public BaseSelfMeleeWeaponCtrl selfMeleeWeaponCtrl;

    public List<BaseSkillCtrl> skillList = new List<BaseSkillCtrl>();

    bool isResetPos = false;



    // Use this for initialization
    void Start() {
        rbody = GetComponent<Rigidbody>();
        tr = GetComponent<Transform>();
        type = ObjType.TANK;
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update() {
        if (isLocalLogic) {
            Move();
         
            if (Input.GetKeyDown(KeyCode.J))
            {
                Attack();
            }

            if (isAttack) {
                AttackCountDown();
            }

            if(isHurt)
                HurtCountDown();
        }
        CheckRot();
    }

    public bool isChangedDir = false;
    public bool isLeft = false;
    protected void Move()
    {
        isChangedDir = false;
        Vector3 moveDir = Vector3.zero;
        if (Input.GetKey(KeyCode.W))
        {
            moveDir += tr.up;
        }
        else if (Input.GetKey(KeyCode.S))
        {
			moveDir -= tr.up;
        }

        if (Input.GetKey(KeyCode.A))
        {
            moveDir -= tr.right;
            if (!isLeft) {
                isLeft = true;
            }
      
        }
        else if (Input.GetKey(KeyCode.D))
        {
            moveDir += tr.right;
            if (isLeft)
            {
                isLeft = false;
            }
        }

        animator.SetFloat("speed", moveDir.magnitude);
        tr.Translate (moveDir*Time.deltaTime);
    }

    void CheckRot()
    {
        if (isLeft)
        {
            tr.rotation = Quaternion.Euler(0, 180, 0);
        }
        else {
            tr.rotation = Quaternion.Euler(0, 0, 0);
        }
    }

    //通过UI事件发射射线
    public bool IsPointerOverUIObject(Vector2 screenPosition)
    {
        //实例化点击事件
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current)
        {
            //将点击位置的屏幕坐标赋值给点击事件
            position = new Vector2(screenPosition.x, screenPosition.y)
        };

        List<RaycastResult> results = new List<RaycastResult>();
        //向点击处发射射线
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);

        return results.Count > 0;
    }


    public virtual void Attack() {
        if (isAttack)
            return;

        selfMeleeWeaponCtrl.AttackStart();
        curAttackAbleTime = 0;
        curAttackCD = 0;
        isAttack = true;
    
        animator.SetTrigger("isAttack");
    }

    public void AttackCountDown() {
        if (!isAttackAble && curAttackCD > attackStartTime) {
            isAttackAble = true;
        }

        if (isAttackAble && curAttackAbleTime >= attackAbleTime) {
            curAttackAbleTime = attackAbleTime;
            selfMeleeWeaponCtrl.AttackEnd();
            isAttackAble = false;
        }

        if (curAttackCD >= attackCD) {
            curAttackCD = attackCD;
            isAttack = false;
        }

        if (isAttackAble)
            curAttackAbleTime += Time.deltaTime;

        curAttackCD += Time.deltaTime;
    }



    public override string GetOwnerID()
    {
        return playerID;
    }

    public override int GetOwnerViewID()
    {
        return viewID;
    }

    public override void TakeDamage(int damage, string damageOwnerID, int viewID)
    {
        //if (hp <= damage)
        //{
        //    BaseGameRoot.GetInstance().DoDieReport(nickName);
        //    if(viewID != -1)
        //        BaseGameRoot.GetInstance().DoAddEnergyByKill(viewID);
        //}

        if (isHurt || isDead)
            return;

        BaseGameRoot.GetInstance().RefreshHp(gameObject, -damage, viewID);
    }


    public virtual void ChangeWeapon(string weaponPrefab)
    {
        //if (isLocalLogic) {
         
            //GameObject newWeaponSystem = PhotonNetwork.Instantiate(weaponPrefab, gameObject.transform.position - 0.4f*Vector3.up, gameObject.transform.rotation, 0);
            //newWeaponSystem.GetComponent<BaseWeaponSystemCtrl>().ownerViewID = GetOwnerViewID();
            //BaseGameRoot.GetInstance ().DoChangeTankWeaponForAll(gameObject, newWeaponSystem);
        //}
    }

    public override void Hurt() {
        isHurt = true;
        curHurtColdDown = 0f;
        ChangeHurtColor(Color.red);
    }

    public void HurtCountDown()
    {
      if (curHurtColdDown >= 0.6f)
        {
            BaseGameRoot.GetInstance().DoRefreshObjHurt(gameObject);
        }
        curHurtColdDown += Time.deltaTime;
    }

    public override void HurtRefresh()
    {
        isHurt = false;
        curHurtColdDown = 0.5f;
        ChangeBoneColor(Color.white);
        Debug.Log("HurtRefresh");
    }

    public override void Dead()
    {
        if (!isLocalLogic || isDead)
            return;
        Debug.Log("dead");
        isDead = true;
        curDeadCount = 0;
        isHurt = false;
        curHurtColdDown = 0f;
        if (hurtCoro != null)
        {
            StopCoroutine(hurtCoro);
        }

        isAttack = false;
        isAttackAble = false;
        selfMeleeWeaponCtrl.AttackEnd();

        ChangeBoneColor(Color.white);
        //if (isLocalLogic) {
        //    rbody.velocity = Vector3.zero;
        //    rbody.isKinematic = true;
        //}

        isResetPos = false;
    }

    public void DeadCountDown()
    {
        if (curDeadCount > 1 && !isResetPos)
        {
            isResetPos = true;
            transform.position = BaseGameRoot.GetInstance().GetRebornPos();
        }
        else if (curDeadCount >= 5)
        {
            Reborn();
        }

        curDeadCount += Time.deltaTime;
    }


    Coroutine hurtCoro;
    public void ChangeHurtColor(Color color) {
        if (hurtCoro != null) {
            StopCoroutine(hurtCoro);
        }
        hurtCoro = StartCoroutine(EChangeHurtColor(color));  
    }

    public void ChangeBoneColor(Color color)
    {
        bodySprite.color = color;
        headSprite.color = color;
        handLSprite.color = color;
        handRSprite.color = color;
        legLSprite.color = color;
        legRSprite.color = color;
        headWearSprite.color = color;
    }

    IEnumerator EChangeHurtColor(Color color) {
        ChangeBoneColor(color);

        yield return new WaitForSeconds(0.1f);

        ChangeBoneColor(Color.white);

        hurtCoro = null;
        yield return 0;
    }

    public virtual void Reborn()
    {
        hp = maxHp;
        energy = maxEnergy;
        isDead = false;
        if (isLocalLogic)
        {
            rbody.velocity = Vector3.zero;
            rbody.isKinematic = false;
        }
    }

    private void OnDestroy()
    {
        if(!isLocalLogic)
            BaseGameRoot.GetInstance().RemovePlayer(this);
    }
}
