using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using PhotonHashtable = ExitGames.Client.Photon.Hashtable;

public static class CustomProperty
{
    public const string READY = "Ready";
    public const string LOAD = "Load";
    public const string TEAM = "Team";
    public const string CLIMBER = "Climber";
    public const string NUMBER = "Number";
    public const string LOADTIME = "LoadTime";
    public const string COUNTDOWNTIME = "CountDownTime";
    public const string ROUND = "Round";

    public static bool GetReady(this Player player)
    {
        PhotonHashtable property = player.CustomProperties;
        if (property.ContainsKey(READY))
            return (bool)property[READY];
        else
            return false;
    }

    public static void SetReady(this Player player, bool ready)
    {
        PhotonHashtable property = new PhotonHashtable();
        property[READY] = ready;
        player.SetCustomProperties(property);
    }

    public static bool GetLoad(this Player player)
    {
        PhotonHashtable property = player.CustomProperties;
        if (property.ContainsKey(LOAD))
            return (bool)property[LOAD];
        else
            return false;
    }

    public static void SetLoad(this Player player, bool load)
    {
        PhotonHashtable property = new PhotonHashtable();
        property[LOAD] = load;
        player.SetCustomProperties(property);
    }

    public static PlayerTeam GetPlayerTeam(this Player player)
    {
        PhotonHashtable property = player.CustomProperties;
        if (property.ContainsKey(TEAM))
            return (PlayerTeam)property[TEAM];
        else
            return PlayerTeam.None;
    }

    public static void SetPlayerTeam(this Player player, PlayerTeam team)
    {
        PhotonHashtable property = new PhotonHashtable();
        property[TEAM] = team;
        player.SetCustomProperties(property);
    }

    public static Climber GetClimber(this Player player)
    {
        PhotonHashtable property = player.CustomProperties;
        if (property.ContainsKey(CLIMBER))
            return (Climber)property[CLIMBER];
        else
            return Climber.None;
    }

    public static void SetClimber(this Player player, Climber climber)
    {
        PhotonHashtable property = new PhotonHashtable();
        property[CLIMBER] = climber;
        player.SetCustomProperties(property);
    }

    public static int GetLoadTime(this Room room)
    {
        PhotonHashtable property = room.CustomProperties;
        if (property.ContainsKey(LOADTIME))
            return (int)property[LOADTIME];
        else
            return -1;
    }

    public static void SetLoadTime(this Room room, int loadTime)
    {
        PhotonHashtable property = new PhotonHashtable();
        property[LOADTIME] = loadTime;
        room.SetCustomProperties(property);
    }

    public static int GetCountDownTime(this Room room)
    {
        PhotonHashtable property = room.CustomProperties;
        if (property.ContainsKey(COUNTDOWNTIME))
            return (int)property[COUNTDOWNTIME];
        else
            return -1;
    }

    public static void SetCountDownTime(this Room room, int countDownTime)
    {
        PhotonHashtable property = new PhotonHashtable();
        property[COUNTDOWNTIME] = countDownTime;
        room.SetCustomProperties(property);
    }

    public static Round GetCurrentRound(this Room room)
    {
        PhotonHashtable property = room.CustomProperties;
        if (property.ContainsKey(ROUND))
            return (Round)property[ROUND];
        else
            return Round.ROUND1;
    }

    public static void SetCurrentRound(this Room room, Round round)
    {
        PhotonHashtable property = new PhotonHashtable();
        property[ROUND] = round;
        room.SetCustomProperties(property);
    }
}