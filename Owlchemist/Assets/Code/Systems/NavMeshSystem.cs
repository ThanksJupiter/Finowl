using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavMeshSystem : BaseSystem
{
    private Filter[] filters;

    [System.Serializable]
    public struct Filter
    {
        public Filter(
            int id,
            GameObject go,
            NavMeshComponent navComp
            )
        {
            this.id = id;

            gameObject = go;
            navMeshComponent = navComp;
        }
        public int id;
        public GameObject gameObject;
        public NavMeshComponent navMeshComponent;
    }

    public override void Initialize(Transform[] objects)
    {
        List<Filter> tmpFilters = new List<Filter>();
        int index = 0;
        for (int i = 0; i < objects.Length; i++)
        {
            NavMeshComponent navMeshComponent = objects[i].GetComponent<NavMeshComponent>();

            if (navMeshComponent)
            {
                tmpFilters.Add(new Filter(index, objects[i].gameObject, navMeshComponent));
                navMeshComponent.surface.BuildNavMesh();
                navMeshComponent.OnUpdateNavMesh += UpdatePlayerNavMesh;
            }

        }
        filters = tmpFilters.ToArray();
    }

    public override void SetupInputComponent(InputComponent inputComponent)
    {
        player.eventComponent.OnGatherResource += UpdatePlayerNavMesh;
    }

    public override void Tick(float deltaTime)
    {
        for (int i = 0; i < filters.Length; i++)
        {
            // cache variables
            Filter filter = filters[i];
            NavMeshComponent navMeshComponent = filter.navMeshComponent;

            if (Vector3.Distance(navMeshComponent.transform.position, navMeshComponent.surface.transform.position) >= navMeshComponent.distanceThreshold)
            {
                UpdateNavMesh(navMeshComponent);
            }

        }
    }

    private void UpdateNavMesh(NavMeshComponent navMeshComponent)
    {
        navMeshComponent.surface.RemoveData();
        navMeshComponent.surface.transform.position = navMeshComponent.transform.position;
        navMeshComponent.surface.BuildNavMesh();
    }

    private void UpdatePlayerNavMesh()
    {
        Debug.Log("Updating navmesh");
        player.navMeshComponent.surface.RemoveData();
        player.navMeshComponent.surface.transform.position = player.navMeshComponent.transform.position;
        player.navMeshComponent.surface.BuildNavMesh();
    }
}