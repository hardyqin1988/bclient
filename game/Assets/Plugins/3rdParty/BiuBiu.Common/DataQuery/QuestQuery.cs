using BiuBiu.Common.Constant;
using Firefly.Core.Config;
using Firefly.Core.Variant;
using System.Collections.Generic;

namespace BiuBiu.Common.DataQuery
{
    public class QuestData : IData<int>
    {
        private int _Entry = 0;
        public int Entry
        {
            get
            {
                if (_Entry == 0)
                {
                    _Entry = Map.GetInt(QuestDataDef.quest_id);
                }
                return _Entry;
            }
        }

        public VariantMap Map { get; set; }

        public List<int> Objectives { get; set; }

        public void OnLoadFinish()
        {
            Objectives = new List<int>();
            for (int j = 1; j <= QuestQuery.OBJECTIVE_MAX_NUM; j++)
            {
                int objective_id = Map.GetInt("objective_id_" + j);
                if (objective_id <= 0)
                {
                    continue;
                }

                Objectives.Add(objective_id);
            }
        }
    }

    public class QuestQuery : BaseQuery<int, QuestData>, IDataQuery
    {
        public const int OBJECTIVE_MAX_NUM = 3;

        private static Dictionary<int, List<int>> _RequiredLevelQuests = new Dictionary<int, List<int>>();

        public void OnLoadFinish()
        {
            foreach (QuestData quest_data in GetAll())
            {
                int required_level = quest_data.Map.GetInt(QuestDataDef.required_level);

                List<int> list = null;

                if (!_RequiredLevelQuests.TryGetValue(required_level, out list))
                {
                    list = new List<int>();
                    _RequiredLevelQuests.Add(required_level, list);
                }

                if (list.Contains(quest_data.Entry))
                {
                    continue;
                }

                list.Add(quest_data.Entry);
            }
        }

        public static VariantList GetQuests(QuestType type)
        {
            VariantList quests = VariantList.New();

            foreach (QuestData quest_data in GetAll())
            {
                QuestType quest_type = (QuestType)quest_data.Map.GetByte(QuestDataDef.quest_type);

                if (quest_type != type)
                {
                    continue;
                }

                quests.Append(quest_data.Entry);
            }

            return quests;
        }

        public static VariantList GetQuests(int required_level)
        {
            VariantList quests = VariantList.New();

            for (int level = 0; level <= required_level; level++)
            {
                List<int> list = null;

                if (!_RequiredLevelQuests.TryGetValue(required_level, out list))
                {
                    continue;
                }

                for (int i = 0; i < list.Count; i++)
                {
                    quests.Append(list[i]);
                }
            }

            return quests;
        }

        public static QuestType GetQuestType(int quest_id)
        {
            QuestData questData = GetData(quest_id);
            if (questData != null)
            {
                byte quest_type = questData.Map.GetByte(QuestDataDef.quest_type);
                return (QuestType)quest_type;
            }
            return QuestType.None;
        }
    }
}
