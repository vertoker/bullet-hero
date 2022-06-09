using Leopotam.EcsLite;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Game
{
    public class Loader : MonoBehaviour
    {
        private EcsWorld world;
        private EcsSystems systems;


        private void Start()
        {
            world = new EcsWorld();
            systems = new EcsSystems(world);

        }
    }

#if UNITY_EDITOR
    public class LoaderGUI : Editor
    {
        public override void OnInspectorGUI()
        {

        }
    }
#endif
}