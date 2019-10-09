using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Variables/Shader")]
public class ShaderVariable : ScriptableObject
{
    public Color color { get; set; }

    public void SetColor()
    {
        Shader.SetGlobalColor("AmbientColorGlobal", color);
    }
}
