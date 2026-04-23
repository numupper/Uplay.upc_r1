using System.Runtime.InteropServices;

namespace DllShared;

/// <summary>
/// Uplay Basic List
/// </summary>
[StructLayout(LayoutKind.Sequential)]
public struct BasicList
{
    /// <summary>
    /// Element Count
    /// </summary>
    [MarshalAs(UnmanagedType.I4)]
    public int Count;
    /// <summary>
    /// List Pointer
    /// </summary>
    [MarshalAs(UnmanagedType.SysInt)]
    public IntPtr List;

    /// <summary>
    /// Basic List with Empty list.
    /// </summary>
    public BasicList()
    {
        Count = 0;
        List = IntPtr.Zero;
    }

    /// <summary>
    /// Basic list with <see cref="Count"/> as <paramref name="count"/> and <see cref="List"/> as <paramref name="list"/>.
    /// </summary>
    /// <param name="count">List Count</param>
    /// <param name="list">List pointer</param>
    public BasicList(int count, IntPtr list)
    {
        Count = count;
        List = list;
    }

    /// <inheritdoc/>
    public override readonly string ToString()
    {
        return $"ListPtr: {List} Count: {Count}";
    }
}