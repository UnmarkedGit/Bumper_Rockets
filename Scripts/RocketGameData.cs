using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketGameData
{
    //public const float ASTEROIDS_MIN_SPAWN_TIME = 5.0f;
    //public const float ASTEROIDS_MAX_SPAWN_TIME = 10.0f;

    //public const float PLAYER_RESPAWN_TIME = 4.0f;

    //public const int PLAYER_MAX_LIVES = 3;
    public const int PLAYER_MAX_SIZE = 20;

    //public const string PLAYER_LIVES = "PlayerLives";
    public const string PLAYER_SIZE = "PlayerSize";
    public const string PLAYER_READY = "IsPlayerReady";
    public const string PLAYER_LOADED_LEVEL = "PlayerLoadedLevel";

    public static int GetStartAbility(int argAbility)
    {
        switch (argAbility)
        {
            case 0: return 0;
            case 1: return 3;
            case 2: return 6;
            case 3: return 9;
        }

        return 0;
    }
}
