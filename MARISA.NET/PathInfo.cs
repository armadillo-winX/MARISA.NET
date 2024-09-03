namespace MARISA.NET
{
    internal class PathInfo
    {
        public static string AppPath => typeof(App).Assembly.Location;

        public static string? AppLocation => Path.GetDirectoryName(AppPath);

        public static string? ReplayDirectoryPathSettingsFile => $"{AppLocation}\\ReplayDirectoryPathSettings.xml";

        public static string? MainWindowSettingsFile => $"{AppLocation}\\MainWindowSettings.xml";

        public static string? ReplayFileBackupDirectory => $"{AppLocation}\\backup\\";
    }
}
