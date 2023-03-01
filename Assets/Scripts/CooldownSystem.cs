using System;
using System.Collections.Generic;
using Interfaces;
using UnityEngine;

public class CooldownSystem : MonoBehaviour
{
    private readonly List<CooldownData> cooldowns = new List<CooldownData>();

    public void PutOnCooldown(IHasCooldown cooldown, Action<float> onUpdate = null)
    {
        cooldowns.Add(new CooldownData(cooldown, onUpdate));
    }
    
    void Update()
    {
        ProcessCooldowns();
    }

    private void ProcessCooldowns()
    {
        float deltaTime = Time.deltaTime;

        for (int i = cooldowns.Count - 1; i >= 0; i--)
        {
            if (cooldowns[i].DecrementCooldown(deltaTime))
            {
                cooldowns.RemoveAt(i);
            }
        }
    }

    public bool IsOnCooldown(int id)
    {
        foreach (CooldownData cooldown in cooldowns)
        {
            if (cooldown.Id == id)
            {
                return true;
            }
        }

        return false;
    }

    public float GetRemainingDuration(int id)
    {
        foreach (CooldownData cooldown in cooldowns)
        {
            if (cooldown.Id == id)
            {
                return cooldown.RemainingTime;
            }
        }
        
        return 0.0f;
    }
}

public class CooldownData
{
    public int Id { get; }
    
    public float RemainingTime { get; private set; }
    
    public Action<float> OnUpdate { get; }
    
    public CooldownData(IHasCooldown cooldown, Action<float> onUpdate = null)
    {
        Id = cooldown.Id;
        RemainingTime = cooldown.CooldownDuration;
        OnUpdate = onUpdate;
    }

    public bool DecrementCooldown(float deltaTime)
    {
        RemainingTime = Mathf.Max(RemainingTime - deltaTime, 0.0f);
        
        OnUpdate?.Invoke(RemainingTime);
        
        return RemainingTime == 0.0f;
    }
}
