using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Fruit : MonoBehaviour
{

    public int fruitType;

    public bool hasMered = false;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (hasMered)   //이미 합쳐진 과일은 무시
            return;


        Fruit otherFruit = collision .gameObject . GetComponent<Fruit>();

        if (otherFruit != null && !otherFruit . hasMered && otherFruit .fruitType==fruitType)
        {
            hasMered = true;
            otherFruit.hasMered = true;

            Vector3 mergePosition = (transform.position + otherFruit.transform.position) / 2;  //충돌한것이 과일이고 타일과 같다면


            //게임 매니저에서 Merge구현된것을 호출

            FruitGame gameManager = FindObjectOfType<FruitGame>();
            {
               gameManager.MergeFruit(fruitType, mergePosition);
            }

            //과일들 제거
            Destroy (otherFruit.gameObject);
            Destroy(gameObject);

        }
    }



}
