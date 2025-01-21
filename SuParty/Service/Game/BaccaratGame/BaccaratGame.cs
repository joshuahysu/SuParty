namespace SuParty.Service.Game.BaccaratGame;
using System;
using System.Collections.Generic;
using System.Linq;

public class BaccaratGame
{
    private static Random random = new Random();
    private static List<string> deck = new List<string>();

    public static void Main(string[] args)
    {
        Console.WriteLine("歡迎來到百家樂遊戲！");
        InitializeDeck();
        ShuffleDeck();

        // 發牌
        List<string> playerHand = DealHand();
        List<string> bankerHand = DealHand();

        // 顯示初始牌
        Console.WriteLine($"玩家的牌: {string.Join(", ", playerHand)}");
        Console.WriteLine($"莊家的牌: {string.Join(", ", bankerHand)}");

        // 計算初始點數
        int playerPoints = CalculatePoints(playerHand);
        int bankerPoints = CalculatePoints(bankerHand);

        Console.WriteLine($"玩家的點數: {playerPoints}");
        Console.WriteLine($"莊家的點數: {bankerPoints}");

        // 玩家補牌邏輯
        if (playerPoints < 6)
        {
            string newCard = DrawCard();
            playerHand.Add(newCard);
            playerPoints = CalculatePoints(playerHand);
            Console.WriteLine($"玩家補了一張牌: {newCard}");
            Console.WriteLine($"玩家的點數: {playerPoints}");
        }

        // 莊家補牌邏輯
        if (bankerPoints < 6)
        {
            string newCard = DrawCard();
            bankerHand.Add(newCard);
            bankerPoints = CalculatePoints(bankerHand);
            Console.WriteLine($"莊家補了一張牌: {newCard}");
            Console.WriteLine($"莊家的點數: {bankerPoints}");
        }

        // 判斷結果
        DetermineWinner(playerPoints, bankerPoints);
    }

    // 初始化牌組
    private static void InitializeDeck()
    {
        string[] suits = { "♠", "♥", "♣", "♦" };
        string[] ranks = { "A", "2", "3", "4", "5", "6", "7", "8", "9", "10", "J", "Q", "K" };
        deck.Clear();

        foreach (var suit in suits)
        {
            foreach (var rank in ranks)
            {
                deck.Add($"{rank}{suit}");
            }
        }
    }

    // 洗牌
    private static void ShuffleDeck()
    {
        deck = deck.OrderBy(x => random.Next()).ToList();
    }

    // 發牌
    private static List<string> DealHand()
    {
        List<string> hand = new List<string> { DrawCard(), DrawCard() };
        return hand;
    }

    // 抽牌
    private static string DrawCard()
    {
        string card = deck[0];
        deck.RemoveAt(0);
        return card;
    }

    // 計算點數
    private static int CalculatePoints(List<string> hand)
    {
        int total = 0;
        foreach (var card in hand)
        {
            string rank = card.Substring(0, card.Length - 1); // 去掉花色
            if (int.TryParse(rank, out int value)) // 2-10
            {
                total += value;
            }
            else if (rank == "A")
            {
                total += 1;
            }
            else // J, Q, K
            {
                total += 0;
            }
        }
        return total % 10; // 只取個位數
    }

    // 判斷勝負
    private static void DetermineWinner(int playerPoints, int bankerPoints)
    {
        if (playerPoints > bankerPoints)
        {
            Console.WriteLine("玩家勝利！");
        }
        else if (playerPoints < bankerPoints)
        {
            Console.WriteLine("莊家勝利！");
        }
        else
        {
            Console.WriteLine("平手！");
        }
    }
}