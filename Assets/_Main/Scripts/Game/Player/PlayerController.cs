using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using Photon.Realtime;

public class PlayerController : MonoBehaviourPunCallbacks
{
    public bool isControllable = true;
    [SerializeField] GameObject headTarget;
    [SerializeField] GameObject cameraHolder;
    [SerializeField][Range(0.5f, 10f)]float mouseSensitivity = 5, defaultMovementSpeed = 5;
    float yVelocity = 0f;
    [SerializeField][Range(5f, 25f)]
    float gravity = 15f;
    //jump speed
    [SerializeField][Range(5f, 15f)]
    float jumpSpeed = 10f;
    float movementSpeed = 10f;
    float verticalLookRotation;
    Vector3 move;

    private CharacterController controller;
    public GameObject screenUI;

    private void Awake()
    {
        if (cameraHolder == null)
            cameraHolder = transform.GetComponentInChildren<Camera>().transform.parent.gameObject;
        movementSpeed = defaultMovementSpeed;
        controller = GetComponent<CharacterController>();

        if (!photonView.IsMine)
        {
            //Delete the camera
            GameObject.Destroy(cameraHolder);
            GameObject.Destroy(screenUI);
            //Destroy the controller???
            //GameObject.Destroy(controller);
        }
        //Set a name to game object
#if UNITY_EDITOR
        if(photonView != null && photonView.Owner != null)
            gameObject.name = $"{(photonView.Owner.IsLocal?"Local":"Remote")} - {photonView.Owner.ActorNumber}";
#endif
    }

    private void Update()
    {
        if (!photonView.IsMine)
            return;

        if (!isControllable)
            return;
        Look();
        Move();
        //AbilityUpdate();
    }

    private void FixedUpdate()
    {
        if (!photonView.IsMine)
            return;

        if (!isControllable)
            return;
        //and finally move
        controller.Move(move * Time.fixedDeltaTime);
    }

    private void Look()
    {
        //Rotate the player
        transform.Rotate(Vector3.up * Input.GetAxisRaw("Mouse X") * mouseSensitivity);

        verticalLookRotation += Input.GetAxisRaw("Mouse Y") * mouseSensitivity;
        verticalLookRotation = Mathf.Clamp(verticalLookRotation, -90f, 90f);

        cameraHolder.transform.localEulerAngles = Vector3.left * verticalLookRotation;
        headTarget.transform.localEulerAngles = cameraHolder.transform.localEulerAngles;
    }

    void Move()
    {
        //update speed based onn the input
        Vector3 input = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")).normalized;
        //transofrm it based off the player transform and scale it by movement speed
        move = transform.TransformVector(input) * movementSpeed;

        #region JUMP
        //is it on the ground
        if (controller.isGrounded)
        {
            yVelocity = -gravity * Time.deltaTime;
            //check for jump here
            if (Input.GetButtonDown("Jump"))
            {
                yVelocity = jumpSpeed;
            }
        }
        //now add the gravity to the yvelocity
        yVelocity -= gravity * Time.deltaTime;
        move.y = yVelocity;
        #endregion
        
    }

    public void Activate()
    {
        if (!photonView.IsMine)
            return;
        isControllable = true;
        cameraHolder.gameObject.SetActive(true);
        // Also reset the input values.
        move = Vector3.zero;
    }

    public void Deactivate()
    {
        if (!photonView.IsMine)
            return;
        isControllable = false;
        cameraHolder.gameObject.SetActive(false);
        // Also reset the input values.
        move = Vector3.zero;
    }
    public void OnDeath()
    {
        if (!photonView.IsMine)
            return;
        Debug.Log("Player died!");
        GameEndReason("Attacker Won");
    }

    private void GameEndReason(string reason)
    {
        Hashtable props = new Hashtable
        {
            {"GameSatate", (int)2},
            {"Reason", reason}
        };
        PhotonNetwork.CurrentRoom.SetCustomProperties(props);
    }
}
