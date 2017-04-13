namespace BiuBiu.Common.Opcode
{
    public class ArenaOpcode : SystemOpcode
    {
        public new class CLIENT : SystemOpcode.CLIENT
        {
            // OPCODE_START仅用于方便变动整块Opcode。不应实际使用OPCODE_START。
            public const int ARENA_START_CODE           = OPCODE_END + 1;

            public const int MOVEMENT                   = ARENA_START_CODE + 1;

            public const int JOIN_SCUFFLE_ARENA         = ARENA_START_CODE + 2;
            public const int QUIT_SCUFFLE_ARENA         = ARENA_START_CODE + 3;
        }

        public new class SERVER : SystemOpcode.SERVER
        {
            // OPCODE_START仅用于方便变动整块Opcode。不应实际使用OPCODE_START。
            public const int ARENA_START_CODE = 1;
        }
    }
}
