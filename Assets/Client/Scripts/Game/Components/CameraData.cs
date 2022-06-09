using System.Collections.Generic;
using UnityEngine;
using System;

namespace Game.Components
{
    [Serializable]
    public struct CameraData
    {
        [SerializeField] private List<Zoom> zoom;
        [SerializeField] private List<Pos> pos;
        [SerializeField] private List<Rot> rot;

        public CameraData(List<Zoom> zoom, List<Pos> pos, List<Rot> rot)
        {
            this.zoom = zoom;
            this.pos = pos;
            this.rot = rot;
        }
    }
}