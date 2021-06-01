using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Camera cam;
    public Runtime runtime;
    public InputController inputController;
    private Checkpoint activeCheckpoint;

    public void Start()
    {
        LevelManager.Load("0 demo level");
        activeCheckpoint = LevelManager.level.Checkpoints[0];

        inputController.StartInputController(cam.aspect);
        runtime.LaunchLevel(cam.aspect);
    }

    public void Save(Checkpoint checkpoint)
    {
        activeCheckpoint = checkpoint;
    }
}

public delegate void SaveCheckpoint(Checkpoint checkpoint);