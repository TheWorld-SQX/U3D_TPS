using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;
using UnityEngine.Animations.Rigging;

//[RequireComponent(typeof(CharacterController))]
public class PlayerController : Singleton<PlayerController>
{
    private CharacterController controller;
    [Header("Rigging")]
    [SerializeField]
    private Rig aimRig;
    [SerializeField]
    private Rig gunPoseRig, handIKRig;

    private Animator animator;
    private Vector2 currentAnimationBlendVector;
    private Vector2 refVelocity;
    private Vector3 playerVelocity;
    private bool groundedPlayer;
    private bool isDead;
    private bool noRunning;
    private bool moveOrShoot = true;

    [Header("Player")]
    [SerializeField]
    private float jumpHeight = 1.0f;
    [SerializeField]
    private float gravityValue = -9.81f;
    [SerializeField]
    private float rotationSpeed = 4f;
    [SerializeField]
    private float speedDelta =1f;
    private float allSpeed;

    [Header("InputActions")]
    [SerializeField]
    private InputActionReference movementControl;
    [SerializeField]
    private InputActionReference jumpControl;
    [SerializeField]
    private InputActionReference shiftControl;
    [SerializeField]
    private InputActionReference shootControl;
    [SerializeField]
    private InputActionReference aimControl;

    private Transform cameraMainTransform;

    private CinemachineFreeLook c_CCFreeLook;

    [SerializeField]
    private LayerMask aimColiderLayerMask = new LayerMask();

    //[SerializeField]
    //private Transform debugTransform;

    //预制体 子弹
    [SerializeField]
    private Transform bulletProjectile;

    //子弹生成位置
    [SerializeField]
    private Transform spawnBulletPosition;
    [SerializeField]
    private Canvas aimCanvas;

    private CharacterStats characterStats;
    private Vector3 worldAimTarget;
    private Vector3 aimDirection;
    private bool shootStatus;
    private Vector3 mouseWorldPosition;

    private GameObject bulletTarget;
    private CharacterStats targetstate;

    private void OnEnable()
    {
        movementControl.action.Enable();
        jumpControl.action.Enable();
        shiftControl.action.Enable();
        shootControl.action.Enable();
        aimControl.action.Enable();
    }
    private void OnDisable()
    {
        movementControl.action.Disable();
        jumpControl.action.Disable();
        shiftControl.action.Disable();
        shootControl.action.Disable();
        aimControl.action.Disable();
    }
    protected override void Awake()
    {
        base.Awake();
        //DontDestroyOnLoad(this);
    }
    private void Start()
    {
        c_CCFreeLook = FindObjectOfType<CinemachineFreeLook>();
        cameraMainTransform = FindObjectOfType<CinemachineBrain>().transform;

        //cameraMainTransform = Camera.main.transform;
        controller = gameObject.GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        characterStats = gameObject.GetComponent<CharacterStats>();
        GameManager.Instance.RigsterPlayer(characterStats);

    }

    private void Update()
    {
        //人物死亡
        isDead = characterStats.CurrentHealth == 0;
        if (isDead)
        {
            SwitchAnimation();
            return;
        }

        //确定鼠标（瞄准）方向
        #region 
        //获取屏幕中心（当作瞄准点）位置
        Vector2 screenCenterPoint = new Vector2(Screen.width * 0.7f,Screen.height * 0.55f);
        //初始鼠标位置
        mouseWorldPosition = Vector3.zero;
        //射线 从相机穿过屏幕点的光线
        Ray ray = Camera.main.ScreenPointToRay(screenCenterPoint);
        //Ray ray = Camera.main.ViewportPointToRay(Camera.main.ScreenToViewportPoint(new Vector3( Screen.width * 0.7f, Screen.height * 0.56f,0f)));
        Debug.DrawRay(ray.origin, ray.direction * 10f, Color.red);
        if (Physics.Raycast(ray, out RaycastHit raycastHit, 999f, aimColiderLayerMask,QueryTriggerInteraction.Ignore))
        {
            mouseWorldPosition = raycastHit.point;
            string tag_raycastHIt = raycastHit.collider.tag;
            if (raycastHit.collider.CompareTag(tag_raycastHIt))
            {
                bulletTarget = raycastHit.collider.gameObject;
                //bulletTarget = GameObject.FindGameObjectWithTag(tag_raycastHIt);
            }
        }
        #endregion

        //地面检测 移动
        CharacterMove();

        //瞄准射击
        IfAim();

    }

    //移动；跳跃；
    private void CharacterMove()
    {
        noRunning = shiftControl.action.IsPressed();

        //简单的地面检测
        groundedPlayer = controller.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }

        Vector2 input = movementControl.action.ReadValue<Vector2>();
        //if (input != new Vector2(0,0))
        //{
        //    SoundManager.Instance.WalkAClip();
        //}
        //Vector2.SmoothDamp随时间推移将一个向量逐渐改变为所需目标。向量通过某个类似于弹簧 - 阻尼的函数（它从不超过目标）进行平滑。
        currentAnimationBlendVector = Vector2.SmoothDamp(currentAnimationBlendVector, input, ref refVelocity, 0.1f);
        
