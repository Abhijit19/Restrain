using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
public class PlayerHealth : MonoBehaviourPun
{
    public float Maxhealth = 100;
    private float currentHealth = 50;
    public Image healthImage;

    GameSetup GS;

    private void Awake()
    {
        GS = PhotonView.Find((int)photonView.InstantiationData[0]).GetComponent<GameSetup>();
    }

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = Maxhealth;
    }

    // Update is called once per frame
    void Update()
    {

        
    }

    public void TakeDamage()
    {
        //health -= 10;
        photonView.RPC("RPC_takeDamage", RpcTarget.All);
    }

    [PunRPC]
    public void RPC_takeDamage()
    {
        if (!photonView.IsMine)
            return;

        currentHealth -= 10;
        healthImage.fillAmount = currentHealth / Maxhealth;

        if(currentHealth <= 0)
        {
            Death();
        }
    }

    void Death()
    {
        GS.KillPlayer();
    }
}
