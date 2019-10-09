using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogOfWarSystem : BaseSystem
{
    public Filter[] filters;

    [Header("Fog revealer settings")]
    public LayerMask layer;
    public Mesh mesh;
    public Material material;
    public Camera fogCamera;

    private const int MAX_DECALS = 1023;

    private Matrix4x4[] matrices = new Matrix4x4[MAX_DECALS];
    private int currentDecalAmount = 0;

    // need to make filter for each system, hopefully fix
    [System.Serializable]
    public struct Filter
    {
        public Filter(
            int id,
            GameObject go,
            FogRevealerComponent frc
            )
        {
            this.id = id;

            gameObject = go;
            fogRevealerComponent = frc;
        }

        // why not
        public int id;

        public GameObject gameObject;
        public FogRevealerComponent fogRevealerComponent;
    }

    public override void Initialize(Transform[] objects)
    {
        // list because I don't know size here
        List<Filter> tmpFilters = new List<Filter>();
        int index = 0;
        for (int i = 0; i < objects.Length; i++)
        {
            // check performance
            FogRevealerComponent frc = objects[i].GetComponent<FogRevealerComponent>();

            if (frc)
            {
                tmpFilters.Add(new Filter(index, objects[i].gameObject, frc));
                frc.indexInMatrixList = currentDecalAmount;
                frc.fogRevealerObject = new GameObject();
                frc.fogRevealerObject.name = "FogRevealerObject";
                frc.fogRevealerObject.transform.position = frc.transform.position;
                frc.fogRevealerObject.transform.parent = frc.transform;
                frc.fogRevealerObject.layer = 15;

                frc.meshRenderer = frc.fogRevealerObject.AddComponent<MeshRenderer>();
                frc.meshRenderer.material = frc.material;

                frc.meshFilter = frc.fogRevealerObject.AddComponent<MeshFilter>();
                frc.meshFilter.mesh = frc.revealerMesh;

                frc.currentMeshRevealScale = frc.maxRevealScale;
                frc.fogRevealerObject.transform.localScale = new Vector3(frc.currentMeshRevealScale, frc.currentMeshRevealScale, frc.currentMeshRevealScale);


                //InstantiateMesh(frc, currentDecalAmount, frc.transform.position, frc.revealMeshScale);
            }
        }

        filters = tmpFilters.ToArray();
    }

    public override void SetupInputComponent(InputComponent inputComponent)
    {
        //inputComponent.OnAButtonDown += DebugAPressed;
    }

    private void DebugAPressed()
    {
        Debug.Log("A pressed");
    }

    public override void Tick(float deltaTime)
    {
        for (int i = 0; i < filters.Length; i++)
        {
            // cache variables
            Filter filter = filters[i];

            FogRevealerComponent fogRevComp = filter.fogRevealerComponent;

            // ----- logic -----

            if (fogRevComp.lightComponent.IsLightEnabled())
            {
                float fuelPercentage = fogRevComp.lightComponent.currentFuel / fogRevComp.lightComponent.maxFuel;
                fogRevComp.currentMeshRevealScale = Mathf.Lerp(
                    fogRevComp.minRevealScale,
                    fogRevComp.maxRevealScale,
                    fuelPercentage);

                if (fogRevComp.lightComponent.currentFuel < 30f)
                {
                    fogRevComp.meshRenderer.enabled = !fogRevComp.meshRenderer.enabled;
                }

                fogRevComp.fogRevealerObject.transform.localScale = new Vector3(
                    fogRevComp.currentMeshRevealScale,
                    fogRevComp.currentMeshRevealScale,
                    fogRevComp.currentMeshRevealScale);

            }
            else
            {
                fogRevComp.meshRenderer.enabled = true;
                fogRevComp.fogRevealerObject.transform.localScale = new Vector3(
                    fogRevComp.minRevealScale - .5f,
                    fogRevComp.minRevealScale - .5f,
                    fogRevComp.minRevealScale - .5f);
            }
            //UpdateMesh(fogRevComp, fogRevComp.indexInMatrixList, fogRevComp.transform.position, fogRevComp.revealMeshScale);
        }

        /*MaterialPropertyBlock mpb = new MaterialPropertyBlock();
        Graphics.DrawMeshInstanced(
            mesh,
            0,
            material,
            matrices,
            currentDecalAmount,
            mpb,
            UnityEngine.Rendering.ShadowCastingMode.Off,
            false,
            15
            );*/
    }

    /*private void InstantiateMesh(FogRevealerComponent frc, int index, Vector3 position, float scale)
    {
        matrices[currentDecalAmount].SetTRS(position, Quaternion.identity, new Vector3(scale, scale, scale));
        currentDecalAmount++;
        if (currentDecalAmount == MAX_DECALS)
        {
            currentDecalAmount = 0;
        }
    }

    private void UpdateMesh(FogRevealerComponent frc, int index, Vector3 position, float scale)
    {
        matrices[index].SetTRS(position, Quaternion.identity, new Vector3(scale, scale, scale));
    }*/
}
