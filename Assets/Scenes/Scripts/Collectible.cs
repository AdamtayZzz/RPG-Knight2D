using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 碰到地图上的回复物品
/// </summary>
public class Collectible : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Knight pc = other.GetComponent<Knight>();

        if (pc != null)
        {
            Debug.Log("玩家碰到了回复品");

            int num = Random.Range(0, 4);
            if (num == 0)
            {
                Debug.Log("回复10点生命值");
                pc.ChangeHealth(10);
            }
            else if (num == 1)
            {
                Debug.Log("血量-10");
                pc.ChangeHealth(-10);
            }
            else if (num == 2)
            {
                Debug.Log("蓝量+10");
                pc.ChangeMana(10);
            }
            else
            {
                Debug.Log("蓝量-10");
                pc.ChangeMana(-10);
            }

            //让回复品消失
            Destroy(this.gameObject);
        }
    }
}

