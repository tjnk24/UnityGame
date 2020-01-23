using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : InputComponent
{
    public static PlayerInput Instance
    {
        get { return s_Instance; }
    }

    protected static PlayerInput s_Instance;

    public KeyCode Jump = KeyCode.Space;
    public KeyCode MeleeAttack = KeyCode.K;
    public InputAxis Horizontal = new InputAxis(KeyCode.D, KeyCode.A);
    public InputAxis Vertical = new InputAxis(KeyCode.W, KeyCode.S);

    private void Awake()
    {
        if (s_Instance == null)
        {
            s_Instance = this;
        }
    }

    private void Update()
    {
        GetInputs();
    }

    protected void GetInputs()
    {
        
        Horizontal.Get();
        Vertical.Get();
        
    }
}
