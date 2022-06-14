using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    public float health = 50f;
    private Vector3 mem_pos;
    private Quaternion mem_rot;

    public PlayerController player;

    public void Start()
    {
        mem_pos = transform.position;
        mem_rot = transform.rotation;
    }

    public void TakeDamage(float ammount)
    {
        health -= ammount;
        if (health <= 0f)
        {
            Die();
        }
    }

    private void Die()
    {
        transform.position = mem_pos;
        transform.rotation = mem_rot;
        health = 50f;
        player.IncreasePower();
    }
}
