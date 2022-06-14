using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ia_moving : MonoBehaviour
{
    // Start is called before the first frame update

    public CharacterController enemyController;
    public CharacterController playerController;
    public float speed;
    float x = 1;
    float z = 1;

    // Update is called once per frame
    void Update()
    {
        Vector3 move = transform.right * x + transform.forward * z;
        enemyController.transform.position = Vector3.MoveTowards(enemyController.transform.position, playerController.transform.position, Time.deltaTime);
    }
}


