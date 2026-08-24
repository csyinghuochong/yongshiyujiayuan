using System.Runtime.InteropServices;

/// <summary>
/// iOS native calls must live in an AOT assembly. HybridCLR hot-update
/// assemblies do not support P/Invoke signatures containing managed strings.
/// </summary>
public static class IOSNativeBridge
{
#if UNITY_IOS && !UNITY_EDITOR
    [DllImport("__Internal", EntryPoint = "CheckIphoneYueyu")]
    private static extern void CheckIphoneYueyuNative(
        [MarshalAs(UnmanagedType.LPStr)] string packages);
#endif

    public static void CheckIphoneYueyu(string packages)
    {
#if UNITY_IOS && !UNITY_EDITOR
        CheckIphoneYueyuNative(packages);
#endif
    }
}
