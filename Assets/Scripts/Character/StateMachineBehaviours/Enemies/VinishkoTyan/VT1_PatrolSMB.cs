using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VT1_PatrolSMB : SceneLinkedSMB<EnemyBehaviour>
{
    public override void OnStateEnter(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        m_MonoBehaviour.horizontalDirection = Random.Range(-1, 2); //от -1 до 2, потому что метод почему-то не выдаёт -1, пришлось увеличить, при этом -2 не выдаёт
    }
    public override void OnStateUpdate(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        m_MonoBehaviour.UpdateFacing();

        if (m_MonoBehaviour.CheckForObstacles())
        {
            
            m_MonoBehaviour.horizontalDirection = -m_MonoBehaviour.horizontalDirection;
        }
        
        m_MonoBehaviour.Patrol(m_MonoBehaviour.horizontalDirection);

        if (m_MonoBehaviour.ScanForPlayer() || m_MonoBehaviour.ScanForPlayer(false))
        {
            m_MonoBehaviour.SetAttack();
        }
    }
}