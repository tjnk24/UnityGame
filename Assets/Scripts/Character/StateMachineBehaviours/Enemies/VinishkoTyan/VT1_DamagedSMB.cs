using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VT1_DamagedSMB : SceneLinkedSMB<EnemyBehaviour>
{
    public override void OnStateEnter(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        m_MonoBehaviour.OnDamagedMove();
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        if (m_MonoBehaviour.CheckCurrentHealth())
        {
            m_MonoBehaviour.SetFlee();
        }
        else if (m_MonoBehaviour.ScanForPlayer() || m_MonoBehaviour.ScanForPlayer(false))
        {
            m_MonoBehaviour.SetAttack();
        }
        else
        {
            m_MonoBehaviour.SetChase();
        }
    }
}