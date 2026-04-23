using System.Runtime.InteropServices;

namespace DllShared;

/// <summary>
/// Simple helper for Marshal
/// </summary>
public static class MarshalHelper
{
    /// <summary>
    /// Making an <see cref="IntPtr"/> pointer with <paramref name="values"/> for <see cref="BasicList.List"/>.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="values"></param>
    /// <returns></returns>
    public static IntPtr GetListPtr<T>(List<T> values) where T : struct
    {
        IntPtr main_ptr = Marshal.AllocHGlobal(Marshal.SizeOf<IntPtr>() * values.Count);
        int indx = 0;
        foreach (var item in values)
        {
            IntPtr iptr = Marshal.AllocHGlobal(Marshal.SizeOf<T>());
            Marshal.StructureToPtr(item, iptr, false);
            Marshal.WriteIntPtr(main_ptr, indx * Marshal.SizeOf<IntPtr>(), iptr);
            indx++;
        }
        return main_ptr;
    }

    /// <summary>
    /// Freeing the List.
    /// </summary>
    /// <param name="listPointer"></param>
    public static void FreeList(IntPtr listPointer)
    {
        BasicList upcList = Marshal.PtrToStructure<BasicList>(listPointer);
        FreeListPtr(upcList.Count, upcList.List);
        Marshal.FreeHGlobal(listPointer);
    }

    /// <summary>
    /// Freeing the <see cref="BasicList"/>.
    /// </summary>
    /// <param name="count"></param>
    /// <param name="listPointer"></param>
    public static void FreeListPtr(int count, IntPtr listPointer)
    {
        for (int i = 0; i < count; i++)
        {
            var ptr = Marshal.ReadIntPtr(listPointer, i * Marshal.SizeOf<IntPtr>());
            Marshal.FreeHGlobal(ptr);
        }
        Marshal.FreeHGlobal(listPointer);
    }

    /// <summary>
    /// Writing out <see cref="BasicList"/> to <paramref name="outList"/>.
    /// </summary>
    /// <param name="outList"></param>
    /// <param name="Count"></param>
    /// <param name="ptrToList"></param>
    public static void WriteOutList(IntPtr outList, int Count, IntPtr ptrToList)
    {
        BasicList list = new(Count, ptrToList);
        IntPtr iptr = Marshal.AllocHGlobal(Marshal.SizeOf<BasicList>());
        Marshal.StructureToPtr(list, iptr, false);
        Marshal.WriteIntPtr(outList, 0, iptr);
    }

    /// <summary>
    /// Writing out <paramref name="values"/> to <paramref name="outList"/>.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="outList"></param>
    /// <param name="values"></param>
    public static void WriteOutList<T>(IntPtr outList, List<T> values) where T : struct
    {
        WriteOutList(outList, values.Count, GetListPtr(values));
    }

    /// <summary>
    /// Making an <see cref="IntPtr"/> pointer from <paramref name="struct_t"/>.
    /// </summary>
    /// <typeparam name="T">Any structure</typeparam>
    /// <param name="struct_t">The structure to marshal.</param>
    /// <returns>Allocated and filled Struct Pointer.</returns>
    public static IntPtr ToIntPtr<T>(this T struct_t) where T : struct
    {
        var ptr = Marshal.AllocHGlobal(Marshal.SizeOf<T>());
        Marshal.StructureToPtr(struct_t, ptr, false);
        return ptr;
    }
}