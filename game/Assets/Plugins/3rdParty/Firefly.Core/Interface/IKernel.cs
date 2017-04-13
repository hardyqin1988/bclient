#region License
/*****************************************************************************
 *MIT License
 *
 *Copyright (c) 2017 cathy33

 *Permission is hereby granted, free of charge, to any person obtaining a copy
 *of this software and associated documentation files (the "Software"), to deal
 *in the Software without restriction, including without limitation the rights
 *to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 *copies of the Software, and to permit persons to whom the Software is
 *furnished to do so, subject to the following conditions:

 *The above copyright notice and this permission notice shall be included in all
 *copies or substantial portions of the Software.

 *THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 *IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 *FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 *AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 *LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 *OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
 *SOFTWARE.
 *****************************************************************************/
#endregion

using Firefly.Core.Config;
using Firefly.Core.Data;
using Firefly.Core.Event;
using Firefly.Core.Kernel;
using Firefly.Core.Variant;
using System;

namespace Firefly.Core.Interface
{
    public interface IKernel
    {
        #region ------ ------ ------ ------Core------ ------ ------ ------
        EntityDef GetEntityDef(string type);
        void CreateModule<T>() where T : IModule;
        #endregion

        #region ------ ------ ------ ------Callback------ ------ ------ ------
        void RegisterEntityCallback(string entity_type, EntityEvent event_enum, EventCallback handler);
        void RegisterPropertyCallback(string entity_type, string property_name, PropertyEvent event_enum, EventCallback handler);
        void RegisterRecordCallback(string entity_type, string record_name, RecordEvent event_enum, EventCallback handler);
        void RegisterDataTypeCallback(DataType data_type, EventCallback handler);
        #endregion

        #region ------ ------ ------ ------Entity------ ------ ------ ------
        bool Exists(PersistID pid);
        string GetType(PersistID pid);

        PersistID CreateEntity(string define, PersistID root, VariantList args = null);
        PersistID CreateEntity(PersistID pid, string define, VariantList args = null);

        void DestroyEntity(PersistID pid);
        VariantList ToProto(PersistID pid);
        #endregion

        #region ------ ------ ------ ------Property------ ------ ------ ------
        void SetPropertyBool(PersistID pid, string property_name, bool value);
        void SetPropertyByte(PersistID pid, string property_name, byte value);
        void SetPropertyInt(PersistID pid, string property_name, int value);
        void SetPropertyLong(PersistID pid, string property_name, long value);
        void SetPropertyFloat(PersistID pid, string property_name, float value);
        void SetPropertyString(PersistID pid, string property_name, string value);
        void SetPropertyPid(PersistID pid, string property_name, PersistID value);
        void SetPropertyBytes(PersistID pid, string property_name, Bytes value);
        void SetPropertyInt2(PersistID pid, string property_name, Int2 value);
        void SetPropertyInt3(PersistID pid, string property_name, Int3 value);

        bool      GetPropertyBool(PersistID pid, string property_name);
        byte      GetPropertyByte(PersistID pid, string property_name);
        int       GetPropertyInt(PersistID pid, string property_name);
        long      GetPropertyLong(PersistID pid, string property_name);
        float     GetPropertyFloat(PersistID pid, string property_name);
        string    GetPropertyString(PersistID pid, string property_name);
        PersistID GetPropertyPid(PersistID pid, string property_name);
        Bytes     GetPropertyBytes(PersistID pid, string property_name);
        Int2      GetPropertyInt2(PersistID pid, string property_name);
        Int3      GetPropertyInt3(PersistID pid, string property_name);
        #endregion

        #region ------ ------ ------ ------Record------ ------ ------ ------
        void ClearReocrd(PersistID pid, string record_name);
        int AddRecordRow(PersistID pid, string record_name, VariantList value);
        void SetRecordRow(PersistID pid, string record_name, int row, VariantList value);
        VariantList GetRecordRow(PersistID pid, string record_name, int row);
        VariantList GetRecordRows(PersistID pid, string record_name);
        void DelRecordRow(PersistID pid, string record_name, int row);

