using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 0;
    public Animator anim;

    private bool isMoving;
    private float gravity;
    private CharacterController controller;
    private Vector3 moveDirection = Vector3.zero;
    private PlayerManager playerManager;
    private GameObject player;
    private GameObject meleeArea;
    private GameObject playerModel;
    private AudioSource footSteps;
    private static readonly int IsMoving = Animator.StringToHash("isMoving");


    private void Start()
    {
        controller = GetComponent<CharacterController>();
        meleeArea = GameObject.Find("MeleeArea");
        meleeArea.SetActive(false);
        playerManager = GetComponent<PlayerManager>();
        footSteps = GetComponent<AudioSource>();
        gravity -= 9.81f * Time.deltaTime;
        footSteps.loop = true;

        Transform playerModelTransform = transform.Find("PlayerModel");
        if (playerModelTransform != null)
        {
            playerModel = playerModelTransform.gameObject;
            anim = playerModel.GetComponent<Animator>();
        }
    }

    private void Update()
    {
        if (PauseMenuLogic.Paused)
        {
            isMoving = false;
            footSteps.Stop();
            return;
        }

        if (!playerManager.isDead())
        {
            BaseMovement();
            MouseMovement();
        }
        else
        {
            footSteps.Stop();
        }
    }

    private void BaseMovement()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        // when moving diagonally, the character moves about 1.4142 times as fast, which we need to normalize
        // source: https://forum.unity.com/threads/first-person-controller-walking-twice-as-fast-when-walking-diagonally.120296/
        var inputModifyFactor = (moveX != 0.0 && moveZ != 0.0) ? .7071f : 1.0f;

        if (controller.isGrounded)
        {
            gravity = 0f;
        }
        else
        {
            gravity -= 9.81f * Time.deltaTime;
        }

        moveDirection = new Vector3(moveX, gravity, moveZ);
        moveDirection *= moveSpeed * inputModifyFactor * Time.deltaTime;
        controller.Move(moveDirection);

        if ((moveDirection.x != 0 || moveDirection.z != 0) && !isMoving)
        {
            isMoving = true;
            footSteps.Play();
            anim.SetBool(IsMoving, true);
        }
        else if (moveDirection.x == 0 && moveDirection.z == 0 && isMoving)
        {
            isMoving = false;
            footSteps.Stop();
            anim.SetBool(IsMoving, false);
        }
    }

    private void MouseMovement()
    {
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y,
            Camera.main.transform.position.y - transform.position.y));
        Vector3 lookDirection = new Vector3(worldPos.x, transform.position.y, worldPos.z);
        transform.LookAt(lookDirection);
        Debug.DrawLine(Camera.main.transform.position, lookDirection, Color.red);
    }
}