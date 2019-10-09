using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class BaseSystem : MonoBehaviour
{
    /// <summary>
    /// Call base.Initialize() in Initialize to initialize "global" player filter
    /// </summary>
    public PlayerFilter player { get; set; }

    /// <summary>
    /// Initializes system by creating Filters that each system iterates through to execute
    /// functionality of the system on combinations of Components
    /// </summary>
    /// <param name="objects"></param>
    public abstract void Initialize(Transform[] objects);

    public virtual void SetupInputComponent(InputComponent inputComponent)
    {

    }

    /// <summary>
    /// Iterates through Filters to execute functionality in Systems on combinations of components
    /// </summary>
    /// <param name="deltaTime"></param>
    public abstract void Tick(float deltaTime);
}
