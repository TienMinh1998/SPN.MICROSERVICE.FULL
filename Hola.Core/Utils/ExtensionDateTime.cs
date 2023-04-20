using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hola.Core.Utils
{
    public static class ExtensionDateTime
    {
        public static DateTime ConvertStringToDateTime(this string strDateTime)
        {
            DateTime dateTime;
            DateTime.TryParseExact(strDateTime, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out dateTime);
            return dateTime;
        }

        public static string ProcessString(this string input)
        {
            // Xóa khoảng trắng thừa
            input = input.Trim();

            // Chuyển đổi chữ cái đầu tiên của từ thành chữ viết hoa và các chữ cái khác thành chữ viết thường
            TextInfo ti = CultureInfo.CurrentCulture.TextInfo;
            input = ti.ToTitleCase(input.ToLower());

            // Thêm dấu chấm câu vào cuối chuỗi nếu cần thiết
            if (input.Length > 0 && !char.IsPunctuation(input[input.Length - 1]))
            {
                input += ".";
            }

            // Loại bỏ khoảng trắng thừa giữa các từ
            input = string.Join(" ", input.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries));

            return input;
        }
    }
}
