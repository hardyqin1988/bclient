using System.Collections;

namespace Firefly.Unity.Stage
{
    public abstract class StageBase
    {
        internal string Name;

        public abstract IEnumerator OnLoad();

        public abstract IEnumerator OnUnload();
    }

}
