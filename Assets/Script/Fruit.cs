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
        if (hasMered)   //�̹� ������ ������ ����
            return;


        Fruit otherFruit = collision .gameObject . GetComponent<Fruit>();

        if (otherFruit != null && !otherFruit . hasMered && otherFruit .fruitType==fruitType)
        {
            hasMered = true;
            otherFruit.hasMered = true;

            Vector3 mergePosition = (transform.position + otherFruit.transform.position) / 2;  //�浹�Ѱ��� �����̰� Ÿ�ϰ� ���ٸ�


            //���� �Ŵ������� Merge�����Ȱ��� ȣ��

            FruitGame gameManager = FindObjectOfType<FruitGame>();
            {
               gameManager.MergeFruit(fruitType, mergePosition);
            }

            //���ϵ� ����
            Destroy (otherFruit.gameObject);
            Destroy(gameObject);

        }
    }



}
