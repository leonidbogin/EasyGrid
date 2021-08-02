using EasyGrid.Models;

namespace EasyGrid.Providers
{
    public class SettingsProvider
    {
        public LastCreateParametersModel LastCreateParameters { get; set; }
        public SASPlanetParametersModel SASPlanetParameters { get; set; }


        public SettingsProvider()
        {
            Refresh();
        }

        public void Refresh()
        {
            LastCreateParameters = Properties.Settings.Default.LastCreateParameters ?? new LastCreateParametersModel();
            SASPlanetParameters = Properties.Settings.Default.SASPlanetParameters ?? new SASPlanetParametersModel();
        }

        public void Save()
        {
            Properties.Settings.Default.LastCreateParameters = LastCreateParameters;
            Properties.Settings.Default.SASPlanetParameters = SASPlanetParameters;
            Properties.Settings.Default.Save();
        }
    }
}
