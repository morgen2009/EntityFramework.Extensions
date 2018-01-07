namespace EntityFramework.Extensions.Commons
{
    using System;
    using System.Text;

    public static class EncodingUtils
    {
        public static string ToBase64(this string script)
        {
            return script == null ? null : Convert.ToBase64String(Encoding.UTF8.GetBytes(script));
        }

        public static string FromBase64(this string script)
        {
            return Encoding.UTF8.GetString(Convert.FromBase64String(script));
        }
    }
}