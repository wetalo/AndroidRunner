using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMotor : MonoBehaviour {

    private CharacterController controller;
    private Vector3 moveVector;
    
    private float runningSpeed;
    [SerializeField]
    private float initialRunningSpeed = 10.0f;
    [SerializeField]
    private float laneSwitchingSpeed = 20.0f;
    private float verticalVelocity = 0.0f;
    [SerializeField]
    private float gravity = 12.0f;
    [SerializeField]
    private float movementDelta = 2.0f;

    [SerializeField]
    private float animationDuration = 3.0f;

    private bool isDead = false;

    enum PlayerStates
    {
        running,
        switchingLane
    }

    [SerializeField]
    PlayerStates playerState;

    [SerializeField]
    Transform leftTarget;
    [SerializeField]
    Transform midTarget;
    [SerializeField]
    Transform rightTarget;

    [SerializeField]
    GameManager gameManager;

    Transform currentTarget;

    bool buttonReleased = true;
    float movementDirection;


    bool laneSwitchCommandGiven = false;

    public GameObject thrownObjectPrefab;
    [SerializeField]
    float throwingSpeed;
    [SerializeField]
    float pointsPerItem;

    float timePassed;

    // Use this for initialization
    void Start () {
        controller = GetComponent<CharacterController>();
        playerState = PlayerStates.running;
        currentTarget = midTarget;
        runningSpeed = initialRunningSpeed;
        timePassed = 0f;
	}
	
	// Update is called once per frame
	void Update () {
        timePassed += Time.deltaTime;
	}

    void RunStates()
    {
        if ((Input.GetAxisRaw("Horizontal") != 0) && buttonReleased){
            InitiateLaneSwitch(Input.GetAxisRaw("Horizontal"));
        }

        if (laneSwitchCommandGiven && playerState == PlayerStates.running)
        {
            Debug.Log("Run states ok");
            laneSwitchCommandGiven = false;
            buttonReleased = false;
            if (CanSwitchLane())
            {
                BeginSwitchLane();
            } else
            {
                ThrowItem();
            }
        } else if (laneSwitchCommandGiven)
        {
            Debug.Log("Player state not running");
        }
        Run();
        SwitchLane();
    }

    public void InitiateLaneSwitch(float movementDirection)
    {
        if (timePassed < animationDuration)
        {
            return;
        }

            laneSwitchCommandGiven = true;
        this.movementDirection = movementDirection;
        Debug.Log("this.movementDirection: " + this.movementDirection);
    }

    void BeginRun()
    {
        moveVector.x = 0;
        playerState = PlayerStates.running;
    }

    void Run()
    {
        if(playerState == PlayerStates.running)
        {
            moveVector.z = runningSpeed;//Y - Up and Down
            moveVector.y = verticalVelocity;
        }
    }

    void BeginSwitchLane()
    {
        Debug.Log("In BeginSwitchLane");
        GetTarget();
        playerState = PlayerStates.switchingLane;
        moveVector.x = movementDirection * laneSwitchingSpeed;
        GetComponent<AudioSource>().Play();
    }

    void SwitchLane()
    {
        if(playerState == PlayerStates.switchingLane)
        {
            Debug.Log(Mathf.Abs(transform.position.x - currentTarget.position.x));
            if(
                (Mathf.Abs(transform.position.x - currentTarget.position.x) < movementDelta) || 
                ((movementDirection < 0 && transform.position.x < currentTarget.position.x) || (movementDirection > 0 && transform.position.x > currentTarget.position.x))
            )
            {
                transform.position = new Vector3(currentTarget.position.x, transform.position.y, transform.position.z);
                BeginRun();
            }
            
            moveVector.z = runningSpeed;
            moveVector.y = verticalVelocity;
        }
    }

    private void LateUpdate()
    {
        //Check if player dead
        if (isDead)
        {
            return;
        }

        //Opening Animation
        if (Time.time < animationDuration)
        {
            controller.Move(Vector3.forward * runningSpeed * Time.deltaTime);
            return;
        }

        if (Input.GetAxisRaw("Horizontal") == 0)
        {
            buttonReleased = true;
        }

        UpdateTargetLocations();
        RunStates();
        Gravity();
        controller.Move(moveVector * Time.deltaTime);
    }

    public void SetSpeed(float modifier)
    {
        runningSpeed = initialRunningSpeed + modifier;
    }
    
    /*
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if(hit.point.z > transform.position.z + controller.radius)
        {
            Debug.Log("hit.gameObject.name: " + hit.gameObject.name);
            Death();
        }
    }*/

    void Gravity() {
        if (controller.isGrounded)
        {
            verticalVelocity = -0.5f;
        }
        else
        {
            verticalVelocity -= gravity * Time.deltaTime;
        }
    }

    void Death()
    {
        Debug.Log("Death");
        isDead = true;
        GetComponent<Score>().OnDeath();
        gameManager.PauseButton();
        gameManager.SubmitNewHighScore((int)(GetComponent<Score>().score));
    }

    bool CanSwitchLane()
    {
        Debug.Log("In CanSwitchLane");
        Debug.Log("movementDirection: " + movementDirection);
        //Turn Left
        if (movementDirection < 0)
        {
            if (currentTarget == rightTarget || currentTarget == midTarget)
            {
                return true;
            }
        } else if (movementDirection > 0)
        {
            if (currentTarget == leftTarget || currentTarget == midTarget)
            {
                return true;
            }
        }
        return false;
    }

    void GetTarget()
    {
        if (movementDirection < 0)
        {
            if (currentTarget == rightTarget) 
            {
                currentTarget = midTarget;
            } else if(currentTarget == midTarget)
            {
                currentTarget = leftTarget;
            }
        }
        else if (movementDirection > 0)
        {
            if (currentTarget == leftTarget)
            {
                currentTarget = midTarget;
            }
            else if (currentTarget == midTarget)
            {
                currentTarget = rightTarget;
            }
        }
    }

    void UpdateTargetLocations()
    {
        leftTarget.position = new Vector3(leftTarget.position.x, leftTarget.position.y, transform.position.z);
        midTarget.position = new Vector3(midTarget.position.x, midTarget.position.y, transform.position.z);
        rightTarget.position = new Vector3(rightTarget.position.x, rightTarget.position.y, transform.position.z);
    }

    void ThrowItem()
    {
        if(GetComponent<Inventory>().GetNumObjects() > 0)
        {
            Vector3 targetPosition = Vector3.zero;
            if (currentTarget == rightTarget)
            {
                targetPosition = (Vector3.forward * 100f + Vector3.right * 25f) ;
            }
            else if (currentTarget == leftTarget)
            {
                targetPosition = (Vector3.forward * 100f + Vector3.left *25f);
            }

            GameObject thrownObjectInstance = Instantiate(thrownObjectPrefab, transform.position, transform.rotation);
            thrownObjectInstance.GetComponent<Rigidbody>().AddForce(targetPosition - thrownObjectInstance.transform.position * throwingSpeed,ForceMode.Impulse);

            GetComponent<Inventory>().RemoveObject(0);
            GetComponent<Score>().GetPoints(pointsPerItem);
        }


    }
}

