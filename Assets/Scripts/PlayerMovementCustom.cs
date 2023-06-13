using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementCustom : MonoBehaviourPun
{
     //PhotonView Myplayer_;
    public float sensitivity=2;
    float rotY, desiredrot,rotX;
    public Camera mycam;

    private void Start()
    {
        if(!photonView.IsMine)
        {
            mycam.gameObject.SetActive(false);
        }
    }
    private void Update()
    {
      
        if(!photonView.IsMine)
        {
            Debug.Log("Controlling another guy");
        }
        else if(photonView.IsMine)
        {
            Movement();
        }
    }

    void Movement()
    {
        float xdir = Input.GetAxis("Horizontal");
        float ydir = Input.GetAxis("Vertical");
        transform.position += new Vector3(xdir, 0, ydir);
        rotX = Input.GetAxis("Mouse X");
        rotY -= Input.GetAxis("Mouse Y") * sensitivity;
        desiredrot = Mathf.Clamp(rotY, -90, 90);
        transform.Rotate(0, rotX, 0);
        mycam.transform.LookAt(transform);
        // mycam.transform.rotation = Quaternion.Euler(desiredrot, 0, 0);
        mycam.transform.Rotate(new Vector3(desiredrot, 0, 0));
    }
}
