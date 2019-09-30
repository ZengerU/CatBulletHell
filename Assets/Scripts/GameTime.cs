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
    GameLogic logic;
    #endregion

    #region Public Functions

    #endregion

    #region Private Functions
    private void Start()
    {
        logic = FindObjectOfType<GameLogic>();
        highScore = PlayerPrefs.GetFloat("HS", 0);
    }
    private void Update()
    {
        if (logic.gamePaused) return;
        count += Time.deltaTime;
        GetComponent<Text>().text = $"Best: {Mathf.Max(highScore, count).ToString("n2")}. Current: {count.ToString("n2")}";
    }
    #endregion
}
