/*  This script was created by:
 *  Umut Zenger
 */

using UnityEngine;

/// <summary>
/// Added to endgame canvas.
/// </summary>

public class ReplayListener : MonoBehaviour
{
    #region Public Members

    #endregion

    #region Private Members

    #endregion

    #region Public Functions

    #endregion

    #region Private Functions
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            FindObjectOfType<GameLogic>().RestartGame(); //Calling a function in Game Logic in case of future change.
        }
    }
    #endregion
}
