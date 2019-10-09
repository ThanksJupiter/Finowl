using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TorchSystem : BaseSystem
{
    public Filter[] filters;

    // need to make filter for each system, hopefully fix
    [System.Serializable]
    public struct Filter
    {
        public Filter(int id,
            GameObject go,
            TorchComponent torchC,
            InputComponent inputC,
            InventoryComponent inventoryC
            )
        {
            this.id = id;

            gameObject = go;
            torchComponent = torchC;
            inputComponent = inputC;
            inventoryComponent = inventoryC;
        }

        // why not
        public int id;

        public GameObject gameObject;
        public TorchComponent torchComponent;
        public InputComponent inputComponent;
        public InventoryComponent inventoryComponent;
    }

    public override void Initialize(Transform[] objects)
    {
        // list because I don't know size here
        List<Filter> tmpFilters = new List<Filter>();
        int index = 0;
        for (int i = 0; i < objects.Length; i++)
        {
            // check performance
            TorchComponent torchC = objects[i].GetComponent<TorchComponent>();
            InputComponent inputC = objects[i].GetComponent<InputComponent>();
            InventoryComponent inventoryC = objects[i].GetComponent<InventoryComponent>();

            if (torchC && inputC && inventoryC)
            {
                tmpFilters.Add(new Filter(index, objects[i].gameObject, torchC, inputC, inventoryC));
            }
        }

        filters = tmpFilters.ToArray();
    }

    public override void Tick(float deltaTime)
    {
        for (int i = 0; i < filters.Length; i++)
        {
            // cache variables
            Filter filter = filters[i];

            TorchComponent torchC = filter.torchComponent;
            InputComponent inputC = filter.inputComponent;
            InventoryComponent inventoryC = filter.inventoryComponent;

            if (inputC.GetKeyDown(KeyCode.T))
            {
                torchC.torchLight.enabled = !torchC.torchLight.enabled;
                inputC.avoidPlayer = !inputC.avoidPlayer;
            }

            // ----- logic -----

            // activate/deactivate torch?
            // torch run out
        }
    }
}
