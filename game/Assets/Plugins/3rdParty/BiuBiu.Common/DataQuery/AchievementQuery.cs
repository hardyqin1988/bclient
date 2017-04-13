using BiuBiu.Common.Constant;
using Firefly.Core.Config;
using Firefly.Core.Variant;
using System.Collections.Generic;

namespace BiuBiu.Common.DataQuery
{
    public class AchievementData : IData<int>
    {
        private int _Entry = 0;
        public int Entry
        {
            get
            {
                if (_Entry == 0)
                {
                    _Entry = Map.GetInt(AchievementDataDef.achievement_id);
                }
                return _Entry;
            }
        }

        public VariantMap Map { get; set; }

        public List<int> Objectives { get; set; }

        public void OnLoadFinish()
        {
            Objectives = new List<int>();
            for (int j = 1; j <= AchievementQuery.OBJECTIVE_MAX_NUM; j++)
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

    public class AchievementQuery : BaseQuery<int, AchievementData>, IDataQuery
    {
        public const int OBJECTIVE_MAX_NUM = 3;

        private static Dictionary<ObjectiveType, VariantList> _ObjectiveTypeDic = new Dictionary<ObjectiveType, VariantList>();

        public static VariantList GetAchievements(ObjectiveType type)
        {
            VariantList handle;
            if (!_ObjectiveTypeDic.TryGetValue(type, out handle))
            {
                return VariantList.Empty;
            }

            return handle;

        }

        public void OnLoadFinish()
        {
            foreach (AchievementData achievement_data in GetAll())
            {
                for (int i = 0; i < achievement_data.Objectives.Count; ++i)
                {
                    ObjectiveData objective_data = ObjectiveQuery.GetData(achievement_data.Objectives[i]);
                    if (objective_data == null)
                    {
                        return;
                    }
                    ObjectiveType type = (ObjectiveType)objective_data.Map.GetByte(ObjectiveDataDef.objective_type);
                    VariantList handler;
                    if (!_ObjectiveTypeDic.TryGetValue(type, out handler))
                    {
                        handler = VariantList.New();
                        _ObjectiveTypeDic.Add(type,handler);
                    }
                    handler.Append(achievement_data.Entry);
                }
            }
        }
    }
}
