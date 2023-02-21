using System;
using System.Diagnostics;
using System.Threading;

public class Program
{
    private static Timer? _timer;
    private static bool isActive = false;

    public static void Main()
    {
        Console.WriteLine();
        Console.WriteLine("Welcome to Stream Control!");
        Console.WriteLine("--------------------------");
        Console.WriteLine("App Version: 0.1-beta");
        Console.WriteLine();

        Console.WriteLine("Starting timer...");
        _timer = new Timer(TimerCallback, null, 0, 60_000);

        Console.ReadLine();
    }

    private static void TimerCallback(Object o)
    {
        TimeSpan start = TimeSpan.Parse("22:10");
        TimeSpan shift = TimeSpan.Parse("23:59");
        TimeSpan stop = TimeSpan.Parse("02:55");
        TimeSpan now = DateTime.Now.TimeOfDay;
        now = now.StripSeconds();

        Console.WriteLine($"Now: {now} | Start: {start} | Shift: {shift} | Stop: {stop}");

        if (now >= start && !isActive)
        {
            StartLocalStream();
            isActive = true;
        }
        else if (now >= shift && isActive)
        {
            // Change Media Playback
            ChangeMediaPlayback();
        }
        else if (now == stop && isActive)
        {
            // Stop apps
            KillProcesses("vlc");
            KillProcesses("altacastStandalone");
            isActive = false;
        }
    }   

    private static void ChangeMediaPlayback()
    {
        // Get media playing for max 3 hours
        // Kill any initial VLC
        var vlc = @"C:\Program Files\VideoLAN\VLC\vlc.exe";

        KillProcesses("vlc");

        CreateAndRunProcess(directory: @"C:\Program Files\VideoLan\VLC", fileName: vlc, args: "https://streaming.cnnphilippines.com/live/myStream/playlist.m3u8");
    }

    private static void StartLocalStream()
    {
        var vlc = @"C:\Program Files\VideoLAN\VLC\vlc.exe";
        var altaCast1 = @"D:\altacast\altacastStandalone.exe";
        var altaCast2 = @"D:\altacast2\altacastStandalone.exe";

        CreateAndRunProcess(directory: @"C:\Program Files\VideoLan\VLC", fileName: vlc, args: "http://149.56.147.197:9079/stream");
        CreateAndRunProcess(directory: @"D:\altacast\", fileName: altaCast1, isShell: true, verb: "runas");
        CreateAndRunProcess(directory: @"D:\altacast2\", fileName: altaCast2, isShell: true, verb: "runas");
    }

    private static void CreateAndRunProcess(string directory, string fileName, string args = null, bool isShell = false, string verb = null)
    {
        ProcessStartInfo procInfo = new ProcessStartInfo
        {
            WorkingDirectory = directory,
            FileName = fileName,
            UseShellExecute = isShell,
            Verb = verb,
            Arguments = args
        };
        Process.Start(procInfo);
    }

    private static void KillProcesses(string processName)
    {
        foreach (var process in Process.GetProcessesByName(processName))
        {
            process.Kill();
        }
    }
    
   
}
