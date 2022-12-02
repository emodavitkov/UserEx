namespace UserEx.Data.Common
{
    public class DataConstants
    {
        public class User
        {
            public const int FullNameMinLength = 5;
            public const int FullNameMaxLength = 40;
            public const int PasswordMinLength = 6;
            public const int PasswordMaxLength = 100;
        }

        public class Number
        {
            // public const int MinLength = 5;
            // public const int MaxLength = 15;
            public const int DescriptionMinLength = 2;
        }

        public class Provider
        {
            public const int NameMaxLength = 25;
        }

        public class Partner
        {
            public const int OfficeNameMinLength = 2;
            public const int OfficeNameMaxLength = 25;
            public const int PhoneNumberMinLength = 6;
            public const int PhoneNumberMaxLength = 20;
        }

        public class Rate
        {
            public const int DialCodeNumberMinLength = 1;
            public const int DialCodeNumberMaxLength = 25;
            public const int DestinationNameMinLength = 2;
            public const int DestinationNameMaxLength = 30;
        }

        public class Records
        {
            public const int OfficeNameMinLength = 2;
            public const int OfficeNameMaxLength = 25;
            public const int DialCodeNumberMinLength = 1;
            public const int DialCodeNumberMaxLength = 25;
            public const int PhoneNumberMinLength = 6;
            public const int PhoneNumberMaxLength = 20;
        }
    }
}
