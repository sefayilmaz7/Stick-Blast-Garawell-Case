public static class PrefKeys
{
    public static readonly string PLAYER_CURRENCY = "PLAYER_CURRENCY";
    public static readonly string PLAYER_LEVEL = "PLAYER_LEVEL";
}



public static class SceneKeys
{
    public const string SPLASH_SCENE = "Splash";
    public const string MAIN_MENU_SCENE = "Menu";
    public const string IN_GAME_SCENE = "InGame";


    public static bool IsValidSceneKey(string sceneName)
    {
        return sceneName switch
        {
            SPLASH_SCENE => true,
            MAIN_MENU_SCENE => true,
            IN_GAME_SCENE => true,

            _ => false,
        };
    }
}
