namespace BiuBiu.Common.Entity
{
    public class RoleEntity
    {
        public const string TYPE = "role";
        
        public class Properties
        {

            public const string ENTRY = "entry";

            public const string LEVEL = "level";

            public const string CUR_EXP = "cur_exp";

            public const string NAME = "name";

            public const string HP = "hp";

            public const string POSITION = "position";
        }

        public class Records
        {
            public class AchievementRecord
            {
                public const string RECORD_NAME = "achievement_record";
                public const int COL_ACHIEVEMENT_ID = 0;
                public const int COL_ACHIEVEMENT_STATUS = 1;
            }

            public class AchievementRewardedRecord
            {
                public const string RECORD_NAME = "achievement_rewarded_record";
                public const int COL_ACHIEVEMENT_ID = 0;
            }

            public class ObjectiveRecord
            {
                public const string RECORD_NAME = "objective_record";
                public const int COL_OBJECTIVE_ID = 0;
                public const int COL_OBJECTIVE_STATUS = 1;
                public const int COL_CUR_PROGRESS = 2;
                public const int COL_MAX_PROGRESS = 3;
                public const int COL_PARENT_TYPE = 4;
                public const int COL_PARENT_ID = 5;
            }

            public class QuestRecord
            {
                public const string RECORD_NAME = "quest_record";
                public const int COL_QUEST_ID = 0;
                public const int COL_QUEST_STATUS = 1;
                public const int COL_QUEST_ACCEPT_TIME = 2;
            }

            public class QuestRewardedRecord
            {
                public const string RECORD_NAME = "quest_rewarded_record";
                public const int COL_QUEST_ID = 0;
            }

            public class EquipRecord
            {
                public const string RECORD_NAME = "equip_record";
                public const int COL_EQUIP_INDEX = 0;
                public const int COL_EQUIP_PID = 1;
            }


        }

    }
}