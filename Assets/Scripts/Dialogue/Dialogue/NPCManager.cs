using UnityEngine;
using System.Collections.Generic;

public class NPCManager : MonoBehaviour
{
    public List<NPCData> npcDatas;

    public NPCData GetClosestNPC(Vector3 playerPosition, float maxDistance)
    {
        NPCData closestNPC = default;
        float closestDistance = maxDistance;

        foreach (NPCData npcData in npcDatas)
        {
            float distance = Vector3.Distance(playerPosition, npcData.npc.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestNPC = npcData;
            }
        }

        return closestNPC;
    }
}

[System.Serializable]
public struct NPCData
{
    public Transform npc;
    public DialogueData dialogueData;
}
