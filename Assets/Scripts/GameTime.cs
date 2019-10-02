/*  This script was created by:
 *  Umut Zenger
 */

using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Added to the text object on game HUD. Holds the game time and updates it on the UI.
/// </summary>

public class GameTime : MonoBehaviour
{
    #region Public Members
    public float count = 0, highScore;
    #endregion

    #region Private Members.
    private GameLogic _logic;
    #endregion

    #region Public Functions

    #endregion

    #region Private Functions
    private void Start()
    {
        _logic = FindObjectOfType<GameLogic>();
        highScore = PlayerPrefs.GetFloat("HS", 0);
    }
    private void Update()
    {
        if (_logic.gamePaused) return;
        count += Time.deltaTime;
        GetComponent<Text>().text = $"Best: {Mathf.Max(highScore, count).ToString("n2")}. Current: {count.ToString("n2")}";
    }
    #endregion
}
