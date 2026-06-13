using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MARN_API.Models;

namespace MARN_API.Data.Seed
{
    public class PropertyRuleSeed : IEntityTypeConfiguration<PropertyRule>
    {
        public void Configure(EntityTypeBuilder<PropertyRule> builder)
        {
            builder.HasData(
                new PropertyRule { Id = 1, PropertyId = 1001, Rule = "No Smoking inside the apartment" },
                new PropertyRule { Id = 2, PropertyId = 1001, Rule = "No parties or loud music after 11 PM" },
                
                new PropertyRule { Id = 3, PropertyId = 1002, Rule = "Pets are not allowed" },
                
                new PropertyRule { Id = 4, PropertyId = 1003, Rule = "Single occupancy only" },
                
                new PropertyRule { Id = 5, PropertyId = 1004, Rule = "Respect the neighbors" },
                new PropertyRule { Id = 6, PropertyId = 1004, Rule = "Smoking allowed only in the balcony" },

                // Property 1100
                new PropertyRule { Id = 7, PropertyId = 1100, Rule = "Keep shared spaces clean" },
                new PropertyRule { Id = 8, PropertyId = 1100, Rule = "No guests overnight without roommate approval" },

                // Property 2001
                new PropertyRule { Id = 9, PropertyId = 2001, Rule = "Annual maintenance fees are split" },
                new PropertyRule { Id = 10, PropertyId = 2001, Rule = "Quiet hours after 10 PM" },

                // Property 2002
                new PropertyRule { Id = 11, PropertyId = 2002, Rule = "Families only" },
                new PropertyRule { Id = 12, PropertyId = 2002, Rule = "Small pets allowed with prior consent" },

                // Property 2003
                new PropertyRule { Id = 13, PropertyId = 2003, Rule = "Turn off AC when leaving the studio" },
                new PropertyRule { Id = 14, PropertyId = 2003, Rule = "Checkout is at 12 PM" },

                // Property 2004
                new PropertyRule { Id = 15, PropertyId = 2004, Rule = "Share chores weekly" },
                new PropertyRule { Id = 16, PropertyId = 2004, Rule = "No smoking indoors" },

                // Property 2005
                new PropertyRule { Id = 17, PropertyId = 2005, Rule = "Maintain the garden weekly" },
                new PropertyRule { Id = 18, PropertyId = 2005, Rule = "No events or commercial filming" },

                // Property 2006
                new PropertyRule { Id = 19, PropertyId = 2006, Rule = "Clean feet from sand before entering" },
                new PropertyRule { Id = 20, PropertyId = 2006, Rule = "No loud music on balcony" },

                // Property 2007
                new PropertyRule { Id = 21, PropertyId = 2007, Rule = "Maximum of 4 overnight occupants" },
                new PropertyRule { Id = 22, PropertyId = 2007, Rule = "Inform owner before having visitors" },

                // Property 2008
                new PropertyRule { Id = 23, PropertyId = 2008, Rule = "Respect neighbors' parking spaces" },
                new PropertyRule { Id = 24, PropertyId = 2008, Rule = "No sub-leasing allowed" },

                // Property 2009
                new PropertyRule { Id = 25, PropertyId = 2009, Rule = "No smoking in the studio" },
                new PropertyRule { Id = 26, PropertyId = 2009, Rule = "Check-out by 11 AM" },

                // Property 2010
                new PropertyRule { Id = 27, PropertyId = 2010, Rule = "Pool usage only until 8 PM" },
                new PropertyRule { Id = 28, PropertyId = 2010, Rule = "No loud outdoor activities late at night" },

                // Property 2011
                new PropertyRule { Id = 29, PropertyId = 2011, Rule = "Quiet hours after 10 PM" },

                // Property 2012
                new PropertyRule { Id = 30, PropertyId = 2012, Rule = "Commercial use of the loft is prohibited" },
                new PropertyRule { Id = 31, PropertyId = 2012, Rule = "Ideal for students or professionals" },

                // Property 2013
                new PropertyRule { Id = 32, PropertyId = 2013, Rule = "No modification to the courtyard structure" },
                new PropertyRule { Id = 33, PropertyId = 2013, Rule = "Pets allowed in the courtyard only" },

                // Property 2014
                new PropertyRule { Id = 34, PropertyId = 2014, Rule = "Shared kitchen duties should be respected" },
                new PropertyRule { Id = 35, PropertyId = 2014, Rule = "No loud gatherings" },

                // Property 2015
                new PropertyRule { Id = 36, PropertyId = 2015, Rule = "Do not leave water taps running" },

                // Property 2016
                new PropertyRule { Id = 37, PropertyId = 2016, Rule = "Daily trash disposal is required" },
                new PropertyRule { Id = 38, PropertyId = 2016, Rule = "Turn off air conditioning when out" },

                // Property 2017
                new PropertyRule { Id = 39, PropertyId = 2017, Rule = "Beach wear not allowed inside the living room" },
                new PropertyRule { Id = 40, PropertyId = 2017, Rule = "Maximum 2 occupants" },

                // Property 2018
                new PropertyRule { Id = 41, PropertyId = 2018, Rule = "Key return to the doorman on check-out" },
                new PropertyRule { Id = 42, PropertyId = 2018, Rule = "No loud parties" },

                // Property 2019
                new PropertyRule { Id = 43, PropertyId = 2019, Rule = "Respect local residential rules" },

                // Property 2020
                new PropertyRule { Id = 44, PropertyId = 2020, Rule = "No smoking inside the house" },
                new PropertyRule { Id = 45, PropertyId = 2020, Rule = "No pets allowed" },

                // Property 1201
                new PropertyRule { Id = 46, PropertyId = 1201, Rule = "No smoking inside" },
                new PropertyRule { Id = 47, PropertyId = 1201, Rule = "Respect the historic building rules" },

                // Property 1202
                new PropertyRule { Id = 48, PropertyId = 1202, Rule = "Keep the garden area tidy" },
                new PropertyRule { Id = 49, PropertyId = 1202, Rule = "No noisy gatherings after midnight" },

                // Property 1203
                new PropertyRule { Id = 50, PropertyId = 1203, Rule = "For single occupants only" },

                // Property 1204
                new PropertyRule { Id = 51, PropertyId = 1204, Rule = "Beach access cards must not be shared" },
                new PropertyRule { Id = 52, PropertyId = 1204, Rule = "No pets" },

                // Property 1205
                new PropertyRule { Id = 53, PropertyId = 1205, Rule = "Pool rules must be strictly followed" },
                new PropertyRule { Id = 54, PropertyId = 1205, Rule = "Respect neighbors' privacy" }
            );
        }
    }
}
