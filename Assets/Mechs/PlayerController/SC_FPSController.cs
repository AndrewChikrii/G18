using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]

public class SC_FPSController : MonoBehaviour
{
	float walkingSpeed = 7f;
	float runningSpeed = 9f;
	float jumpSpeed = 8.0f;
	float gravity = 20.0f;
	public Camera playerCamera;
	[SerializeField] float lookSpeed = 2f;
	float lookXLimit = 65.0f;

	CharacterController characterController;
	Vector3 moveDirection = Vector3.zero;
	float rotationX = 0;

	[SerializeField] bool canMove = true;
	[SerializeField] bool crouched = false;
	Transform comCont;
	void Start() {
		characterController = GetComponent<CharacterController>();
		comCont = playerCamera.transform;
		// Lock cursor
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
	}

	void Update() {
		// We are grounded, so recalculate move direction based on axes
		Vector3 forward = transform.TransformDirection(Vector3.forward);
		Vector3 right = transform.TransformDirection(Vector3.right);
		// Press Left Shift to run
		bool isRunning = Input.GetKey(KeyCode.LeftShift);
		float curSpeedX = canMove ? (isRunning ? runningSpeed : walkingSpeed) * Input.GetAxis("Vertical") : 0;
		float curSpeedY = canMove ? (isRunning ? runningSpeed : walkingSpeed) * Input.GetAxis("Horizontal") : 0;
		float movementDirectionY = moveDirection.y;
		moveDirection = (forward * curSpeedX) + (right * curSpeedY);

		if (Input.GetButton("Jump") && canMove && characterController.isGrounded && !crouched) {
			moveDirection.y = jumpSpeed;
		} else {
			moveDirection.y = movementDirectionY;
		}

		if(Input.GetKey(KeyCode.LeftControl)) {
			crouched = true;
			walkingSpeed = 3f;
			runningSpeed = 3f;
			//playerCamera.GetComponent<PRaycast>().FreezeAction(true);
			characterController.height = Mathf.Lerp(characterController.height, 1.25f, 20f * Time.deltaTime);
			characterController.center = new Vector3(0f, Mathf.Lerp(characterController.center.y, -0.375f, 20f * Time.deltaTime), 0f);
			playerCamera.transform.localPosition = new Vector3(0f, Mathf.Lerp(playerCamera.transform.localPosition.y, 0.15f, 20f * Time.deltaTime), 0f);
			
		} else if (characterController.height < 2f) {
			crouched = false;
			walkingSpeed = 7f;
			runningSpeed = 9f;
			//playerCamera.GetComponent<PRaycast>().FreezeAction(false);
			characterController.height = Mathf.Lerp(characterController.height, 2f, 20f * Time.deltaTime);
			characterController.center = new Vector3(0f, Mathf.Lerp(characterController.center.y, 0f, 20f * Time.deltaTime), 0f);
			playerCamera.transform.localPosition = new Vector3(0f, Mathf.Lerp(playerCamera.transform.localPosition.y, 0.7f, 20f * Time.deltaTime), 0f);
		}
		
		if (!characterController.isGrounded) {
			moveDirection.y -= gravity * Time.deltaTime;
		}

		// Move the controller
		characterController.Move(moveDirection * Time.deltaTime);

		// Player and Camera rotation
		if (canMove) {
			rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
			rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
			playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
			transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);
		}
	}

	public void FreezeMove () {
		canMove = false;
	}


}

/*
    assign to player
*/