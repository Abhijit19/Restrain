using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class BulletMove : MonoBehaviourPun
{
    // Start is called before the first frame update
    float bulletSpeed = 250;
    void Start()
    {
        //Destroy(this.gameObject, 2);
        photonView.RPC("destroyBullet", RpcTarget.All);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += transform.forward * bulletSpeed*Time.deltaTime;

        //transform.Translate(transform.forward * bulletSpeed * Time.deltaTime);
        
    }

    /*private void OnCollisionEnter(Collision collisionDetuction)
    {
        if (collisionDetuction.gameObject.tag == "Enemy")
        {
            Destroy(collisionDetuction.gameObject);
            Destroy(this.gameObject);
        }
    }*/
    
    [PunRPC]
    public void destroyBullet()
    {
        if(gameObject!=null)
        Destroy(gameObject, 2f);
    }
}
