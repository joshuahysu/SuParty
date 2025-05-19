using System.ComponentModel.DataAnnotations;

namespace SuParty.Service.Referrer
{
    class Mservice
    {
        static void Main1(string[] args)
        {
            // 建立會員樹狀結構
            ReferrerMember root = new ReferrerMember("A", "老大");
            ReferrerMember b = new ReferrerMember("B", "左邊的人");
            ReferrerMember c = new ReferrerMember("C", "右邊的人");
            root.AddChild(b, true);  // 左
            root.AddChildAuto(c, false); // 右

            ReferrerMember d = new ReferrerMember("D", "左的下線");
            b.AddChild(d, true);

            // 模擬 D 銷售產品產生點數
            BonusCalculator.AddBV(d, 1200);
            BonusCalculator.AddBV(d, 1200);

            // 執行分潤結算
            BonusCalculator.CalculateWeeklyBonus(new List<ReferrerMember> { root, b, c, d });

            // 顯示結果
            Console.WriteLine("====== 會員收益結果 ======");
            foreach (var member in new[] { root, b, c, d })
            {
                Console.WriteLine($"{member.Name}（{member.Id}）：收入 = {member.TotalEarnings}");
                foreach (var log in member.BonusLogs)
                    Console.WriteLine($"  - {log}");
            }
            BonusCalculator.DistributeLeadershipBonus(new List<ReferrerMember> { root, b, c, d });

            Console.WriteLine("\n完成。按任意鍵結束...");
            Console.ReadKey();
        }
    }

    public class ReferrerMember
    {
        [Key]
        public string Id { get; set; }

        public string Name { get; set; }

        // 外鍵屬性 + 導覽屬性
        public string? LeftId { get; set; }
        public ReferrerMember? Left { get; set; }

        public string? RightId { get; set; }
        public ReferrerMember? Right { get; set; }

        public string? SponsorId { get; set; }
        public ReferrerMember? Sponsor { get; set; }

        public int LeftPoints { get; set; }
        public int RightPoints { get; set; }
        public int TotalEarnings { get; set; }

        public List<string> BonusLogs { get; set; } = new();

        public ReferrerMember(string id, string name)
        {
            Id = id;
            Name = name;
        }

        // 無參數建構子（EF Core 需要）
        public ReferrerMember() { }

        public void AddChild(ReferrerMember child, bool isLeft)
        {
            if (isLeft)
            {
                if (Left != null) throw new Exception("左線已有人");
                Left = child;
            }
            else
            {
                if (Right != null) throw new Exception("右線已有人");
                Right = child;
            }

            child.Sponsor = this;
        }
        public void AddChildAuto(ReferrerMember child, bool preferLeft=true)
        {
            var queue = new Queue<ReferrerMember>();
            queue.Enqueue(this);

            while (queue.Count > 0)
            {
                var current = queue.Dequeue();

                if (preferLeft)
                {
                    if (current.Left == null)
                    {
                        current.Left = child;
                        child.Sponsor = current;
                        return;
                    }
                    else queue.Enqueue(current.Left);

                    if (current.Right == null)
                    {
                        current.Right = child;
                        child.Sponsor = current;
                        return;
                    }
                    else queue.Enqueue(current.Right);
                }
                else // 右優先
                {
                    if (current.Right == null)
                    {
                        current.Right = child;
                        child.Sponsor = current;
                        return;
                    }
                    else queue.Enqueue(current.Right);

                    if (current.Left == null)
                    {
                        current.Left = child;
                        child.Sponsor = current;
                        return;
                    }
                    else queue.Enqueue(current.Left);
                }
            }

            throw new Exception("無可用空位");
        }

    }
    public static class BonusCalculator
    {
        const int RequiredLeftPoints = 1200;
        const int RequiredRightPoints = 1200;
        const int BonusAmount = 5000;
        const int ReferralBonus = 500;

        // 點數向上累加
        public static void AddBV(ReferrerMember source, int points)
        {
            ReferrerMember? current = source.Sponsor;
            while (current != null)
            {
                if (current.Left == source)
                    current.LeftPoints += points;
                else if (current.Right == source)
                    current.RightPoints += points;

                source = current;
                current = current.Sponsor;
            }
        }

        

        // 新增層級獎金設定
        static readonly List<(int Level, double Percent)> ManagementLevels = new()
        {
        (1, 0.05), // 上一層 5%
        (2, 0.03)  // 上上層 3%
        };
        // 每週分潤結算
        public static void CalculateWeeklyBonus(List<ReferrerMember> members)
        {
            foreach (var m in members)
            {
                while (m.LeftPoints >= RequiredLeftPoints && m.RightPoints >= RequiredRightPoints)
                {
                    m.TotalEarnings += BonusAmount;
                    m.BonusLogs.Add($"週獎金：${BonusAmount}（左{RequiredLeftPoints} + 右{RequiredRightPoints}）");

                    m.LeftPoints -= RequiredLeftPoints;
                    m.RightPoints -= RequiredRightPoints;

                    // 推薦獎
                    if (m.Sponsor != null)
                    {
                        m.Sponsor.TotalEarnings += ReferralBonus;
                        m.Sponsor.BonusLogs.Add($"推薦獎金：${ReferralBonus}（來自 {m.Name}）");
                    }

                    // 管理獎
                    ApplyManagementBonus(m, BonusAmount);
                }
            }
        }

        // 往上層層給管理獎
        private static void ApplyManagementBonus(ReferrerMember earner, int earnedBonus)
        {
            ReferrerMember? current = earner.Sponsor;
            int level = 1;

            while (current != null && level <= ManagementLevels.Count)
            {
                double percent = ManagementLevels[level - 1].Percent;
                int mgmtBonus = (int)(earnedBonus * percent);
                current.TotalEarnings += mgmtBonus;
                current.BonusLogs.Add($"管理獎：${mgmtBonus}（來自第 {level} 層 {earner.Name}）");

                current = current.Sponsor;
                level++;
            }
        }

        public static void DistributeLeadershipBonus(List<ReferrerMember> members)
        {
            // 假設每週抽出 1% 總發放額度作為領導獎
            int totalBonus = members.Sum(m => m.TotalEarnings);
            int pool = (int)(totalBonus * 0.01);

            // 假設資格：獎金總額 >= 10000
            var leaders = members.Where(m => m.TotalEarnings >= 10000).ToList();

            if (leaders.Any())
            {
                int perLeader = pool / leaders.Count;
                foreach (var leader in leaders)
                {
                    leader.TotalEarnings += perLeader;
                    leader.BonusLogs.Add($"領導獎：${perLeader}（來自總獎金池）");
                }
            }
        }

    }

}