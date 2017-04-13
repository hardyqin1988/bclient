namespace BiuBiu.Common.Opcode
{
    public class CustomOpcode
    {
        public class CLIENT
        {
            // OPCODE_START仅用于方便变动整块Opcode。不应实际使用OPCODE_START。
            public const int CUSTOM_START_CODE          = 1;

            public const int MOVEMENT                   = CUSTOM_START_CODE + 1;
            public const int SUBMIT_QUEST               = CUSTOM_START_CODE + 2;
            public const int SUBMIT_ACHIEVEMENT         = CUSTOM_START_CODE + 3;
        }

        public class SERVER
        {
            // OPCODE_START仅用于方便变动整块Opcode。不应实际使用OPCODE_START。
            public const int CUSTOM_START_CODE = 1;
        }
    }
}
