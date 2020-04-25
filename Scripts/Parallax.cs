using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    private Renderer Renderer;
    public float ScrollSpeed = 0.5f;

    void Start()
    {
        Renderer = gameObject.GetComponent<Renderer>();
    }

    void Update()
    {
        if (Input.GetAxisRaw("Horizontal") > 0)
        {
            Vector2 textureOffset = new Vector2((Renderer.material.mainTextureOffset.x + ScrollSpeed), 0);

            Renderer.material.mainTextureOffset = textureOffset;
        }
        else if (Input.GetAxisRaw("Horizontal") < 0)
        {
            Vector2 textureOffset = new Vector2((Renderer.material.mainTextureOffset.x - ScrollSpeed), 0);

            Renderer.material.mainTextureOffset = textureOffset;
        }
    }
}
