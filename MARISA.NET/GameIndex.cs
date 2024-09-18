using System.Collections.Generic;

namespace MARISA.NET
{
    internal class GameIndex
    {
        private readonly static Dictionary<string, string> _gameNameDictionary = new()
        {
            { "Th06", "東方紅魔郷" },
            { "Th07", "東方妖々夢" },
            { "Th08", "東方永夜抄" },
            { "Th09", "東方花映塚" },
            { "Th095", "東方文花帖" },
            { "Th10", "東方風神録" },
            { "Th11", "東方地霊殿" },
            { "Th12", "東方星蓮船" },
            { "Th125", "ダブルスポイラー" },
            { "Th128", "妖精大戦争" },
            { "Th13", "東方神霊廟" },
            { "Th14", "東方輝針城" },
            { "Th143", "弾幕アマノジャク" },
            { "Th15", "東方紺珠伝" },
            { "Th16", "東方天空璋" },
            { "Th165", "秘封ナイトメアダイアリー" },
            { "Th17", "東方鬼形獣" },
            { "Th18", "東方虹龍洞" },
            { "Th185", "バレットフィリア達の闇市場" },
            { "Th19", "東方獣王園" }
        };

        public static string GetGameName(string gameId)
        {
            if (_gameNameDictionary.TryGetValue(gameId, out string? gameName))
            {
                return gameName;
            }
            else
            {
                return string.Empty;
            }
        }
    }
}
