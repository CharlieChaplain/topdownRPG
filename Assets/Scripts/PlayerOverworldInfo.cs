using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerOverworldInfo : MonoBehaviour
{
    public bool leader; //true=leader, false=follower. The leader is moved via input, the follower follows the leader but can jump independantly. They can swap.
    public Animator anim;

    //this target will put this gameobject in the same position as the target
    public GameObject target; //the object to mirror

    //put these in a singleton maybe...?
    public GameObject firstPlayer;
    public GameObject secondPlayer;

    private bool swapped; //makes sure swap button isn't spammable

    private float lerpSpeed = 8f;
    private int coroutineCount = 0;

    // Start is called before the first frame update
    void Start()
    {
        swapped = false;
    }
    

    // Update is called once per frame
    void Update()
    {
        target.GetComponent<PlayerMove>().SetLeader(leader);
        target.GetComponent<PlayerMove>().SetAnim(anim);

        if(coroutineCount == 0)
        {
            SetPosition();
        }

        if (Input.GetAxis("Swap") > 0 && !swapped)
        {
            swapped = true;
            ChangeLeader();
        }

        if (Input.GetAxis("Swap") == 0)
            swapped = false;
    }

    void SetPosition()
    {
        transform.position = target.transform.position;
        transform.forward = target.transform.forward;
    }

    void ChangeLeader()
    {
        leader = !leader;
        if (leader)
            target = firstPlayer;
        else
            target = secondPlayer;
        StartCoroutine("Swap", target.transform.position);
    }

    IEnumerator Swap(Vector3 _target)
    {
        coroutineCount++;
        float timer = 0;
        float waitTime = 1f / lerpSpeed;
        while (timer < waitTime)
        {
            transform.position = Vector3.Lerp(transform.position, _target, timer / waitTime);
            timer += Time.deltaTime;
            yield return null;
        }
        transform.position = _target;
        coroutineCount--;
    }
}
