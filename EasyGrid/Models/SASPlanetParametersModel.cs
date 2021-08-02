using System.Configuration;

namespace EasyGrid.Models
{
    [SettingsSerializeAs(SettingsSerializeAs.Xml)]
    public class SASPlanetParametersModel
    {
        public string LastSelectionPath { get; set; }
    }
}
