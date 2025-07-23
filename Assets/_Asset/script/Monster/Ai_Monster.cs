using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Ai_Monster : MonoBehaviour
{

    //public GameObject mainHome;
    //public float moveSpeed = 3f;
    //public float detectPlayerRadius = 5f;
    //public float attackRange = 1f;
    //public float attackCooldown = 1.5f;

    //private GameObject targetPlayer;
    //private string state = "MoveToHome";
    //private float lastAttackTime = 0f;
    //public float deslay = 1f;
    //public Rigidbody rb;
    //public Animator anim;

    //private bool isWaitingAfterAttack = false;

    //void OnDrawGizmosSelected()
    //{
    //    // Vẽ vùng phát hiện Player (màu xanh)
    //    Gizmos.color = Color.blue;
    //    Gizmos.DrawWireSphere(transform.position, detectPlayerRadius);

    //    // Vẽ vùng tấn công (màu đỏ)
    //    Gizmos.color = Color.red;
    //    Gizmos.DrawWireSphere(transform.position, attackRange);
    //}

    //void Update()
    //{
    //    if (mainHome == null || isWaitingAfterAttack) return;

    //    // Tìm player gần nhất trong vùng phát hiện
    //    targetPlayer = FindClosestPlayer();

    //    float distanceToHome = Vector3.Distance(transform.position, mainHome.transform.position);
    //    float distanceToPlayer = targetPlayer != null ? Vector3.Distance(transform.position, targetPlayer.transform.position) : Mathf.Infinity;

    //    switch (state)
    //    {
    //        case "MoveToHome":
    //            if (targetPlayer != null)
    //            {
    //                state = "ChasePlayer";
    //            }
    //            else if (distanceToHome <= attackRange)
    //            {
    //                TryAttack(mainHome);
    //            }
    //            else
    //            {
    //                MoveTowards(mainHome.transform.position);
    //            }
    //            break;

    //        case "ChasePlayer":
    //            if (targetPlayer == null || distanceToPlayer > detectPlayerRadius)
    //            {
    //                state = "MoveToHome";
    //            }
    //            else if (distanceToPlayer <= attackRange)
    //            {
    //                TryAttack(targetPlayer);
    //            }
    //            else
    //            {
    //                MoveTowards(targetPlayer.transform.position);
    //            }
    //            break;
    //    }
    //}

    //void MoveTowards(Vector3 targetPosition)
    //{
    //    Vector3 direction = (targetPosition - transform.position).normalized;

    //    // Xoay mặt về hướng di chuyển
    //    if (direction != Vector3.zero)
    //        transform.rotation = Quaternion.LookRotation(direction);

    //    // Di chuyển bằng Rigidbody
    //    Vector3 move = transform.position + direction * moveSpeed * Time.deltaTime;
    //    rb.MovePosition(move);
    //    anim.SetBool("isMoving", true);
    //}

    //void TryAttack(GameObject target)
    //{
    //    if (Time.time - lastAttackTime >= attackCooldown && !isWaitingAfterAttack)
    //    {
    //        Debug.Log("Tấn công " + target.name);

    //        // ✅ Xoay mặt về phía mục tiêu tấn công
    //        Vector3 direction = (target.transform.position - transform.position).normalized;
    //        direction.y = 0f; // Giữ không xoay theo trục Y nếu không cần
    //        if (direction != Vector3.zero)
    //        {
    //            transform.rotation = Quaternion.LookRotation(direction);
    //        }


    //        anim.SetTrigger("attack");
    //        anim.SetBool("isMoving", false);

    //        lastAttackTime = Time.time;

    //        // Bắt đầu nghỉ sau tấn công
    //        StartCoroutine(WaitAfterAttack(deslay));

    //        // TODO: Gây sát thương nếu cần
    //    }
    //}

    //IEnumerator WaitAfterAttack(float waitTime)
    //{
    //    isWaitingAfterAttack = true;
    //    yield return new WaitForSeconds(waitTime);
    //    isWaitingAfterAttack = false;
    //}

    //GameObject FindClosestPlayer()
    //{
    //    GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
    //    GameObject closest = null;
    //    float minDistance = Mathf.Infinity;

    //    foreach (GameObject p in players)
    //    {
    //        float dist = Vector3.Distance(transform.position, p.transform.position);
    //        if (dist <= detectPlayerRadius && dist < minDistance)
    //        {
    //            minDistance = dist;
    //            closest = p;
    //        }
    //    }

    //    return closest;
    //}
    public GameObject mainHome;
    public float detectPlayerRadius = 5f;
    public float attackRange = 1f;
    public float attackCooldown = 1.5f;
    public float delayAfterAttack = 1f;
    public float dame;
    public Animator anim;
    public Control_collider_attack control_Collider_Attack;

    public List<Transform> defaultPathPoints;

    private GameObject targetPlayer;
    private string state = "MoveToHome";
    private float lastAttackTime = 0f;
    private int defaultPathIndex = 0;

    private NavMeshAgent agent;
    private float lastClickTime = -999f;
    private bool isFollowingClick = false;
    private bool isWaitingAfterAttack = false;



    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        if (defaultPathPoints.Count > 0)
        {
            agent.SetDestination(defaultPathPoints[0].position);
        }
    }

    void Update()
    {
        if (isWaitingAfterAttack) return;

        HandleMouseClick();

        targetPlayer = FindClosestPlayer();
        float distanceToPlayer = targetPlayer != null ? Vector3.Distance(transform.position, targetPlayer.transform.position) : Mathf.Infinity;

        switch (state)
        {
            case "MoveToHome":
                if (targetPlayer != null)
                {
                    state = "ChasePlayer";
                }
                else if (ReachedDestination())
                {
                    float distanceToHome = Vector3.Distance(transform.position, mainHome.transform.position);
                    if (defaultPathIndex < defaultPathPoints.Count - 1)
                    {
                        GoToNextDefaultPoint();
                    }
                    else if (distanceToHome <= attackRange + 5)
                    {
                        Debug.Log("🟢 Trong tầm tấn công mainHome! Bắt đầu tấn công");
                        TryAttack(mainHome);

                    }
                    else
                    {
                        Debug.Log("🟡 Chưa tới gần mainHome, tiếp tục đi tới");
                        agent.SetDestination(mainHome.transform.position);
                    }

                    //else if (Vector3.Distance(transform.position, mainHome.transform.position) > agent.stoppingDistance)
                    //{
                    //    Debug.Log("🟡 Chưa tới mainHome, tiếp tục đi tới");
                    //    agent.SetDestination(mainHome.transform.position);
                    //}
                    //else
                    //{
                    //    Debug.Log("🟢 Đã tới mainHome! Chuẩn bị tấn công");
                    //    TryAttack(mainHome);
                    //}
                }
                break;

            case "ChasePlayer":
                if (targetPlayer == null || distanceToPlayer > detectPlayerRadius)
                {
                    state = "MoveToHome";
                    GoToCurrentDefaultOrHome();
                }
                else if (distanceToPlayer <= attackRange)
                {
                    TryAttack(targetPlayer);
                }
                else
                {
                    agent.SetDestination(targetPlayer.transform.position);
                }
                break;

            case "ClickMove":
                if (targetPlayer != null)
                {
                    state = "ChasePlayer";
                }
                else if (Time.time - lastClickTime >= 3f)
                {
                    isFollowingClick = false;
                    state = "MoveToHome";
                    GoToCurrentDefaultOrHome();
                }
                break;
        }

        anim.SetBool("isMoving", agent.velocity.magnitude > 0.1f);
    }

    void HandleMouseClick()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                agent.SetDestination(hit.point);
                isFollowingClick = true;
                lastClickTime = Time.time;
                state = "ClickMove";
            }
        }
    }

    void TryAttack(GameObject target)
    {
        if (Time.time - lastAttackTime >= attackCooldown)
        {
            lastAttackTime = Time.time;
            anim.SetTrigger("attack");
            control_Collider_Attack.dame = dame;

            agent.isStopped = true;

            Vector3 direction = (target.transform.position - transform.position).normalized;
            direction.y = 0f;
            if (direction != Vector3.zero)
            {
                transform.rotation = Quaternion.LookRotation(direction);
            }

            StartCoroutine(WaitAfterAttack(delayAfterAttack));
        }
    }

    IEnumerator WaitAfterAttack(float waitTime)
    {
        isWaitingAfterAttack = true;
        yield return new WaitForSeconds(waitTime);
        isWaitingAfterAttack = false;
        agent.isStopped = false;
    }

    GameObject FindClosestPlayer()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        GameObject closest = null;
        float minDistance = Mathf.Infinity;

        foreach (GameObject p in players)
        {
            float dist = Vector3.Distance(transform.position, p.transform.position);
            if (dist <= detectPlayerRadius && dist < minDistance)
            {
                minDistance = dist;
                closest = p;
            }
        }

        return closest;
    }

    bool ReachedDestination()
    {
        return !agent.pathPending && agent.remainingDistance <= agent.stoppingDistance;
    }

    void GoToNextDefaultPoint()
    {
        if (defaultPathPoints.Count == 0) return;
        defaultPathIndex++;
        agent.SetDestination(defaultPathPoints[defaultPathIndex].position);
    }

    void GoToCurrentDefaultOrHome()
    {
        if (defaultPathIndex < defaultPathPoints.Count)
        {
            agent.SetDestination(defaultPathPoints[defaultPathIndex].position);
        }
        else
        {
            agent.SetDestination(mainHome.transform.position);
        }
    }
}