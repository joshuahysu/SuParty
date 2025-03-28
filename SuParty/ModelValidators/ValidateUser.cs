using System.ComponentModel.DataAnnotations;
using Huede.FMC2.Entities;
namespace Huede.FMC2.ModelValidators
{
    /// <summary>
    /// 代表一位使用者(醫師/護理人員/維護人員)
    /// </summary>
    public partial class ValidateUser:User
    {
        /// <summary>
        /// 初始化使用者與外鍵屬性
        /// </summary>
        public ValidateUser():base()
        {   
        }

        #region 欄位

        /// <summary>
        /// 登入密碼的雜湊值
        /// </summary>
        public string Password { get; set; } = "123456";

        /// <summary>
        /// 人員編號
        /// </summary>
        [Required(ErrorMessage = "人員編號是必填欄位")]
        [MinLength(1, ErrorMessage = "人員編號是必填欄位")]
        public string UserNumber { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        [Required(ErrorMessage = "姓名是必填欄位")]
        [MinLength(1, ErrorMessage = "姓名是必填欄位")]
        public string Name { get; set; }
      
        #endregion

    }
}
