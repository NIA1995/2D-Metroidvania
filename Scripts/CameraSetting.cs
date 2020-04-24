using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSetting : MonoBehaviour {

    public Transform Target;

    Vector3 velocity = Vector3.zero;

    public float smoothTime = .15f;

    public bool YMaxEnabled = false;

    public float YMaxValue = 0;

    public bool YMinEnabled = false;
    public float YMinValue = 0;

    public bool XMaxEnabled = false;
    public float XMaxValue = 0;

    public bool XMinEnabled = false;
    public float XMinValue = 0;

    void Awake()
    {
        Target = GameObject.Find("Player").gameObject.transform;
    }

    void FixedUpdate () {

        Vector3 targetPos = Target.position;

        if (YMinEnabled && YMaxEnabled)
        {
            targetPos.y = Mathf.Clamp(Target.position.y, YMinValue, YMaxValue);
        }
        else if (YMinEnabled)
        {
            targetPos.y = Mathf.Clamp(Target.position.y, YMinValue, Target.position.y);
        }
        else if (YMaxEnabled)
        {
            targetPos.y = Mathf.Clamp(Target.position.y, YMinValue, Target.position.y);
        }

        if (XMinEnabled && XMaxEnabled)
        {
            targetPos.x = Mathf.Clamp(Target.position.x, XMinValue, XMaxValue);
        }
        else if (XMinEnabled)
        {
            targetPos.x = Mathf.Clamp(Target.position.x, XMinValue, Target.position.x);
        }
        else if (XMaxEnabled)
        {
            targetPos.x = Mathf.Clamp(Target.position.x, Target.position.x, XMaxValue);
        }

        targetPos.z = transform.position.z;

        transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref velocity, smoothTime);
		
	}
}
