using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMapFollow : MonoBehaviour
{
    public Transform player;

    void Start()
    {
        player = GameObject.FindObjectOfType<PlayerBehaviour>().gameObject.transform;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = player.position; ; // + new Vector3(0.0f, 0.0f, -10f);
    }
}