        //设置 跑、走 的速度；
        #region 
        //按住shift键，则是 慢走 速度
        if (noRunning)
        {
            allSpeed = 0.05f * speedDelta;
            //动画混合树 Speed值 控制跑到走的过程
            currentAnimationBlendVector.x = Mathf.Clamp(input.x, -0.21f,0.21f);
            currentAnimationBlendVector.y = Mathf.Clamp(input.y, -0.21f,0.21f);
            SoundManager.Instance.WalkAClip();
        }
        //没有按下shift 则是 慢跑 速度
        if (!noRunning)
        {
            allSpeed = 0.055f * speedDelta;
        }

        Vector3 move = new Vector3(currentAnimationBlendVector.x, 0, currentAnimationBlendVector.y );
        animator.SetFloat("Speed",  Mathf.Abs(currentAnimationBlendVector.x) + Mathf.Abs(currentAnimationBlendVector.y));

        move = move.z * cameraMainTransform.forward.normalized + move.x * cameraMainTransform.right.normalized;
        move.y = 0f;
        //x,z轴方向上的移动（使用CharacterController封装的Move方法）
        controller.Move(allSpeed * Time.deltaTime * move);
        #endregion
        
        //旋转人物朝向
        #region 
        if (input != Vector2.zero)
        {
            if (moveOrShoot)
            {
                float targetAngle = Mathf.Atan2(input.x, input.y) * Mathf.Rad2Deg + cameraMainTransform.eulerAngles.y;
                Quaternion rotation = Quaternion.Euler(0f, targetAngle, 0f);
                this.transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * rotationSpeed);
            }
        }
        #endregion

        //判断是否按下跳跃键控制人物跳跃
        #region 
        if (jumpControl.action.triggered && groundedPlayer)
        {
            animator.SetTrigger("Space");
            //重力模拟；
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
            if (groundedPlayer && playerVelocity.y < 0)
            {
                playerVelocity.y = 0f;
                //animator.SetTrigger("Drop");
                animator.SetFloat("Speed", 0);
            }
        }
        //向上跳；三维向量playerVelocity是一个只在y轴方向上的有向量值
        playerVelocity.y += gravityValue * Time.deltaTime * 1f;
        //y轴方向上的移动（使用CharacterController封装的Move方法）
        controller.Move(playerVelocity * Time.deltaTime);
        #endregion
    }

    //鼠标左右键瞄准
    private void IfAim()
    {
        //按下瞄准（鼠标右键）
        if (aimControl.action.IsPressed())
        {
            worldAimTarget = mouseWorldPosition;
            worldAimTarget.y = transform.position.y;
            aimDirection = (worldAimTarget - transform.position).normalized;
            transform.forward = Vector3.Lerp(transform.forward, aimDirection, Time.deltaTime * 400f);

            aimRig.weight += Time.deltaTime * 10;

            aimCanvas.gameObject.SetActive(true);
            c_CCFreeLook.m_Lens.FieldOfView = 30f;
            c_CCFreeLook.m_YAxis.m_MaxSpeed = 0.666f;//x轴在0.15移动了20%的距离
            c_CCFreeLook.m_XAxis.m_MaxSpeed = 100f;//y轴在同样时间也移动了相对自身的10%距离

            //射击 （鼠标左键射击）
            shootStatus = shootControl.action.WasPressedThisFrame();
            if (shootStatus && shootControl.action.IsPressed())
            {
                Shoot();
            }
        }
        else
        {
            aimRig.weight -= Time.deltaTime * 10;
            c_CCFreeLook.m_Lens.FieldOfView = 40f;
            c_CCFreeLook.m_YAxis.Value = 0.47f;
            c_CCFreeLook.m_YAxis.m_MaxSpeed = 0f;
            c_CCFreeLook.m_XAxis.m_MaxSpeed = 108f;
            this.MoveAndShoot(true);
            aimCanvas.gameObject.SetActive(false);
        }
    }
    private void Shoot()
    {
        this.MoveAndShoot(false);

        worldAimTarget = mouseWorldPosition;
        worldAimTarget.y = transform.position.y;
        aimDirection = (worldAimTarget - transform.position).normalized;
        transform.forward = Vector3.Lerp(transform.forward, aimDirection, Time.deltaTime * 400f);

        //实例化子弹并向前发射
        Vector3 aimDir = (mouseWorldPosition - spawnBulletPosition.position).normalized;
        Instantiate(bulletProjectile, spawnBulletPosition.position, Quaternion.LookRotation(aimDir, Vector3.up));
        Hit();
    }
    //子弹击中目标计算伤害
    private void Hit()
    {
        //不为空 表示获取到标签为BulletTarge的物体；即if (raycastHit.collider.CompareTag("BulletTarget")){bulletTarget = GameObject.FindGameObjectWithTag("BulletTarget");}
        if (bulletTarget!= null)
        {
            targetstate = bulletTarget.GetComponent<CharacterStats>();
            if(targetstate != null && targetstate.CurrentHealth > 0)
            {
                bulletTarget.GetComponent<Animator>().SetTrigger("Hit");
                targetstate.TakeDamage(characterStats, targetstate);
            }
        }
    }

    //通过参数控制 人物在移动中的旋转
    private void MoveAndShoot(bool boolValue)
    {
        moveOrShoot = boolValue;
    }

    private void SwitchAnimation()
    {
        animator.SetBool("Death", isDead);
        aimRig.weight = 0;
        handIKRig.weight = 0;
        gunPoseRig.weight = 0;
        gameObject.GetComponentInChildren<RigTransform>().transform.position = new Vector3(0f, 0f, 0f);

        //GetComponentInChildren<BoxCollider>().isTrigger = false;
        //GetComponentInChildren<Rigidbody>().useGravity = true;
    }
}
