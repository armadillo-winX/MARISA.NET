using System.Reflection;

namespace MARISA.NET
{
    internal class ReplayDirectoryPath
    {
        public static string? Th06ReplayDirectory { get; set; }

        public static string? Th07ReplayDirectory { get; set; }

        public static string? Th08ReplayDirectory { get; set; }

        public static string? Th09ReplayDirectory { get; set; }

        public static string? Th10ReplayDirectory { get; set; }

        public static string? Th11ReplayDirectory { get; set; }

        public static string? Th12ReplayDirectory { get; set; }

        public static string? Th13ReplayDirectory { get; set; }

        public static string? Th14ReplayDirectory { get; set; }

        public static string? Th15ReplayDirectory { get; set; }

        public static string? Th16ReplayDirectory { get; set; }

        public static string? Th17ReplayDirectory { get; set; }

        public static string? Th18ReplayDirectory { get; set; }

        public static void SetReplayDirectoryPath(string? gameId, string replayDirectoryPath)
        {
            if (!string.IsNullOrEmpty(gameId))
            {
                //プロパティ名からプロパティを取得
                PropertyInfo? replayDirectoryPathProperty = typeof(ReplayDirectoryPath).GetProperty($"{gameId}ReplayDirectory");
                //取得したプロパティに値を代入
                replayDirectoryPathProperty.SetValue(null, replayDirectoryPath);
            }
        }

        public static string? GetReplayDirectoryPath(string? gameId)
        {
            string? replayDirectory;
            if (!string.IsNullOrEmpty(gameId))
            {
                PropertyInfo? replayDirectoryPathProperty = typeof(ReplayDirectoryPath).GetProperty($"{gameId}ReplayDirectory");

                if (replayDirectoryPathProperty != null)
                {
                    replayDirectory
                    = replayDirectoryPathProperty.GetValue(null, null) != null ?
                    replayDirectoryPathProperty.GetValue(null, null).ToString() :
                    string.Empty;
                }
                else
                {
                    replayDirectory = string.Empty;
                }
            }
            else
            {
                replayDirectory = string.Empty;
            }

            return replayDirectory;
        }
    }
}
