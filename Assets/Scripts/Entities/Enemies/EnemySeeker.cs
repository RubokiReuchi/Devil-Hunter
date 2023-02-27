using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemySeeker : MonoBehaviour
{
    bool hunting = false;
    [HideInInspector] public Vector3 targetPosition;
    public Vector3[] loopingArrayOffset;
    List<Vector3> loopingArray = new();
    int currentLoopPoint = 0;
    Enemy enemy;
    public float nextWayPointDistance;

    Path path;
    int currentWaypoint = 0;

    Seeker seeker;
    Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        enemy = GetComponent<Enemy>();
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();

        SetLoopPath();
        targetPosition = loopingArray[currentLoopPoint];

        InvokeRepeating("UpdatePath", 0, 0.5f);
    }

    void UpdatePath()
    {
        if (seeker.IsDone()) seeker.StartPath(rb.position, targetPosition, OnPathComplete);
    }

    void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }

    void FixedUpdate()
    {
        if (path == null) return;

        if (currentWaypoint >= path.vectorPath.Count)
        {
            return;
        }

        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
        Vector3 force = direction * enemy.speed * Time.deltaTime;

        transform.position += force;

        if (Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]) < nextWayPointDistance)
        {
            currentWaypoint++;
        }

        if (!hunting  && Vector2.Distance(rb.position, loopingArray[currentLoopPoint]) < 0.5f)
        {
            currentLoopPoint++;
            if (currentLoopPoint == loopingArray.Count) currentLoopPoint = 0;
            targetPosition = loopingArray[currentLoopPoint];
        }

        if (targetPosition.x - rb.position.x > 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        else if (targetPosition.x - rb.position.x < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
    }

    void SetLoopPath()
    {
        for (int i = 0; i < loopingArrayOffset.Length; i++)
        {
            loopingArray.Add(transform.position + loopingArrayOffset[i]);
        }
    }

    public void FollowPlayer()
    {
        if (hunting) return;
        if (GameObject.FindGameObjectWithTag("Dante") == null) return;
        hunting = true;
        targetPosition = GameObject.FindGameObjectWithTag("Dante").transform.position;
        UpdatePath();
    }

    public void GoToLoopPath()
    {
        if (!hunting) return;
        hunting = false;
        targetPosition = loopingArray[currentLoopPoint];
        UpdatePath();
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawCube(transform.position + loopingArrayOffset[0], new Vector3(0.1f, 0.1f, 0));
        Gizmos.DrawCube(transform.position + loopingArrayOffset[1], new Vector3(0.1f, 0.1f, 0));
        Gizmos.DrawCube(transform.position + loopingArrayOffset[2], new Vector3(0.1f, 0.1f, 0));
        Gizmos.DrawCube(transform.position + loopingArrayOffset[3], new Vector3(0.1f, 0.1f, 0));
    }
}
