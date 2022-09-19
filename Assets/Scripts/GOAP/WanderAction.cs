using SGoap;
using Assets.SGOAP.Scripts.Basic;
using Pathfinding;
using UnityEngine;

public class WanderAction : AnimationAction
{
    public float MoveSpeed = 2;
    private Path _pathToWander;

    private int _currentWaypoint = 0;
    private int _nextWaypointDistance = 3;
    private bool _hasFoundEnemy;

    private void ResetPathfinding()
    {
        _currentWaypoint = 0;
        _pathToWander = null;
    }

    public void Start()
    {
        AgentData.OurAStarSeeker.pathCallback += PathfinderCallback;
    }

    public void OnDisable()
    {
        AgentData.OurAStarSeeker.pathCallback -= PathfinderCallback;
    }

    public void Update()
    {
        if (!_hasFoundEnemy)
        {
            return;
        }

        States.SetState("GoToTarget", 1);
        _hasFoundEnemy = false;
    }

    /// <summary>
    /// Basically called every LateUpdate() for the object
    /// </summary>
    /// <returns></returns>
    public override EActionStatus Perform()
    {
        //Check if we need to create a new wandering path
        if (_pathToWander is null)
        {
            ResetPathfinding();
            var randomPath = RandomPath.Construct(OurPosition, 5000);
            randomPath.spread = 1000;

            OurAStarSeeker.StartPath(randomPath, PathfinderCallback);
            return EActionStatus.Running;
        }

        var reachedEndOfPath = false;
        var distanceToWaypoint = 0f;
        while (!reachedEndOfPath)
        {
            distanceToWaypoint = Vector3.Distance(
                OurPosition,
                _pathToWander.vectorPath[_currentWaypoint]);
            if (distanceToWaypoint >= _nextWaypointDistance)
            {
                break;
            }

            //Are we at the end of our path yet?
            if (_currentWaypoint + 1 < _pathToWander.vectorPath.Count)
            {
                _currentWaypoint++;
            }
            else
            {
                reachedEndOfPath = true;
            }
        }
        // Slow down smoothly upon approaching the end of the path
        // This value will smoothly go from 1 to 0 as the agent approaches the last waypoint in the path.
        var speedFactor = reachedEndOfPath ? Mathf.Sqrt(distanceToWaypoint / _nextWaypointDistance) : 1f;

        // Direction to the next waypoint
        // Normalize it so that it has a length of 1 world unit
        var directionToGo = (_pathToWander.vectorPath[_currentWaypoint] - OurPosition).normalized;
        // Multiply the direction by our desired speed to get a velocity
        var velocity = directionToGo * MoveSpeed * speedFactor;

        OurPosition += velocity * Time.deltaTime;

        return EActionStatus.Running;
    }

    private void PathfinderCallback(Path pathfinderResult)
    {
        if (pathfinderResult.error)
        {
            Debug.Log($"Pathfinding failed. Reasons: {pathfinderResult.errorLog}");
            return;
        }

        _pathToWander = pathfinderResult;
    }
}
