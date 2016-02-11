﻿using UnityEngine;
using System.Collections;

namespace DotOriko.Core {
    public abstract class DotOrikoSingleton<T> : DotOrikoComponent where T :MonoBehaviour {
        public static T Instance {
            get {
                if (_instance == null) CreateInstance();
                return _instance;
            }
        }

        protected static T _instance;

        private static void CreateInstance() {
            var g = new GameObject();
            g.name = typeof(T).Name + "_singletone";
            _instance = g.AddComponent<T>();
        }

        protected override void OnInitialize() {
            base.OnInitialize();
            DontDestroyOnLoad(this);
        }

        protected override void OnStart() {
            base.OnStart();
        }

        protected override void OnUpdate() {
            base.OnUpdate();
        }

        protected override void OnScheduledUpdate() {
            base.OnScheduledUpdate();
        }
    }
}