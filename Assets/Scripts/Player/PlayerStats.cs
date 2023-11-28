using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StatType
{
    Strength,
    Agility,
    Intelligence,
    Vitatity,
    Dexterity,
    Endurance,
    Defense,
}

public class Stat
{
    public StatType type;
    public float baseValue;
    public float modifier; // For temp modifiers
//Get Current Value of Stat
    public float Value => baseValue + modifier;
}

public class CharacterStats
{
    public Stat strength;
    public Stat agility; 
    public Stat intelligence;
    public Stat vitality;
    public Stat dexterity;

    public Stat endurance;

    public Stat defense;

     
  

    public void Initialize()
    {
        strength.baseValue = 10;
        agility.baseValue = 10;
        intelligence.baseValue = 10;
        vitality.baseValue = 10;
        dexterity.baseValue = 10;
        endurance.baseValue = 10;   
        defense.baseValue = 10;
    }

    public void ApplyModifier(StatType statType, float modifier)
    {
        Stat stat = GetStat(statType);

        stat.modifier += modifier;
    

    }

    public float GetStatValue(StatType statType)
    {
        Stat stat = GetStat(statType);
        
        return stat.Value;
    }

    private Stat GetStat(StatType statType)
    {
        switch(statType)
        {
            case StatType.Strength:
                return strength;
            case StatType.Agility:
                return agility;
            case StatType.Intelligence:
                return intelligence;
            case StatType.Vitatity:
                return vitality;
            case StatType.Dexterity:
                return dexterity;
            case StatType.Endurance:
                return endurance;
            case StatType.Defense:
                return defense;
            default:
                throw new System.ArgumentException("Invalid Stat Type");
        }
    }
}
            






    


 
