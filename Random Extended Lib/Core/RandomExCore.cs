using System;
using System.Collections.Generic;
using UnityEngine;

namespace RandomEx
{
    [System.Serializable]
    public class RandomItem
    {
        public string itemName;          // アイテム名
        public float value;              // 重みまたは確率
        public bool isPickup;            // ピックアップ対象かどうか
        public float pickupMultiplier = 1.0f; // ピックアップアイテムの倍率
    }

    // ランダム選択の方法を選択するEnum
    public enum RandomSelectionType
    {
        Probability,           // 確率に基づく選択
        Weighted,              // 重み付けによる選択
        CumulativeProbability  // 累積確率に基づく選択
    }

    public class RandomExCore
    {
        // 1つのアイテムを選ぶメソッド（選択方法を指定）
        public static string GetRandomItemFromList(List<RandomItem> randomItems, RandomSelectionType selectionType = RandomSelectionType.Weighted)
        {
            var items = new List<string>();
            var values = new List<float>();

            foreach (var randomItem in randomItems)
            {
                items.Add(randomItem.itemName);
                // ピックアップ対象アイテムには倍率を掛ける
                if (randomItem.isPickup)
                {
                    values.Add(randomItem.value * randomItem.pickupMultiplier); // ピックアップアイテムには倍率を適用
                }
                else
                {
                    values.Add(randomItem.value); // 通常の重み
                }
            }

            switch (selectionType)
            {
                case RandomSelectionType.Probability:
                    return GetRandomItem(items, values);
                case RandomSelectionType.Weighted:
                    return GetWeightedRandomItem(items, values);
                case RandomSelectionType.CumulativeProbability:
                    return GetItemByCumulativeProbability(items, values);
                default:
                    throw new ArgumentException("Invalid selection type.");
            }
        }

        // 複数のアイテムを選ぶメソッド
        public static List<string> GetRandomItemsFromList(List<RandomItem> randomItems, RandomSelectionType selectionType = RandomSelectionType.Weighted, int count = 1)
        {
            var results = new List<string>();
            var items = new List<string>();
            var values = new List<float>();

            foreach (var randomItem in randomItems)
            {
                items.Add(randomItem.itemName);
                // ピックアップ対象アイテムには倍率を掛ける
                if (randomItem.isPickup)
                {
                    values.Add(randomItem.value * randomItem.pickupMultiplier); // ピックアップアイテムには倍率を適用
                }
                else
                {
                    values.Add(randomItem.value); // 通常の重み
                }
            }

            for (int i = 0; i < count; i++)
            {
                results.Add(GetRandomItemFromList(randomItems, selectionType));
            }

            return results;
        }

        // 重み付け、確率ベース、累積確率のメソッドの例
        private static T GetRandomItem<T>(List<T> items, List<float> probabilities)
        {
            float totalProbability = 0f;
            foreach (var prob in probabilities)
                totalProbability += prob;

            float randomValue = UnityEngine.Random.Range(0f, 1f);
            float cumulative = 0f;

            for (int i = 0; i < items.Count; i++)
            {
                cumulative += probabilities[i];
                if (randomValue <= cumulative)
                    return items[i];
            }

            return items[items.Count - 1];
        }

        private static T GetWeightedRandomItem<T>(List<T> items, List<float> weights)
        {
            float totalWeight = 0f;
            foreach (var weight in weights)
                totalWeight += weight;

            if (totalWeight == 0f)
                throw new ArgumentException("The sum of the weights must not be zero.");

            if (totalWeight != 1f)
            {
                for (int i = 0; i < weights.Count; i++)
                {
                    weights[i] /= totalWeight; // 正規化
                }
            }

            float randomValue = UnityEngine.Random.Range(0f, 1f);
            float cumulativeWeight = 0f;

            for (int i = 0; i < items.Count; i++)
            {
                cumulativeWeight += weights[i];
                if (randomValue <= cumulativeWeight)
                    return items[i];
            }

            return items[items.Count - 1]; // fallback
        }

        private static T GetItemByCumulativeProbability<T>(List<T> items, List<float> weights)
        {
            float totalWeight = 0f;
            foreach (var weight in weights)
                totalWeight += weight;

            if (totalWeight == 0f)
                throw new ArgumentException("The sum of the weights must not be zero.");

            List<float> cumulativeProbabilities = new List<float>();
            float cumulative = 0f;
            foreach (var weight in weights)
            {
                cumulative += weight;
                cumulativeProbabilities.Add(cumulative);
            }

            float randomValue = UnityEngine.Random.Range(0f, 1f);

            for (int i = 0; i < cumulativeProbabilities.Count; i++)
            {
                if (randomValue <= cumulativeProbabilities[i])
                    return items[i];
            }

            return items[items.Count - 1]; // fallback
        }
    }
}
