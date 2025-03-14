using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SuParty.Data;
using SuParty.Service.Wallet;
using System;
using System.Collections.Generic;
using System.Linq;
namespace SuParty.Service.Referrer
{
    public class ReferrersService
    {
        private readonly ApplicationDbContext _dbContext;

        public ReferrersService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public void onlyOne(string id,decimal amount,decimal b) {
            var user=_dbContext.UserWallets.Find(id);
            var ReferrerId=user.Referrers.FirstOrDefault();
            if (!ReferrerId.IsNullOrEmpty())
            {
                var Referrer = _dbContext.UserWallets.Find(ReferrerId);
                _dbContext.UpdateWallet(ReferrerId, amount * b);
            }
        }


        public void t(string id)
        {
            //誰可以分
        }
    }
    
        // 獨立事業經營者（IBO）
        public class IBO
        {
            public string Name { get; set; }
            public decimal PersonalBV { get; set; } // 個人業務量
            public List<IBO> LeftLine { get; set; } // 左線下線
            public List<IBO> RightLine { get; set; } // 右線下線
            public decimal RetailProfit { get; private set; } // 零售利潤
            public string Level { get; private set; } // 級別（如白金）

            public IBO(string name)
            {
                Name = name;
                LeftLine = new List<IBO>();
                RightLine = new List<IBO>();
                Level = "新手";
            }

            // 添加下線到左線或右線
            public void AddDownline(IBO downline, bool isLeft)
            {
                if (isLeft) LeftLine.Add(downline);
                else RightLine.Add(downline);
            }

            // 計算零售利潤（25% 利潤率）
            public void CalculateRetailProfit(decimal wholesalePricePerUnit, int unitsSold)
            {
                decimal retailPricePerUnit = wholesalePricePerUnit * 1.25m;
                RetailProfit = (retailPricePerUnit - wholesalePricePerUnit) * unitsSold;
            }

            // 計算總 BV（個人 + 左右線）
            public decimal GetTotalBV()
            {
                decimal leftBV = LeftLine.Sum(d => d.GetTotalBV());
                decimal rightBV = RightLine.Sum(d => d.GetTotalBV());
                return PersonalBV + leftBV + rightBV;
            }

            // 設定級別
            public void SetLevel()
            {
                decimal totalBV = GetTotalBV();
                if (totalBV >= 15000) Level = "鑽石";
                else if (totalBV >= 10000) Level = "白金";
                else if (totalBV >= 1200) Level = "銀級";
                else Level = "新手";
            }
        }

        // 業績獎金計算器
        public static class PerformanceBonusCalculator
        {
            private static readonly Dictionary<decimal, decimal> BonusTiers = new Dictionary<decimal, decimal>
        {
            { 300m, 0.03m },  // 3%
            { 600m, 0.06m },  // 6%
            { 1200m, 0.09m }, // 9%
            { 4000m, 0.15m }, // 15%
            { 10000m, 0.21m }, // 21%
            { 15000m, 0.25m } // 25%
        };

            // 根據 BV 獲取獎金百分比
            public static decimal GetBonusPercentage(decimal bv)
            {
                var tier = BonusTiers.Keys.Where(k => bv >= k).OrderByDescending(k => k).FirstOrDefault();
                return tier == 0 ? 0m : BonusTiers[tier];
            }

            // 計算總獎金（個人 + 左右線差額）
            public static decimal CalculateTotalBonus(IBO ibo)
            {
                ibo.SetLevel();
                decimal totalBV = ibo.GetTotalBV();
                decimal personalBV = ibo.PersonalBV;
                decimal totalBonusPercentage = GetBonusPercentage(totalBV);
                decimal personalBonusPercentage = GetBonusPercentage(personalBV);

                // 個人獎金
                decimal personalBonus = personalBV * personalBonusPercentage;

                // 左線和右線差額獎金
                decimal leftBV = ibo.LeftLine.Sum(d => d.GetTotalBV());
                decimal rightBV = ibo.RightLine.Sum(d => d.GetTotalBV());
                decimal teamBonus = 0m;

                foreach (var downline in ibo.LeftLine.Concat(ibo.RightLine))
                {
                    decimal downlineTotalBV = downline.GetTotalBV();
                    decimal downlineBonusPercentage = GetBonusPercentage(downlineTotalBV);
                    teamBonus += downlineTotalBV * (totalBonusPercentage - downlineBonusPercentage);
                }

                // 額外領導獎金（假設白金以上有 5% 額外獎勵）
                decimal leadershipBonus = ibo.Level == "白金" || ibo.Level == "鑽石" ? totalBV * 0.05m : 0m;

                return personalBonus + teamBonus + leadershipBonus;
            }
        }

        class Program
        {
            static void Main1(string[] args)
            {
                // 模擬結構
                var topIBO = new IBO("Alice");
                topIBO.PersonalBV = 2000m; // Alice 個人 BV
                topIBO.CalculateRetailProfit(80m, 20);

                var leftIBO = new IBO("Bob");
                leftIBO.PersonalBV = 1000m; // 左線 Bob
                leftIBO.CalculateRetailProfit(80m, 10);

                var rightIBO = new IBO("Charlie");
                rightIBO.PersonalBV = 800m; // 右線 Charlie
                rightIBO.CalculateRetailProfit(80m, 8);

                var leftDownline = new IBO("David");
                leftDownline.PersonalBV = 400m; // Bob 的下線
                leftDownline.CalculateRetailProfit(80m, 4);

                // 建立左右線結構
                topIBO.AddDownline(leftIBO, true); // 左線
                topIBO.AddDownline(rightIBO, false); // 右線
                leftIBO.AddDownline(leftDownline, true);

                // 計算並顯示結果
                DisplayResults(topIBO);
                DisplayResults(leftIBO);
                DisplayResults(rightIBO);
                DisplayResults(leftDownline);
            }

            static void DisplayResults(IBO ibo)
            {
                Console.WriteLine($"=== {ibo.Name} 的收入 ===");
                Console.WriteLine($"級別: {ibo.Level}");
                Console.WriteLine($"個人零售利潤: ${ibo.RetailProfit}");
                Console.WriteLine($"總 BV: {ibo.GetTotalBV()}");
                Console.WriteLine($"總獎金: ${PerformanceBonusCalculator.CalculateTotalBonus(ibo)}");
                Console.WriteLine();
            }
        }   
}