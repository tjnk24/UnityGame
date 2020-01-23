using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrouchSMB : SceneLinkedSMB<PlayerCharacter>
{
    public override void OnStateUpdate(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        m_MonoBehaviour.UpdateFacing();
        m_MonoBehaviour.CheckForGrounded();
        m_MonoBehaviour.CheckForCrouching();

        if (m_MonoBehaviour.CheckForMeleeAttackInput())
        {
            m_MonoBehaviour.SetMeleeAttack();
        }
    }
}
