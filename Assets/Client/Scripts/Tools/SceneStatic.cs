using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public static class SceneStatic
{
    private const string MENU = "menu";
    private const string GAME = "game";
    private const string EDITOR = "editor";
    //private const string STORE = "store";

    private static readonly LoadSceneParameters single = new LoadSceneParameters(LoadSceneMode.Single, LocalPhysicsMode.None);
    private static readonly LoadSceneParameters additive = new LoadSceneParameters(LoadSceneMode.Additive, LocalPhysicsMode.None);

    public static void LoadMenu()
    {
        SceneManager.LoadScene(MENU, single);
    }
    public static void LoadGame(string name_level)
    {
        SceneManager.LoadScene(GAME, single);
    }
    public static void LoadGameInEditor()
    {
        SceneManager.LoadScene(GAME, additive);
    }
    public static void LoadEditor()
    {
        SceneManager.LoadScene(EDITOR, single);
    }
}
