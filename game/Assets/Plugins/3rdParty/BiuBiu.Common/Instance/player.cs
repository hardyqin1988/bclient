namespace BiuBiu.Common.Instance
{
    public class player
    {
        public string pid { get; set; }
        public int entry { get; set; }
        public int level { get; set; }
        public int cur_exp { get; set; }
        public string name { get; set; }
    }

    public class player_achievement_record
    {
        public string pid { get; set; }
        public int row { get; set; }
        public int achievement_id { get; set; }
        public byte achievement_status { get; set; }
    }

    public class player_achievement_rewarded_record
    {
        public string pid { get; set; }
        public int row { get; set; }
        public int achievement_id { get; set; }
    }

    public class player_objective_record
    {
        public string pid { get; set; }
        public int row { get; set; }
        public int objective_id { get; set; }
        public byte objective_status { get; set; }
        public int cur_progress { get; set; }
        public int max_progress { get; set; }
        public byte parent_type { get; set; }
        public int parent_id { get; set; }
    }

    public class player_quest_record
    {
        public string pid { get; set; }
        public int row { get; set; }
        public int quest_id { get; set; }
        public byte quest_status { get; set; }
        public long quest_accept_time { get; set; }
    }

    public class player_quest_rewarded_record
    {
        public string pid { get; set; }
        public int row { get; set; }
        public int quest_id { get; set; }
    }


}
