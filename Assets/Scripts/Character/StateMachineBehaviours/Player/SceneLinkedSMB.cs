using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneLinkedSMB<TMonoBehaviour> : StateMachineBehaviour
    where TMonoBehaviour : MonoBehaviour
{
    protected TMonoBehaviour m_MonoBehaviour;

    public static void Initialize (Animator animator, TMonoBehaviour monoBehaviour)
    {
        SceneLinkedSMB<TMonoBehaviour>[] sceneLinkedSMBs = animator.GetBehaviours<SceneLinkedSMB<TMonoBehaviour>>();

        for (int i = 0; i < sceneLinkedSMBs.Length; i++)
        {
            sceneLinkedSMBs[i].InternalInitialise(animator, monoBehaviour);
        }
    }

    protected void InternalInitialise(Animator animator, TMonoBehaviour monoBehaviour)
    {
        m_MonoBehaviour = monoBehaviour;
        OnStart(animator);
    }

    public virtual void OnStart(Animator animator) { }
}
