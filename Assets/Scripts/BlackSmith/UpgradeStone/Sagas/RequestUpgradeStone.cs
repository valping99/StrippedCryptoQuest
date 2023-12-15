using CryptoQuest.Item.MagicStone;
using IndiGames.Core.Events;

namespace CryptoQuest.BlackSmith.UpgradeStone.Sagas
{
    public class RequestUpgradeStone : ActionBase
    {
        public IMagicStone[] Stones;

        public RequestUpgradeStone(IMagicStone[] stoneIds)
        {
            Stones = stoneIds;
        }
    }

    public class ResponseUpgradeStoneFailed : ActionBase { }

    public class ResponseUpgradeStoneSuccess : ActionBase
    {
        public IMagicStone Stone;

        public ResponseUpgradeStoneSuccess(IMagicStone stone)
        {
            Stone = stone;
        }
    }
}