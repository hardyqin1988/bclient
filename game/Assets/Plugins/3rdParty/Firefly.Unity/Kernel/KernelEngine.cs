using Firefly.Core.Config;
using Firefly.Core.Data;
using Firefly.Core.Interface;
using Firefly.Core.Variant;
using Firefly.Unity.Databind;
using Firefly.Unity.Global;
using System.Collections;
using UnityEngine;

namespace Firefly.Unity.Kernel
{
    public class KernelEngine : SingletonEngine<KernelEngine>, IEngine
    {
        public CKernel    Kernel     { get; private set; }

        public PersistID  MainRole   { get; set; }

        public Definition Define     { get; private set; }

        private void HandleRecordData(IKernel kernel, PersistID self, VariantList args)
        {
            string record_name = args.SubtractString();

            DatabindEngine.Instance.CallRecord(self, record_name, args);
        }

        private void HandlePropertyData(IKernel kernel, PersistID self, VariantList args)
        {
            string property_name = args.SubtractString();

            DatabindEngine.Instance.CallProperty(self, property_name, args);
        }

        protected override IEnumerator Startup()
        {
            Define = Definition.Load(AssetPath.GameConfigPath);

            yield return new WaitForFixedUpdate();

            Kernel = new CKernel(Define);

            Kernel.RegisterDataTypeCallback(DataType.Property, HandlePropertyData);
            Kernel.RegisterDataTypeCallback(DataType.Record, HandleRecordData);

            yield return new WaitForFixedUpdate();
        }

        protected override IEnumerator Shutdown()
        {
            yield return new WaitForFixedUpdate();
        }
    }
}
