using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapData : MonoBehaviour
{
    public Image MapImage;

    void Start()
    {
        MapImage = GetComponent<Image>();
    }
}
