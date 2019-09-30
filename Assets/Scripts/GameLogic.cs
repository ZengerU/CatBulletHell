/*  This script was created by:
 *  Umut Zenger
 */

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Script responsible with input management and game flow.
/// </summary>

public class GameLogic : MonoBehaviour
{
    #region Public Members
    public Animator spriteAnim;
    public Transform playerChar, meteorParent;
    public GameObject endGameCanvas;
    [Range(0.5f, 5.0f)]
    public float playerBaseSpeed = 1f;
    [HideInInspector]
    public bool gamePaused = false;
    #endregion

    #region Private Members
    float activeCatSpeed = 1;
    Vector3 playerBaseScale;
    GameTime scoreCalc;
    #endregion

    #region Public Functions
    //Called via PlayerCollission script in the event of a collission.
    public void GameOver()
    {
        playerChar.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
        GetComponent<MeteorSpawner>().enabled = false;
        foreach (Transform child in meteorParent)
        {
            child.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
        }
        gamePaused = true;
        Invoke("EndGame", 2);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    #endregion

    #region Private Functions
    private void Start()
    {
        playerBaseScale = playerChar.localScale;
        scoreCalc = FindObjectOfType<GameTime>();
    }
    private void Update()
    {
        CheckInput();
    }

    void CheckInput()
    {
        if (gamePaused) return;

        //Player Movement
        Vector3 tmp = Vector3.zero;
        tmp.x = Input.GetAxisRaw("Horizontal") * playerBaseSpeed * activeCatSpeed;
        tmp.y = Input.GetAxisRaw("Vertical") * playerBaseSpeed * activeCatSpeed;
        playerChar.GetComponent<Rigidbody2D>().velocity = tmp;

        //Change Active Cat
        if (Input.GetButtonDown("Jump"))
        {
            GameObject currentCat, nextCat;
            currentCat = nextCat = playerChar.GetChild(0).gameObject;
            foreach (Transform child in playerChar)
            {
                if (child.gameObject.activeSelf)
                {
                    currentCat = child.gameObject;
                    nextCat = playerChar.GetChild((currentCat.transform.GetSiblingIndex() + 1) % playerChar.childCount).gameObject;
                    break;
                }
            }
            currentCat.SetActive(false);
            nextCat.SetActive(true);
            playerChar.localScale = playerBaseScale * nextCat.GetComponent<CatProperty>().size;
            activeCatSpeed = nextCat.GetComponent<CatProperty>().speed;
            spriteAnim.SetTrigger("next");
        }
    }

    //Collision occured, game ending.
    void EndGame()
    {
        PlayerPrefs.SetFloat("HS", Mathf.Max(scoreCalc.count, scoreCalc.highScore));
        endGameCanvas.SetActive(true);
        Text endText = endGameCanvas.GetComponentInChildren<Text>();
        endText.text = InsertScore(endText.text);
    }
    //Enter score/highscore to gameend text block.
    string InsertScore(string inp)
    {
        return inp.Replace("<scoretext>", scoreCalc.count.ToString("n2")).Replace("<hscoretext>", PlayerPrefs.GetFloat("HS").ToString("n2"));
    }
    #endregion
}
