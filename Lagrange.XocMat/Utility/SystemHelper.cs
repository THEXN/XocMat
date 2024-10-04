using Lagrange.XocMat.Extensions;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text.Json.Nodes;

namespace Lagrange.XocMat.Utility;

public class SystemHelper
{

    [DllImport("psapi.dll")]
    private static extern bool EmptyWorkingSet(IntPtr lpAddress);

    public static void FreeMemory()
    {
        GC.Collect();
        GC.WaitForPendingFinalizers();
        foreach (var process in Process.GetProcesses())
        {
            if ((process.ProcessName == "System") && (process.ProcessName == "Idle"))
                continue;
            try
            {
                EmptyWorkingSet(process.Handle);
            }
            catch { }
        }
    }

    #region ����ڴ���ϢAPI
    [DllImport("kernel32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool GlobalMemoryStatusEx(ref MEMORY_INFO mi);

    //�����ڴ����Ϣ�ṹ
    [StructLayout(LayoutKind.Sequential)]
    public struct MEMORY_INFO
    {
        public uint dwLength; //��ǰ�ṹ���С
        public uint dwMemoryLoad; //��ǰ�ڴ�ʹ����
        public ulong ullTotalPhys; //�ܼ������ڴ��С
        public ulong ullAvailPhys; //���������ڴ��С
        public ulong ullTotalPageFile; //�ܼƽ����ļ���С
        public ulong ullAvailPageFile; //�ܼƽ����ļ���С
        public ulong ullTotalVirtual; //�ܼ������ڴ��С
        public ulong ullAvailVirtual; //���������ڴ��С
        public ulong ullAvailExtendedVirtual; //���� ���ֵʼ��Ϊ0
    }
    #endregion

    #region ��ʽ��������С
    /// <summary>
    /// ��ʽ��������С
    /// </summary>
    /// <param name="size">������B��</param>
    /// <returns>�Ѹ�ʽ��������</returns>
    public static string FormatSize(double size)
    {
        double d = (double)size;
        int i = 0;
        while ((d > 1024) && (i < 5))
        {
            d /= 1024;
            i++;
        }
        string[] unit = { "B", "KB", "MB", "GB", "TB" };
        return (string.Format("{0} {1}", Math.Round(d, 2), unit[i]));
    }
    #endregion

    #region ��õ�ǰ�ڴ�ʹ�����
    /// <summary>
    /// ��õ�ǰ�ڴ�ʹ�����
    /// </summary>
    /// <returns></returns>
    public static MEMORY_INFO GetMemoryStatus()
    {
        MEMORY_INFO mi = new MEMORY_INFO();
        mi.dwLength = (uint)System.Runtime.InteropServices.Marshal.SizeOf(mi);
        GlobalMemoryStatusEx(ref mi);
        return mi;
    }
    #endregion

    #region ��õ�ǰ���������ڴ��С
    /// <summary>
    /// ��õ�ǰ���������ڴ��С
    /// </summary>
    /// <returns>��ǰ���������ڴ棨B��</returns>
    public static ulong GetAvailPhys()
    {
        MEMORY_INFO mi = GetMemoryStatus();
        return mi.ullAvailPhys;
    }
    #endregion

    #region ��õ�ǰ��ʹ�õ��ڴ��С
    /// <summary>
    /// ��õ�ǰ��ʹ�õ��ڴ��С
    /// </summary>
    /// <returns>��ʹ�õ��ڴ��С��B��</returns>
    public static ulong GetUsedPhys()
    {
        MEMORY_INFO mi = GetMemoryStatus();
        return (mi.ullTotalPhys - mi.ullAvailPhys);
    }
    #endregion

    #region ��õ�ǰ�ܼ������ڴ��С
    /// <summary>
    /// ��õ�ǰ�ܼ������ڴ��С
    /// </summary>
    /// <returns&gt;�ܼ������ڴ��С��B��&lt;/returns&gt;
    public static ulong GetTotalPhys()
    {
        MEMORY_INFO mi = GetMemoryStatus();
        return mi.ullTotalPhys;
    }
    #endregion

    public static void KillChrome()
    {
        foreach (var process in Process.GetProcesses())
        {
            if (process.ProcessName.Contains("chrome"))
            {
                process.Kill();
            }
        }
    }

    public static Internal.Socket.Internet.Item? GetItemById(int id)
    {
        var assembly = Assembly.GetExecutingAssembly();
        var file = "Lagrange.XocMat.Resources.TerrariaID.json";
        var stream = assembly.GetManifestResourceStream(file)!;
        using var reader = new StreamReader(stream);
        var jobj = reader.ReadToEnd().ToObject<JsonNode>()!;
        var array = jobj["��Ʒ"]!.AsArray();
        foreach (var item in array)
        {
            if (item != null && item["ID"]!.GetValue<int>() == id)
            {
                return new ()
                {
                    Name = item["��������"]!.GetValue<string>(),
                    netID = id
                };
            }
        }
        return null;
    }

    public static List<Lagrange.XocMat.Internal.Socket.Internet.Item> GetItemByName(string name)
    {
        var list = new List<Lagrange.XocMat.Internal.Socket.Internet.Item>();
        var assembly = Assembly.GetExecutingAssembly();
        var file = "Lagrange.XocMat.Resources.TerrariaID.json";
        var stream = assembly.GetManifestResourceStream(file)!;
        using var reader = new StreamReader(stream);
        var jobj = JsonNode.Parse(reader.ReadToEnd());
        var array = jobj?["��Ʒ"]?.AsArray()!;
        foreach (var item in array)
        {
            if (item != null && item["��������"]!.GetValue<string>().Contains(name))
            {
                list.Add(new ()
                {
                    Name = item["��������"]!.GetValue<string>(),
                    netID = item["ID"]!.GetValue<int>()
                });
            }
        }
        return list;
    }

    public static List<Lagrange.XocMat.Internal.Socket.Internet.Item> GetItemByIdOrName(string ji)
    {
        var list = new List<Lagrange.XocMat.Internal.Socket.Internet.Item>();
        if (int.TryParse(ji, out var i))
        {
            var item = GetItemById(i);
            if (item != null)
                list.Add(item);
        }
        else
        {
            list.AddRange(GetItemByName(ji));
        }
        return list;
    }
}
