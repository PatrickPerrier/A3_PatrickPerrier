using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Player_Controler : MonoBehaviour
{
    public LayerMask whatCanBeClickedOn;

    private NavMeshAgent myAgent;

    [SerializeField]
    ParticleSystem mParticle;

    [SerializeField]
    ParticleSystem aParticle;

    [SerializeField]
    float attackRange = 2;

    RaycastHit hitInfo;

    [SerializeField]
    Animator animator;

    private void Start()
    {
        myAgent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        CheckAttackRange();
        if (Input.GetMouseButton (1))
        {
            Debug.Log("Click");
            Ray myRay = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(myRay, out hitInfo, 200, whatCanBeClickedOn))
            {
                Instantiate(mParticle, hitInfo.point, Quaternion.identity);
                myAgent.SetDestination(hitInfo.point);

                animator.SetBool("isWalking", true);
                Debug.Log("Coroutine ends");
            }
        }
        //Check to see if agent is walking
        if (myAgent.velocity.magnitude <= 0f)
        {
            animator.SetBool("isWalking", false);
        }
    }

    private void CheckAttackRange()
    {
        Debug.Log("Coroutine Started");
        
        if (myAgent.hasPath == true)
        {
            if (hitInfo.transform.gameObject.CompareTag("Enemy") && myAgent.remainingDistance < attackRange)
            {
                GameObject enemy = hitInfo.transform.gameObject as GameObject;
                if (enemy != null)
                {
                    myAgent.isStopped = true;
                    StartCoroutine(Attack());

                    Destroy(enemy.gameObject, 1f);
                }
                
            }
        }
    }
    private IEnumerator Attack()
    {
        animator.SetBool("isAttacking", true);

        yield return new WaitForSeconds(1);

        Instantiate(aParticle, hitInfo.point, Quaternion.identity);
        
        animator.SetBool("isAttacking", false);
        myAgent.isStopped = false;
    }

    
    
}
