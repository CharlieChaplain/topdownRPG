using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{ 
    public float moveSpeed = 1f;

    private CharacterController controller;

    public GameObject debugText;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    private void Move()
    {
        float horz = Input.GetAxis("Horizontal");
        float vert = Input.GetAxis("Vertical");

        debugText.GetComponent<UnityEngine.UI.Text>().text = "horz:" + horz + "\nvert:" + vert;


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

        Vector3 deltaPos = new Vector3(horz, 0, vert);
        controller.Move(deltaPos * moveSpeed * Time.deltaTime);
    }
}
