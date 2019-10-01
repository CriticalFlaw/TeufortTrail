using System;
using System.IO;
using System.Windows;
using System.Windows.Forms;
using Newtonsoft.Json;
using TeufortTrail;

namespace TeufortEditor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public GameSettings Settings = new GameSettings();

        public MainWindow()
        {
            InitializeComponent();
            LoadSettingsFile();
            _Status.Content = "Settings Initialized.";
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            SaveSettingsFile();
            _Status.Content = "Settings Saved.";
        }

        private void Load_Click(object sender, RoutedEventArgs e)
        {
            LoadSettingsFile(true);
            _Status.Content = "Settings Loaded.";
        }

        public void SaveSettingsFile()
        {
            Settings.STARTING_MONEY = Convert.ToInt32(StartingMoney.Text);

            var values = JsonConvert.SerializeObject(Settings);
            using (var writer = new StreamWriter(Properties.Settings.Default.FILE_PATH))
                writer.WriteLine(values);
        }

        public void LoadSettingsFile(bool openDialogOverride = false)
        {
            if (string.IsNullOrWhiteSpace(Properties.Settings.Default.FILE_PATH) || openDialogOverride)
            {
                var fileDialog = new OpenFileDialog()
                {
                    FileName = "settings*",
                    Filter = "JSON Files|*.json",
                    CheckFileExists = true
                };

                if (fileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK && (!string.IsNullOrWhiteSpace(fileDialog.FileName)))
                    Properties.Settings.Default.FILE_PATH = fileDialog.FileName;
                Properties.Settings.Default.Save();
            }

            using (var reader = new StreamReader(Properties.Settings.Default.FILE_PATH))
                Settings = JsonConvert.DeserializeObject<GameSettings>(reader.ReadToEnd());

            if (Settings == null)
                System.Windows.Application.Current.Shutdown();

            LoadGameSettings();
        }

        public void LoadGameSettings()
        {
            _FilePath.Content = Properties.Settings.Default.FILE_PATH;
            StartingMoney.Text = Settings.STARTING_MONEY.ToString();
        }
    }
}