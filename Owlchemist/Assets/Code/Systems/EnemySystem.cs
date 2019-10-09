using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EnemySystem : BaseSystem
{
    public Filter[] filters;
    EnvironmentComponent environmentComponent;
    CartComponent transportableComponent;

    // used for getting player position & toggling offensive behaviour
    InputComponent inputComponent;

    public GameObject returnGO;

    public Text loseTXT;
    public Image blockIMG;
    float alpha = 0;

    private bool chasePlayer;

    // need to make filter for each system, hopefully fix
    [System.Serializable]
    public struct Filter
    {
        public Filter(int id,
            GameObject go,
            EnemyComponent ec,
            CombatComponent cc,
            HealthComponent hc
            )
        {
            this.id = id;

            gameObject = go;
            enemyComponent = ec;
            combatComponent = cc;
            healthComponent = hc;
        }

        // why not
        public int id;

        public GameObject gameObject;
        public EnemyComponent enemyComponent;
        public CombatComponent combatComponent;
        public HealthComponent healthComponent;
    }

    public override void Initialize(Transform[] objects)
    {
        // list because I don't know size here
        List<Filter> tmpFilters = new List<Filter>();
        int index = 0;
        for (int i = 0; i < objects.Length; i++)
        {
            // check performance
            EnemyComponent ec = objects[i].GetComponent<EnemyComponent>();
            CombatComponent cc = objects[i].GetComponent<CombatComponent>();
            HealthComponent hc = objects[i].GetComponent<HealthComponent>();

            if (cc && hc)
            {
                tmpFilters.Add(new Filter(index, objects[i].gameObject, ec, cc, hc));
            }
        }

        filters = tmpFilters.ToArray();

        environmentComponent = GetComponentInChildren<EnvironmentComponent>();
        transportableComponent = GetComponentInChildren<CartComponent>();
        inputComponent = GetComponentInChildren<InputComponent>();
    }

    bool gameOver = false;
    public override void Tick(float deltaTime)
    {
        for (int i = 0; i < filters.Length; i++)
        {
            // cache variables
            Filter filter = filters[i];

            EnemyComponent enemyComp = filter.enemyComponent;
            CombatComponent combComp = filter.combatComponent;
            HealthComponent healthComp = filter.healthComponent;

            // ----- logic -----
            if (gameOver)
            {
                if (Input.GetKeyDown(KeyCode.F))
                {
                    Restart();
                }
                alpha += deltaTime * 0.3f;
            }

            float dst = Vector3.Distance(filter.gameObject.transform.position, inputComponent.transform.position);
            if (dst > enemyComp.aggroRange)
            {
                enemyComp.agent.ResetPath();
                return;
            }

            if (inputComponent.avoidPlayer)
            {
                enemyComp.isPursuingTransportable = false;
                //EngageTransform(enemyComp, returnGO.transform);
            }

            if (enemyComp.isActive)
            {
                if (!inputComponent.avoidPlayer)
                {
                    //EngageTransform(enemyComp, inputComponent.gameObject.transform);
                }
            }

            /*float remainingDist = Vector3.Distance(enemyComp.transform.position, transportableComponent.transform.position);
            if (enemyComp.agent.hasPath && remainingDist < 2f)
            {
                gameOver = true;
                loseTXT.gameObject.SetActive(true);
                blockIMG.gameObject.SetActive(true);
                blockIMG.color = new Color(blockIMG.color.r, blockIMG.color.g, blockIMG.color.b, alpha);
            }*/
        }
    }

    private void Restart()
    {
        SceneManager.LoadScene(0);
    }

    private void EngageTransform(EnemyComponent ec, Transform t)
    {
        ec.agent.SetDestination(t.position);
        ec.isPursuingTransportable = true;
    }
}
