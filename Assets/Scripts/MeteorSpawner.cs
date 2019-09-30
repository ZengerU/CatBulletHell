/*  This script was created by:
 *  Umut Zenger
 */

using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script responsible for creating and managing meteors.
/// </summary>

public class MeteorSpawner : MonoBehaviour
{
    #region Public Members
    public Renderer meteorSpawnBoundaryBox;
    public Transform meteorParent;
    public List<GameObject> meteorTypeList = new List<GameObject>();
    [Range(0.5f, 5.0f)]
    public float meteorSpeed = 1f, meteorSpawnInterval = 1;
    [Range(3.0f, 15.0f)]
    public float meteorMaxSpeed = 4;
    [Tooltip("The amount meteor speed gets multiplied with on each step.")]
    [Range(1.0f + float.Epsilon, 2.0f)]
    public float speedStepMulti = 1.2f;
    [Tooltip("Interval inbetween two steps.")]
    [Range(1.0f, 30.0f)]
    public float simSpeedUpdateInterval = 10;
    #endregion

    #region Private Members

    float spawnAreaLeftLimit, spawnAreaRightLimit;
    float meteorSpawnCountdown = 0;
    float speedCountdown = 0;

    #endregion

    #region Public Functions

    #endregion

    #region Private Functions
    void Start()
    {
        spawnAreaLeftLimit = meteorSpawnBoundaryBox.bounds.center.x - (meteorSpawnBoundaryBox.bounds.size.x / 2);
    }
    void FixedUpdate()
    {
        meteorSpawnCountdown -= Time.deltaTime;
        speedCountdown -= Time.deltaTime;
        if (speedCountdown <= 0)
            updateSpeedMultiplier();
        if (meteorSpawnCountdown <= 0)
            spawnMeteor();
    }
    //Spawn a random meteor on a random area within boundaries, reset timer.
    void spawnMeteor()
    {
        Vector3 target_position = new Vector3(spawnAreaLeftLimit + (Random.value * meteorSpawnBoundaryBox.bounds.size.x), meteorSpawnBoundaryBox.bounds.center.y, meteorSpawnBoundaryBox.bounds.center.z);
        int meteorType = Random.Range(0, meteorTypeList.Count);

        //print($"Spawning meteor type {meteorType} at position {target_position}");

        GameObject tmp = Instantiate(meteorTypeList[meteorType], target_position, Quaternion.identity, meteorParent);
        tmp.GetComponent<Rigidbody2D>().velocity = new Vector3(0, meteorSpeed * -1, 0);

        meteorSpawnCountdown = meteorSpawnInterval;
    }
    //Update simulation speed multiplier, reset timer.
    void updateSpeedMultiplier()
    {
        if (meteorSpeed == meteorMaxSpeed)
        {
            speedCountdown = float.MaxValue;
            return;
        }
        meteorSpeed *= speedStepMulti;
        meteorSpeed = Mathf.Min(meteorMaxSpeed, meteorSpeed);
        print($"Updating speed to: {meteorSpeed}.");
        foreach (Transform child in meteorParent)
        {
            child.GetComponent<Rigidbody2D>().velocity = new Vector3(0, meteorSpeed * -1, 0);
        }
        speedCountdown = simSpeedUpdateInterval;
    }
    #endregion
}
