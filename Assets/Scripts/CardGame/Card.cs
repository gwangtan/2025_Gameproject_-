using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Card : MonoBehaviour
{
    public int cardValue;
    public Sprite cardImage;
    public TextMeshPro cardText;

    //카드 정보 초기화 함수
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

