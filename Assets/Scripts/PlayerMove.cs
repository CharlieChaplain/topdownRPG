using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{ 
    public float moveSpeed = 1f;
    public float jumpForce = 10f;

    private CharacterController controller;

    public GameObject debugText;
    public Animator anim;

    public bool grounded;

    private bool jumpPressed; //prevents jump from being held down

    private float verticalVelocity = 0;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        CheckGrounded();
        Move();
        CheckJumpButton();
    }

    private void Move()
    {
        float horz = Input.GetAxis("Horizontal");
        float vert = Input.GetAxis("Vertical");

        


        //Determining forward, only uses 8 directions
        //N
        if (vert > .75f && horz > -.25f && horz < .25f)
            transform.forward = new Vector3(0, 0, 1f);
        //NE
        else if (horz > .5f && vert > .5f)
            transform.forward = new Vector3(.707f, 0, .707f);
        //E
        else if (horz > .75f && vert > -.25f && vert < .25f)
            transform.forward = new Vector3(1f, 0, 0);
        //SE
        else if (horz > .5f && vert < -.5f)
            transform.forward = new Vector3(.707f, 0, -.707f);
        //S
        else if (vert < -.75f && horz > -.25f && horz < .25f)
            transform.forward = new Vector3(0, 0, -1f);
        //SW
        else if (horz < -.5f && vert < -.5f)
            transform.forward = new Vector3(-.707f, 0, -.707f);
        //W
        else if (horz < -.75f && vert > -.25f && vert < .25f)
            transform.forward = new Vector3(-1f, 0, 0);
        //NW
        else if (horz < -.5f && vert > .5f)
            transform.forward = new Vector3(-.707f, 0, .707f);

        

        //multiplies vertical movement by a factor so it's not slow, but only if there's no horz movement to prevent diagonal movement from being too fast.
        //this multiplier changes with the camera angle
        if (horz == 0)
            vert *= 1.5f;

        //Jump stuff
        float up = 0;
        if (Input.GetAxis("Jump") > 0 && grounded && !jumpPressed)
        {
            verticalVelocity += jumpForce;
            jumpPressed = true;
            grounded = false;
        }

        Vector2 flatMovement = new Vector2(horz, vert) * moveSpeed; //measures speed on the x/z plane

        verticalVelocity += Physics.gravity.y;
        if (grounded)
            verticalVelocity = 0;
        Vector3 deltaPos = new Vector3(flatMovement.x, verticalVelocity, flatMovement.y);

        controller.Move(deltaPos * Time.deltaTime);

        debugText.GetComponent<UnityEngine.UI.Text>().text = verticalVelocity.ToString();

        anim.SetFloat("speed", flatMovement.magnitude);
        anim.SetFloat("vertSpeed", controller.velocity.y);
    }

    private void CheckGrounded()
    {
        grounded = false;
        RaycastHit hit;
        LayerMask mask = LayerMask.GetMask("Ground");
        //checks all 4 corners of collider
        if (Physics.Raycast(transform.position + new Vector3(1.7f, 0, 1.7f), Vector3.down, out hit, 0.5f, mask, QueryTriggerInteraction.Collide) ||
            Physics.Raycast(transform.position + new Vector3(-1.7f, 0, 1.7f), Vector3.down, out hit, 0.5f, mask, QueryTriggerInteraction.Collide) ||
            Physics.Raycast(transform.position + new Vector3(-1.7f, 0, -1.7f), Vector3.down, out hit, 0.5f, mask, QueryTriggerInteraction.Collide) ||
            Physics.Raycast(transform.position + new Vector3(1.7f, 0, -1.7f), Vector3.down, out hit, 0.5f, mask, QueryTriggerInteraction.Collide))
        {
            grounded = true;
        }
    }

    private void CheckJumpButton()
    {
        if(Input.GetAxis("Jump") == 0)
        {
            jumpPressed = false;
        }
    }
}
