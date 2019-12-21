using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform Follow_target;
    public float t = 3;
    private Vector2 targetPos;
    private Vector2 selfPos;
    
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        targetPos = new Vector2(transform.position.x, transform.position.y);
        selfPos = new Vector2(Follow_target.position.x, Follow_target.position.y);
        Vector2 result = Vector2.Lerp(targetPos, selfPos, t * Time.deltaTime);
        transform.position = new Vector3(result.x, result.y, transform.position.z);
    }
}
