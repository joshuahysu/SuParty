using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace SuParty.Data.DataModel.RealEstate
{
    public enum City
    {
        // 直轄市
        Taipei,        // 臺北市
        NewTaipei,     // 新北市
        Taoyuan,       // 桃園市
        Taichung,      // 臺中市
        Tainan,        // 臺南市
        Kaohsiung,     // 高雄市

        // 縣市
        Keelung,       // 基隆市
        HsinchuCity,   // 新竹市
        ChiayiCity,    // 嘉義市
        HsinchuCounty, // 新竹縣
        Miaoli,        // 苗栗縣
        Changhua,      // 彰化縣
        Nantou,        // 南投縣
        Yunlin,        // 雲林縣
        ChiayiCounty,  // 嘉義縣
        Pingtung,      // 屏東縣
        Yilan,         // 宜蘭縣
        Hualien,       // 花蓮縣
        Taitung,       // 臺東縣
        Penghu,        // 澎湖縣
        Kinmen,        // 金門縣
        Lienchiang     // 連江縣 (馬祖)
    }    
}