global using System;
global using System.IO;
global using System.Windows;

using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.Windows.Controls;

namespace MARISA.NET
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string? _gameId;

        private string? GameId
        {
            get
            {
                return _gameId;
            }

            set
            {
                _gameId = value;
            }
        }

        private readonly Dictionary<string, int> GameDictionary =
           new()
           {
                { "Th06", 0 },
                { "Th07", 1 },
                { "Th08", 2 },
                { "Th09", 3 },
                { "Th10", 4 },
                { "Th11", 5 },
                { "Th12", 6 },
                { "Th13", 7 },
                { "Th14", 8 },
                { "Th15", 9 },
                { "Th16", 10 },
                { "Th17", 11 },
                { "Th18", 12 }
           };

        public MainWindow()
        {
            InitializeComponent();

            this.GameId = string.Empty;
            this.Title = $"MARISA ver{VersionInfo.AppVersion}";

            AppNameBlock.Text = VersionInfo.AppName;
            AppVersionBlock.Text = $"Version.{VersionInfo.AppVersion}";
            CopyrightBlock.Text = VersionInfo.Copyright;

            try
            {
                SettingsConfigurator.ConfigureReplayDirectoryPathSettings();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"リプレイディレクトリの設定の構成に失敗しました。\n{ex.Message}", "エラー",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }

            try
            {
                ConfigureMainWindowSettings();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"メインウィンドウ設定の構成に失敗しました。\n{ex.Message}", "エラー",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ViewReplayFilesList()
        {
            if (GameComboBox.SelectedIndex > -1)
            {
                string? gameId = this.GameId;

                try
                {
                    ObservableCollection<ReplayFileInfo>? replayFileInfos = ReplayFile.GetReplayFiles(gameId);
                    if (replayFileInfos.Count >= 0)
                    {
                        ReplayFilesDataGrid.DataContext = replayFileInfos;
                    }
                }
                catch (Exception ex)
                {
                    _ = MessageBox.Show(this, ex.Message, "エラー",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void ConfigureMainWindowSettings()
        {
            MainWindowSettings mainWindowSettings = SettingsConfigurator.ConfigureMainWindowSettings();

            GameComboBox.SelectedIndex = GameDictionary[mainWindowSettings.SelectedGameId];
            this.Width = mainWindowSettings.Width;
            this.Height = mainWindowSettings.Height;
        }

        private void SaveMainWindowSettings()
        {
            MainWindowSettings mainWindowSettings = new()
            {
                SelectedGameId = this.GameId,
                Width = this.Width,
                Height = this.Height
            };

            SettingsConfigurator.SaveMainWindowSettings(mainWindowSettings);
        }

        private void BrowseButtonClick(object sender, RoutedEventArgs e)
        {
            OpenFolderDialog openFolderDialog = new();

            if (openFolderDialog.ShowDialog() == true)
            {
                ReplayDirectoryPathBox.Text = openFolderDialog.FolderName;
                ReplayDirectoryPath.SetReplayDirectoryPath(this.GameId, openFolderDialog.FolderName);

                ViewReplayFilesList();
            }
        }

        private void GameComboBoxSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (GameComboBox.SelectedIndex > -1)
            {
                ComboBoxItem gameItem = (ComboBoxItem)GameComboBox.SelectedItem;
                string gameId = gameItem.Uid;
                if (gameId != null)
                {
                    this.GameId = gameId;

                    string? replayDirectory = ReplayDirectoryPath.GetReplayDirectoryPath(gameId);
                    ReplayDirectoryPathBox.Text = replayDirectory;

                    ViewReplayFilesList();
                }
                else
                {
                    this.GameId = string.Empty;
                }
            }
        }

        private void WindowClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                SettingsConfigurator.SaveReplayDirectoryPathSettings();
                SaveMainWindowSettings();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, $"設定の保存に失敗しました。\n{ex.Message}", "エラー",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}