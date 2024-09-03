namespace MARISA.NET
{
    /// <summary>
    /// RenameDialog.xaml の相互作用ロジック
    /// </summary>
    public partial class RenameDialog : Window
    {
        private string? _replayFileName;

        public string? ReplayFileName
        {
            get
            {
                return _replayFileName;
            }

            set
            {
                _replayFileName = value;
                FileNameBox.Text = value;
            }
        }

        public RenameDialog()
        {
            InitializeComponent();
        }

        private void RenameButtonClick(object sender, RoutedEventArgs e)
        {
            if (FileNameBox.Text.Length > 0)
            {
                this.ReplayFileName = FileNameBox.Text;
                this.DialogResult = true;
            }
            else
            {
                MessageBox.Show(this, "ファイル名を空にはできません。", "リプレイファイルのリネーム",
                    MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }
    }
}
