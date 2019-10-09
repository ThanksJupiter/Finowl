using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEditor;

public class NavMeshGenerator
{
    [MenuItem("Tools/Assign Enemy Navigation")]
    public static void AssignEnemyNav()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject environment = GameObject.FindGameObjectWithTag("Environment");


        for (int i = 0; i < enemies.Length; i++)
        {
            NavMeshComponent navComp = enemies[i].GetComponent<NavMeshComponent>();

            if (enemies[i].GetComponent<NavMeshAgent>() == null)
            {
                enemies[i].AddComponent<NavMeshAgent>();
            }

            if (enemies[i].GetComponent<NavMeshComponent>() == null)
            {
                enemies[i].AddComponent<NavMeshComponent>();
            }

            if (enemies[i].GetComponent<NavMeshComponent>() != null && 
                enemies[i].GetComponent<NavMeshComponent>().surface == null &&
                enemies[i].GetComponent<NavMeshAgent>() != null)
            {
                foreach (GameObject e in enemies)
                {
                    environment.AddComponent<NavMeshSurface>().transform.position = enemies[i].GetComponent<NavMeshAgent>().transform.position;
                    navComp.surface.collectObjects = CollectObjects.Volume;
                    navComp.surface.size = new Vector3(10, 10, 10);
                    navComp.surface.overrideTileSize = true;
                    navComp.surface.tileSize = 16;
                }
            }
        }
    }

    [MenuItem("Tools/Assign Player Navigation")]
    public static void AssignPlayerNav()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        GameObject environment = GameObject.FindGameObjectWithTag("Environment");

        NavMeshComponent navComp = player.GetComponent<NavMeshComponent>();

        if (player.GetComponent<NavMeshAgent>() == null)
        {
            player.AddComponent<NavMeshAgent>();
        }

        if (navComp == null)
        {
            player.AddComponent<NavMeshComponent>();
        }

        if (navComp != null &&
            navComp.surface == null &&
            player.GetComponent<NavMeshAgent>() != null)
        { 
            environment.AddComponent<NavMeshSurface>().transform.position = player.GetComponent<NavMeshAgent>().transform.position;
            navComp.surface.collectObjects = CollectObjects.Volume;
            navComp.surface.size = new Vector3(10, 10, 10);
            navComp.surface.overrideTileSize = true;
            navComp.surface.tileSize = 16;
        }
    }
}
