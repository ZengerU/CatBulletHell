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
    [HideInInspector]
    public bool gamePaused = false;
    #endregion

    #region Private Members
    [SerializeField]
    private Animator _spriteAnim;
    [SerializeField]
    private Transform _playerChar, _meteorParent;
    [SerializeField]
    private GameObject _endGameCanvas;
    [SerializeField]
    [Range(0.5f, 5.0f)]
    private float _playerBaseSpeed = 1f;
    private float _activeCatSpeed = 1;
    private Vector3 _playerBaseScale;
    private GameTime _scoreCalc;
    #endregion

    #region Public Functions
    //Called via PlayerCollission script in the event of a collission.
    public void GameOver()
    {
        _playerChar.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
        GetComponent<MeteorSpawner>().enabled = false;
        foreach (Transform child in _meteorParent)
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
        _playerBaseScale = _playerChar.localScale;
        _scoreCalc = FindObjectOfType<GameTime>();
    }
    private void Update()
    {
        CheckInput();
    }

    private void CheckInput()
    {
        if (gamePaused) return;

        //Player Movement
        Vector3 tmp = Vector3.zero;
        tmp.x = Input.GetAxisRaw("Horizontal") * _playerBaseSpeed * _activeCatSpeed;
        tmp.y = Input.GetAxisRaw("Vertical") * _playerBaseSpeed * _activeCatSpeed;
        _playerChar.GetComponent<Rigidbody2D>().velocity = tmp;

        //Change Active Cat
        if (Input.GetButtonDown("Jump"))
        {
            GameObject currentCat, nextCat;
            currentCat = nextCat = _playerChar.GetChild(0).gameObject;
            foreach (Transform child in _playerChar)
            {
                if (child.gameObject.activeSelf)
                {
                    currentCat = child.gameObject;
                    nextCat = _playerChar.GetChild((currentCat.transform.GetSiblingIndex() + 1) % _playerChar.childCount).gameObject;
                    break;
                }
            }
            currentCat.SetActive(false);
            nextCat.SetActive(true);
            _playerChar.localScale = _playerBaseScale * nextCat.GetComponent<CatProperty>().size;
            _activeCatSpeed = nextCat.GetComponent<CatProperty>().speed;
            _spriteAnim.SetTrigger("next");
        }
    }

    //Collision occured, game ending.
    private void EndGame()
    {
        PlayerPrefs.SetFloat("HS", Mathf.Max(_scoreCalc.count, _scoreCalc.highScore));
        _endGameCanvas.SetActive(true);
        Text endText = _endGameCanvas.GetComponentInChildren<Text>();
        endText.text = InsertScore(endText.text);
    }
    //Enter score/highscore to gameend text block.
    private string InsertScore(string inp)
    {
        return inp.Replace("<scoretext>", _scoreCalc.count.ToString("n2")).Replace("<hscoretext>", PlayerPrefs.GetFloat("HS").ToString("n2"));
    }
    #endregion
}
