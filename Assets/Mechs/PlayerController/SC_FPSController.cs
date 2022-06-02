using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

[RequireComponent(typeof(CharacterController))]

public class SC_FPSController : MonoBehaviour
{
    [SerializeField] float walkingSpeed = 3f;
    float walkingSpeedTempo;
    [SerializeField] float runningSpeed = 4.5f;
    float runningSpeedTempo;
    [SerializeField] float jumpSpeed = 8.0f;
    float gravity = 20.0f;
    public Camera playerCamera;
    [SerializeField] float lookSpeed = 2f;
    float lookXLimit = 65.0f;

    CharacterController characterController;
    Vector3 moveDirection = Vector3.zero;
    float rotationX = 0;

    RaycastHit hidingHit;
    bool rayHidingShot;

    [SerializeField] public static bool canMove = true;
    [SerializeField] bool crouched = false;

    RaycastHit crouchHit;
    bool rayCrouchShot;

    Transform comCont;
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        comCont = playerCamera.transform;
        // Lock cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        walkingSpeedTempo = walkingSpeed;
        runningSpeedTempo = runningSpeed;
    }

    void Update()
    {
        // We are grounded, so recalculate move direction based on axes
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);
        // Press Left Shift to run
        bool isRunning = Input.GetKey(KeyCode.LeftShift);
        float curSpeedX = canMove ? (isRunning ? runningSpeed : walkingSpeed) * Input.GetAxis("Vertical") : 0;
        float curSpeedY = canMove ? (isRunning ? runningSpeed : walkingSpeed) * Input.GetAxis("Horizontal") : 0;
        float movementDirectionY = moveDirection.y;
        moveDirection = (forward * curSpeedX) + (right * curSpeedY);

        if (Input.GetButton("Jump") && canMove && characterController.isGrounded && !crouched)
        {
            moveDirection.y = jumpSpeed;
        }
        else
        {
            moveDirection.y = movementDirectionY;
        }

        // Check if crouched under obstacle
        Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.up) * 1f, Color.grey);
        rayCrouchShot = Physics.Raycast(transform.position, transform.TransformDirection(Vector3.up), out crouchHit, 1f);

        // Crouch with ctrl
        if (Input.GetKey(KeyCode.LeftControl))
        {
            crouched = true;
            walkingSpeed = walkingSpeedTempo / 2f;
            runningSpeed = runningSpeedTempo / 2f;
            //playerCamera.GetComponent<PRaycast>().FreezeAction(true);
            characterController.height = Mathf.Lerp(characterController.height, 1.25f, 20f * Time.deltaTime);
            characterController.center = new Vector3(0f, Mathf.Lerp(characterController.center.y, -0.375f, 20f * Time.deltaTime), 0f);
            playerCamera.transform.localPosition = new Vector3(playerCamera.transform.localPosition.x, Mathf.Lerp(playerCamera.transform.localPosition.y, 0.1f, 20f * Time.deltaTime), 0f);

        }
        else if (characterController.height < 2f)
        {

            if (!crouchHit.collider)
            {
                crouched = false;
                walkingSpeed = walkingSpeedTempo;
                runningSpeed = runningSpeedTempo;
                //playerCamera.GetComponent<PRaycast>().FreezeAction(false);
                characterController.height = Mathf.Lerp(characterController.height, 2f, 20f * Time.deltaTime);
                characterController.center = new Vector3(0f, Mathf.Lerp(characterController.center.y, 0f, 20f * Time.deltaTime), 0f);
                playerCamera.transform.localPosition = new Vector3(playerCamera.transform.localPosition.x, Mathf.Lerp(playerCamera.transform.localPosition.y, 0.7f, 20f * Time.deltaTime), 0f);
            }

        }

        //Check if hiding
        Debug.DrawRay(transform.position, playerCamera.transform.TransformDirection(Vector3.forward) * 1f, Color.grey);
        rayHidingShot = Physics.Raycast(transform.position, playerCamera.transform.TransformDirection(Vector3.forward), out hidingHit, 1f);

        if (hidingHit.collider)
        {
            //Look out with Q, E
            if (Input.GetKey(KeyCode.Q))
            {
                playerCamera.transform.localPosition = new Vector3(Mathf.Lerp(playerCamera.transform.localPosition.x, -0.35f, 30f * Time.deltaTime), playerCamera.transform.localPosition.y, 0f);
            }
            else if (Input.GetKey(KeyCode.E))
            {
                playerCamera.transform.localPosition = new Vector3(Mathf.Lerp(playerCamera.transform.localPosition.x, 0.35f, 30f * Time.deltaTime), playerCamera.transform.localPosition.y, 0f);
            }
            else
            {
                playerCamera.transform.localPosition = new Vector3(Mathf.Lerp(playerCamera.transform.localPosition.x, 0f, 20f * Time.deltaTime), playerCamera.transform.localPosition.y, 0f);
            }
        }
        else
        {
            playerCamera.transform.localPosition = new Vector3(Mathf.Lerp(playerCamera.transform.localPosition.x, 0f, 20f * Time.deltaTime), playerCamera.transform.localPosition.y, 0f);
        }

        if (!characterController.isGrounded)
        {
            moveDirection.y -= gravity * Time.deltaTime;
        }

        // Move the controller
        characterController.Move(moveDirection * Time.deltaTime);

        // Player and Camera rotation
        if (canMove)
        {
            rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
            rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
            playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
            transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);
        }
    }
}