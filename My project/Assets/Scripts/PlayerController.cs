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

    //camera
    private float yaw;
    private float pitch;

    // Start is called before the first frame update
    void Start()
    {
        Cc = GetComponent<CharacterController>();
        nbJump = 0;
        yaw = 0f;
        pitch = 0f;

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
        ApplyCamera();
        ApplyPower();
        Cc.Move(moveDirection * Time.deltaTime);
    }

    void ApplyPower(){
        if (power > 10){
            power = 10;
        } else if (power < 0){
            power = 0;
        }
        if (Input.GetKey(KeyCode.U) && power == 10){
            usingPower = true;
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

    void ApplyMove(bool allowFlyMove = false, float speed = 6f)
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

    void ApplyJump(float speedJump = 8f)
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

    void ApplyCamera(float speedCamera = 12f)
    {
        if (Input.GetMouseButton(0) || true)
        {
            yaw += speedCamera * Input.GetAxis("Mouse X");
            pitch -= speedCamera * Input.GetAxis("Mouse Y");

            transform.eulerAngles = new Vector3(pitch, yaw, 0);
        }
    }

    public void TakeDamage(float damage)
    {
        if (usingPower == false){ // invisible when using power
            life -= damage;
            healthSlider.value = life;
        }
    }

    public void IncreasePower()
    {
        if (power < 10 && usingPower == false){
            power++;
        }
    }
}
