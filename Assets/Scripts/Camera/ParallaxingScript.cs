using UnityEngine;
using System.Collections;

public class ParallaxingScript : MonoBehaviour {

    public Vector2 speed;

    private Material mat;

    void Start()
    {
        mat = GetComponent<Renderer>().material;
        Vector2 offset = mat.mainTextureOffset;
        offset.x += Random.value;
        mat.mainTextureOffset = offset;
    }

    void Update()
    {
        Vector2 offset = mat.mainTextureOffset;
        offset += Time.deltaTime * speed;
        mat.mainTextureOffset = offset;
    }
}
