namespace Firefly.Network.Constant
{
    public enum LoginValidateResult : byte
    {
        LogonSuccess             = 0,
        LogonFail_NullUUID       = 1,
        LogonFail_IncorrectToken = 2,
    }

    public enum RegisterAuthResult : byte
    {
        DuplicateUid = 0,
        Success      = 1,
    }
}
