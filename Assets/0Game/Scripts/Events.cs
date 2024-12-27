public static class Events
{
    
    public delegate void OnGameModeChange(GameMode game_mode_changed);
    public static OnGameModeChange onGameModeChange;

    
    public delegate void OnGameStateChange(GameState game_state);
    public static OnGameStateChange onGameStateChange;   

    public delegate void OnStartLevel(int so_index);
    public static OnStartLevel onStartLevel;

    public delegate void OnEndLevel(bool win);
    public static OnEndLevel onEndLevel;
}
