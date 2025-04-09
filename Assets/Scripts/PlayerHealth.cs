using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public int maxLives = 3;
    public int currentLives;

    public float invincibleTime = 1.0f;
    public bool isinvincible = false;

    // Start is called before the first frame update
    void Start()
    {
        currentLives = maxLives;
    }

    // Update is called once per frame
    private void OTtriggerEnter(Collider other)
    {
        if (other . CompareTag("MIssile"))
        {
            currentLives--;
            Destroy(other .gameObject);
        }
    }

    void GameOver()
    {
        gameObject.SetActive(false);
        Invoke("RestartGame", 3.0f);
    }
    void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene(). name);
    }
}
