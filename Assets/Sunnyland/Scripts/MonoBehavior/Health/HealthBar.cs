using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private GameObject healthPrefab;

    public const int numSlots = 5;
    [HideInInspector] public Player character;
    public List<GameObject> slots = new List<GameObject>();
    
    
    public void Start()
    {
        CreateSlots();
        
    }
    /*private void OnEnable()
    {
        CreateSlots();
    }*/
    public void CreateSlots()
    {
        
        if (healthPrefab != null)
        {
            for (int i = 0 ; i < character.hitPoints; i++)
            {
                GameObject newSlot = Instantiate(healthPrefab);
                //newSlot.name = "HealthSlot_" + i;
                newSlot.transform.SetParent(gameObject.transform.GetChild(0).transform);
                
                slots.Add(newSlot);
                
            }
            
        }
        
    }

    public bool AddItem(float HitPoints)
    {
        if (HitPoints != 0)
        {
            
            GameObject newSlot = Instantiate(healthPrefab);
            
            newSlot.transform.SetParent(gameObject.transform.GetChild(0).transform);
            slots.Add(newSlot);
            return true;
        }
        return false;
    }
    public void DeleteItem()
    {
        Destroy(slots.Last());
        slots.Remove(slots.Last());
        
    }
}
