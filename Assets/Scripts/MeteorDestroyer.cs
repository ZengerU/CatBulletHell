/*  This script was created by:
 *  Umut Zenger
 */

using System;
using UnityEngine;

/// <summary>
/// Added to the boundry below gameplay area. Acts as a garbage collector for meteors.
/// </summary>

public class MeteorDestroyer : MonoBehaviour
{
    private MeteorSpawner _spawner;

    private void Awake()
    {
        _spawner = FindObjectOfType<MeteorSpawner>();
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Meteor"))
            _spawner.SendObjectToPool(other.gameObject);
    }
}
