using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Items : MonoBehaviour
{
    SpriteRenderer Renderer;

    public string  Name;
    public int     Code;

    void Start()
    {
        Renderer = GetComponent<SpriteRenderer>();

        StartCoroutine(DisappearItem()); 
    }

    IEnumerator DisappearItem()
    {
        yield return new WaitForSeconds(60.0f);

        StartCoroutine(CommonFunction.AlphaBlink(new Color(1, 1, 1, 0), new Color(1, 1, 1, 1), Renderer));

        yield return new WaitForSeconds(2.0f);

        Destroy(gameObject);
    }
}
