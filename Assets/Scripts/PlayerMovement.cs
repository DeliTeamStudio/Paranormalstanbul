using Unity.Mathematics;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private float moveSpeed;
    [SerializeField] private float walkSpeed;
    [SerializeField] private float runSpeed;

    private Vector3 moveDirection;
    private Vector3 velocity;

    [SerializeField] private bool isGrounded;
    [SerializeField] private float groundCheckDistance;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private float gravity;
    [SerializeField] private float jumpHeight;

    private CharacterController controller;
    private Animator anim;



    private void Start()
    {
        controller = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();

    }


    private void Update()
    {
        Move();
    }


    private void Move()
    {
        isGrounded = Physics.CheckSphere(transform.position, groundCheckDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;

        }

        float moveWay = Input.GetAxis("Horizontal");

        moveDirection = new Vector3(0, 0, moveWay);
        moveDirection = transform.TransformDirection(moveDirection);

        //transform.rotation = Quaternion.Euler(0.0f, -90f, 0.0f);

        if (moveDirection != Vector3.zero && !Input.GetKey(KeyCode.LeftShift) && isGrounded)
        {
            Walk();//anim
        }
        else if (moveDirection != Vector3.zero && Input.GetKey(KeyCode.LeftShift) && isGrounded)

        {
            moveDirection *= moveSpeed;
            Run();//anim
        }
        else if (moveDirection == Vector3.zero && isGrounded)
        {

            Idle();//anim
        }


        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            Jump();//anim
        }


        //moveDirection *= moveSpeed;
        controller.Move(moveDirection * Time.deltaTime);
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

    }

    private void Walk()
    {

        moveSpeed = walkSpeed;

        anim.SetBool("toIdle", false);
        anim.SetBool("toRun", false);
        anim.SetBool("toJump", false);
        anim.SetBool("toWalk", true);


    }

    private void Run()
    {

        moveSpeed = runSpeed;

        anim.SetBool("toIdle", false);
        anim.SetBool("toWalk", false);
        anim.SetBool("toJump", false);
        anim.SetBool("toRun", true);
    }

    private void Idle()
    {

        anim.SetBool("toRun", false);
        anim.SetBool("toWalk", false);
        anim.SetBool("toJump", false);
        anim.SetBool("toIdle", true);

    }

    private void Jump()
    {

        anim.SetBool("toRun", false);
        anim.SetBool("toWalk", false);
        anim.SetBool("toIdle", false);
        anim.SetBool("toJump", true);

        velocity.y = math.sqrt(jumpHeight * -2 * gravity);


    }

    private void BackRun()
    {
        //moveSpeed = walkSpeed;
        //BackRun
    }




}
