using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirborneSMB : SceneLinkedSMB<PlayerCharacter>
{
    public override void OnStateUpdate(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        m_MonoBehaviour.UpdateFacing();
        m_MonoBehaviour.HorizontalMovement();
        m_MonoBehaviour.CheckForGrounded();
        m_MonoBehaviour.VerticalAnimation();

        if (m_MonoBehaviour.CheckForMeleeAttackInput())
        {
            m_MonoBehaviour.SetMeleeAttack();
        }
    }
}
