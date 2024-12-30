using System;
using Google.Protobuf;
namespace SuParty.Service.UseGoogleProtobuf
{
    public class UseGoogleProtobuf
    {
        public UseGoogleProtobuf() {

            //// 創建一個 Person 物件
            //var person = new Person
            //{
            //    Name = "John Doe",
            //    Id = 123,
            //    Email = "john.doe@example.com"
            //};

            //// 序列化為二進位資料
            //using (var output = new MemoryStream())
            //{
            //    person.WriteTo(output);
            //    byte[] data = output.ToArray();

            //    // 將二進位資料寫入檔案
            //    string filePath = "person.dat"; // 指定檔案名稱
            //    File.WriteAllBytes(filePath, data);

            //    Console.WriteLine($"Data has been written to {filePath}");
            //}
        }

    }

    internal class Person
    {
        public string Name { get; set; }
        public int Id { get; set; }
        public string Email { get; set; }
    }
}
