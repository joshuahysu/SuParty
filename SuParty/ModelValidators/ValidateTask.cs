using System;
using System.ComponentModel.DataAnnotations;
using Huede.FMC2.Entities;
namespace Huede.FMC2.ModelValidators
{
    /// <summary>
    /// 表示一份工作單
    /// </summary>
    public partial class ValidateTask:Task
    {
        /// <summary>
        /// 初始化工作單與外鍵屬性
        /// </summary>
        public ValidateTask() : base()
        {

        }

        #region 欄位 

        /// <summary>
        /// 工作單號碼，為院內識別碼或條碼
        /// </summary>
        [Required(ErrorMessage = "工作單號是必填欄位")]
        [MinLength(1, ErrorMessage = "工作單號是必填欄位")]
        public string TaskNumber { get; set; }

        /// <summary>
        /// [Meta] 建立時間
        /// </summary>
        [Required(ErrorMessage = "建立時間是必填欄位")]
        public DateTime CreateTime { get; set; }

        #endregion

        #region 外鍵
        /// <summary>
        /// 此工作所屬之區域
        /// </summary>
        [Required(ErrorMessage = "區域是必填欄位")]
        public virtual Location Location { get; set; }
        #endregion
    }
}
