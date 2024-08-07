        using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryController : MonoBehaviour
{
    [SerializeField] private GameObject inventoryPanel;
    private bool state;
    public InventoryObject[] slots;
    public Image[] slotsImages;
    public int[] slotAmount;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            state = !state;
            inventoryPanel.SetActive(state);
            if (state)
            {
                Cursor.lockState = CursorLockMode.Confined;
                Cursor.visible = true;
            }
            else 
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }            
        }

        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
        if (Physics.Raycast(ray, out hit, 30f))
        {
            if (hit.collider.tag == "Object")
            {
                if (Input.GetKeyDown(KeyCode.G))
                {
                    for (int i = 0; i < slots.Length; i++)
                    {
                        if (slots[i] == null || slots[i].name == hit.transform.GetComponent<Colectable>().Object.name)
                        {
                            slots[i] = hit.transform.GetComponent<Colectable>().Object;
                            slotAmount[i] ++;
                            slotsImages[i].sprite = slots[i].Sprite;
                            Destroy(hit.transform.gameObject);
                            break;
                        }
                        
                    }
                }
            }
        }
    }
}
