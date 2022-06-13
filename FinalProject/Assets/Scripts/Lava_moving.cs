using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lava_moving : MonoBehaviour
{
    // Start is called before the first frame update
    public float delayBetweenFrames;

    private Renderer rend;
    private Material[] mat;  //Just in case you have more than one materials on a Renderer.
    private float timeOfLastTextureShift;

    void Start()
    {
        rend = GetComponent<Renderer>();
        mat = rend.materials;
    }

    void Update()
    {
        float timePassed = Time.timeSinceLevelLoad - timeOfLastTextureShift;
        if (timePassed < delayBetweenFrames) return;


        Vector2 newOffset = new Vector2(mat[0].GetTextureOffset("_BaseMap").x + 0.1f, 0);
        rend.materials[0].SetTextureOffset("_BaseMap", newOffset);
        timeOfLastTextureShift = Time.timeSinceLevelLoad;
    }
}
