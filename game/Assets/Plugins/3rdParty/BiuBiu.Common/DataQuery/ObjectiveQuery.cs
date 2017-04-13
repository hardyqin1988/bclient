using Firefly.Core.Config;
using Firefly.Core.Variant;

namespace BiuBiu.Common.DataQuery
{
    public class ObjectiveData : IData<int>
    {
        private int _Entry = 0;
        public int Entry
        {
            get
            {
                if (_Entry == 0)
                {
                    _Entry = Map.GetInt(ObjectiveDataDef.objective_id);
                }
                return _Entry;
            }
        }

        public VariantMap Map { get; set; }

        public void OnLoadFinish()
        {
        }
    }

    public class ObjectiveQuery : BaseQuery<int, ObjectiveData>
    {
        
    }
}
