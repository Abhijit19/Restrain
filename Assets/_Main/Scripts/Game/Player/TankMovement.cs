using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankMovement : MonoBehaviourPunCallbacks
{
    public bool isControllable = false;
    [SerializeField] GameObject cameraHolder;
    [SerializeField] [Range(0.5f, 10f)] float mouseSensitivity = 5, defaultMovementSpeed = 5;
    float yVelocity = 0f;
    [SerializeField]
    [Range(5f, 25f)]
    float gravity = 15f;
    //jump speed
    [SerializeField]
    [Range(5f, 15f)]
    float jumpSpeed = 10f;
    float movementSpeed = 10f;
    float verticalLookRotation;
    Vector3 move;

    public AudioSource m_MovementAudio;         // Reference to the audio source used to play engine sounds. NB: different to the shooting audio source.
    public AudioClip m_EngineIdling;            // Audio to play when the tank isn't moving.
    public AudioClip m_EngineDriving;           // Audio to play when the tank is moving.
    public float m_PitchRange = 0.2f;           // The amount by which the pitch of the engine noises can vary.
    private float m_OriginalPitch;              // The pitch of the audio source at the start of the scene.

    private CharacterController controller;

    private void Awake()
    {
        if (cameraHolder == null)
            cameraHolder = transform.GetComponentInChildren<Camera>().transform.parent.gameObject;
        movementSpeed = defaultMovementSpeed;
        controller = GetComponent<CharacterController>();
        Deactivate();
    }

    private void Start()
    {

        // Store the original pitch of the audio source.
        m_OriginalPitch = m_MovementAudio.pitch;

#if UNITY_EDITOR
        if (photonView != null && photonView.Owner != null)
            gameObject.name = gameObject.name +  $"{(photonView.Owner.IsLocal ? "Local" : "Remote")} - {photonView.Owner.ActorNumber}";
#endif
    }

    private void Update()
    {
        if (!isControllable)
            return;
        Look();
        Move();
        EngineAudio();
        //AbilityUpdate();
    }

    private void FixedUpdate()
    {
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

    [ContextMenu("Activate")]
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
        isControllable = false;
        cameraHolder.gameObject.SetActive(false);
        // Also reset the input values.
        move = Vector3.zero;
    }


    private void EngineAudio()
    {
        // If there is no input (the tank is stationary)...
        if (Mathf.Abs(move.magnitude) < 0.1f )//&& Mathf.Abs(m_TurnInputValue) < 0.1f)
        {
            // ... and if the audio source is currently playing the driving clip...
            if (m_MovementAudio.clip == m_EngineDriving)
            {
                // ... change the clip to idling and play it.
                m_MovementAudio.clip = m_EngineIdling;
                m_MovementAudio.pitch = Random.Range(m_OriginalPitch - m_PitchRange, m_OriginalPitch + m_PitchRange);
                m_MovementAudio.Play();
            }
        }
        else
        {
            // Otherwise if the tank is moving and if the idling clip is currently playing...
            if (m_MovementAudio.clip == m_EngineIdling)
            {
                // ... change the clip to driving and play.
                m_MovementAudio.clip = m_EngineDriving;
                m_MovementAudio.pitch = Random.Range(m_OriginalPitch - m_PitchRange, m_OriginalPitch + m_PitchRange);
                m_MovementAudio.Play();
            }
        }
    }

}
