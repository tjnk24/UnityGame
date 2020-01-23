using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VT1_IdleSMB : SceneLinkedSMB<EnemyBehaviour>
{
    public override void OnStateUpdate(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        if (m_MonoBehaviour.ScanForPlayer() || m_MonoBehaviour.ScanForPlayer(false))
        {
            m_MonoBehaviour.SetSpotted();
        }
    }
}
