using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VT1_DeadSMB : SceneLinkedSMB<EnemyBehaviour>
{
    public override void OnStateEnter(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        m_MonoBehaviour.OnDeadColliderResize();
    }
    public override void OnStateUpdate(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        
    }
}
