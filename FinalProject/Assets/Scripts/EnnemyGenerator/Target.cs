using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    private float speed = 5f;
    private float canAttack;
    private float health = 50f;
    private float damage = 10f;
    private float distanceDamage;

    private int reference;
    private bool alive;

    private PlayerController player;
    private CharacterController Cc;

    void Start()
    {
        Cc = GetComponent<CharacterController>();
        canAttack = 0f;
    }

    void Update()
    {
        Vector3 new_pos = Vector3.MoveTowards(Cc.transform.position, player.getPlayerPosition(), Time.deltaTime * speed);
        transform.LookAt(player.getPlayerPosition());

        Cc.transform.position = new_pos;
        Attack();

    }

    public void Setup(float start_health, float start_damage, float distance, int bot_number, PlayerController player_to_attack){
        health = start_health;
        damage = start_damage;
        distanceDamage = distance;
        player = player_to_attack;
        reference = bot_number;
        gameObject.SetActive(true);
        alive = true;
    }

    public void TakeDamage(float ammount)
    {
        health -= ammount;
        if (health <= 0f)
        {
            Die();
        }
    }

    public float getHealth(){
        return health;
    }
     public float getDamage(){
        return damage;
     }

    public void Attack(){
        if (Vector3.Distance(Cc.transform.position, player.getPlayerPosition()) <= distanceDamage){
            if (canAttack <= 0f){
                player.TakeDamage(damage);
                canAttack = 1.5f;
            }
        }
        canAttack -= Time.deltaTime;
        if (canAttack < 0f){
            canAttack = 0f;
        }
    }

    public bool IsAlive(){
        return alive;
    }

    private void Die()
    {
        player.IncreasePower();
        alive = false;
        Destroy(gameObject);
    }
}
