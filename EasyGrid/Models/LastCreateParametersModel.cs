using System.Configuration;

namespace EasyGrid.Models
{
    [SettingsSerializeAs(SettingsSerializeAs.Xml)]
    public class LastCreateParametersModel
    {
        public string LastSavePath { get; set; }
        public int SquareSize { get; set; }
    }
}
