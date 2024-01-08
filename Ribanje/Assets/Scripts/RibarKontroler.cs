using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class RibarKontroler : MonoBehaviour
{
    Rigidbody2D body;
    public Animator animator;
    private HealthController health;

    float horizontal;
    float vertical;
    Vector3 playerOrijentacija;

    public float brzinaKretanja;

    bool canMove = true;

    // flags to mark progress
    public bool bKeyItem1PickedUp = false;
    
    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        health = GetComponent<HealthController>();
    }

    // Update is called once per frame
    void Update()
    {

        checkAttack();

        if (canMove) {
            checkMovement();
        }

        testHealthController();

    }

    void FixedUpdate() {
        
        body.velocity = new Vector2(horizontal * brzinaKretanja, vertical * brzinaKretanja);

    }

    void checkAttack(){

        if (Input.GetKeyDown(KeyCode.X)){
            animator.SetBool("Attack", true);
            canMove = false;
            horizontal = 0;
            vertical = 0;
        }
        
        if (Input.GetKeyUp(KeyCode.X)){
            animator.SetBool("Attack", false);
            canMove = true;
        }
    }

    void checkMovement(){

        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical"); 
        playerOrijentacija = transform.localScale;

        if (horizontal < 0)
            playerOrijentacija.x = math.abs(transform.localScale.x); 
        if (horizontal > 0)
            playerOrijentacija.x = math.abs(transform.localScale.x) * -1;
        
        transform.localScale = playerOrijentacija;
        animator.SetFloat("Run", Mathf.Abs(horizontal));
        animator.SetFloat("RunUp", vertical);
    }

    public void gameOver() 
    {
        Debug.Log("GAME OVER!");
    }

    public void takeDamage(int amount)
    {
        health.deductHearts(amount);
        if (health.currentHealth <= 0)
        {
            gameOver();
        }
        StartCoroutine(health.Invulnerability());
    }

    public void heal(int amount)
    {
        health.addHearts(amount);
    }

    public void decreaseMaxHealth(int amount)
    {
        health.deductMaxHearts(amount);
    }

    public void increaseMaxHealth(int amount)
    {
        health.addMaxHearts(amount);
    }

    void testHealthController()
    {
        if (Input.GetKeyUp(KeyCode.O))
        {
            takeDamage(1);
        }

        if (Input.GetKeyUp(KeyCode.P))
        {
            heal(1);
        }

        if (Input.GetKeyUp(KeyCode.K))
        {
            decreaseMaxHealth(2);
        }

        if (Input.GetKeyUp(KeyCode.L))
        {
            increaseMaxHealth(1);
        }

    }

    public void SetMovement(bool canMove)
    {
        this.canMove = canMove;
    }

    public void PickUpKeyItem1()
    {
        bKeyItem1PickedUp = true;
    }

}
