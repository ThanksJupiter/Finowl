using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogCamera : MonoBehaviour
{
    Camera fogCamera;
    RenderTexture texture;

    public Shader renderShader;

    public MeshRenderer fogMeshRenderer;

    // Start is called before the first frame update
    void Start()
    {
        fogCamera = GetComponent<Camera>();

        texture = new RenderTexture(1024, 1024, 0);
        fogCamera.targetTexture = texture;
        fogMeshRenderer.material.SetTexture("_FogTex", texture);

        fogMeshRenderer.material.SetVector("_FogOffset", transform.position);
        fogMeshRenderer.material.SetFloat("_FogSize", fogCamera.orthographicSize);
    }

    // Update is called once per frame
    void Update()
    {
        fogCamera.RenderWithShader(renderShader, "");
    }
}
