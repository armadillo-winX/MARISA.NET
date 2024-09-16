using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Xml;

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
            ObservableCollection<ReplayFileInfo> replayFileInfos = [];

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
            FileInfo fileInfo = new(replayFilePath);
            long fileSize = fileInfo.Length;
            return $"{fileSize / 1024} KiB";
        }

        public static string? GetGameId(string replayFilePath)
        {
            string replayName = Path.GetFileNameWithoutExtension(replayFilePath);
            return GameIdDictionary[replayName.Split('_')[0]];
        }

        public static string Import(string replayFilePath)
        {
            string gameId = GetGameId(replayFilePath);
            string? replayDirectory = ReplayDirectoryPath.GetReplayDirectoryPath(gameId);
            string replayName = Path.GetFileNameWithoutExtension(replayFilePath);
            if (Directory.Exists(replayDirectory))
            {
                try
                {
                    string newReplayFile = $"{replayDirectory}\\{replayName}.rpy";
                    int i = 0;
                    while (File.Exists(newReplayFile))
                    {
                        i++;
                        newReplayFile = $"{replayDirectory}\\{replayName}-{i}.rpy";
                    }

                    File.Move(replayFilePath, newReplayFile);
                    return $"成功:{newReplayFile}";
                }
                catch (Exception ex)
                {
                    return $"エラー:{ex.Message}";
                }
            }
            else
            {
                return $"取り込み先ディレクトリが存在しませんでした。Game:{gameId}";
            }
        }

        public static void Rename(string gameId, string replayFileName, string newReplayFileName)
        {
            string replayDirectory = ReplayDirectoryPath.GetReplayDirectoryPath(gameId);

            if (replayFileName != newReplayFileName)
            {
                File.Move($"{replayDirectory}\\{replayFileName}", $"{replayDirectory}\\{newReplayFileName}");
            }
        }

        public static bool Exists(string gameId, string replayFileName)
        {
            string replayDirectory = ReplayDirectoryPath.GetReplayDirectoryPath(gameId);

            return File.Exists($"{replayDirectory}\\{replayFileName}");
        }

        public static void Delete(string gameId, string replayFileName)
        {
            string replayDirectory = ReplayDirectoryPath.GetReplayDirectoryPath(gameId);
            File.Delete($"{replayDirectory}\\{replayFileName}");
        }

        public static void CreateBackup(string gameId, string replayFilePath, string backupName, string comment)
        {
            string backupDirectory = $"{PathInfo.ReplayFileBackupDirectory}\\{gameId}";
            if (!Directory.Exists(backupDirectory))
            {
                Directory.CreateDirectory(backupDirectory);
            }

            File.Copy(replayFilePath, $"{backupDirectory}\\{backupName}.rpy", true);
            CreateBackupInformationFile(gameId, replayFilePath, backupName, comment);
        }

        public static void CreateBackupInformationFile(
            string gameId, string replayFilePath, string backupName, string comment)
        {
            string backupInformationFilePath = $"{PathInfo.ReplayFileBackupDirectory}\\{gameId}\\{backupName}.mbakinfo";

            XmlDocument backupInformationXml = new();
            XmlNode docNode = backupInformationXml.CreateXmlDeclaration("1.0", "UTF-8", null);
            _ = backupInformationXml.AppendChild(docNode);

            XmlNode rootNode = backupInformationXml.CreateElement("ReplayBackupInformation");
            _ = backupInformationXml.AppendChild(rootNode);

            XmlNode gameIdNode = backupInformationXml.CreateElement("GameId");
            _ = gameIdNode.AppendChild(backupInformationXml.CreateTextNode(gameId));
            _ = rootNode.AppendChild(gameIdNode);

            XmlNode sourceFileNode = backupInformationXml.CreateElement("SourceFilePath");
            _ = sourceFileNode.AppendChild(backupInformationXml.CreateElement(replayFilePath));
            _ = rootNode.AppendChild(sourceFileNode);

            XmlNode commentNode = backupInformationXml.CreateElement("Comment");
            _ = commentNode.AppendChild(backupInformationXml.CreateElement(comment));
            _ = rootNode.AppendChild(commentNode);

            backupInformationXml.Save(backupInformationFilePath);
        }
    }
}
