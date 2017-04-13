using DevEditor.Custom;
using UnityEditor;

public class MenuItems
{
    [MenuItem("Development/Build/Pak/Build All")]
    static public void BuildAllPakToStreamingPath()
    {
        BuildPak.BuildAllPakToStreamingPath();
    }

    [MenuItem("Development/Build/Pak/Build Config")]
    static public void BuildConfigPakToStreamingPath()
    {
        BuildPak.BuildConfigPakToStreamingPath();
    }

    [MenuItem("Development/Build/Pak/Build Lua")]
    static public void BuildLuaPakToStreamingPath()
    {
        BuildPak.BuildLuaPakToStreamingPath();
    }

    [MenuItem("Development/Build/Pak/Build Bundle")]
    static public void BuildBundlePakToStreamingPath()
    {
        BuildPak.BuildBundlePakToStreamingPath();
    }

    [MenuItem("Development/AssetBundles/Build AssetBundles")]
    static public void BuildAssetBundles()
    {
        BuildBundle.BuildAssetBundles();
    }

    [MenuItem("Development/AssetBundles/Build Player (for use with engine code stripping)")]
    static public void BuildPlayer()
    {
        BuildBundle.BuildPlayer();
    }

    [MenuItem("Development/Check/Clear Extra CSharpCode")]
    public static void ClearExtraCSharpCode()
    {
        FileCheckTools.ClearExtraCSharpCode();
    }

    [MenuItem("Development/Check/Delate Empty Dir")]
    static void DelateEmptyDir()
    {
        FileCheckTools.DeleteEmptyDir();
    }

    [MenuItem("Development/Check/Reset All Bundle Names")]
    public static void ResetAllBundleNames()
    {
        BundleChecker.ResetAllBundleNames();
    }

    [MenuItem("Development/Synchro/ClientCore/UpdateRoleProperty")]
    public static void UpdateRoleProperty()
    {
        SyncClientCore.UpdateRoleProperty();
    }

    [MenuItem("Development/Synchro/Common/DownloadEntityDefine")]
    public static void DownloadEntityDefine()
    {
        SyncCommon.DownloadEntityDefine();
    }

    [MenuItem("Development/Synchro/Common/DownloadCodes")]
    public static void DownloadCodes()
    {
        SyncCommon.DownloadCodes();
    }

    [MenuItem("Development/Synchro/DataQuery/RefreshDataDef")]
    public static void RefreshDataQuery()
    {
        SyncDataQuery.RefreshDataQuery();
    }

    [MenuItem("Development/Synchro/DataQuery/Upload2Server")]
    public static void Upload2Server()
    {
        SyncDataQuery.Upload2Server();
    }

    [MenuItem("Development/Synchro/DataQuery/Excel2Xml")]
    // Use this for initialization
    public static void Excel2Xml()
    {
        SyncDataQuery.Excel2Xml();
    }

    [MenuItem("Development/Synchro/Localization/xls2language")]
    public static void Excel2Language()
    {
        SyncLocalization.Excel2Language();
    }
}
