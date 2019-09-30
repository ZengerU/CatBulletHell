/*  This script was created by:
 *  Umut Zenger
 */

using UnityEngine;

/// <summary>
/// Added to the explosion prefab, public function is triggered by animation event.
/// </summary>

public class SelfDestroy : MonoBehaviour
{
    #region Public Members

    #endregion

    #region Private Members

    #endregion

    #region Public Functions

    public void selfDestruct()
    {
        Destroy(gameObject);
    }

    #endregion

    #region Private Functions

    #endregion
}
