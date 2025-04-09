using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;

public class CubeManager : MonoBehaviour
{
    public CubeGenerater[] generatedCubes = new CubeGenerater[5];
    // Start is called before the first frame update

    public float timer = 0f;
    public float interval = 3f;


    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= interval)
        {
            RandomizeCubeActivation();
            timer = 0f;
        }
    }

    public void RandomizeCubeActivation()
    {
        for (int i = 0; i < generatedCubes.Length; i++)
        {
            int randomNum = Random.Range(0, 2);
            if(randomNum == 1)
            {
                generatedCubes[i].GenCube();
            }

        }
    }
}
