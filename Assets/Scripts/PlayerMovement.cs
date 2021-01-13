using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 0;
    public Animator anim;
    
    private CharacterController controller;
    private Vector3 moveDirection = Vector3.zero;
    private PlayerManager playerManager;
    private GameObject player;
    private GameObject meleeArea;
    private GameObject playerModel;
    private AudioSource footSteps;
    


    private void Start() {
        controller = GetComponent<CharacterController>();
        meleeArea = GameObject.Find("MeleeArea");
        meleeArea.SetActive(false);
        playerManager = GetComponent<PlayerManager>();
        footSteps = GetComponent<AudioSource>();
        
        Transform playerModelTransform = transform.Find("PlayerModel");
        if (playerModelTransform != null)
        {
            playerModel = playerModelTransform.gameObject;
            anim = playerModel.GetComponent<Animator>();
        }
    }

    private void Update()
    {
        if (!playerManager.isDead())
        {
            BaseMovement();
            MouseMovement();
        }
    }

    private void BaseMovement()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        moveDirection = new Vector3(moveX, 0, moveZ);
        moveDirection *= moveSpeed;
        controller.Move(moveDirection);

        anim.SetFloat("horizontal", moveX);
        anim.SetFloat("vertical", moveZ);

        if (Input.GetButtonDown("Horizontal") || Input.GetButtonDown("Vertical"))
            footSteps.Play();
        else if (!Input.GetButton("Horizontal") && !Input.GetButton("Vertical") && footSteps.isPlaying)
            footSteps.Stop();
    }
    
    private void MouseMovement()
    {
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.transform.position.y - transform.position.y)); 
        Vector3 lookDirection = new Vector3(worldPos.x, transform.position.y, worldPos.z);
        transform.LookAt(lookDirection);
        Debug.DrawLine(Camera.main.transform.position, lookDirection, Color.red);
    }
}
