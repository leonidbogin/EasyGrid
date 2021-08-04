using System.Configuration;

namespace EasyGrid.Models
{
    [SettingsSerializeAs(SettingsSerializeAs.Xml)]
    public class SASParametersModel
    {
        public string LastSelectionPath { get; set; }
    }
}
