using System.Runtime.InteropServices;

namespace DllShared;

/// <summary>
/// Loading any dll.
/// </summary>
public partial class LoadDll
{
    static readonly Dictionary<string, IntPtr> FileToModule = [];

    /// <summary>
    /// Path to load dlls from.
    /// </summary>
    public static string PluginPath = "dlls";

    /// <summary>
    /// Load dlls from <see cref="PluginPath"/>.
    /// </summary>
    public static void LoadPlugins()
    {
        if (!Directory.Exists(Path.Combine(AOTHelper.CurrentPath, PluginPath)))
            return;
        var files = Directory.GetFiles(Path.Combine(AOTHelper.CurrentPath, PluginPath), "*.dll");
        foreach (var file in files)
        {
            FileToModule.Add(file, NativeLibrary.Load(file));
        }
    }

    /// <summary>
    /// Unload all dlls.
    /// </summary>
    public static void FreePlugins()
    {
        foreach (var file in FileToModule)
        {
            NativeLibrary.Free(file.Value);
        }
        FileToModule.Clear();
    }
}
