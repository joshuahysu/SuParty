﻿using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace SuParty.Data.DataModel
{
    public class UserData
    {
        public string Id { get; set; } = "";
        public string Name { get; set; } = "";
        public DateTime Birthday { get; set; }
        public string? Email { get; set; } = "";
        public string NickName { get; set; } = "";
        public string Gender { get; set; } = "";
        /// <summary>
        /// 自介
        /// </summary>
        public string Introduction { get; set; } = "";
        public string Income { get; set; } = "";
        public string Budget { get; set; } = "";
        public string IG_Url { get; set; } = "";
        public string ExtraUrl { get; set; } = "";
        public List<string> ChatRooms { get; set; } = new();
        /// <summary>
        /// 儲存使用者的身高（以字串格式表示）。
        /// </summary>
        public string Height { get; set; } = "";

        /// <summary>
        /// 儲存使用者的體重（以字串格式表示）。
        /// </summary>
        public string Weight { get; set; } = "";

        /// <summary>
        /// 儲存使用者的胸圍（以字串格式表示）。
        /// </summary>
        public string Bust { get; set; } = "";

        /// <summary>
        /// 儲存使用者的腰圍（以字串格式表示）。
        /// </summary>
        public string Waist { get; set; } = "";

        /// <summary>
        /// 儲存使用者的臀圍（以字串格式表示）。
        /// </summary>
        public string Hips { get; set; } = "";
        /// <summary>
        /// 大頭貼位置
        /// </summary>
        public string AvatarUrl { get; set; } = "";

    }
}