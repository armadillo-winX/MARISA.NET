using System.Xml.Serialization;

namespace MARISA.NET
{
    internal class SettingsConfigurator
    {
        public static ReplayDirectoryPathSettings? _replayDirectoryPathSettings = new();

        public static void SaveReplayDirectoryPathSettings()
        {
            string? replayDirectoryPathSettingsFile = PathInfo.ReplayDirectoryPathSettingsFile;

            _replayDirectoryPathSettings.Th06 = ReplayDirectoryPath.Th06ReplayDirectory;
            _replayDirectoryPathSettings.Th07 = ReplayDirectoryPath.Th07ReplayDirectory;
            _replayDirectoryPathSettings.Th08 = ReplayDirectoryPath.Th08ReplayDirectory;
            _replayDirectoryPathSettings.Th09 = ReplayDirectoryPath.Th09ReplayDirectory;
            _replayDirectoryPathSettings.Th10 = ReplayDirectoryPath.Th10ReplayDirectory;
            _replayDirectoryPathSettings.Th11 = ReplayDirectoryPath.Th11ReplayDirectory;
            _replayDirectoryPathSettings.Th12 = ReplayDirectoryPath.Th12ReplayDirectory;
            _replayDirectoryPathSettings.Th13 = ReplayDirectoryPath.Th13ReplayDirectory;
            _replayDirectoryPathSettings.Th14 = ReplayDirectoryPath.Th14ReplayDirectory;
            _replayDirectoryPathSettings.Th15 = ReplayDirectoryPath.Th15ReplayDirectory;
            _replayDirectoryPathSettings.Th16 = ReplayDirectoryPath.Th16ReplayDirectory;
            _replayDirectoryPathSettings.Th17 = ReplayDirectoryPath.Th17ReplayDirectory;
            _replayDirectoryPathSettings.Th18 = ReplayDirectoryPath.Th18ReplayDirectory;
            _replayDirectoryPathSettings.Th19 = ReplayDirectoryPath.Th19ReplayDirectory;

            if (!string.IsNullOrEmpty(replayDirectoryPathSettingsFile))
            {
                // XmlSerializerを使ってファイルに保存（SettingSerializerオブジェクトの内容を書き込む）
                XmlSerializer replayDirectoryPathSettingsSerializer = new(typeof(ReplayDirectoryPathSettings));
                FileStream fileStream = new(replayDirectoryPathSettingsFile, FileMode.Create);
                // オブジェクトをシリアル化してXMLファイルに書き込む
                replayDirectoryPathSettingsSerializer.Serialize(fileStream, _replayDirectoryPathSettings);
                fileStream.Close();
            }
        }
        public static void ConfigureReplayDirectoryPathSettings()
        {
            string? replayDirectoryPathSettingsFile = PathInfo.ReplayDirectoryPathSettingsFile;
            if (!string.IsNullOrEmpty(replayDirectoryPathSettingsFile) && File.Exists(replayDirectoryPathSettingsFile))
            {
                XmlSerializer ReplayDirectoryPathSettingsSerializer = new(typeof(ReplayDirectoryPathSettings));
                FileStream fileStream = new(replayDirectoryPathSettingsFile, FileMode.Open);

                _replayDirectoryPathSettings 
                    = (ReplayDirectoryPathSettings)ReplayDirectoryPathSettingsSerializer.Deserialize(fileStream);
                fileStream.Close();

                ReplayDirectoryPath.Th06ReplayDirectory = _replayDirectoryPathSettings.Th06;
                ReplayDirectoryPath.Th07ReplayDirectory = _replayDirectoryPathSettings.Th07;
                ReplayDirectoryPath.Th08ReplayDirectory = _replayDirectoryPathSettings.Th08;
                ReplayDirectoryPath.Th09ReplayDirectory = _replayDirectoryPathSettings.Th09;
                ReplayDirectoryPath.Th10ReplayDirectory = _replayDirectoryPathSettings.Th10;
                ReplayDirectoryPath.Th11ReplayDirectory = _replayDirectoryPathSettings.Th11;
                ReplayDirectoryPath.Th12ReplayDirectory = _replayDirectoryPathSettings.Th12;
                ReplayDirectoryPath.Th13ReplayDirectory = _replayDirectoryPathSettings.Th13;
                ReplayDirectoryPath.Th14ReplayDirectory = _replayDirectoryPathSettings.Th14;
                ReplayDirectoryPath.Th15ReplayDirectory = _replayDirectoryPathSettings.Th15;
                ReplayDirectoryPath.Th16ReplayDirectory = _replayDirectoryPathSettings.Th16;
                ReplayDirectoryPath.Th17ReplayDirectory = _replayDirectoryPathSettings.Th17;
                ReplayDirectoryPath.Th18ReplayDirectory = _replayDirectoryPathSettings.Th18;
                ReplayDirectoryPath.Th19ReplayDirectory = _replayDirectoryPathSettings.Th19;
            }
            else
            {
                ReplayDirectoryPath.Th06ReplayDirectory = string.Empty;
                ReplayDirectoryPath.Th07ReplayDirectory = string.Empty;
                ReplayDirectoryPath.Th08ReplayDirectory = string.Empty;
                ReplayDirectoryPath.Th09ReplayDirectory = string.Empty;
                ReplayDirectoryPath.Th10ReplayDirectory = string.Empty;
                ReplayDirectoryPath.Th11ReplayDirectory = string.Empty;
                ReplayDirectoryPath.Th12ReplayDirectory = string.Empty;
                ReplayDirectoryPath.Th13ReplayDirectory = string.Empty;
                ReplayDirectoryPath.Th14ReplayDirectory = string.Empty;
                ReplayDirectoryPath.Th15ReplayDirectory = string.Empty;
                ReplayDirectoryPath.Th16ReplayDirectory = string.Empty;
                ReplayDirectoryPath.Th17ReplayDirectory = string.Empty;
                ReplayDirectoryPath.Th18ReplayDirectory = string.Empty;
                ReplayDirectoryPath.Th19ReplayDirectory = string.Empty;
            }
        }

        public static void SaveMainWindowSettings(MainWindowSettings mainWindowSettings)
        {
            string? mainWindowSettingsFile = PathInfo.MainWindowSettingsFile;

            XmlSerializer mainWindowSettingsSerializer = new(typeof(MainWindowSettings));
            FileStream fileStream = new(mainWindowSettingsFile, FileMode.Create);
            mainWindowSettingsSerializer.Serialize(fileStream, mainWindowSettings);
            fileStream.Close();
        }

        public static MainWindowSettings ConfigureMainWindowSettings()
        {
            string? mainWindowSettingsFile = PathInfo.MainWindowSettingsFile;

            MainWindowSettings mainWindowSettings = new();

            if (File.Exists(mainWindowSettingsFile))
            {
                XmlSerializer mainWindowSettingsSerializer = new(typeof(MainWindowSettings));
                FileStream fileStream = new(mainWindowSettingsFile, FileMode.Open);

                mainWindowSettings = (MainWindowSettings)mainWindowSettingsSerializer.Deserialize(fileStream);
                fileStream.Close();
            }
            else
            {
                mainWindowSettings.SelectedGameId = GameIndex.Th06;
                mainWindowSettings.Width = 600;
                mainWindowSettings.Height = 400;
            }

            return mainWindowSettings;
        }
    }
}
