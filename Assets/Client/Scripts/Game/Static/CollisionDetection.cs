using System.Collections;
using Unity.Mathematics;
using UnityEngine;
using Data;
using Game.Core;

namespace Game.Static
{
    public static class CollisionDetection
    {
        public static bool GetDetection(float3 pos, float3 rot, float3 sca, float3 player, SpriteType type)
        {
            switch (type)
            {
                case SpriteType.Square:
                    return CircleRectangle(pos, rot, sca, player);
                case SpriteType.Circle:
                    return CircleEllipse(pos, rot, sca, player);
                default:
                    return false;
            }
        }

        public static bool CircleRectangle(float3 pos, float3 rot, float3 sca, float3 player)
        {
            player = GameUtils.RotateVector(player - pos, -rot.z);

            float distanceX = Mathf.Abs(player.x);
            float distanceY = Mathf.Abs(player.y);

            if (distanceX > (sca.x / 2 + Player.RADIUS)) { return false; }
            if (distanceY > (sca.y / 2 + Player.RADIUS)) { return false; }
            if (distanceX <= (sca.x / 2)) { return true; }
            if (distanceY <= (sca.y / 2)) { return true; }

            float cDist_sqX = distanceX - sca.x / 2;
            float cDist_sqY = distanceY - sca.y / 2;
            float cDist_sq = cDist_sqX * cDist_sqX + cDist_sqY * cDist_sqY;

            return cDist_sq <= Player.RADIUS * Player.RADIUS;
        }

        public static bool CircleEllipse(float3 pos, float3 rot, float3 sca, float3 player)
        {
            return false;
        }
    }
}