using UnityEngine;

public class FogRevealerComponent : BaseComponent
{
    [Header("Settings")]
    public float maxRevealScale = 6f;
    public float minRevealScale = 2f;

    public float currentMeshRevealScale { get; set; }
    public Mesh revealerMesh;
    public Material material;

    public int indexInMatrixList;

    public GameObject fogRevealerObject;
    public MeshFilter meshFilter;
    public MeshRenderer meshRenderer;

    public LightComponent lightComponent;
    public GameObject torchObject;

    private void Awake()
    {
        lightComponent = GetComponent<LightComponent>();
    }
}
