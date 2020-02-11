using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MirrorPosition : MonoBehaviour
{
    //this script will put a gameobject in the same position as another
    public GameObject target; //the object to mirror

    // Update is called once per frame
    void Update()
    {
        transform.position = target.transform.position;
        transform.forward = target.transform.forward;
    }
}
