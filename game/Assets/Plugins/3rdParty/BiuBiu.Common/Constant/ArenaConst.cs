namespace BiuBiu.Common.Constant
{
    public enum ArenaMode : int
    {
        None,
        Training    = 1,
        Scuffle     = 2,
    }

    public static class ArenaConst
    {
        public const int SINGLE_BATTLE_NODE_FLAG    = 600000;
        public const int TEAM_BATTLE_NODE_FLAG      = 700000;

        public const int LOBBY_REALM_CAPACITY = 1000;

        public const int SCUFFLE_ARENA_CAPACITY = 15;
    }
}
