//using AutoMapper;
//using System.Collections.Generic;
//using System.ComponentModel.DataAnnotations;

//namespace Huede.FMC2.ModelValidators
//{
//    internal class ValidateModel
//    {
//        /// <summary>
//        /// 驗證欄位屬性是否合規
//        /// </summary>
//        /// <param name="validateData"></param>
//        /// <returns>結果字串</returns>
//        internal static string ValidateResult<TDestination>(object validateData)
//        {
//            if (validateData == null) return "驗證資料不應為null";

//            //轉成驗證類別
//            validateData = FatherToSon<TDestination>(validateData);
//            string resultMessage = string.Empty;
//            var context = new System.ComponentModel.DataAnnotations.ValidationContext(validateData, null, null);
//            var results = new List<ValidationResult>();
//            bool isValid = Validator.TryValidateObject(validateData, context, results, true);

//            if (!isValid)
//            {
//                foreach (var validationResult in results)
//                {
//                    resultMessage += validationResult.ErrorMessage;
//                }
//                return resultMessage;
//            }
//            return "Success";
//        }
//        /// <summary>
//        /// 將父類別轉換為子類別
//        /// </summary>
//        /// <typeparam name="TDestination"></typeparam>
//        /// <param name="data"></param>
//        /// <returns></returns>
//        internal static TDestination FatherToSon<TDestination>(object data)
//        {
//            // 配置 AutoMapper
//            var config = new MapperConfiguration(cfg =>
//            {
//                cfg.CreateMap(data.GetType(), typeof(TDestination)); // 使用 data 的類型作為來源類型
//            });

//            var mapper = config.CreateMapper();
//            TDestination mappedResult = mapper.Map<TDestination>(data); // 使用映射將 data 轉換為 TDestination

//            return mappedResult; // 返回映射的結果
//        }
//    }
//}
