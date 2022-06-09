using System.Collections;
using UnityEngine;
using Data;

namespace Game.Static
{
    public static class CollisionDetection
    {
        public static bool GetDetection(Vector3 pos, Vector3 rot, Vector3 sca, Vector3 player, SpriteType type)
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

        public static bool CircleRectangle(Vector3 pos, Vector3 rot, Vector3 sca, Vector3 player)
        {
            player = GameUtils.RotateVector(player - pos, -rot.z);

            float distanceX = Mathf.Abs(player.x);
            float distanceY = Mathf.Abs(player.y);

            if (distanceX > (sca.x / 2 + GameUtils.PLAYER_RADIUS)) { return false; }
            if (distanceY > (sca.y / 2 + GameUtils.PLAYER_RADIUS)) { return false; }
            if (distanceX <= (sca.x / 2)) { return true; }
            if (distanceY <= (sca.y / 2)) { return true; }

            float cDist_sqX = distanceX - sca.x / 2;
            float cDist_sqY = distanceY - sca.y / 2;
            float cDist_sq = cDist_sqX * cDist_sqX + cDist_sqY * cDist_sqY;

            return cDist_sq <= GameUtils.PLAYER_RADIUS * GameUtils.PLAYER_RADIUS;
        }

        public static bool CircleEllipse(Vector3 pos, Vector3 rot, Vector3 sca, Vector3 player)
        {
            return false;
        }
    }
}