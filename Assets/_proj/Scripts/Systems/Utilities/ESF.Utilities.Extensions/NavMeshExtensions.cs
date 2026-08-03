using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace ESF.Utilities.Extensions
{
    public static class NavMeshExtensions
    {
        public static int GetAgentTypeIdFromName(string name)
        {
            for (int i = 0; i < NavMesh.GetSettingsCount(); i++)
            {
                var settings = NavMesh.GetSettingsByIndex(i);
                var agentName = NavMesh.GetSettingsNameFromID(settings.agentTypeID);
                if (agentName == name)
                    return settings.agentTypeID;
            }

            return -1;
        }

        public static NavMeshPath CalculatePath(Vector3 startPos, Vector3 endPos, int areaMask, int agentTypeId)
        {
            var navMeshQueryFilter = new NavMeshQueryFilter();
            navMeshQueryFilter.areaMask = areaMask;
            navMeshQueryFilter.agentTypeID = agentTypeId;

            NavMeshPath path = new NavMeshPath();
            NavMesh.CalculatePath(startPos, endPos, navMeshQueryFilter, path);
            return path;
        }
    }
}