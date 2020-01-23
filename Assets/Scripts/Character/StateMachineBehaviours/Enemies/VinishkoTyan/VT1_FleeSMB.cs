using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VT1_FleeSMB : SceneLinkedSMB<EnemyBehaviour>
{
    public override void OnStateEnter(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        m_MonoBehaviour.horizontalDirection = -m_MonoBehaviour.horizontalDirection;
    }
    public override void OnStateUpdate(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        m_MonoBehaviour.UpdateFacing();

        if (!m_MonoBehaviour.CheckForObstacles())
        {
            m_MonoBehaviour.HorizontalMovement(m_MonoBehaviour.horizontalDirection);
        }
        else if (m_MonoBehaviour.ScanForPlayer() || m_MonoBehaviour.ScanForPlayer(false))
        {
            m_MonoBehaviour.SetAttack();
        }
        else
        {
            m_MonoBehaviour.SetPatrol();
        }
    }
}
