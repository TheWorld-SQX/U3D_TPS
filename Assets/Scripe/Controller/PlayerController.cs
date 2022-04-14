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

    //Ԥ���� �ӵ�
    [SerializeField]
    private Transform bulletProjectile;

    //�ӵ�����λ��
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
        //��������
        isDead = characterStats.CurrentHealth == 0;
        if (isDead)
        {
            SwitchAnimation();
            return;
        }

        //ȷ����꣨��׼������
        #region 
        //��ȡ��Ļ���ģ�������׼�㣩λ��
        Vector2 screenCenterPoint = new Vector2(Screen.width * 0.7f,Screen.height * 0.55f);
        //��ʼ���λ��
        mouseWorldPosition = Vector3.zero;
        //���� �����������Ļ��Ĺ���
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

        //������ �ƶ�
        CharacterMove();

        //��׼���
        IfAim();

    }

    //�ƶ�����Ծ��
    private void CharacterMove()
    {
        noRunning = shiftControl.action.IsPressed();

        //�򵥵ĵ�����
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
        //Vector2.SmoothDamp��ʱ�����ƽ�һ�������𽥸ı�Ϊ����Ŀ�ꡣ����ͨ��ĳ�������ڵ��� - ����ĺ��������Ӳ�����Ŀ�꣩����ƽ����
        currentAnimationBlendVector = Vector2.SmoothDamp(currentAnimationBlendVector, input, ref refVelocity, 0.1f);
        
        //���� �ܡ��� ���ٶȣ�
        #region 
        //��סshift�������� ���� �ٶ�
        if (noRunning)
        {
            allSpeed = 0.05f * speedDelta;
            //��������� Speedֵ �����ܵ��ߵĹ���
            currentAnimationBlendVector.x = Mathf.Clamp(input.x, -0.21f,0.21f);
            currentAnimationBlendVector.y = Mathf.Clamp(input.y, -0.21f,0.21f);
            SoundManager.Instance.WalkAClip();
        }
        //û�а���shift ���� ���� �ٶ�
        if (!noRunning)
        {
            allSpeed = 0.055f * speedDelta;
        }

        Vector3 move = new Vector3(currentAnimationBlendVector.x, 0, currentAnimationBlendVector.y );
        animator.SetFloat("Speed",  Mathf.Abs(currentAnimationBlendVector.x) + Mathf.Abs(currentAnimationBlendVector.y));

        move = move.z * cameraMainTransform.forward.normalized + move.x * cameraMainTransform.right.normalized;
        move.y = 0f;
        //x,z�᷽���ϵ��ƶ���ʹ��CharacterController��װ��Move������
        controller.Move(allSpeed * Time.deltaTime * move);
        #endregion
        
        //��ת���ﳯ��
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

        //�ж��Ƿ�����Ծ������������Ծ
        #region 
        if (jumpControl.action.triggered && groundedPlayer)
        {
            animator.SetTrigger("Space");
            //����ģ�⣻
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
            if (groundedPlayer && playerVelocity.y < 0)
            {
                playerVelocity.y = 0f;
                //animator.SetTrigger("Drop");
                animator.SetFloat("Speed", 0);
            }
        }
        //����������ά����playerVelocity��һ��ֻ��y�᷽���ϵ�������ֵ
        playerVelocity.y += gravityValue * Time.deltaTime * 1f;
        //y�᷽���ϵ��ƶ���ʹ��CharacterController��װ��Move������
        controller.Move(playerVelocity * Time.deltaTime);
        #endregion
    }

    //������Ҽ���׼
    private void IfAim()
    {
        //������׼������Ҽ���
        if (aimControl.action.IsPressed())
        {
            worldAimTarget = mouseWorldPosition;
            worldAimTarget.y = transform.position.y;
            aimDirection = (worldAimTarget - transform.position).normalized;
            transform.forward = Vector3.Lerp(transform.forward, aimDirection, Time.deltaTime * 400f);

            aimRig.weight += Time.deltaTime * 10;

            aimCanvas.gameObject.SetActive(true);
            c_CCFreeLook.m_Lens.FieldOfView = 30f;
            c_CCFreeLook.m_YAxis.m_MaxSpeed = 0.666f;//x����0.15�ƶ���20%�ľ���
            c_CCFreeLook.m_XAxis.m_MaxSpeed = 100f;//y����ͬ��ʱ��Ҳ�ƶ�����������10%����

            //��� �������������
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

        //ʵ�����ӵ�����ǰ����
        Vector3 aimDir = (mouseWorldPosition - spawnBulletPosition.position).normalized;
        Instantiate(bulletProjectile, spawnBulletPosition.position, Quaternion.LookRotation(aimDir, Vector3.up));
        Hit();
    }
    //�ӵ�����Ŀ������˺�
    private void Hit()
    {
        //��Ϊ�� ��ʾ��ȡ����ǩΪBulletTarge�����壻��if (raycastHit.collider.CompareTag("BulletTarget")){bulletTarget = GameObject.FindGameObjectWithTag("BulletTarget");}
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

    //ͨ���������� �������ƶ��е���ת
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
