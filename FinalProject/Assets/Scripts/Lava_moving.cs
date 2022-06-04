using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lava_moving : MonoBehaviour
{
    // Scroll main texture based on time

    float scrollSpeed = 0.1f;
    float offset = 0f;
    Renderer rend;

    void Start()
    {
        rend = GetComponent<Renderer>();
    }

    void Update()
    {
        offset += Time.deltaTime * scrollSpeed;
        offset = offset % 1;
        rend.material.SetTextureOffset("_MainTex", new Vector2(-offset, 0));
    }
}
