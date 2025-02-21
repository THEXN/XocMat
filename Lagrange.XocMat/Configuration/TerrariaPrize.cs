

using Lagrange.XocMat.Attributes;
using Lagrange.XocMat.Internal.Terraria;
using Newtonsoft.Json;

namespace Lagrange.XocMat.Configuration;

[ConfigSeries]
public class TerrariaPrize : JsonConfigBase<TerrariaPrize>
{
    [JsonProperty("抽奖费用")]
    public long Fess = 10;

    [JsonProperty("奖池内容")]
    public List<Prize> Pool = [];

    protected override string Filename => "Prize";

    public Prize? Next()
    {
        Random random = new Random();
        int totalWeight = Pool.Sum(x => x.Probability);
        int randomNumber = random.Next(1, totalWeight + 1);
        int num = 0;
        foreach (Prize prize in Pool)
        {
            num += prize.Probability;
            if (randomNumber <= num)
            {
                return prize;
            }
        }
        return Pool.Count != 0 ? Pool[random.Next(0, Pool.Count - 1)] : null;
    }

    public Prize? GetPrize(int id)
    {
        return id > 0 && id <= Pool.Count ? Pool[id - 1] : null;
    }

    public List<Prize> Nexts(int count)
    {
        List<Prize> res = [];
        for (int i = 0; i < count; i++)
        {
            Prize? item = Next();
            if (item != null)
                res.Add(item);
        }
        return res;
    }

    public bool Add(string name, int id, int rate, int max, int min)
    {
        if (Pool.Any(x => x.Name == name || x.ID == id))
            return false;

        Pool.Add(new Prize()
        {
            Name = name,
            ID = id,
            Probability = rate,
            Max = max,
            Min = min
        });
        return true;
    }

    public bool Remove(string name)
    {
        return Pool.RemoveAll(x => x.Name == name) > 0;

    }

    public bool Remove(Prize prize)
    {
        return Pool.Remove(prize);

    }
}
