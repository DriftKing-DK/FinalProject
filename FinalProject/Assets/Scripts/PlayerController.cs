using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public float life;

    public Slider healthSlider;
    public Slider powerSlider;
    public ParticleSystem showPower;

    private Vector3 moveDirection = Vector3.zero;
    private CharacterController Cc;

    //jump
    private const int nbJumpAllowed = 2;
    private int nbJump;

    //power
    private float power;
    private bool usingPower;

    // Start is called before the first frame update
    void Start()
    {
        Cc = GetComponent<CharacterController>();
        nbJump = 0;

        healthSlider.minValue = 0;
        healthSlider.maxValue = life;
        healthSlider.value = life;

        showPower.Stop();
        power = 0;
        powerSlider.minValue = 0;
        powerSlider.maxValue = 10;
        usingPower = false;
    }

    // Update is called once per frame
    void Update()
    {
        ApplyMove(allowFlyMove:true);
        ApplyGravity();
        ApplyJump();
        ApplyPower();
        Cc.Move(moveDirection * Time.deltaTime);
    }

    public void Respawn(){
        life = healthSlider.maxValue;
        healthSlider.value = life;
        power = 0;
        powerSlider.value = power;
    }

    void ApplyPower(){
        if (power > 10){
            power = 10;
        } else if (power < 0){
            power = 0;
        }
        if (Input.GetKey(KeyCode.U) && power == 10){
            usingPower = true;
            life += 20;
            if (life > healthSlider.maxValue){
                life = healthSlider.maxValue;
            }
            healthSlider.value = life;
            showPower.Play();
        } else if (usingPower && power <= 0){
            usingPower = false;
            showPower.Stop();
        }
        if (usingPower){
            power -= Time.deltaTime;
        }
        
        powerSlider.value = power;
    }

    void ApplyMove(bool allowFlyMove = false, float speed = 10f)
    {
        Vector3 vect = moveDirection;
        if (allowFlyMove || Cc.isGrounded)
        {
            vect = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            vect = transform.TransformDirection(vect);
            vect *= speed;
        }
        moveDirection.x = vect.x;
        moveDirection.z = vect.z;
    }

    void ApplyGravity(float gravity = 20f)
    {
        moveDirection.y -= gravity * Time.deltaTime;
    }

    void ApplyJump(float speedJump = 15f)
    {
        if (Input.GetButtonDown("Jump"))
        {
            if (nbJump < nbJumpAllowed || (usingPower && nbJump <= nbJumpAllowed)) // extra jump with power
            {
                moveDirection.y = speedJump;
                nbJump += 1;
            }
        }
        else if (Cc.isGrounded)
        {
            nbJump = 0;
        }
    }

    public void TakeDamage(float damage)
    {
        if (usingPower == false){ // invisible when using power
            life -= damage;
        }
        if (life < 0){
            life = 0;
        }
            
        healthSlider.value = life;
    }

    public void IncreasePower()
    {
        if (power < 10 && usingPower == false){
            power++;
        }
    }

    public Vector3 getPlayerPosition(){
        return Cc.transform.position;
    }
}
