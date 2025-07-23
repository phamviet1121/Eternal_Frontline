using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MoveToClick : MonoBehaviour
{
    private NavMeshAgent agent;
    private float lastClickTime = -999f;
    private bool isFollowingClick = false;
    private int defaultPathIndex = 0;
    public List<Transform> defaultPathPoints; // Gắn trong Inspector

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        // Đi đến điểm đầu tiên trong đường mặc định
        if (defaultPathPoints != null && defaultPathPoints.Count > 0)
        {
            agent.SetDestination(defaultPathPoints[0].position);
        }
    }

    void Update()
    {
        // Check chuột click
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                agent.SetDestination(hit.point);
                agent.isStopped = false;

                lastClickTime = Time.time;
                isFollowingClick = true;
            }
        }

        // Nếu đã đến điểm đích
        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
            {
                agent.isStopped = true;

                // Nếu đang đi đường mặc định thì chuyển sang điểm tiếp theo
                if (!isFollowingClick && defaultPathPoints.Count > 0)
                {
                    defaultPathIndex = (defaultPathIndex + 1) % defaultPathPoints.Count;
                    agent.SetDestination(defaultPathPoints[defaultPathIndex].position);
                    agent.isStopped = false;
                }
            }
        }

        // Nếu đang đi theo click mà quá 3s không click → quay về đường mặc định
        if (isFollowingClick && Time.time - lastClickTime >= 3f)
        {
            isFollowingClick = false;

            if (defaultPathPoints.Count > 0)
            {
                agent.SetDestination(defaultPathPoints[defaultPathIndex].position);
                agent.isStopped = false;
            }
        }
    }
}
