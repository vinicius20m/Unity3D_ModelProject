using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using Cinemachine.Utility;
using Cinemachine ;
using UnityEngine.InputSystem;


[RequireComponent(typeof(CharacterAnimationController))]
public class MyCharacterController : MonoBehaviour
{

    [SerializeField]float speed = 5 ;
    [SerializeField]float runSpeed = 1.70f ;
    [SerializeField]float timeToPeak = 1f ;
    [SerializeField]float maxHeight = 2f ;
    float gravity ; 
    float jumpSpeed ;
    float yVelocity = 0 ;
    float cameraFact = 5 ;
    float count = 0 ;
    float sprint = 1.7f ;
    float jumpFact = 0 ;
    public float _jumpFact
    {
        get{return jumpFact;}
        private set{;}
    }

    bool draged = false ;
    bool dragg = false ;
    bool grounded = true ;

    PlayerInputActions pInput ;
    CharacterAnimationController pAnim ;
    CharacterController cController ;

    private Vector2 directionInput = Vector2.zero ;
    public Vector2 _directionInput
    {
        get{return directionInput;}
        private set{;}
    }
    Vector2 mousePos = Vector2.zero ;
    Vector2 rightStick = Vector2.zero ;
    Vector2 initialPos = Vector2.zero ;
    Vector2 res = Vector2.zero ;
    Vector2 res2 = Vector2.zero ;
    Vector2 res3 = Vector2.zero ;

    Vector3 move = Vector3.zero ;

    private void Awake() {

        gravity = 2 * maxHeight / Mathf.Pow(timeToPeak, 2) ;
        jumpSpeed = gravity * timeToPeak ;
        
        cController = GetComponent<CharacterController>() ;
        pAnim = GetComponent<CharacterAnimationController>() ;
        pInput = new PlayerInputActions() ;
        pInput.GroundControls.Enable() ;

        pInput.GroundControls.Attack.performed += OnAttackInput ;
        pInput.GroundControls.Jump.performed += OnJumpInput ;

        // Debug.Log(GameObject.Find("/Third Person Camera").GetComponent<CinemachineFreeLook>());
    }

    void Update()
    {
        
        bool run = pInput.GroundControls.Run.ReadValue<float>() > 0 ? true : false ;
        bool drag = (pInput.GroundControls.Drag.ReadValue<float>() > 0) ? true : false ;

        pAnim._isRunning = run ;
        pAnim._isJumping = !grounded ;

        if(!grounded){

            yVelocity += gravity * Time.deltaTime * -1 ;
            jumpFact = yVelocity / jumpSpeed ;
        }

        rightStick = Gamepad.current.rightStick.ReadValue() ;
        dragg = rightStick != Vector2.zero ? true : false ;
        
//      CAMERA
        if(drag || dragg){

            mousePos = pInput.GroundControls.Look.ReadValue<Vector2>() ;
            initialPos = draged ? initialPos : mousePos ;
            res = mousePos - initialPos ;
            res3 = draged ? (res - res2) : Vector2.zero ;
            this.transform.Rotate(0, (dragg ? rightStick.x  : res3.x / cameraFact), 0) ;
            res2 = mousePos - initialPos ;
            draged = true ;
        }else
            draged = false ;

        directionInput = pInput.GroundControls.Movement.ReadValue<Vector2>()  ;

//      SPRINT
        float Sprint = 1 ;
        if(run && directionInput.y > 0.72f && !draged){
            
            count += Time.deltaTime ;

            if(count > 1.5f)
                Sprint = sprint ;
            else
                Sprint = 1 ;

            if(count > 6f)
                count = 0 ;
        }else
            count = 0 ;

        pAnim._isSprinting = Sprint > 1 ? true : false ;

//      MOVE
        move = grounded ? (new Vector3(directionInput.x, 0, directionInput.y).normalized * (run ? runSpeed : 1) * Sprint) : move ;
        move.y = yVelocity ;
        cController.Move(this.transform.TransformDirection(move  * speed * Time.deltaTime)) ;



    }

//  FUNCTIONS/METHODS
    void OnAttackInput(InputAction.CallbackContext callback)
    {

        Debug.Log("attack") ;
    }

    void OnJumpInput(InputAction.CallbackContext callback)
    {
        
        if(grounded)
            yVelocity = jumpSpeed ;
    }

//  COLLISIONS
    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Ground")){

            grounded = true;
            StartCoroutine(LandingWait()) ;
        }
        
    }
    
    void OnCollisionExit(Collision collision)
    {

        if(collision.gameObject.CompareTag("Ground"))
            grounded = false;
    }

//  COROUTINES
    IEnumerator LandingWait()
    {
        pInput.GroundControls.Disable() ;
        yield return new WaitForSeconds(0.5f) ;
        pInput.GroundControls.Enable() ;
    }
}
