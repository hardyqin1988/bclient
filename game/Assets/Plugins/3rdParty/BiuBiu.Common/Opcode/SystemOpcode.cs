namespace BiuBiu.Common.Opcode
{
    public class SystemOpcode
    {
        public class CLIENT
        {
            // OPCODE_START仅用于方便变动整块Opcode。不应实际使用OPCODE_START。
            public const int OPCODE_START            = 0;
            public const int LOGIN_VALIDATE_REQUEST  = OPCODE_START + 2;
            public const int SELECT_ROLE             = OPCODE_START + 3;

            public const int REQUEST_SERVER_TIME     = OPCODE_START + 17;
            public const int GM_REQUEST              = OPCODE_START + 18;

            public const int ROLE_OFFLINE            = OPCODE_START + 5;
            public const int INIT_ROLE_FINISH        = OPCODE_START + 101;
            public const int QUERY_ROLE              = OPCODE_START + 102;
            public const int SET_ROLE_NAME           = OPCODE_START + 103;
            public const int GET_ENTITY              = OPCODE_START + 106;
            public const int LOAD_RECORD_ROWS        = OPCODE_START + 107;

            public const int CUSTOM_MSG_REQUEST      = OPCODE_START + 2016;

            public const int OPCODE_END              = 10000;
        }

        public class SERVER
        {
            // OPCODE_START仅用于方便变动整块Opcode。不应实际使用OPCODE_START。
            public const int OPCODE_START            = 0; //0x20000000;
            public const int SHAKE_HANDS             = OPCODE_START + 1;
            public const int LOGIN_VALIDATE_RESPONSE = OPCODE_START + 2;

            public const int INIT_ROLE               = OPCODE_START + 12;
            public const int ADD_ENTITY              = OPCODE_START + 13;
            public const int PROPERTY_CHANGED        = OPCODE_START + 14;
            public const int RECORD_CHANGED          = OPCODE_START + 15;
            public const int CONTAINER_CHANGED       = OPCODE_START + 16;

            public const int RESPONSE_SERVER_TIME    = OPCODE_START + 17;
            public const int GM_RESPONSE             = OPCODE_START + 18;

            public const int CUSTOM_MSG_RESPONSE     = OPCODE_START + 2016;
            public const int BATTLE_MSG_RESPONSE     = OPCODE_START + 2017;
            public const int SOCIAL_MSG_RESPONSE     = OPCODE_START + 2018;

            public const int SYSTEM_INFO             = OPCODE_START + 3016;
            
        }
    }
}
