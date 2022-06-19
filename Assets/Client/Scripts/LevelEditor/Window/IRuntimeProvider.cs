using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Provider;

namespace LevelEditor.Windows
{
    public interface IRuntimeProvider
    {
        public void Init(Runtime runtime);
    }
}