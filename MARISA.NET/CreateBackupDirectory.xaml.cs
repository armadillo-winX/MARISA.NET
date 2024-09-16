namespace MARISA.NET
{
    /// <summary>
    /// CreateBackupDirectory.xaml の相互作用ロジック
    /// </summary>
    public partial class CreateBackupDirectory : Window
    {
        public string? GameId { get; set; }

        public string? ReplayFilePath { get; set; }

        public CreateBackupDirectory()
        {
            InitializeComponent();
        }

        private bool CheckFileName(string fileName)
        {
            string[] prohibitedLetters = ["\\", "/", ":", "*", "?", "\"", "<", ">", "|"];
            foreach (string prohibitedLetter in prohibitedLetters)
            {
                if (fileName.Contains(prohibitedLetter))
                {
                    return false;
                }
            }

            return true;
        }

        private void CreateButtonClick(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(this.GameId) && !string.IsNullOrEmpty(this.ReplayFilePath))
            {
                try
                {
                    string backupName = BackupNameBox.Text;
                    string commnet = CommentBox.Text;
                    if (backupName.Length > 0 && CheckFileName(backupName))
                    {
                        if (!ReplayFile.BackupExists(this.GameId, backupName))
                        {
                            ReplayFile.CreateBackup(
                            this.GameId, this.ReplayFilePath, backupName, commnet);
                            this.Close();
                        }
                        else
                        {
                            MessageBoxResult result =
                                MessageBox.Show(this, $"'{backupName}' は既に存在します。\n上書きしますか?",
                                "バックアップの作成", MessageBoxButton.YesNo, MessageBoxImage.Information);
                            if (result == MessageBoxResult.Yes)
                            {
                                ReplayFile.CreateBackup(
                                this.GameId, this.ReplayFilePath, backupName, commnet);
                                this.Close();
                            }
                        }
                    }
                    else if (backupName.Length == 0)
                    {
                        MessageBox.Show(this, "バックアップ名を空にはできません。", "バックアップの作成",
                            MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    }
                    else if (!CheckFileName(backupName))
                    {
                        MessageBox.Show(this,
                            "バックアップ名に以下の文字は含めることはできません。\n\\/:*?\"<>|", "バックアップの作成",
                            MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(this, $"バックアップの作成に失敗しました。\n{ex.Message}", "エラー",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show(this,
                    "作品あるいはバックアップ作成元のリプレイファイルが指定されていないのでバックアップの作成ができません。",
                    "エラー",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
