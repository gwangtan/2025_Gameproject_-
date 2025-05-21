using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitGame : MonoBehaviour
{

    public GameObject[] fruitPrefabs;   //,과일 프리팹 선언

    public float[] fruitSize = { 0.5f, 0.7f, 0.9f, 1.1f, 1.3f, 1.5f, 1.7f, 1.9f }; //과일 크기 선언

    public GameObject currentFruit; //현재 들고있는 과일
    public int currentFruitType;

    public float fruitStartHeights = 6.0f; //과일 시작시 높이 설정
    public float gameWidth = 5.0f;      //게임판 넓이
    public bool isGameOver = false;      //게임 상태
    public Camera mainCamera;            //카메라 참조 (마우스 위치 변환 필요)
    public float fruitTimer;

    public float gameHeight;

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
        SpawnNewFruit();
        gameHeight = fruitStartHeights + 0.5f;
        fruitTimer = -3.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (isGameOver) return;

        if (fruitTimer >= 0)
        {
            fruitTimer -= Time.deltaTime;
        }


        if (fruitTimer < 0 && fruitTimer > -2)
        {
            CheckgameOver();
            SpawnNewFruit();
            fruitTimer = -3.0f;

        }
        if (currentFruit != null)
        {
            Vector3 mousePosition = Input.mousePosition;
            Vector3 worldPosition = mainCamera.ScreenToWorldPoint(mousePosition);

            Vector3 newPosition = currentFruit.transform.position;
            newPosition.x = worldPosition.x;

            float halfFruitSize = fruitSize[currentFruitType] / 2f;
            if (newPosition.x < -gameWidth / 2 + halfFruitSize)
            {
                newPosition.x = -gameWidth / 2 + halfFruitSize;
            }
            if (newPosition.x > gameWidth / 2 + halfFruitSize)
            {
                newPosition.x = gameWidth / 2 + halfFruitSize;
            }

            currentFruit.transform.position = newPosition;
        }

        if (Input.GetMouseButtonDown(0) && fruitTimer == -3.0f)
        {
            DropFruit();
        }

    }


    void SpawnNewFruit()
    {
        if (!isGameOver)
        {
            currentFruitType = Random.Range(0, 3);


            Vector3 mousePosition = Input.mousePosition;
            Vector3 worldPosition = mainCamera.ScreenToWorldPoint(mousePosition);

            Vector3 spawnPosition = new Vector3(worldPosition.x, fruitStartHeights, 0);

            float halfFruitsize = fruitSize[currentFruitType] / 2;

            spawnPosition.x = Mathf.Clamp(spawnPosition.x, gameWidth / 2 + halfFruitsize, gameWidth / 2 - halfFruitsize);

            currentFruit = Instantiate(fruitPrefabs[currentFruitType]);
            currentFruit.transform.localScale = new Vector3(fruitSize[currentFruitType], fruitSize[currentFruitType], 1);

            Rigidbody2D rb = currentFruit.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.gravityScale = 0f;
            }
        }
    }

    void DropFruit()
    {
        Rigidbody2D rb = currentFruit.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.gravityScale = 1f;
            currentFruit = null;
            fruitTimer = 1.0f;
        }
    }


    public void MergeFruit(int fruitType, Vector3 position)
    {
        if (fruitType < fruitPrefabs.Length - 1)
        {
            GameObject newFruit = Instantiate(fruitPrefabs[fruitType + 1], position, Quaternion.identity);
            newFruit.transform.localScale = new Vector3(fruitSize[fruitType + 1], fruitSize[fruitType + 1], 1.0f);
        }
    }

    public void CheckgameOver()
    {
        Fruit[] allFruits = FindObjectsOfType<Fruit>();

        float gameOverHeight = gameHeight;

        for (int i = 0; i < allFruits.Length; i++)
        {
            if (allFruits[i] != null)
            {
                Rigidbody2D rb = allFruits[i].GetComponent<Rigidbody2D>();

                if (rb != null && rb.velocity.magnitude < 0.1f && allFruits[i].transform.position.y > gameOverHeight)
                {
                    isGameOver = true;
                    Debug.Log("게임 오버");

                    break;
                }
            }
        }
    }
}

                
