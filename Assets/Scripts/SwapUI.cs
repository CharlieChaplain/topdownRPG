using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwapUI : MonoBehaviour
{
    public bool leader;

    public RectTransform firstPosition;
    public RectTransform secondPosition;

    public RawImage button;
    public Texture[] buttonIcons = new Texture[2];

    private bool swapped = false;
    private RectTransform rectTrans;

    private float lerpSpeed = 8f;

    private int coroutineCount = 0;

    // Start is called before the first frame update
    void Start()
    {
        rectTrans = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetAxis("Swap") > 0 && !swapped)
        {
            swapped = true;
            ChangeBadge();
        }
        //makes sure the swap coroutine isn't running AND the button isn't held down to make swapping possible again.
        if (Input.GetAxis("Swap") == 0 && coroutineCount == 0)
            swapped = false;
    }

    void ChangeBadge()
    {
        leader = !leader;
        if(leader)
        {
            button.texture = buttonIcons[0];
            Vector3 newPos = firstPosition.position;
            StartCoroutine("Swap", newPos);
        } else
        {
            button.texture = buttonIcons[1];
            Vector3 newPos = secondPosition.position;
            StartCoroutine("Swap", newPos);
        }
        
    }

    IEnumerator Swap(Vector3 target)
    {
        coroutineCount++;
        float timer = 0;
        float waitTime = 1f / lerpSpeed;
        while (timer < waitTime)
        {
            Vector3 trans = Vector3.Lerp(rectTrans.position, target, timer / waitTime);
            //snaps position to integer values (plus 0.5 due to positioning)
            trans.x = Mathf.Floor(trans.x) + 0.5f;
            trans.y = Mathf.Floor(trans.y) + 0.5f;
            rectTrans.position = trans;
            timer += Time.deltaTime;
            yield return null;
        }
        rectTrans.position = target;
        coroutineCount--;
    }
}
