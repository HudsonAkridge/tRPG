using SGoap;
using Assets.SGOAP.Scripts.Basic;
using Pathfinding;
using UnityEngine;

public class GoToAction : AnimationAction
{
    public float Range = 1;
    public float MoveSpeed = 5;
    private Vector3 _lastTargetPosition;
    private Path _pathToTarget;
    private int _currentWaypoint = 0;
    private int _nextWaypointDistance = 3;

    private void ResetPathfinding()
    {
        _currentWaypoint = 0;
        _pathToTarget = null;
    }

    public float GetDistanceToTarget() => Vector3.Distance(AgentData.OurCurrentTargetPosition, OurPosition);

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
        if (AgentData.CurrentTarget == null)
        {
            //We can't do anything here
            return;
        }

        //Goto
        if (GetDistanceToTarget() <= Range)
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
        if (AgentData.CurrentTarget is null)
        {
            Debug.Log("No Target found when attempting to GoToAction.");
            return EActionStatus.Failed;
        }

        //AgentData.Target = CurrentTarget;
        if (GetDistanceToTarget() <= Range)
        {
            return EActionStatus.Success;
        }

        //Check if we had a last targeted position and need a new path.
        if (_lastTargetPosition != AgentData.OurCurrentTargetPosition)
        {
            _lastTargetPosition = AgentData.OurCurrentTargetPosition;
            ResetPathfinding();
            AgentData.OurAStarSeeker.StartPath(OurPosition, AgentData.OurCurrentTargetPosition, PathfinderCallback);
        }

        if (_pathToTarget is not null)
        {
            var reachedEndOfPath = false;
            var distanceToWaypoint = 0f;
            while (!reachedEndOfPath)
            {
                distanceToWaypoint = Vector3.Distance(
                    AgentData.OurRootTransform.position,
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
            var directionToGo = (_pathToTarget.vectorPath[_currentWaypoint] - OurPosition).normalized;
            // Multiply the direction by our desired speed to get a velocity
            var velocity = directionToGo * MoveSpeed * speedFactor;

            OurPosition += velocity * Time.deltaTime;
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
