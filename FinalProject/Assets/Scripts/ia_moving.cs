using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ia_moving : MonoBehaviour
{
    // Start is called before the first frame update

    public PlayerController playerController;
    public float speed;
    float x = 1;
    float z = 1;

    private CharacterController Cc;

    void Start(){
        Cc = GetComponent<CharacterController>();
    }


    // Update is called once per frame
    void Update()
    {
        Vector3 move = transform.right * x + transform.forward * z;
        Vector3 new_pos = Vector3.MoveTowards(Cc.transform.position, playerController.getPlayerPosition(), Time.deltaTime);
        new_pos.y = 0;
        Cc.transform.position = new_pos;
    }
}


