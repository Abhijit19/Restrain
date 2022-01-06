using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotgunBulletMove : MonoBehaviour
{
    // Start is called before the first frame update
    float bulletSpeed = 150;
    void Start()
    {
        Destroy(this.gameObject, 2);
    }

    // Update is called once per frame
    void Update()
    {
        if(PlayerMovement.shotgunBool == true)
        {
            transform.position += transform.forward * bulletSpeed * Time.deltaTime ;
        }
        
        if(PlayerMovement.fastShotGunBool == true)
        {
            transform.position += transform.forward * bulletSpeed * 2 * Time.deltaTime ;
        }

    }
}
