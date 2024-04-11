using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.UI;
public class HealthAndVariables : MonoBehaviour
{
    [Header("Player Health")]
    public float MaxHealth = 100f;
    public float Health;
    public Slider HealthSlider;

    [Header("Player hunger")]
    public float MaxHunger = 100f;
    public float Hunger;
    public float HungerOT;
    public Slider HungerSlider;

    [Header("Player thirst")]
    public float MaxThirst = 100f;
    public float Thirst;
    public float ThirstOT;
    public Slider ThirstSlider;

    private void Start()
    {
        if(LoadLife() <= 0)
        {
            Health = MaxHealth;
        }else
        {
            Health = LoadLife();
        }
        GameController.OnsavedGame += SaveLife;
    }

    private void Update()
    {
        Hunger = Hunger - HungerOT * Time.deltaTime;
        Thirst = Thirst - HungerOT * Time.deltaTime;
        UpdateSliders();

       /* if (Hunger <= 0f)
        {
            Health = Health - 0.5f * Time.deltaTime;
        }*/
    }

    public void TakeFallDamage()
    {

        Health -= 10f;
    }

    public void Damege(int Damege) 
    {
        Health -= Damege;
    }

    public void UpdateSliders()
    {
        HealthSlider.value = Health ;
        //HungerSlider.value = Hunger / MaxHunger;
        //ThirstSlider.value = Thirst / MaxThirst;
    }

    private float LoadLife()
    {
        Save save = FindAnyObjectByType<Save>();
        IDataReader read = save.ReadHudValues(1);
        while (read.Read())
        {
            return read.GetFloat(1);
        }

        return 0;

    }
    private void SaveLife()
    {
        Save save = FindAnyObjectByType<Save>();
        if (save != null)
        {
            save.UpdateHudValues(1, Health, true);
            save.Close();
        }
        else
        {
            Debug.LogWarning("FUDEU!");
        }
    }
}