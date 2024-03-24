using System.Collections.ObjectModel;
using System.Diagnostics;

namespace MARISA.NET
{
    internal class ReplayFile
    {
        private readonly static Dictionary<string, string> GameIdDictionary
            = new()
            {
                { "th6", "Th06" },
                { "th7", "Th07" },
                { "th8", "Th08" },
                { "th9", "Th09" },
                { "th10", "Th10" },
                { "th11", "Th11" },
                { "th12", "Th12" },
                { "th13", "Th13" },
                { "th14", "Th14" },
                { "th15", "Th15" },
                { "th16", "Th16" },
                { "th17", "Th17" },
                { "th18", "Th18" },
                { "th19", "Th19" }
            };

        public static ObservableCollection<ReplayFileInfo> GetReplayFiles(string? gameId)
        {
            ObservableCollection<ReplayFileInfo> replayFileInfos = new();

            string? replayDirectory = ReplayDirectoryPath.GetReplayDirectoryPath(gameId);
            if (!string.IsNullOrWhiteSpace(replayDirectory))
            {
                string[] replayFilesList = Directory.GetFiles(replayDirectory, "*.rpy", SearchOption.TopDirectoryOnly);
                foreach (string replayFile in replayFilesList)
                {
                    ReplayFileInfo replayFileInfo;
                    try
                    {
                        replayFileInfo = GetReplayFileInfo(replayFile);
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex.Message);

                        replayFileInfo = new()
                        {
                            FileName = Path.GetFileName(replayFile),
                            UpdateDate = "Error",
                            FileSize = "Error"
                        };
                    }

                    replayFileInfos.Add(replayFileInfo);
                }

                return replayFileInfos;
            }
            else
            {
                return replayFileInfos;
            }
        }

        private static ReplayFileInfo GetReplayFileInfo(string replayFilePath)
        {
            DateTime updateTime = File.GetLastWriteTime(replayFilePath);
            string fileSize = GetReplayFileSize(replayFilePath);

            return new ReplayFileInfo
            {
                FileName = Path.GetFileName(replayFilePath),
                UpdateDate = updateTime.ToString("yyyy/MM/dd HH:mm:ss"),
                FileSize = fileSize
            };
        }

        private static string GetReplayFileSize(string replayFilePath)
        {
            FileInfo fileInfo = new FileInfo(replayFilePath);
            long fileSize = fileInfo.Length;
            return $"{fileSize / 1024} KiB";
        }

        public static string? GetGameId(string replayFile)
        {
            string replayName = Path.GetFileNameWithoutExtension(replayFile);
            return GameIdDictionary[replayName.Split('_')[0]];
        }
    }
}
