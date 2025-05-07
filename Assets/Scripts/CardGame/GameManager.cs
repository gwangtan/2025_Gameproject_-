using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    public GameObject cardPrefab;
    public Sprite[] cardImages;
    public Transform deckArea;
    public Transform handArea;

    public Button drawButton;
    public TextMeshProUGUI deckCountText;

    public float cardSpacing = 2.0f;
    public int maxHandSize = 6;

    public GameObject[] deckCards;
    public int deckCount;

    public GameObject[] handCards;
    public int handCount;


    private int[] prefedinedDeck = new int[]
    {
        1,1,1,1,1,1,1,1,
        2,2,2,2,2,2,
        3,3,3,3,
        4,4,
    };

    public Transform mergeArea;
    public Button mergeButton;
    public int maxMergeSize = 3;

    public GameObject[] mergeCards;
    public int mergeCount;

    // Start is called before the first frame update
    void Start()
    {
        deckCards = new GameObject[prefedinedDeck.Length];
        handCards = new GameObject[maxHandSize];
        mergeCards = new GameObject[maxMergeSize];

        initializeDeck();
        ShuffleDeck();

        if (drawButton != null)
        {
            drawButton.onClick.AddListener(OnDrawButtomClicked);
        }

        if (mergeButton != null)
        {
            mergeButton.onClick.AddListener(OnMergeButtonClicked);
            mergeButton.interactable = false;
        }

    }

    void Update()
    {

    }

    void ShuffleDeck()
    {
        for (int i = 0; i < deckCount - 1; i++)
        {
            int j = Random.Range(i, deckCount);
            GameObject temp = deckCards[i];
            deckCards[i] = deckCards[j];
            deckCards[j] = temp;

        }
    }

    void initializeDeck()
    {
        deckCount = prefedinedDeck.Length;

        for (int i = 0; i < prefedinedDeck.Length; i++)
        {
            int value = prefedinedDeck[i];

            int imageIndex = value - 1;
            if (imageIndex >= cardImages.Length || imageIndex < 0)
            {
                imageIndex = 0;
            }

            GameObject newCardObj = Instantiate(cardPrefab, deckArea.position, Quaternion.identity);
            newCardObj.SetActive(false);

            Card cardComp = newCardObj.GetComponent<Card>();
            if (cardComp != null)
            {
                cardComp.IntCard(value, cardImages[imageIndex]);
            }
            deckCards[i] = newCardObj;
        }


    }
    public void ArrangeHand()
    {
        if (handCount == 0)
            return;

        float startX = -(handCount - 1) * cardSpacing / 2;

        for (int i = 0; i < handCount; i++)
        {
            if (handCards[i] != null)
            {
                Vector3 newPos = handArea.position + new Vector3(startX + i * cardSpacing, 0, -0.005f);
                handCards[i].transform.position = newPos;
            }
        }

    }

    void OnDrawButtomClicked()
    {
        DrawCardToHand();
    }

    public void DrawCardToHand()
    {
        if (handCount >= maxHandSize)
        {
            Debug.Log("���а� ���� á���ϴ�.");
            return;
        }
        if (deckCount <= 0)
        {
            Debug.Log("���� ���̻� ī�尡 �����ϴ�.");
            return;
        }
        GameObject drawnCard = deckCards[0];

        for (int i = 0; i < deckCount - 1; i++)
        {
            deckCards[i] = deckCards[i + 1];
        }
        deckCount--;

        drawnCard.SetActive(true);
        handCards[handCount] = drawnCard;
        handCount++;

        drawnCard.transform.SetParent(handArea);

        ArrangeHand();
    }

    void OnDrawButtonClicked()
    {
        DrawCardToHand();
    }

    void UpdateMergeButtonState()
    {
        if (mergeButton != null)
        {
            mergeButton.interactable = (mergeCount == 2 || mergeCount == 3);
        }
    }

    void MergeCards()
    {
        if (mergeCount != 2 && mergeCount != 3)
        {
            Debug.Log("������ �Ͻ÷��� ī�尡 2�� �Ǵ� 3�� �ʿ��մϴ�.");
            return;
        }

        int firstCard = mergeCards[0].GetComponent<Card>().cardValue;

        for (int i = 1; i < mergeCount - 1; i++)
        {
            Card card = mergeCards[0].GetComponent<Card>();
            if (card == null || card.cardValue != firstCard)
            {
                Debug.Log("���� ������ ī�常 �����Ҽ� �ֽ��ϴ�.");
                return;
            }
        }

        int newValue = firstCard + 1;

        if (newValue > cardImages.Length)
        {
            Debug.Log("ī�� �ִ밪�� ���⤶���ϴ�.");
            return;
        }

        for (int i = 0; i < mergeCount; i++)
        {
            if (mergeCards[i] != null)
            {
                mergeCards[i].SetActive(false);
            }
        }

        GameObject newCard = Instantiate(cardPrefab, mergeArea.position, Quaternion.identity);

        Card newCardTemp = newCard.GetComponent<Card>();
        if (newCardTemp != null)
        {
            int imageindex = newValue - 1;
        }

        for (int i = 0; i < maxMergeSize; i++)
        {
            mergeCards[i] = null;
        }
        mergeCount = 0;

        UpdateMergeButtonState();

        handCards[handCount] = newCard;
        handCount++;
        newCard.transform.SetParent(handArea);

        ArrangeHand();
    }

    void OnMergeButtonClicked()
    {
        MergeCards();
    }


    public void ArrangeMerge()
    {
        if (mergeCount == 0)
            return;

        float startX = -(mergeCount - 1) * cardSpacing / 2;

        for (int i = 0; i < mergeCount; i++)
        {
            if (mergeCards[i] != null)
            {
                Vector3 newPos = mergeArea.position + new Vector3(startX + i * cardSpacing, 0, -0.005f);
                mergeCards[i].transform.position = newPos;
            }
        }

    }

    public void MoveCardToMerge(GameObject card)
    {
        if (mergeCount >= maxMergeSize)
        {
            Debug.Log("���� ������ ���� á���ϴ�.");
            return;
        }

        for (int I = 0; I < handCount; I++)
        {
            if (handCards[1] == card)
            {
                for(int j = I; j < handCount -1; j++)
                {
                    handCards[j] = handCards [j + 1];
                }
                handCards[handCount - 1] = null;
                handCount--;

                ArrangeHand();
                break;
            }
        }

        mergeCards[mergeCount] = card;
        mergeCount++;

        card.transform.SetParent(mergeArea);
        ArrangeHand();
        UpdateMergeButtonState();
    }
}










