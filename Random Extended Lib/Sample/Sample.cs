using System.Collections.Generic;
using UnityEngine;
using RandomEx;

public class Sample : MonoBehaviour
{
    public List<RandomItem> randomItems;  // インスペクターで設定したアイテムリスト

    void Start()
    {
        // 確率ベースでランダムアイテムを選択
        string result = RandomExCore.GetRandomItemFromList(randomItems, RandomSelectionType.Probability);
        Debug.Log("Probability-based result: " + result);

        // 重み付けベースでランダムアイテムを選択
        result = RandomExCore.GetRandomItemFromList(randomItems, RandomSelectionType.Weighted);
        Debug.Log("Weighted-based result: " + result);

        // 累積確率ベースでランダムアイテムを選択
        result = RandomExCore.GetRandomItemFromList(randomItems, RandomSelectionType.CumulativeProbability);
        Debug.Log("Cumulative-based result: " + result);

        // 10連ガチャ
        var tenPullResults = RandomExCore.GetRandomItemsFromList(randomItems, RandomSelectionType.Weighted, 100);
        foreach (var pullResult in tenPullResults)
        {
            Debug.Log("10連結果: " + pullResult);
        }
    }
}
