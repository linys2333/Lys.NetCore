namespace LysCore.Common
{
    public static class LysConstants
    {
        public static class Validations
        {
            public const int GuidStringLength = 36;
            public const int MoblieStringLength = 11;
            public const int PasswordHashStringLength = 128;
        }

        public static class Errors
        {
            public static readonly string EntityNotFound = "找不到指定的数据";
        }
    }
}