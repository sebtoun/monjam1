﻿using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class ExecutionOrderGroups : ScriptableObject 
{
    [Serializable]
    public class Group
    {
        public string Name;
        public int Order;
    }

    public Group[] GroupsData;

    public Dictionary<string, int> Groups
    {
        get
        {
            if (GroupsData == null)
            {
                Debug.LogError("Object not properly initialized.", this);
                return null;
            }
            var groups = GroupsData.ToDictionary(g => g.Name, g => g.Order);
            if (!groups.ContainsKey("Default"))
                groups["Default"] = 0;
            return groups;
        } 
    }
}
