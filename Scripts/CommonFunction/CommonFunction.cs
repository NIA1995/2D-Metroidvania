using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CommonFunction : MonoBehaviour
{
    public static IEnumerator AlphaBlink(Color A, Color B, SpriteRenderer Renderer, float TargetTime = 0.2f, int RepeatCount = 1)
    {
        for(var i = 0; i < RepeatCount; ++i)
        {
            yield return new WaitForSeconds(TargetTime);
            Renderer.color = A;
            yield return new WaitForSeconds(TargetTime);
            Renderer.color = B;
        }
    }

    public static IEnumerator FadeIn(GameObject TargetObejct, float TargetTime = 1.0f)
    {
        float TempTime = 0.0f;

        while(TempTime < TargetTime)
        {
            TargetObejct.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, TempTime / TargetTime);

            yield return new WaitForSeconds(Time.deltaTime);

            TempTime += Time.deltaTime;
        }

        yield return null;
    }

    public static IEnumerator FadeOut(GameObject TargetObejct, float TargetTime = 0.5f, bool DestroyBool = false)
    {
        while (TargetTime > 0.0f)
        {
            TargetObejct.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, TargetTime / 1);

            yield return new WaitForSeconds(Time.deltaTime);

            TargetTime -= Time.deltaTime;
        }

        if(DestroyBool)
        {
            Destroy(TargetObejct);
        }

        yield return null;
    }

    public static IEnumerator Lerp(Image TargetObject, float Speed)
    {
        while(TargetObject.color.a < 255)
        {
            TargetObject.color = Color.Lerp(TargetObject.color, new Color(0, 0, 0, 255), Speed * Time.deltaTime);

            yield return new WaitForSeconds(0.02f);
        }

        yield return new WaitForSeconds(0.1f);

        while (TargetObject.color.a > 0)
        {
            TargetObject.color = Color.Lerp(TargetObject.color, new Color(0, 0, 0, 0), Speed * Time.deltaTime);

            yield return new WaitForSeconds(0.02f);
        }
    }
}
