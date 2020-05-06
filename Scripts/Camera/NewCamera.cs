using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewCamera : MonoBehaviour
{
    public Transform Target;
    public float Speed;

    public Vector2 Center;
    public Vector2 Size;

    float Height;
    float Width;

    public static NewCamera Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public MapSize DefaultMapSize;

    void Start()
    {
        Height = Camera.main.orthographicSize;
        Width = Height * Screen.width / Screen.height;

        Center = DefaultMapSize.gameObject.transform.position;
        Size = DefaultMapSize.GizmosSize;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(Center, Size);
    }

    private void Update()
    {
        transform.position = Vector3.Lerp(transform.position, Target.position, Time.deltaTime);

        float Lx = Size.x * 0.5f - Width;
        float ClampX = Mathf.Clamp(transform.position.x, -Lx + Center.x, Lx + Center.x);

        float Ly = Size.y * 0.5f - Height;
        float ClampY = Mathf.Clamp(transform.position.y, -Ly + Center.y, Ly + Center.y);

        transform.position = new Vector3(ClampX, ClampY, -10f);
    }
}
