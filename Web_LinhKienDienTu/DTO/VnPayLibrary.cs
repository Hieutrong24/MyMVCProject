using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace Web_LinhKienDienTu.Lib
{
    public class VnPayLibrary
    {
        private readonly SortedList<string, string> requestData = new SortedList<string, string>();
        private readonly SortedList<string, string> responseData = new SortedList<string, string>();

        // Thêm dữ liệu request (dùng cho tạo URL)
        public void AddRequestData(string key, string value)
        {
            if (!string.IsNullOrEmpty(value))
                requestData[key] = value;
        }

        // Thêm dữ liệu response (dùng khi nhận dữ liệu phản hồi từ VNPAY)
        public void AddResponseData(string key, string value)
        {
            if (!string.IsNullOrEmpty(value))
                responseData[key] = value;
        }

        // Lấy dữ liệu từ phản hồi
        public string GetResponseData(string key)
        {
            return responseData.ContainsKey(key) ? responseData[key] : null;
        }

        // Tạo URL thanh toán
        public string CreateRequestUrl(string baseUrl, string hashSecret)
        {
            // 1. Sắp xếp key theo thứ tự alphabet
            var sortedData = requestData.OrderBy(x => x.Key)
                                        .Where(x => !string.IsNullOrEmpty(x.Value));

            // 2. Tạo rawData để ký (KHÔNG URL Encode)
            string rawData = string.Join("&", sortedData.Select(x => $"{x.Key}={x.Value}"));

            // 3. Tạo chữ ký HMAC SHA512
            string vnp_SecureHash = HmacSHA512(hashSecret, rawData);

            // 4. Tạo query string (URL Encode từng key và value)
            var query = string.Join("&", sortedData.Select(x =>
                $"{HttpUtility.UrlEncode(x.Key)}={HttpUtility.UrlEncode(x.Value)}"));

            return $"{baseUrl}?{query}&vnp_SecureHash={vnp_SecureHash}";
        }


        // Xác thực chữ ký trả về từ VNPAY
        public bool ValidateSignature(string hashSecret)
        {
            if (!responseData.ContainsKey("vnp_SecureHash")) return false;

            string vnp_SecureHash = responseData["vnp_SecureHash"];
            responseData.Remove("vnp_SecureHash");

            // Cần sắp xếp lại để đảm bảo thứ tự
            var sortedData = responseData.OrderBy(x => x.Key)
                                         .Where(x => !string.IsNullOrEmpty(x.Value));
            string rawData = string.Join("&", sortedData.Select(x => $"{x.Key}={x.Value}"));

            string myChecksum = HmacSHA512(hashSecret, rawData);

            return vnp_SecureHash.Equals(myChecksum, StringComparison.InvariantCultureIgnoreCase);
        }


        // Sinh chuỗi query chuẩn để ký
        private string GetQueryString(IDictionary<string, string> data)
        {
            var sortedData = data.OrderBy(x => x.Key)
                                 .Where(x => !string.IsNullOrEmpty(x.Value) && x.Key != "vnp_SecureHash");

            return string.Join("&", sortedData.Select(kvp => $"{kvp.Key}={kvp.Value}"));
        }



        // Hàm mã hóa HMAC SHA512
        //public string HmacSHA512(string key, string inputData)
        //{
        //    using (var hmac = new HMACSHA512(Encoding.UTF8.GetBytes(key)))
        //    {
        //        byte[] hashValue = hmac.ComputeHash(Encoding.UTF8.GetBytes(inputData));
        //        var hex = BitConverter.ToString(hashValue).Replace("-", "").ToLower();
        //        return hex;
        //    }
        //}

        public static string HmacSHA512(string key, string inputData)
        {
            byte[] keyBytes = Encoding.UTF8.GetBytes(key);
            byte[] inputBytes = Encoding.UTF8.GetBytes(inputData);
            using (var hmac = new HMACSHA512(keyBytes))
            {
                byte[] hashValue = hmac.ComputeHash(inputBytes);
                return BitConverter.ToString(hashValue).Replace("-", "").ToLower();
            }
        }


        public string DebugSignature(string hashSecret)
        {
            if (!responseData.ContainsKey("vnp_SecureHash"))
                return "Không có trường vnp_SecureHash trong dữ liệu phản hồi!";

            string vnp_SecureHash = responseData["vnp_SecureHash"];

            // Sắp xếp lại responseData
            var sortedData = responseData.OrderBy(x => x.Key)
                                         .Where(x => !string.IsNullOrEmpty(x.Value) && x.Key != "vnp_SecureHash");

            string rawData = string.Join("&", sortedData.Select(x => $"{x.Key}={x.Value}"));
            string myChecksum = HmacSHA512(hashSecret, rawData);

            var log = new StringBuilder();
            log.AppendLine("===== DEBUG CHỮ KÝ VNPAY =====");
            log.AppendLine("1. DỮ LIỆU RAW (dùng để hash):");
            log.AppendLine(rawData);
            log.AppendLine();
            log.AppendLine("2. Chữ ký VNPAY gửi về (vnp_SecureHash):");
            log.AppendLine(vnp_SecureHash);
            log.AppendLine();
            log.AppendLine("3. Chữ ký bạn tự tính (HmacSHA512):");
            log.AppendLine(myChecksum);
            log.AppendLine();
            log.AppendLine("4. So sánh: " + (vnp_SecureHash.Equals(myChecksum, StringComparison.InvariantCultureIgnoreCase) ? "✅ TRÙNG KHỚP" : "❌ KHÔNG KHỚP"));

            log.AppendLine("\n5. Tất cả dữ liệu nhận được từ VNPAY:");
            foreach (var item in responseData)
            {
                log.AppendLine($"{item.Key} = {item.Value}");
            }

            log.AppendLine("=================================");

            return log.ToString();
        }


    }
}
