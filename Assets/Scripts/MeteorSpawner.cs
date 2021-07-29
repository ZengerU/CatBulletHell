/*  This script was created by:
 *  Umut Zenger
 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

/// <summary>
/// Script responsible for creating and managing meteors.
/// </summary>

public class MeteorSpawner : MonoBehaviour
{
    #region Public Members

    #endregion

    #region Private Members
    
    [SerializeField]
    private Renderer meteorSpawnBoundaryBox;
    [SerializeField]
    private Transform meteorParent, meteorPool;
    [SerializeField]
    private List<Rigidbody2D> meteorTypeList = new List<Rigidbody2D>();
    [SerializeField]
    [Range(0.5f, 5.0f)]
    private float meteorSpeed = 1f, meteorSpawnInterval = 1;
    [SerializeField]
    [Range(3.0f, 15.0f)]
    private float meteorMaxSpeed = 4;
    [SerializeField]
    [Tooltip("The amount meteor speed gets multiplied with on each step.")]
    [Range(1.0f + float.Epsilon, 2.0f)]
    private float speedStepMultiplier = 1.2f;
    [SerializeField]
    [Tooltip("Interval inbetween two steps.")]
    [Range(1.0f, 30.0f)]
    private float speedStepInterval = 10;
    [SerializeField]
    [Tooltip("Amount of objects to spawn at each pool at start.")]
    [Range(5, 30)]
    private int poolInitialCount = 20;
    

    private float _spawnAreaLeftLimit, _meteorSpawnCountdown, _speedCountdown;

    private List<Queue<Rigidbody2D>> _meteorPool = new List<Queue<Rigidbody2D>>();

    #endregion

    #region Public Functions

    #endregion

    #region Private Functions

    private void Awake()
    {
        foreach (Rigidbody2D o in meteorTypeList)
        {
            _meteorPool.Add(new Queue<Rigidbody2D>());
        }
        for (int i = 0; i < poolInitialCount; i++)
        {
            for(int j = 0; j < meteorTypeList.Count; j++)
            {
                _meteorPool[j].Enqueue(Instantiate(meteorTypeList[j],new Vector3(-100, 100, 0), Quaternion.identity, meteorPool));
            }
        }
    }

    private void Start()
    {
        _spawnAreaLeftLimit = meteorSpawnBoundaryBox.bounds.center.x - (meteorSpawnBoundaryBox.bounds.size.x / 2);
    }
    private void FixedUpdate()
    {
        _meteorSpawnCountdown -= Time.fixedDeltaTime;
        _speedCountdown -= Time.fixedDeltaTime;
        if (_speedCountdown <= 0)
            UpdateSpeedMultiplier();
        if (_meteorSpawnCountdown <= 0)
            SpawnMeteor();
    }
    //Spawn a random meteor on a random area within boundaries, reset timer.
    private void SpawnMeteor()
    {
        Vector3 target_position = new Vector3(_spawnAreaLeftLimit + (Random.value * meteorSpawnBoundaryBox.bounds.size.x), meteorSpawnBoundaryBox.bounds.center.y, meteorSpawnBoundaryBox.bounds.center.z);
        int meteorType = Random.Range(0, meteorTypeList.Count);
        Queue<Rigidbody2D> queue = _meteorPool[meteorType];
        Rigidbody2D rb;
        if (queue.Count == 0)
        {
            rb = Instantiate(meteorTypeList[meteorType], target_position, Quaternion.identity, meteorParent);
        }
        else
        {
            rb = queue.Dequeue();
        }

        rb.transform.position = target_position;
        rb.transform.parent = meteorParent;
        rb.velocity = new Vector3(0, meteorSpeed * -1, 0);

        _meteorSpawnCountdown = meteorSpawnInterval;
    }
    //Update simulation speed multiplier, reset timer.
    private void UpdateSpeedMultiplier()
    {
        if (meteorSpeed == meteorMaxSpeed)
        {
            _speedCountdown = float.MaxValue;
            return;
        }

        meteorSpawnInterval *= .999f;
        meteorSpeed *= speedStepMultiplier;
        meteorSpeed = Mathf.Min(meteorMaxSpeed, meteorSpeed);
        foreach (Rigidbody2D child in meteorParent.GetComponentsInChildren<Rigidbody2D>())
        {
            child.velocity = new Vector3(0, meteorSpeed * -1, 0);
        }
        _speedCountdown = speedStepInterval;
        print("here");
    }

    public void SendObjectToPool(GameObject meteor)
    {
        foreach (Rigidbody2D rb in meteorTypeList)
        {
            if (meteor.name.Contains(rb.name))
            {
                Rigidbody2D rbToSend = meteor.GetComponent<Rigidbody2D>(); 
                rbToSend.velocity = Vector2.zero;
                meteor.transform.parent = meteorPool;
                _meteorPool[meteorTypeList.IndexOf(rb)].Enqueue(rbToSend);
            }
        }
    }
    #endregion
}
