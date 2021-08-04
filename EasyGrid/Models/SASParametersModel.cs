using System.Configuration;

namespace EasyGrid.Models
{
    [SettingsSerializeAs(SettingsSerializeAs.Xml)]
    public class SASParametersModel
    {
        public string SASPath { get; set; }
    }
}
