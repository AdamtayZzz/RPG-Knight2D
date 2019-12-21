using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 伤害陷阱
/// </summary>
public class DamageArea : MonoBehaviour {

	void OnTriggerStay2D(Collider2D other)
    {
        Knight pc = other.GetComponent<Knight>();
        if(pc!=null)
        {
            //Debug.Log("玩家碰到了陷阱");
            pc.ChangeHealth(-2);
        }
    }
}
