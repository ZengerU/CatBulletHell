/*  This script was created by:
 *  Umut Zenger
 */

using UnityEngine;

/// <summary>
/// Added to player character object.
/// </summary>

public class PlayerCollission : MonoBehaviour
{
    #region Public Members


    #endregion

    #region Private Members

    [SerializeField]
    private GameObject _explosionPrefab;

    #endregion

    #region Public Functions

    #endregion

    #region Private Functions
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Meteor")
        {
            print("Collision occured, ending game.");
            Vector3 explosionPos = collision.GetComponent<Collider2D>().ClosestPoint(transform.position);
            Instantiate(_explosionPrefab, explosionPos, Quaternion.identity);
            FindObjectOfType<GameLogic>().GameOver();
        }
    }
    #endregion
}
