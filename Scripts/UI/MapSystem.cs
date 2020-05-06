using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapSystem : MonoBehaviour
{
    private bool TabIsOpen = false;

    public MapData[] MapList;

    private CanvasGroup Group;

    public Image NowMap;
    public int NowMapIndex = 0;

    public static MapSystem Instance;

    void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        Group = GetComponent<CanvasGroup>();

        MapList = transform.Find("MapTab").transform.Find("Panel").transform.Find("Image").transform.Find("RoomList").GetComponentsInChildren<MapData>();
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Tab))
        {
            if(TabIsOpen)
            {
                Group.alpha = 0;
                TabIsOpen = false;
            }
            else
            {
                Group.alpha = 1;
                TabIsOpen = true;

                NowMap = MapList[NowMapIndex].MapImage;

                NowMap.color = Color.red;
            }
        }
    }

    public void ChangeNowMap(int MapIndex)
    {
        NowMap.color = Color.white;

        NowMap = MapList[MapIndex].MapImage;
        NowMapIndex = MapIndex;

        NowMap.color = Color.red;
    }
}
