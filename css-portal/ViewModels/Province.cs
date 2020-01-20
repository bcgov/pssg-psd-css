using System;
using System.Collections.Generic;
using System.Linq;
using Gov.Pssg.Css.Public.Utility;

namespace Gov.Pssg.Css.Public.ViewModels
{
    public class Province
    {
        public string Name { get; }

        public string Value { get; }

        public Province(string name, string value)
        {
            Name = name;
            Value = value;
        }

        public static string LabelAlberta = "Alberta";
        public static string LabelBritishColumbia = "British Columbia";
        public static string LabelManitoba = "Manitoba";
        public static string LabelNewBrunswick = "New Brunswick";
        public static string LabelNewfoundland = "Newfoundland";
        public static string LabelNorthwestTerritories = "Northwest Territories";
        public static string LabelNovaScotia = "Nova Scotia";
        public static string LabelNunavut = "Nunavut";
        public static string LabelOntario = "Ontario";
        public static string LabelPrinceEdwardIsland = "Prince Edward Island";
        public static string LabelQuebec = "Quebec";
        public static string LabelSaskatchewan = "Saskatchewan";
        public static string LabelYukon = "Yukon";

        public static IEnumerable<Province> GetProvinces()
        {
            var provinces = new List<Province>
            {
                new Province(LabelAlberta, Constants.ProvinceAlberta),
                new Province(LabelBritishColumbia, Constants.ProvinceBritishColumbia),
                new Province(LabelManitoba, Constants.ProvinceManitoba),
                new Province(LabelNewBrunswick, Constants.ProvinceNewBrunswick),
                new Province(LabelNewfoundland, Constants.ProvinceNewfoundland),
                new Province(LabelNorthwestTerritories, Constants.ProvinceNorthwestTerritories),
                new Province(LabelNovaScotia, Constants.ProvinceNovaScotia),
                new Province(LabelNunavut, Constants.ProvinceNunavut),
                new Province(LabelOntario, Constants.ProvinceOntario),
                new Province(LabelPrinceEdwardIsland, Constants.ProvincePrinceEdwardIsland),
                new Province(LabelQuebec, Constants.ProvinceQuebec),
                new Province(LabelSaskatchewan, Constants.ProvinceSaskatchewan),
                new Province(LabelYukon, Constants.ProvinceYukon),
            };

            return provinces;
        }
    }
}
