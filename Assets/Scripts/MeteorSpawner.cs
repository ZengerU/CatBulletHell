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

    #endregion

    #region Private Members
    
    [SerializeField]
    private Renderer _meteorSpawnBoundaryBox;
    [SerializeField]
    private Transform _meteorParent;
    [SerializeField]
    private List<GameObject> _meteorTypeList = new List<GameObject>();
    [SerializeField]
    [Range(0.5f, 5.0f)]
    private float _meteorSpeed = 1f, _meteorSpawnInterval = 1;
    [SerializeField]
    [Range(3.0f, 15.0f)]
    private float _meteorMaxSpeed = 4;
    [SerializeField]
    [Tooltip("The amount meteor speed gets multiplied with on each step.")]
    [Range(1.0f + float.Epsilon, 2.0f)]
    private float _speedStepMultiplier = 1.2f;
    [SerializeField]
    [Tooltip("Interval inbetween two steps.")]
    [Range(1.0f, 30.0f)]
    private float _speedStepInterval = 10;

    private float _spawnAreaLeftLimit, _meteorSpawnCountdown = 0, _speedCountdown = 0;

    #endregion

    #region Public Functions

    #endregion

    #region Private Functions
    private void Start()
    {
        _spawnAreaLeftLimit = _meteorSpawnBoundaryBox.bounds.center.x - (_meteorSpawnBoundaryBox.bounds.size.x / 2);
    }
    private void FixedUpdate()
    {
        _meteorSpawnCountdown -= Time.deltaTime;
        _speedCountdown -= Time.deltaTime;
        if (_speedCountdown <= 0)
            UpdateSpeedMultiplier();
        if (_meteorSpawnCountdown <= 0)
            SpawnMeteor();
    }
    //Spawn a random meteor on a random area within boundaries, reset timer.
    private void SpawnMeteor()
    {
        Vector3 target_position = new Vector3(_spawnAreaLeftLimit + (Random.value * _meteorSpawnBoundaryBox.bounds.size.x), _meteorSpawnBoundaryBox.bounds.center.y, _meteorSpawnBoundaryBox.bounds.center.z);
        int meteorType = Random.Range(0, _meteorTypeList.Count);

        //print($"Spawning meteor type {meteorType} at position {target_position}");

        GameObject tmp = Instantiate(_meteorTypeList[meteorType], target_position, Quaternion.identity, _meteorParent);
        tmp.GetComponent<Rigidbody2D>().velocity = new Vector3(0, _meteorSpeed * -1, 0);

        _meteorSpawnCountdown = _meteorSpawnInterval;
    }
    //Update simulation speed multiplier, reset timer.
    private void UpdateSpeedMultiplier()
    {
        if (_meteorSpeed == _meteorMaxSpeed)
        {
            _speedCountdown = float.MaxValue;
            return;
        }
        _meteorSpeed *= _speedStepMultiplier;
        _meteorSpeed = Mathf.Min(_meteorMaxSpeed, _meteorSpeed);
        print($"Updating speed to: {_meteorSpeed}.");
        foreach (Transform child in _meteorParent)
        {
            child.GetComponent<Rigidbody2D>().velocity = new Vector3(0, _meteorSpeed * -1, 0);
        }
        _speedCountdown = _speedStepInterval;
    }
    #endregion
}
