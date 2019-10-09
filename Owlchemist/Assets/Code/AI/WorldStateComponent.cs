using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldStateComponent : BaseComponent
{
    public HashSet<KeyValuePair<string, object>> worldState = new HashSet<KeyValuePair<string, object>>();

    KeyValuePair<string, object> playerLitState = new KeyValuePair<string, object>("playerLit", true);

    public void AddKVP(KeyValuePair<string, object> kvp)
    {
        worldState.Add(kvp);
    }

    public HashSet<KeyValuePair<string, object>> GetWorldState()
    {
        HashSet<KeyValuePair<string, object>> worldState = new HashSet<KeyValuePair<string, object>>();
        worldState.Add(new KeyValuePair<string, object>("damagePlayer", false));
        worldState.Add(new KeyValuePair<string, object>("stalkPlayer", false));

        return worldState;
    }

    public void SetLightState(bool value)
    {
        if (worldState.Contains(playerLitState))
        {
            worldState.Remove(playerLitState);
            playerLitState = new KeyValuePair<string, object>(playerLitState.Key, value);
            worldState.Add(playerLitState);
        }
    }

    public bool IsPlayerLit()
    {
        KeyValuePair<string, object> playerLitState = new KeyValuePair<string, object>("playerLit", true);

        return worldState.Contains(playerLitState);
    }
}
