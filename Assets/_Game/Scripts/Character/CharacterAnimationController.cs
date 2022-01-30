using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimationKeys
{

    public const string yAxis = "yAxis" ;
    public const string xAxis = "xAxis" ;
    public const string HorizontalSpeed = "HorizontalSpeed" ;
    public const string isRunning = "isRunning" ;
    public const string isJumping = "isJumping" ;
    public const string isSprinting = "isSprinting" ;
    public const string jumpFact = "jumpFact" ;
}

[RequireComponent(typeof(Animator))]
public class CharacterAnimationController : MonoBehaviour
{

    private bool isRunning = false ;
    public bool _isRunning 
    {

        get {return isRunning;}
        set {
                /* Debug.Log(value); */
                isRunning = value ;
        }
    }

    private bool isJumping = false ;
    public bool _isJumping 
    {

        get {return isJumping;}
        set {
                /* Debug.Log(value); */
                isJumping = value ;
        }
    }

    private bool isSprinting = false ;
    public bool _isSprinting 
    {

        get {return isSprinting;}
        set {
                /* Debug.Log(value); */
                isSprinting = value ;
        }
    }

    MyCharacterController pController ; 
    Animator animator ;
    Rigidbody rb ;

    private void Awake() {
        
        rb = GetComponent<Rigidbody>() ;
        pController = GetComponent<MyCharacterController>() ;
        animator = GetComponent<Animator>() ;
    }

    void Update()
    {

        animator.SetBool(CharacterAnimationKeys.isRunning, this.isRunning) ;
        animator.SetBool(CharacterAnimationKeys.isJumping, this.isJumping) ;
        animator.SetBool(CharacterAnimationKeys.isSprinting, this.isSprinting) ;

        animator.SetFloat(CharacterAnimationKeys.yAxis, pController._directionInput.y) ;
        animator.SetFloat(CharacterAnimationKeys.xAxis, pController._directionInput.x) ;
        animator.SetFloat(CharacterAnimationKeys.jumpFact, pController._jumpFact) ;
    }
}
