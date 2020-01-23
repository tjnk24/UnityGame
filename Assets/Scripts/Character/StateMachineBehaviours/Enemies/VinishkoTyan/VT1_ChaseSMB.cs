using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VT1_ChaseSMB : SceneLinkedSMB<EnemyBehaviour>
{
    public override void OnStateUpdate(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        m_MonoBehaviour.UpdateFacing();

        if (!m_MonoBehaviour.CheckForObstacles())
        {
            m_MonoBehaviour.HorizontalMovement(m_MonoBehaviour.horizontalDirection);
        }
        else
        {
            m_MonoBehaviour.SetPatrol();
        }

        if (m_MonoBehaviour.ScanForPlayer() || m_MonoBehaviour.ScanForPlayer(false))
        {
            m_MonoBehaviour.SetAttack();
        }
    }
}
