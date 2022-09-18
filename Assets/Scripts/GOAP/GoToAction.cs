using SGoap;
using Assets.SGOAP.Scripts.Basic;
using Pathfinding;
using UnityEngine;

public class GoToAction : AnimationAction
{
    public Transform CurrentTarget;

    public float Range = 1;
    public float MoveSpeed = 5;
    public float DistanceToTarget => Vector3.Distance(CurrentTarget.position, AgentData.RootTransformObject?.position ?? transform.position);
    private Vector3 _lastTargetPosition;
    private Path _pathToTarget;
    private int _currentWaypoint = 0;
    private int _nextWaypointDistance = 3;

    private void ResetPathfinding()
    {
        _currentWaypoint = 0;
        _pathToTarget = null;
    }

    public override void Start()
    {
        base.Start();

        AgentData.AStarSeeker.pathCallback += PathfinderCallback;
    }

    public void OnDisable()
    {
        AgentData.AStarSeeker.pathCallback -= PathfinderCallback;
    }

    public void Update()
    {
        if (DistanceToTarget <= Range)
        {
            States.SetState("InAttackRange", 1);
        }
        else
        {
            States.RemoveState("InAttackRange");
        }
    }

    /// <summary>
    /// Basically called every LateUpdate() for the object
    /// </summary>
    /// <returns></returns>
    public override EActionStatus Perform()
    {
        AgentData.Target = CurrentTarget;
        if (DistanceToTarget <= Range)
        {
            return EActionStatus.Success;
        }

        //Check if we had a last targeted position and need a new path.
        if (_lastTargetPosition != CurrentTarget.transform.position)
        {
            _lastTargetPosition = CurrentTarget.transform.position;
            ResetPathfinding();
            AgentData.AStarSeeker.StartPath(AgentData.RootTransformObject.position, CurrentTarget.position, PathfinderCallback);
        }

        if (_pathToTarget is not null)
        {
            var reachedEndOfPath = false;
            var distanceToWaypoint = 0f;
            while (!reachedEndOfPath)
            {
                distanceToWaypoint = Vector3.Distance(
                    AgentData.RootTransformObject.position,
                    _pathToTarget.vectorPath[_currentWaypoint]);
                if (distanceToWaypoint >= _nextWaypointDistance)
                {
                    break;
                }

                //Are we at the end of our path yet?
                if (_currentWaypoint + 1 < _pathToTarget.vectorPath.Count)
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
            var directionToGo = (_pathToTarget.vectorPath[_currentWaypoint] - AgentData.RootTransformObject.position).normalized;
            // Multiply the direction by our desired speed to get a velocity
            var velocity = directionToGo * MoveSpeed * speedFactor;

            AgentData.ParentPosition += velocity * Time.deltaTime;
        }

        return EActionStatus.Running;
    }

    private void PathfinderCallback(Path pathfinderResult)
    {
        if (pathfinderResult.error)
        {
            Debug.Log($"Pathfinding failed. Reasons: {pathfinderResult.errorLog}");
            return;
        }

        _pathToTarget = pathfinderResult;
    }
}
