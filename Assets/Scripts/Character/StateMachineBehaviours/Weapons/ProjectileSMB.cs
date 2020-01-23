using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileSMB : SceneLinkedSMB<Projectile>
{
    public override void OnStateEnter(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        m_MonoBehaviour.StartAttack();
    }
    public override void OnStateExit(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        m_MonoBehaviour.EndAttack();
    }
}
