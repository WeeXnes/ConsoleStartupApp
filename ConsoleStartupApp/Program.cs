// See https://aka.ms/new-console-template for more information

using System.Diagnostics;
using System.Management;
using System.Security.Principal;
using System.Text;
using Microsoft.Win32;

Console.OutputEncoding = Encoding.UTF8;


bool isElevated;
using (WindowsIdentity identity = WindowsIdentity.GetCurrent())
{
    WindowsPrincipal principal = new WindowsPrincipal(identity);
    isElevated = principal.IsInRole(WindowsBuiltInRole.Administrator);
}

string GPUInfo = (from x in new ManagementObjectSearcher("select * from Win32_VideoController").Get().Cast<ManagementObject>()
    select x.GetPropertyValue("Name")).FirstOrDefault().ToString();
string CPUInfo = (string)Registry.GetValue(@"HKEY_LOCAL_MACHINE\HARDWARE\DESCRIPTION\System\CentralProcessor\0\", "ProcessorNameString", null);
CPUInfo = CPUInfo + " | " + Environment.ProcessorCount + " Cores";
string OSInfo = (from x in new ManagementObjectSearcher("SELECT Caption FROM Win32_OperatingSystem").Get().Cast<ManagementObject>()
    select x.GetPropertyValue("Caption")).FirstOrDefault().ToString();

ConsoleColor prevColor = Console.ForegroundColor;


Console.ForegroundColor = ConsoleColor.Blue;
Console.WriteLine("\ue70f " + OSInfo);

if (Contains(GPUInfo, "intel", StringComparison.OrdinalIgnoreCase))
{
    Console.ForegroundColor = ConsoleColor.Cyan;
}else if (Contains(GPUInfo, "amd", StringComparison.OrdinalIgnoreCase))
{
    Console.ForegroundColor = ConsoleColor.Red;
}else if (Contains(GPUInfo, "nvidia", StringComparison.OrdinalIgnoreCase))
{
    Console.ForegroundColor = ConsoleColor.Green;
}
Console.WriteLine("\uf108 " + GPUInfo);


if (Contains(CPUInfo, "intel", StringComparison.OrdinalIgnoreCase))
{
    Console.ForegroundColor = ConsoleColor.Cyan;
}else if (Contains(CPUInfo, "amd", StringComparison.OrdinalIgnoreCase))
{
    Console.ForegroundColor = ConsoleColor.Cyan;
}
Console.WriteLine("\ufca4 " + CPUInfo);
if (isElevated)
{
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine("\ufc7e Admin Privilages Active");
}



Console.ForegroundColor = prevColor;


static bool Contains(string source, string toCheck, StringComparison comp)
{
    return source?.IndexOf(toCheck, comp) >= 0;
}