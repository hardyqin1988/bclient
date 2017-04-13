namespace BiuBiu.Common.Command
{
    public class COMMAND
    {
        public const int PLAYER_START_COMMAND = 1001;
        public const int PLAYER_END_COMMAND   = 1999;

        public const int ARENA_START_COMMAND  = 2001;
        public const int ARENA_END_COMMAND    = 2999;

        public static bool SatisfyPlayer(int command)
        {
            return (command > PLAYER_START_COMMAND && command < PLAYER_END_COMMAND);
        }

        public static bool SatisfyArena(int command)
        {
            return (command > ARENA_START_COMMAND && command < ARENA_END_COMMAND);
        }
    }
}
