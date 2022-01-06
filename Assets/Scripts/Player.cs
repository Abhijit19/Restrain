using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    float playerSpeed = 80.0f;
    bool playerMovementBool = true;
    bool infectorMovementBool = false;
    bool escBool = false;
    public GameObject infector;
    public GameObject playerCamera;
    public GameObject infectorCamera;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        PlayerMove();
        Infectors();
        EscapeButton();
    }

    void PlayerMove()
    {
        if(playerMovementBool == true)
        {
            float xp = Input.GetAxis("Horizontal") * playerSpeed * Time.deltaTime;
            float yp = Input.GetAxis("Vertical") * playerSpeed * Time.deltaTime;
            transform.position += new Vector3(xp, 0, yp);
        }
    }

    void Infectors()
    {
        if(infectorMovementBool == true)
        {
            float xp = Input.GetAxis("Horizontal") * playerSpeed * Time.deltaTime;
            float yp = Input.GetAxis("Vertical") * playerSpeed * Time.deltaTime;
            infector.transform.position += new Vector3(xp, 0, yp);
        }
    }

    void EscapeButton()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            escBool = true;
            playerCamera.SetActive(true);
            playerMovementBool = true;
            infectorCamera.SetActive(false);
            infectorMovementBool = false;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.tag == "zone1" && escBool == false)
        {
            Debug.Log("Player is in Zone1");
            infectorMovementBool = true;
            playerMovementBool = false;
            infectorCamera.SetActive(true);
            playerCamera.SetActive(false);

        }
    }
}
