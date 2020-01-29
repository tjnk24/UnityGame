using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VT1_AttackSMB : SceneLinkedSMB<EnemyBehaviour>
{
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
    }
    public override void OnStateUpdate(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {

        int playerHealth = m_MonoBehaviour.GetPlayerHealth();

        m_MonoBehaviour.UpdateFacing();

        m_MonoBehaviour.ScanForPlayer();
        m_MonoBehaviour.ScanForPlayer(false);

        if (!m_MonoBehaviour.ScanForPlayer() && !m_MonoBehaviour.ScanForPlayer(false))
        {
            m_MonoBehaviour.SetChase();
        }
        else if (playerHealth <= 0)
        {
            m_MonoBehaviour.SetPatrol();
        }
    }
}