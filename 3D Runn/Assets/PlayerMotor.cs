using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMotor : MonoBehaviour {

	private const float LANE_DISTANCE = 1.5f;
	private const float TURN_SPEED = 0.05f;
	//Movement
	private CharacterController controller;
	private float jumpForce = 5.0f;
	private float gravity = 12.0f;
	private float verticalVelocity;
	private float speed = 7.0f;

	private int desiredLane	 = 1; // 0 = Left, 1 = Middle, 2 = Right

	//FUnctionality
	private bool isRunning = false;

	//animation
	private Animator anim;


	// Use this for initialization
	void Start () {
		controller = GetComponent<CharacterController>();
		anim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {

		if (!isRunning)
			return;


		if(Input.GetKeyDown(KeyCode.LeftArrow)){
			moveLane(false);
		}
		if(Input.GetKeyDown(KeyCode.RightArrow)){
			moveLane(true);
		}

		//calculate	 where we should in future
		Vector3 targetPosition = transform.position.z * Vector3.forward;
		if (desiredLane == 0)
			targetPosition	+= Vector3.left * LANE_DISTANCE;
		else if (desiredLane == 2)
			targetPosition	+= Vector3.right * LANE_DISTANCE;

		//calculate our move delta
		Vector3 moveVector = Vector3.zero;
		moveVector.x = (targetPosition - transform.position).normalized.x * speed;

		bool isGrounded = IsGrounded();
		anim.SetBool("Grounded", isGrounded);

		//calculate Y
		if (isGrounded){
			verticalVelocity = -0.1f;
			
			if(Input.GetKeyDown(KeyCode.Space)){
				//Jump
				anim.SetTrigger("Jump");
				verticalVelocity = jumpForce;
			}

			else if(Input.GetKeyDown(KeyCode.DownArrow))
			{
				StartSliding();
				Invoke("StopSliding", 1.0f);
			}
		}
		else {
			verticalVelocity -= (gravity * Time.deltaTime);
			//fast falling mehcanic
			if(Input.GetKeyDown(KeyCode.Space)){
				verticalVelocity = -jumpForce;
			}
		}
		moveVector.y = verticalVelocity;
		moveVector.z = speed;

		// move the char
		controller.Move(moveVector * Time.deltaTime);

		//rotate the char
		Vector3 dir = controller.velocity;
		dir.y = 0;
		transform.forward = Vector3 .Lerp(transform.forward, dir, TURN_SPEED) ;
	}

	private void StartSliding()
	{
		anim.SetBool("Sliding", true);
		controller.height/=2;
		controller.center=new Vector3(controller.center.x, controller.center.y/2, controller.center.z);

	}

	private void StopSliding()
	{
		anim.SetBool("Sliding", false);
		controller.height*=2;
		controller.center=new Vector3(controller.center.x, controller.center.y*2, controller.center.z);

	
	}

	private void moveLane(bool goingRight)
	{
		//left 
		if (!goingRight)
		{
			desiredLane--;
			if(desiredLane == -1)
				desiredLane = 0;
		}
		else {
			desiredLane++;
			if(desiredLane == 3)
				desiredLane = 2;
		}
	}

	private bool IsGrounded() {
		Ray groundRay = new Ray(new Vector3(controller.bounds.center.x, (controller.bounds.center.y - controller.bounds.extents.y) + 0.2f, controller.bounds.center.z), Vector3.down);
		Debug.DrawRay(groundRay.origin, groundRay.direction, Color.cyan, 1.0f);

		return Physics.Raycast(groundRay, 0.2f + 0.1f);
	}

	public void StartRunning() {
		isRunning = true;
		anim.SetTrigger("StartRunning");
	}

	private void Crash()
	{
		anim.SetTrigger("Death");
		isRunning=false;
	}

	private void OnControllerColliderHit(ControllerColliderHit hit)
	{
		switch(hit.gameObject.tag)
		{
			case "Obstacle":
				Crash();
				break;
		}
	}
}
