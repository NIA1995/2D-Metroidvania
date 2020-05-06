using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneChange : MonoBehaviour
{
    public Image TargetRenderer;

    private void Awake()
    {
        Portal.PortalEvent += Fade;
    }

    private void Fade()
    {
        StopAllCoroutines();

        StartCoroutine(CommonFunction.Lerp(TargetRenderer, 50f));
    }
}
