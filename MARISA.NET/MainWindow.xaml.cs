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
                { "Th18", 12 },
                { "Th19", 13 }
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

            EnableDragDrop(this);
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

        private void EnableDragDrop(Control control)
        {
            //ドラッグ＆ドロップを受け付けられるようにする
            control.AllowDrop = true;

            //ドラッグが開始された時のイベント処理（マウスカーソルをドラッグ中のアイコンに変更）
            control.PreviewDragOver += (s, e) =>
            {
                //ファイルがドラッグされたとき、カーソルをドラッグ中のアイコンに変更し、そうでない場合は何もしない。
                e.Effects = e.Data.GetDataPresent(DataFormats.FileDrop) ? DragDropEffects.Copy : e.Effects = DragDropEffects.None;
                e.Handled = true;
            };

            //ドラッグ＆ドロップが完了した時の処理（ファイル名を取得し、ファイルの中身をTextプロパティに代入）
            control.PreviewDrop += (s, e) =>
            {
                if (e.Data.GetDataPresent(DataFormats.FileDrop)) // ドロップされたものがファイルかどうか確認する。
                {
                    string[] paths = (string[])e.Data.GetData(DataFormats.FileDrop);
                    //--------------------------------------------------------------------
                    // ここに、ドラッグ＆ドロップ受付時の処理を記述する
                    //--------------------------------------------------------------------
                    MainTabControl.SelectedIndex = 1;

                    foreach (string path in paths)
                    {
                        ReplayFilesListBox.Items.Add(path);
                    }
                }
            };

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

        private async void ImportButtonClick(object sender, RoutedEventArgs e)
        {
            if (ReplayFilesListBox.Items.Count > 0)
            {
                string[] replayFiles = new string[ReplayFilesListBox.Items.Count];

                int i = 0;
                foreach (object replayFileItem in ReplayFilesListBox.Items)
                {
                    replayFiles[i] = (string)replayFileItem;
                    i++;
                }

                ImportButton.IsEnabled = false;
                await Task.Run(() =>
                {
                    foreach (string replayFile in replayFiles)
                    {
                        string message = ReplayFile.Import(replayFile);

                        this.Dispatcher.Invoke((Action)(() =>
                        {
                            ReplayFilesListBox.Items.Remove(replayFile);
                            OutputBox.Text += $"{message}\n";
                        }
                        ));
                    }
                });
                ImportButton.IsEnabled = true;
                ViewReplayFilesList();
            }
            else
            {
                MessageBox.Show(this, "取り込むリプレイファイルがありません。", "リプレイファイルの取り込み",
                    MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }

        private void ReloadButtonClick(object sender, RoutedEventArgs e)
        {
            ViewReplayFilesList();
        }

        private void RenameButtonClick(object sender, RoutedEventArgs e)
        {
            if (ReplayFilesDataGrid.Items.Count > 0
                && ReplayFilesDataGrid.SelectedIndex >= 0)
            {
                ReplayFileInfo selectedReplayFileInfo = ReplayFilesDataGrid.SelectedItem as ReplayFileInfo;
                string replayFileName = selectedReplayFileInfo.FileName;

                RenameDialog renameDialog = new()
                {
                    ReplayFileName = Path.GetFileNameWithoutExtension(replayFileName),
                    Owner = this
                };

                if (renameDialog.ShowDialog() == true)
                {
                    string newReplayFileName = $"{renameDialog.ReplayFileName}.rpy";

                    if (!ReplayFile.Exists(this.GameId, newReplayFileName))
                    {
                        try
                        {
                            ReplayFile.Rename(this.GameId, replayFileName, newReplayFileName);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(this, $"リプレイファイルのリネームに失敗しました。\n{ex.Message}", "エラー",
                                MessageBoxButton.OK, MessageBoxImage.Error);
                        }

                        ViewReplayFilesList();
                    }
                    else
                    {
                        MessageBox.Show(this, $"'{newReplayFileName}' は既に存在します。", "リプレイファイルのリネーム",
                            MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    }
                }
            }
        }

        private void DeleteButtonClick(object sender, RoutedEventArgs e)
        {
            if (ReplayFilesDataGrid.Items.Count > 0
                && ReplayFilesDataGrid.SelectedIndex >= 0)
            {
                ReplayFileInfo selectedReplayFileInfo = ReplayFilesDataGrid.SelectedItem as ReplayFileInfo;
                string replayFileName = selectedReplayFileInfo.FileName;

                MessageBoxResult result = MessageBox.Show(
                    this, $"'{replayFileName}' を削除してもよろしいですか。", "リプレイファイルの削除",
                    MessageBoxButton.YesNo, MessageBoxImage.Information);
                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        ReplayFile.Delete(this.GameId, replayFileName);

                        ViewReplayFilesList();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(this, $"リプレイファイルの削除に失敗しました。\n{ex.Message}", "エラー",
                            MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }
    }
}