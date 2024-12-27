using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public GameMode GameMode { get; private set; }

    public GameState GameState { get; private set; }

    private GameObject current_game;


    private void Awake()
    {
        Instance = this;
    }

    public void ChangeGameState(GameState game_state)
    {
        GameState = game_state;
        Events.onGameStateChange?.Invoke(game_state);
    }

    public void ChangeGameMode(GameMode new_gameMode)
    {
        GameMode = new_gameMode;
        DataController.instance.LoadData(new_gameMode);
        Events.onGameModeChange?.Invoke(new_gameMode);
    }

    public void EnterGameMode(int index)
    {
        var data = DataController.instance.so_ListGameType.data_game_type_list[index];
        var cur_level = PrefsData.GetCurrentLevelCount(data.gameMode);
        if (cur_level >= DataController.instance.TotalLevelByGameMode(data.gameMode))
        {
            ExitGameMode();
            return;
        }

        UIController.Instance.uiGameplayMask.OnEnterGameMode();
        if (current_game is not null)
            DestroyImmediate(current_game);

        ChangeGameMode(data.gameMode);
        var current_level = Instantiate(data.gameTypePrefabs, UIController.Instance.uiGameplayContainer);
        current_level.StartLevel(cur_level);
        current_game = current_level.gameObject;

    }

    public void ExitGameMode()
    {
        ChangeGameMode(GameMode.None);
        ChangeGameState(GameState.Home);
        if (current_game is not null)
            DestroyImmediate(current_game);
    }

}

public enum GameMode
{
    None = 0, Cryptogram1 = 1, Cryptogram2 = 2, Sudoku = 3, Wordscapes = 4, PictureCross = 7
}


public enum GameState
{
    Home = 0, InProgress = 1, Win = 2, Lose = 3, Loading = 4, Hint = 5
}