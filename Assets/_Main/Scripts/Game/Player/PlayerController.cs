using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using Photon.Realtime;

public class PlayerController : MonoBehaviourPunCallbacks
{
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

    [Header("Usable Items")]
    public UsableItem[] usableItems;
    private int itemIndex = -1;
    private int previousItemIndex = -1;

    private void Awake()
    {
        if (cameraHolder == null)
            cameraHolder = transform.GetComponentInChildren<Camera>().transform.parent.gameObject;
        movementSpeed = defaultMovementSpeed;
        controller = GetComponent<CharacterController>();

        usableItems = GetComponents<UsableItem>();
        if(usableItems.Length > 0)
        {
            EquipItem(0);
        }

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
        Look();
        Move();
        AbilityUpdate();
    }

    private void FixedUpdate()
    {
        if (!photonView.IsMine)
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

    #region Abilities
    void AbilityUpdate()
    {
        //Equip Items
        for (int i = 0; i < usableItems.Length; i++)
        {
            if (Input.GetKeyDown((i + 1).ToString()))
            {
                EquipItem(i);
                break;
            }
        }

        if(Input.GetAxisRaw("Mouse ScrollWheel") > 0)
        {
            if (itemIndex >= usableItems.Length - 1)
                EquipItem(0);
            else
                EquipItem(itemIndex + 1);
        }
        else if (Input.GetAxisRaw("Mouse ScrollWheel") < 0)
        {
            if(itemIndex <= 0)
                EquipItem(usableItems.Length - 1);
            else
                EquipItem(itemIndex - 1);
        }
        
        //Check for fire input
        if (Input.GetButton("Fire1") && usableItems[itemIndex].CanUse)
        {
            if (!PhotonNetwork.IsConnected)
            {
                RPC_TriggerAbility(itemIndex);
                return;
            }
            //m_Abilities[0].TriggerAbility();
            //Debug.Log("Fire :"+Time.time);
            photonView.RPC("RPC_TriggerAbility", RpcTarget.All, itemIndex);
        }
    }

    private void EquipItem(int itemIndex)
    {
        //If we are changing to the same item, then skip it
        if (this.itemIndex == itemIndex)
            return;
        //If we have a previous item already equiped, then unequip
        if (previousItemIndex != -1)
            usableItems[previousItemIndex].Unequip();
        //equip the selected item
        usableItems[itemIndex].Equip();
        this.itemIndex = itemIndex;
        this.previousItemIndex = itemIndex;

        //make local player to send itemIndex
        if (photonView.IsMine)
        {
            Hashtable hash = new Hashtable();
            hash.Add("itemIndex", itemIndex);
            PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
        }
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        if(!photonView.IsMine && targetPlayer == photonView.Owner)
        {
            EquipItem((int)changedProps["itemIndex"]);
        }
    }

    [PunRPC]
    public void RPC_TriggerAbility(int index)
    {
        //Debug.Log("RPC Fire :" + Time.time);
        usableItems[index].UseItem();

    }
    #endregion
}
