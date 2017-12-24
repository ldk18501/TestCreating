﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace smallone
{
    //掉落太低的东西自我销毁
    public class SelfDestroy : MonoBehaviour
    {
        bool _bDestroy;
        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (!_bDestroy && transform.position.y < -100)
            {
                _bDestroy = true;
                GameObject.Destroy(gameObject);
            }
        }
    }
}
