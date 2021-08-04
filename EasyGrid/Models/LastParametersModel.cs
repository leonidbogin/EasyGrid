using System.Configuration;

namespace EasyGrid.Models
{
    [SettingsSerializeAs(SettingsSerializeAs.Xml)]
    public class LastParametersModel
    {
        public string LastSavePath { get; set; }
        public int SquareSize { get; set; }
    }
}
