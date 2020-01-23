using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VT1_SpottedSMB : SceneLinkedSMB<EnemyBehaviour>
{
    public override void OnStateUpdate(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        m_MonoBehaviour.UpdateFacing();

        m_MonoBehaviour.ScanForPlayer();
        m_MonoBehaviour.ScanForPlayer(false);

        if (!m_MonoBehaviour.ScanForPlayer() && !m_MonoBehaviour.ScanForPlayer(false))
        {
            m_MonoBehaviour.SetChase();
        }
    }
}
