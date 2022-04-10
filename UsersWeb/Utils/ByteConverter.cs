namespace UsersWeb.Utils
{
    public static class ByteConverter
    {
        public static byte[] GetHexBytes(string hexedString)
        {
            var bytes = new byte[hexedString.Length / 2];
            for (var i = 0; i < bytes.Length; i++)
            {
                var strPos = i * 2;
                var chars = hexedString.Substring(strPos, 2);
                bytes[i] = Convert.ToByte(chars, 16);
            }
            return bytes;
        }
        
        public static string GetHexString(byte[] bytes)
        {
            return BitConverter.ToString(bytes).Replace("-", "").ToUpper();
        }
    }
}
