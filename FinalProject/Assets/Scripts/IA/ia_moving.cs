using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ia_moving : MonoBehaviour
{
    // Start is called before the first frame update

    public CharacterController controller;
    public float speed;
    float x = 0;
    float z = 0;

    // Update is called once per frame
    void Update()
    {
        if (Time.frameCount % 31 * Time.deltaTime == 0)
        {
            x = Random.Range(-1f, 1f);
            z = Random.Range(-1f, 1f);
        }

        Debug.Log("x : " + x);
        Debug.Log("z : " + z);

        Vector3 move = transform.right * x + transform.forward * z;

        controller.Move(move * speed * Time.deltaTime);
    }
}


