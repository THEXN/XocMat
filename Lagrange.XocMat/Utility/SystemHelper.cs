using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text.Json.Nodes;
using Lagrange.XocMat.Extensions;
using Newtonsoft.Json.Linq;

namespace Lagrange.XocMat.Utility;

public class SystemHelper
{

    [DllImport("psapi.dll")]
    private static extern bool EmptyWorkingSet(IntPtr lpAddress);

    public static void FreeMemory()
    {
        GC.Collect();
        GC.WaitForPendingFinalizers();
        foreach (Process process in Process.GetProcesses())
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
        return string.Format("{0} {1}", Math.Round(d, 2), unit[i]);
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
        return mi.ullTotalPhys - mi.ullAvailPhys;
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
        foreach (Process process in Process.GetProcesses())
        {
            if (process.ProcessName.Contains("chrome") && CanAccessProcess(process))
            {
                process.Kill();
            }
        }
    }

    public static bool CanAccessProcess(Process process)
    {
        try
        {
            // ���Է��ʽ��̵�һ����ҪȨ�޵�����
            ProcessModuleCollection modules = process.Modules;
            return true;
        }
        catch (Win32Exception ex) when (ex.NativeErrorCode == 5) // �ܾ�����
        {
            return false;
        }
        catch // �����쳣����������˳�
        {
            return false;
        }
    }

    public static Internal.Socket.Internet.Item? GetItemById(int id)
    {
        Assembly assembly = Assembly.GetExecutingAssembly();
        string file = "Lagrange.XocMat.Resources.Json.TerrariaID.json";
        Stream stream = assembly.GetManifestResourceStream(file)!;
        using StreamReader reader = new StreamReader(stream);
        JObject jobj = reader.ReadToEnd().ToObject<JObject>()!;
        JArray array = (JArray)jobj["��Ʒ"]!;
        foreach (JToken item in array)
        {
            if (item != null && item["ID"]!.Value<int>() == id)
            {
                return new()
                {
                    Name = item["��������"]!.Value<string>()!,
                    netID = id
                };
            }
        }
        return null;
    }

    public static List<Internal.Socket.Internet.Item> GetItemByName(string name)
    {
        List<Internal.Socket.Internet.Item> list = [];
        Assembly assembly = Assembly.GetExecutingAssembly();
        string file = "Lagrange.XocMat.Resources.Json.TerrariaID.json";
        Stream stream = assembly.GetManifestResourceStream(file)!;
        using StreamReader reader = new StreamReader(stream);
        JsonNode? jobj = JsonNode.Parse(reader.ReadToEnd());
        JsonArray array = jobj?["��Ʒ"]?.AsArray()!;
        foreach (JsonNode? item in array)
        {
            if (item != null && item["��������"]!.GetValue<string>().Contains(name))
            {
                list.Add(new()
                {
                    Name = item["��������"]!.GetValue<string>(),
                    netID = item["ID"]!.GetValue<int>()
                });
            }
        }
        return list;
    }

    public static List<Internal.Socket.Internet.Item> GetItemByIdOrName(string ji)
    {
        List<Internal.Socket.Internet.Item> list = [];
        if (int.TryParse(ji, out int i))
        {
            Internal.Socket.Internet.Item? item = GetItemById(i);
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
