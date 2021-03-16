using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Camera cam;
    public Runtime runtime;
    public InputController inputController;
    [SerializeField] private List<Checkpoint> checkpoints;
    private Checkpoint activeCheckpoint;

    public void Start()
    {
        Level level = FileManager.Load("0 demo level");

        checkpoints = level.checkpoints;
        activeCheckpoint = new Checkpoint(true, "main", 0);

        for (int i = 0; i < checkpoints.Count; i++)
        {

        }

        inputController.StartInputController(cam.aspect);
        runtime.LaunchLevel(level.data, level.prefabs, level.effects, cam.aspect);
    }

    public void Update()
    {

    }

    public void Save(Checkpoint checkpoint)
    {
        activeCheckpoint = checkpoint;
    }
}

public delegate void SaveCheckpoint(Checkpoint checkpoint);