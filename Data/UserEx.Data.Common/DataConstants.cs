﻿namespace UserEx.Data.Common
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

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
            public const int MinLength = 5;
            public const int MaxLength = 15;
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
    }
}
