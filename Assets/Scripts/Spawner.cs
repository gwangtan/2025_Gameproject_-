using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{

    public GameObject coinPrefabs;
    public GameObject MissilePrefabs;

    [Header("스폰 타이밍 설정")]
    public float minSpawnInterval = 0.5f;
    public float maxSpawnInterval = 2.0f;


    [Header("동전 스폰 확률 설정")]
    [Range(0, 100)]
    public int coinSpwanChance = 50;

    public float timer = 0.0f;
    public float nextSpawnTime;

    // Start is called before the first frame update
    void Start()
    {
        SetNextSpawnTIme();
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= nextSpawnTime)
        {
            SpawnObject();
            timer = 0.0f;
            SetNextSpawnTIme();
        }
    }

    void SetNextSpawnTIme()
    {

        nextSpawnTime = Random.Range(minSpawnInterval, maxSpawnInterval);
    }

    void SpawnObject()
    {
        Transform spawnTransform = transform;


        int randomValue = Random.Range(0, 100);
        if (randomValue < coinSpwanChance)
        {
            Instantiate(coinPrefabs, spawnTransform.position, spawnTransform.rotation);
        }
        else
        {
            Instantiate(MissilePrefabs, spawnTransform.position, spawnTransform.rotation);

        }
    }
}
