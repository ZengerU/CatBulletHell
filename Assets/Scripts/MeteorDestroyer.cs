/*  This script was created by:
 *  Umut Zenger
 */

using UnityEngine;

/// <summary>
/// Added to the boundry below gameplay area. Acts as a garbage collector for meteors.
/// </summary>

public class MeteorDestroyer : MonoBehaviour
{
    #region Public Members

    #endregion

    #region Private Members

    #endregion

    #region Public Functions

    #endregion

    #region Private Functions
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Meteor")
            Destroy(other.gameObject);
    }
    #endregion
}
