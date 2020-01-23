using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttackSMB : SceneLinkedSMB<PlayerCharacter>
{
    public override void OnStateEnter(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        m_MonoBehaviour.EnableMeeleeAttack();
        
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        m_MonoBehaviour.UpdateFacing();

        if (!m_MonoBehaviour.CheckForCrouching())
        {
            m_MonoBehaviour.HorizontalMovement();
        }
        m_MonoBehaviour.CheckForGrounded();
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        m_MonoBehaviour.DisableMeeleeAttack();
    }
}
