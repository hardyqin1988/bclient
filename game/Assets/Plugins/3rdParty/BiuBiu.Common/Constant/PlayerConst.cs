namespace BiuBiu.Common.Constant
{
    public enum QuestStatus : byte
    {
        None                      = 0,
        Complete                  = 1,                // 完成
        Doing                     = 2,                // 执行中
        Failed                    = 3,                // 失败
        Rewarded                  = 4,                // 已经交付
    }

    public enum QuestAcceptResult : byte
    {
        Success                   = 0,
        Failed_InvalidQuest       = 1,
        Failed_LowLevel           = 2,
        Failed_Rewarded           = 3,
        Failed_Haven              = 5,
        Failed_InventoryNotEnough = 6,
        Failed_LackTime           = 7,                  //时间没到
    }

    public enum QuestSubmitResult : byte
    {
        Success                   = 0,
        Failed_InvalidQuest       = 1,
        Failed_NotHaven           = 2,
        Failed_UnComplete         = 3,
        Failed_InventoryNotEnough = 4,
    }

    public enum QuestType : byte
    {
        None                      = 0,
        MainQuest                 = 1,                //主线任务
        SideQuest                 = 2,                //支线任务
        DailyQuest                = 3,                //日常任务
    }

    public enum ObjectiveStatus : byte
    {
        None                      = 0,
        Complete                  = 1,                // 完成
        Doing                     = 2,                // 执行中
        Failed                    = 3,                // 失败
    }

    public enum ObjectiveParent : byte
    {
        Quest                     = 1,
        Achievement               = 2,
    }

    public enum ObjectiveType : byte
    {
        None                      = 0,
    }
}
