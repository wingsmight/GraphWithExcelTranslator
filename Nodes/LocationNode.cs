using System;
using System.Collections.Generic;

namespace ExcelToGraph
{
    public class LocationNode : Node
    {
        public static readonly Dictionary<string, string> translatedNames = new Dictionary<string, string>()
        {
            {"деревня издали", "VillageFarView"},
            {"деревня вблизи", "VillageCloseViewDay"},
            {"лавка дандаро", "DandaroShop"},
            {"таверна в селе", "TavernInKondin"},
            {"хижина тётки", "ZariGrandmaShack"},
            {"деревня вечер", "VillageCloseViewNight"},
            {"4", "VillageFired"},
        };


        private string locationName;


        public LocationNode(Vector2 position, int referenceId, string locationName) : base(position, referenceId)
        {
            this.locationName = TranslateLocationName(locationName);
        }


        private string TranslateLocationName(string ruName)
        {
            if (string.IsNullOrEmpty(ruName))
                return "BlackScreen";

            ruName = ruName.ToLower();

            foreach (var translatedName in translatedNames)
            {
                if (ruName.Contains(translatedName.Key))
                {
                    return translatedName.Value;
                }
            }

            throw new Exception($"Cannot translate location name \"{ruName}\"");
        }
        public override string ToString()
        {
            string text = base.ToString() + '\n';
            text += TAB.Multiply(4) + "name: " + locationName;

            return text;
        }
    }
}
