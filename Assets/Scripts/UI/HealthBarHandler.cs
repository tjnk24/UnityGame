using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBarHandler : MonoBehaviour
{
    [SerializeField] private PlayerHealthUI healthBar;

    void Start()
    {
        healthBar.SetSize(.4f);
    }
}
