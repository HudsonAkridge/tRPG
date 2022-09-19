using SGoap;
using Assets.SGOAP.Scripts.Basic;
using Pathfinding;
using UnityEngine;

public class WanderAction : AnimationAction
{
    public float MoveSpeed = 5;
    private Path _pathToWander;

    private int _currentWaypoint = 0;
    private int _nextWaypointDistance = 3;
    private bool _hasFoundEnemy;
    private Vector3 _startingPosition;
    private bool _isCalculatingPath;
    private bool _forceCreateNewPath;

    private void ResetPathfinding()
    {
        _currentWaypoint = 0;
        _pathToWander = null;
        _forceCreateNewPath = false;
    }

    public void Start()
    {
        AgentData.OurAStarSeeker.pathCallback += PathfinderCallback;
        _startingPosition = OurPosition;
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
        //Check if we need to create a new wandering path from our starting position (so we don't wander too far)
        if (_forceCreateNewPath ||(_pathToWander is null && !_isCalculatingPath))
        {
            ResetPathfinding();
            var randomPath = RandomPath.Construct(OurPosition, 50000);
            randomPath.spread = 1000;

            OurAStarSeeker.StartPath(randomPath, PathfinderCallback);
            _isCalculatingPath = true;
            return EActionStatus.Running;
        }

        //We're already calculating the path but it hasn't finished yet
        if (_pathToWander is null)
        {
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
                _forceCreateNewPath = true;
                //TODO: Find a target along the way to end early
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
        _isCalculatingPath = false;
    }
}
