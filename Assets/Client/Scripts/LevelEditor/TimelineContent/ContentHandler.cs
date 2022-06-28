using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LevelEditor.TimelineContent
{
    public abstract class ContentHandler : MonoBehaviour
    {
        public virtual void UpdateUI(int startFrame, int endFrame, int startHeigth, int endHeigth)
        {

        }
    }
}