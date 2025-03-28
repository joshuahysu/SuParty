using System;
using System.ComponentModel.DataAnnotations;
using Huede.FMC2.Entities;
namespace Huede.FMC2.ModelValidators
{
    /// <summary>
    /// 代表一位病人(孕/產婦)
    /// </summary>
    public partial class ValidatePatient : Patient
    {
        #region 欄位

        /// <summary>
        /// 病人識別碼，例如身份證號、護照號碼、健保卡號等
        /// </summary>
        [Required(ErrorMessage = "身份證號/護照號碼是必填欄位")]
        [MinLength(1, ErrorMessage = "身份證號/護照號碼是必填欄位")]
        public string Identifier { get; set; }

        /// <summary>
        /// 病歷號
        /// </summary>
        [Required(ErrorMessage = "病歷號是必填欄位")]
        [MinLength(1, ErrorMessage = "病歷號是必填欄位")]
        public string ChartNumber { get; set; }   

        /// <summary>
        /// 姓名
        /// </summary>
        [Required(ErrorMessage = "姓名是必填欄位")]
        [MinLength(1, ErrorMessage = "姓名是必填欄位")]
        public string Name { get; set; }

        /// <summary>
        /// 生日
        /// </summary>
        [Required(ErrorMessage = "生日是必填欄位")]
        public DateTime Birthday { get; set; }   

        #endregion

    }
}
