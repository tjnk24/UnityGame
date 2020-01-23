using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkSMB : SceneLinkedSMB<PlayerCharacter>
{
    public override void OnStateUpdate(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        m_MonoBehaviour.UpdateFacing();
        m_MonoBehaviour.HorizontalMovement();
        m_MonoBehaviour.CheckForGrounded();
        m_MonoBehaviour.CheckForCrouching();

        if (m_MonoBehaviour.CheckForJumpInput())
        {
            m_MonoBehaviour.Jump();
        }
        else if (m_MonoBehaviour.CheckForMeleeAttackInput())
        {
            if (m_MonoBehaviour.horizontalMovementIndex != 0)
            {
                m_MonoBehaviour.SetWalkMeleeAttack();
            }
            else
            {
                m_MonoBehaviour.SetMeleeAttack();
            }
        }
    }
}
