using EasyGrid.Models;

namespace EasyGrid.Providers
{
    public class SettingsProvider
    {
        public LastParametersModel LastCreateParameters { get; set; }
        public SASParametersModel SASPlanetParameters { get; set; }


        public SettingsProvider()
        {
            Refresh();
        }

        public void Refresh()
        {
            LastCreateParameters = Properties.Settings.Default.LastParameters ?? new LastParametersModel();
            SASPlanetParameters = Properties.Settings.Default.SASParameters ?? new SASParametersModel();
        }

        public void Save()
        {
            Properties.Settings.Default.LastParameters = LastCreateParameters;
            Properties.Settings.Default.SASParameters = SASPlanetParameters;
            Properties.Settings.Default.Save();
        }
    }
}