        int FindRecordRowBool(PersistID pid, string record_name, int col, bool value);
        int FindRecordRowByte(PersistID pid, string record_name, int col, byte value);
        int FindRecordRowInt(PersistID pid, string record_name, int col, int value);
        int FindRecordRowFloat(PersistID pid, string record_name, int col, float value);
        int FindRecordRowLong(PersistID pid, string record_name, int col, long value);
        int FindRecordRowString(PersistID pid, string record_name, int col, string value);
        int FindRecordRowPid(PersistID pid, string record_name, int col, PersistID value);
        int FindRecordRowBytes(PersistID pid, string record_name, int col, Bytes value);
        int FindRecordRowInt2(PersistID pid, string record_name, int col, Int2 value);
        int FindRecordRowInt3(PersistID pid, string record_name, int col, Int3 value);

        void SetRecordBool(PersistID pid, string record_name, int row, int col, bool value);
        void SetRecordByte(PersistID pid, string record_name, int row, int col, byte value);
        void SetRecordInt(PersistID pid, string record_name, int row, int col, int value);
        void SetRecordLong(PersistID pid, string record_name, int row, int col, long value);
        void SetRecordFloat(PersistID pid, string record_name, int row, int col, float value);
        void SetRecordString(PersistID pid, string record_name, int row, int col, string value);
        void SetRecordPid(PersistID pid, string record_name, int row, int col, PersistID value);
        void SetRecordBytes(PersistID pid, string record_name, int row, int col, Bytes value);
        void SetRecordInt2(PersistID pid, string record_name, int row, int col, Int2 value);
        void SetRecordInt3(PersistID pid, string record_name, int row, int col, Int3 value);

        bool      GetRecordBool(PersistID pid, string record_name, int row, int col);
        byte      GetRecordByte(PersistID pid, string record_name, int row, int col);
        int       GetRecordInt(PersistID pid, string record_name, int row, int col);
        long      GetRecordLong(PersistID pid, string record_name, int row, int col);
        float     GetRecordFloat(PersistID pid, string record_name, int row, int col);
        string    GetRecordString(PersistID pid, string record_name, int row, int col);
        PersistID GetRecordPid(PersistID pid, string record_name, int row, int col);
        Bytes     GetRecordBytes(PersistID pid, string record_name, int row, int col);
        Int2      GetRecordInt2(PersistID pid, string record_name, int row, int col);
        Int3      GetRecordInt3(PersistID pid, string record_name, int row, int col);
        #endregion

        #region ------ ------ ------ ------Log------ ------ ------ ------
        void Trace(string message);
        void Trace(string message, params object[] args);
        void Debug(string message);
        void Debug(string message, params object[] args);
        void Info(string message);
        void Info(string message, params object[] args);
        void Warn(string message);
        void Warn(string message, params object[] args);
        void Error(Exception ex);
        void Error(Exception ex, string message, params object[] args);
        void Fatal(Exception ex);
        void Fatal(Exception ex, string message, params object[] args);
        #endregion

        #region ------ ------ ------ ------Msg------ ------ ------ ------
        void RegisterCommandCallback(int command_msg, EventCallback handler);
        void RegisterCustomCallback(int custom_msg, EventCallback handler);
        #endregion

        #region ------ ------ ------ ------Timer------ ------ ------ ------
        void Update();
        void AddCountdown(PersistID pid, string countdown_name, long over_millseconds, TimerCallback countdown);
        void DelCountdown(PersistID pid, string countdown_name);
        void AddHeartbeat(PersistID pid, string heartbeat_name, long gap_millseconds, int count, TimerCallback heartbeat);
        void DelHeartbeat(PersistID pid, string heartbeat_name);
        #endregion
    }
}
