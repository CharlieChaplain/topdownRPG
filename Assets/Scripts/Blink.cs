using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blink : MonoBehaviour
{
    public List<Texture> allFaces;
    public Material faceMat;

    private float timer = 4f;

    // Update is called once per frame
    void Update()
    {
        if(timer <= 0)
        {
            timer = Random.Range(4f, 6f);
            StartCoroutine("CloseEyes");
        }
        timer -= Time.deltaTime;
    }

    IEnumerator CloseEyes()
    {
        faceMat.SetTexture("_MainTex", allFaces[1]);

        yield return new WaitForSeconds(0.2f);

        faceMat.SetTexture("_MainTex", allFaces[0]);
    }

}
