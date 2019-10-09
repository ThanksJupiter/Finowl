using UnityEngine;

public class HealthComponent : BaseComponent
{
    public int maxHealth = 3;

    public int currentHealth { get; set; } //private set
    public float currentGranularHealth;

    public bool isTutorialNoTakeDamage { get; set; }

    public float maxGranularHealth = 100f;
    public float granularHealthRestorationAmount = 10f;
    public float darknessGranularHealthDrain = 10f;
    public float darknessHealthDrainTime = 1f;

    public bool isHealing { get; set; }
    public float healthAlterAmount { get; set; }
    public float healthAlterDuration { get; set; }
    public float currentAlterTime { get; set; }
    public float alterTickAmount { get; set; }

    public bool isTakingDamage { get; set; }

    public float fogDamageInterval = 1f;
    public float currentFogDamageTimer { get; set; }

    public delegate void PlayerTakeDamageDelegate();
    public PlayerTakeDamageDelegate OnPlayerTakeDamage;

    public delegate void PlayerRestoreDamageDelegate();
    public PlayerRestoreDamageDelegate OnPlayerRestoreDamage;

    public delegate void PlayerDecreaseGranularHealthDelegate();
    public PlayerDecreaseGranularHealthDelegate OnPlayerDecreaseGranularHealth;

    public delegate void PlayerIncreaseGranularHealthDelegate();
    public PlayerIncreaseGranularHealthDelegate OnPlayerIncreaseGranularHealth;

    public delegate void PlayerDieDelegate();
    public PlayerDieDelegate OnPlayerDied;

    public delegate void PlayerRessurectedDelegate();
    public PlayerRessurectedDelegate OnPlayerRessurected;

    private void Awake()
    {
        currentHealth = maxHealth;
        currentGranularHealth = maxGranularHealth;
        isHealing = false;
        healthAlterDuration = 0f;
        currentAlterTime = 0f;
        isTutorialNoTakeDamage = true;
        currentFogDamageTimer = 0f;

        OnPlayerRessurected += InternalRestoreFullHealth;
    }

    public void TakeGranularDamage(float amount)
    {
        currentGranularHealth -= amount;
        OnPlayerDecreaseGranularHealth?.Invoke();

        if (currentGranularHealth <= 0)
        {
            if (currentHealth > 1)
            {
                TakeDamage();
                currentGranularHealth = 100f;
            }
            else
            {
                isDead = true;
                Kill();
            }
        }
    }

    public void RestoreGranularDamage(float amount)
    {
        if (currentGranularHealth < maxGranularHealth && currentHealth <= maxHealth)
        {
            currentGranularHealth += amount;
            OnPlayerIncreaseGranularHealth?.Invoke();
            if (currentGranularHealth >= maxGranularHealth)
            {
                RestoreDamage(1);
            }
        }
    }

    private void InternalRestoreFullHealth()
    {
        currentHealth = 1;
        isHealing = true;
        RestoreGranularDamageOverTime(300f, 3f, true);
    }

    [System.Obsolete]
    public void TakeDamage(int amount)
    {
        if (currentHealth == 0)
        {
            return;
        }
        currentHealth -= amount;
        OnPlayerTakeDamage?.Invoke();

        if (currentHealth <= 0)
        {
            isDead = true;
            Kill();
        }
    }

    private void TakeDamage()
    {
        if (currentHealth == 1)
        {
            if (currentHealth <= 0)
            {
                isDead = true;
                Kill();
            }
            return;
        }
        else
        {
            OnPlayerTakeDamage?.Invoke();
            currentHealth -= 1;
        }
    }

    [System.Obsolete]
    public void RestoreGranularDamageOverTime(float amount, float time)
    {
        isHealing = true;
        healthAlterAmount = amount;
        healthAlterDuration = time;
        currentAlterTime = 0f;
        alterTickAmount = healthAlterAmount / time;
    }

    public void RestoreGranularDamageOverTime(float amount, float time, bool overrideDamage)
    {
        if (isTakingDamage)
        {
            if (!overrideDamage) { return; }
            else
            {
                isHealing = true;
            }
        }
        else
        {
            isHealing = true;
        }

        healthAlterAmount = amount;
        healthAlterDuration = time;
        currentAlterTime = 0f;
        alterTickAmount = healthAlterAmount / time;
    }

    public void TakeGranularDamageOverTime(float amount, float time, bool overrideHealing)
    {
        if (isHealing)
        {
            if (!overrideHealing) { return; }
            else
            {
                isTakingDamage = true;
            }
        }
        else
        {
            isTakingDamage = true;
        }

        healthAlterAmount = amount;
        healthAlterDuration = time;
        currentAlterTime = 0f;
        alterTickAmount = healthAlterAmount / time;
    }

    public void RestoreDamage(int amount)
    {
        if (currentHealth + amount <= maxHealth)
        {
            currentHealth += amount;
            currentGranularHealth = 1f;
            OnPlayerRestoreDamage?.Invoke();
        }
    }

    private void Kill()
    {
        GetComponent<EnemyComponent>()?.SetIsActive(false);
        OnPlayerDied?.Invoke();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            TakeGranularDamageOverTime(100f, 1f, true);
        }
        else if (Input.GetKeyDown(KeyCode.K))
        {
            RestoreGranularDamageOverTime(100f, 1f, true);
        }
    }

    public bool isDead { get; set; }
}
