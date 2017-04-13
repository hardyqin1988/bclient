namespace BiuBiu.Common.Entity
{
    public class GladiatorEntity
    {
        public const string TYPE = "gladiator";
        
        public class Properties
        {

            public const string HP = "hp";

            public const string POSITION = "position";
        }

        public class Records
        {
            public class EquipRecord
            {
                public const string RECORD_NAME = "equip_record";
                public const int COL_EQUIP_INDEX = 0;
                public const int COL_EQUIP_PID = 1;
            }


        }

    }
}