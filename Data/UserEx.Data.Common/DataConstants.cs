namespace UserEx.Data.Common
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class DataConstants
    {
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
            public const int NameMaxLength = 25;
            public const int PhoneNumberMaxLength = 20;
        }
    }
}
