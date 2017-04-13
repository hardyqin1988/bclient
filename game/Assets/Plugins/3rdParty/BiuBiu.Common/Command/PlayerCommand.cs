namespace BiuBiu.Common.Command
{
    public class PLAYER_COMMAND : COMMAND
    {
        public const int ACCEPT_QUEST         = PLAYER_START_COMMAND + 1;         // 接受任务   
        public const int SUBMIT_QUEST         = PLAYER_START_COMMAND + 2;         // 提交任务
        public const int COMPLETE_QUEST       = PLAYER_START_COMMAND + 3;         // 完成任务
        public const int REFRESH_QUEST        = PLAYER_START_COMMAND + 4;         // 重置任务
        public const int REQUEST_AWARD        = PLAYER_START_COMMAND + 10;        // 请求奖励
        public const int ADD_OBJECTIVE        = PLAYER_START_COMMAND + 11;        // 添加行为
        public const int UPDATE_OBJECTIVE     = PLAYER_START_COMMAND + 12;        // 更新行为
        public const int DEL_OBJECTIVE        = PLAYER_START_COMMAND + 13;        // 删除行为
        public const int UPGRADE_PLAYER_LEVEL = PLAYER_START_COMMAND + 33;        // 玩家升级
        public const int ADD_EXP              = PLAYER_START_COMMAND + 36;        // 添加经验

        public const int ACCEPT_ACHIEVEMENT   = PLAYER_START_COMMAND + 123;       // 接受成就   
        public const int SUBMIT_ACHIEVEMENT   = PLAYER_START_COMMAND + 124;       // 提交成就
        public const int COMPLETE_ACHIEVEMENT = PLAYER_START_COMMAND + 125;       // 完成成就
    }
}
