﻿using System;
using UnityEngine;

namespace CryptoQuest.Audio
{
    public class AudioManager : MonoBehaviour
    {
        [Header("SoundEmitters Pool")]
        [SerializeField] private SoundEmitterPoolSO _soundEmitterPool = default;

        [SerializeField] private int _soundEmitterPoolSize = 10;

        private void Awake()
        {
            _soundEmitterPool.Create(_soundEmitterPoolSize, this.transform);
            // var soundEmitter = _soundEmitterPool.Request();
        }
    }
}