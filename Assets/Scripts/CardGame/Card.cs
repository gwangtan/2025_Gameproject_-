using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Card : MonoBehaviour
{
    public int cardValue;
    public Sprite cardImage;
    public TextMeshPro cardText;

    //ī�� ���� �ʱ�ȭ �Լ�
    public void IntCard(int value, Sprite Image)
    {
        cardValue = value;
        cardImage = Image;

        GetComponent<SpriteRenderer>().sprite = Image;

        if (cardText != null)
        {
            cardText.text = cardValue.ToString();
        }
    }
}

