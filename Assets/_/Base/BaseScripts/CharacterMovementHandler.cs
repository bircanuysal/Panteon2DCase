﻿using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

public class CharacterMovementHandler : MonoBehaviour {

    private const float speed = 40f;
    private int currentPathIndex;
    private List<Vector3> pathVectorList;
    private bool isMoving = false;
    public bool IsMoving {  get { return isMoving; } }
    private void Update()
    {
        HandleMovement();
        if (Input.GetMouseButtonDown(0))
        {
            SetTargetPosition(UtilsClass.GetMouseWorldPosition());
        }
        if (Input.GetMouseButtonDown(1))
        {
            Pathfinding pathfinding = MapManager.Instance.Pathfinding;
            Vector3 mouseWorldPosition = UtilsClass.GetMouseWorldPosition();
            pathfinding.GetGrid().GetXY(mouseWorldPosition, out int x, out int y);
            pathfinding.GetNode(x, y).SetIsWalkable(!pathfinding.GetNode(x, y).isWalkable);
        }
    }
    
    private void HandleMovement() 
    {
        if (pathVectorList != null)
        {
            Vector3 targetPosition = pathVectorList[currentPathIndex];
            if (Vector3.Distance(transform.position, targetPosition) > 1f) 
            {
                Vector3 moveDir = (targetPosition - transform.position).normalized;
                float distanceBefore = Vector3.Distance(transform.position, targetPosition);
                transform.position = transform.position + moveDir * speed * Time.deltaTime;
                isMoving = true;
            } 
            else 
            {
                currentPathIndex++;
                if (currentPathIndex >= pathVectorList.Count) 
                {
                    StopMoving();
                }
            }
        }
    }

    private void StopMoving() 
    {
        isMoving = false;
        pathVectorList = null;
    }

    public Vector3 GetPosition() 
    {
        return transform.position;
    }

    public void SetTargetPosition(Vector3 targetPosition)
    {
        currentPathIndex = 0;
        pathVectorList = Pathfinding.Instance.FindPath(GetPosition(), targetPosition);

        if (pathVectorList != null && pathVectorList.Count > 1)
        {
            pathVectorList.RemoveAt(0);
        }
    }

}