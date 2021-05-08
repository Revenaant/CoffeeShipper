using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomizeMaterial : MonoBehaviour
{
    [SerializeField]
    private Material[] materials;

    [SerializeField]
    private Renderer renderer;
    
    // Start is called before the first frame update
    void Start()
    {
        renderer.material = materials[Random.Range(0, materials.Length - 1)];
    }
}
