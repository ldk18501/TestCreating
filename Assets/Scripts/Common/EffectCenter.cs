﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lean.Pool;

public class EffectCenter : DoozyUI.Singleton<EffectCenter>
{
    GameObject[] _effObjs;
    public static Dictionary<string, GameObject> dictEffObjs;
    void Awake()
    {
        //_effObjs = Resources.LoadAll<GameObject>("Prefabs/Effects");
        dictEffObjs = new Dictionary<string, GameObject>();
    }

    public static GameObject LoadOrGetEffObj(string effName)
    {
        GameObject effObj = null;
        dictEffObjs.TryGetValue(effName, out effObj);

        if (effObj == null)
        {
            effObj = Resources.Load<GameObject>("Prefabs/Effects/" + effName);
            if (effObj != null)
                dictEffObjs.Add(effName, effObj);
        }

        return effObj;
    }

    public ParticleCtrller SpawnEffect(string name, Vector3 pos, Vector3 angle, bool nameOk = true)
    {
        var obj = LoadOrGetEffObj(name);

        if (obj == null)
            return null;

        var clone = LeanPool.Spawn(obj, pos, Quaternion.Euler(angle), null);
        if (clone != null)
        {
            return clone.gameObject.AddMissingComponent<ParticleCtrller>();
        }

        //var effObj = LeanPool.Spawn(obj, pos, Quaternion.Euler(angle), null, nameOk);
        return null;
    }

}