using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    [Header("Bullet Properties")]
    public Vector3 direction;
    public float speed;
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        MoveBullet();
    }

    private void MoveBullet()
    {
        transform.position += direction * speed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        switch(other.gameObject.name)
        {
            case "Platform":
                Destroy(this.gameObject);
                break;
            case "Player":
                Destroy(this.gameObject);
                break;
        }
        Destroy(this.gameObject);
    }
}
