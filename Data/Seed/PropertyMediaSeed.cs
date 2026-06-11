using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MARN_API.Models;

namespace MARN_API.Data.Seed
{
    public class PropertyMediaSeed : IEntityTypeConfiguration<PropertyMedia>
    {
        public void Configure(EntityTypeBuilder<PropertyMedia> builder)
        {
            builder.HasData(
                // Core properties (1001-1004, 1100)
                new PropertyMedia { Id = 3001, PropertyId = 1001, Path = "/images/properties/property1001-main.jpg", IsPrimary = true },
                new PropertyMedia { Id = 3002, PropertyId = 1001, Path = "/images/properties/property1001-sec.jpg", IsPrimary = false },
                new PropertyMedia { Id = 3003, PropertyId = 1002, Path = "/images/properties/property1002-main.jpg", IsPrimary = true },
                new PropertyMedia { Id = 3004, PropertyId = 1002, Path = "/images/properties/property1002-sec.jpg", IsPrimary = false },
                new PropertyMedia { Id = 3005, PropertyId = 1003, Path = "/images/properties/property1003-main.jpg", IsPrimary = true },
                new PropertyMedia { Id = 3006, PropertyId = 1003, Path = "/images/properties/property1003-sec.jpg", IsPrimary = false },
                new PropertyMedia { Id = 3007, PropertyId = 1004, Path = "/images/properties/property1004-main.jpg", IsPrimary = true },
                new PropertyMedia { Id = 3008, PropertyId = 1004, Path = "/images/properties/property1004-sec.jpg", IsPrimary = false },
                new PropertyMedia { Id = 3009, PropertyId = 1100, Path = "/images/properties/property1100-main.jpg", IsPrimary = true },
                new PropertyMedia { Id = 3010, PropertyId = 1100, Path = "/images/properties/property1100-sec.jpg", IsPrimary = false },

                // CSV properties (2001-2020)
                new PropertyMedia { Id = 3011, PropertyId = 2001, Path = "/images/properties/property2001-main.jpg", IsPrimary = true },
                new PropertyMedia { Id = 3012, PropertyId = 2001, Path = "/images/properties/property2001-sec.jpg", IsPrimary = false },
                new PropertyMedia { Id = 3013, PropertyId = 2002, Path = "/images/properties/property2002-main.jpg", IsPrimary = true },
                new PropertyMedia { Id = 3014, PropertyId = 2002, Path = "/images/properties/property2002-sec.jpg", IsPrimary = false },
                new PropertyMedia { Id = 3015, PropertyId = 2003, Path = "/images/properties/property2003-main.jpg", IsPrimary = true },
                new PropertyMedia { Id = 3016, PropertyId = 2003, Path = "/images/properties/property2003-sec.jpg", IsPrimary = false },
                new PropertyMedia { Id = 3017, PropertyId = 2004, Path = "/images/properties/property2004-main.jpg", IsPrimary = true },
                new PropertyMedia { Id = 3018, PropertyId = 2004, Path = "/images/properties/property2004-sec.jpg", IsPrimary = false },
                new PropertyMedia { Id = 3019, PropertyId = 2005, Path = "/images/properties/property2005-main.jpg", IsPrimary = true },
                new PropertyMedia { Id = 3020, PropertyId = 2005, Path = "/images/properties/property2005-sec.jpg", IsPrimary = false },
                new PropertyMedia { Id = 3021, PropertyId = 2006, Path = "/images/properties/property2006-main.jpg", IsPrimary = true },
                new PropertyMedia { Id = 3022, PropertyId = 2006, Path = "/images/properties/property2006-sec.jpg", IsPrimary = false },
                new PropertyMedia { Id = 3023, PropertyId = 2007, Path = "/images/properties/property2007-main.jpg", IsPrimary = true },
                new PropertyMedia { Id = 3024, PropertyId = 2007, Path = "/images/properties/property2007-sec.jpg", IsPrimary = false },
                new PropertyMedia { Id = 3025, PropertyId = 2008, Path = "/images/properties/property2008-main.jpg", IsPrimary = true },
                new PropertyMedia { Id = 3026, PropertyId = 2008, Path = "/images/properties/property2008-sec.jpg", IsPrimary = false },
                new PropertyMedia { Id = 3027, PropertyId = 2009, Path = "/images/properties/property2009-main.jpg", IsPrimary = true },
                new PropertyMedia { Id = 3028, PropertyId = 2009, Path = "/images/properties/property2009-sec.jpg", IsPrimary = false },
                new PropertyMedia { Id = 3029, PropertyId = 2010, Path = "/images/properties/property2010-main.jpg", IsPrimary = true },
                new PropertyMedia { Id = 3030, PropertyId = 2010, Path = "/images/properties/property2010-sec.jpg", IsPrimary = false },
                new PropertyMedia { Id = 3031, PropertyId = 2011, Path = "/images/properties/property2011-main.jpg", IsPrimary = true },
                new PropertyMedia { Id = 3032, PropertyId = 2011, Path = "/images/properties/property2011-sec.jpg", IsPrimary = false },
                new PropertyMedia { Id = 3033, PropertyId = 2012, Path = "/images/properties/property2012-main.jpg", IsPrimary = true },
                new PropertyMedia { Id = 3034, PropertyId = 2012, Path = "/images/properties/property2012-sec.jpg", IsPrimary = false },
                new PropertyMedia { Id = 3035, PropertyId = 2013, Path = "/images/properties/property2013-main.jpg", IsPrimary = true },
                new PropertyMedia { Id = 3036, PropertyId = 2013, Path = "/images/properties/property2013-sec.jpg", IsPrimary = false },
                new PropertyMedia { Id = 3037, PropertyId = 2014, Path = "/images/properties/property2014-main.jpg", IsPrimary = true },
                new PropertyMedia { Id = 3038, PropertyId = 2014, Path = "/images/properties/property2014-sec.jpg", IsPrimary = false },
                new PropertyMedia { Id = 3039, PropertyId = 2015, Path = "/images/properties/property2015-main.jpg", IsPrimary = true },
                new PropertyMedia { Id = 3040, PropertyId = 2015, Path = "/images/properties/property2015-sec.jpg", IsPrimary = false },
                new PropertyMedia { Id = 3041, PropertyId = 2016, Path = "/images/properties/property2016-main.jpg", IsPrimary = true },
                new PropertyMedia { Id = 3042, PropertyId = 2016, Path = "/images/properties/property2016-sec.jpg", IsPrimary = false },
                new PropertyMedia { Id = 3043, PropertyId = 2017, Path = "/images/properties/property2017-main.jpg", IsPrimary = true },
                new PropertyMedia { Id = 3044, PropertyId = 2017, Path = "/images/properties/property2017-sec.jpg", IsPrimary = false },
                new PropertyMedia { Id = 3045, PropertyId = 2018, Path = "/images/properties/property2018-main.jpg", IsPrimary = true },
                new PropertyMedia { Id = 3046, PropertyId = 2018, Path = "/images/properties/property2018-sec.jpg", IsPrimary = false },
                new PropertyMedia { Id = 3047, PropertyId = 2019, Path = "/images/properties/property2019-main.jpg", IsPrimary = true },
                new PropertyMedia { Id = 3048, PropertyId = 2019, Path = "/images/properties/property2019-sec.jpg", IsPrimary = false },
                new PropertyMedia { Id = 3049, PropertyId = 2020, Path = "/images/properties/property2020-main.jpg", IsPrimary = true },
                new PropertyMedia { Id = 3050, PropertyId = 2020, Path = "/images/properties/property2020-sec.jpg", IsPrimary = false },

                // Admin Dashboard Scenario Properties (1201-1205)
                new PropertyMedia { Id = 3051, PropertyId = 1201, Path = "/images/properties/property1201-main.jpg", IsPrimary = true },
                new PropertyMedia { Id = 3052, PropertyId = 1201, Path = "/images/properties/property1201-sec.jpg", IsPrimary = false },
                new PropertyMedia { Id = 3053, PropertyId = 1202, Path = "/images/properties/property1202-main.jpg", IsPrimary = true },
                new PropertyMedia { Id = 3054, PropertyId = 1202, Path = "/images/properties/property1202-sec.jpg", IsPrimary = false },
                new PropertyMedia { Id = 3055, PropertyId = 1203, Path = "/images/properties/property1203-main.jpg", IsPrimary = true },
                new PropertyMedia { Id = 3056, PropertyId = 1203, Path = "/images/properties/property1203-sec.jpg", IsPrimary = false },
                new PropertyMedia { Id = 3057, PropertyId = 1204, Path = "/images/properties/property1204-main.jpg", IsPrimary = true },
                new PropertyMedia { Id = 3058, PropertyId = 1204, Path = "/images/properties/property1204-sec.jpg", IsPrimary = false },
                new PropertyMedia { Id = 3059, PropertyId = 1205, Path = "/images/properties/property1205-main.jpg", IsPrimary = true },
                new PropertyMedia { Id = 3060, PropertyId = 1205, Path = "/images/properties/property1205-sec.jpg", IsPrimary = false }
            );
        }
    }
}
