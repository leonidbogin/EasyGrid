namespace EasyGrid.Providers
{
    public class SettingsProvider
    {
        public string LastFilePath { get; set; }
        public int LastSquareSize { get; set; }


        public SettingsProvider()
        {
            Refresh();
        }

        public void Refresh()
        {
            LastFilePath = Properties.Settings.Default.LastFilePath;
            LastSquareSize = Properties.Settings.Default.LastSquareSize;
        }

        public void Save()
        {
            Properties.Settings.Default.LastFilePath = LastFilePath;
            Properties.Settings.Default.LastSquareSize = LastSquareSize;
            Properties.Settings.Default.Save();
        }
    }
}
