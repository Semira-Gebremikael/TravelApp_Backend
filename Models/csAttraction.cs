
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.Design;
using System.Diagnostics.Metrics;
using System.Reflection.Emit;
using System.Reflection.Metadata;
using Microsoft.VisualBasic;

namespace Models
{
    public class csAttraction : IAttraction, ISeed<csAttraction>
    {
        public virtual Guid AttractionId { get; set; }
        public virtual enCategory Category { get; set; }

        public virtual string AttractionName { get; set; }

        public virtual string Description { get; set; }

        public virtual IAddress Address { get; set; }

        public virtual List<IPerson> Persons { get; set; }


        public virtual List<IComment> Comments { get; set; }


        public override string ToString() => $" {AttractionName} is {Category} ";

        public string ToString( string categoryName)
        {
            var sRet = $"Attraction [{AttractionId}]";

            if (Address != null)
                sRet += $"\n  - Lives at {Address}";
            else
                sRet += $"\n  - Has no address";


            //if (Attractions != null && Attractions.Count > 0)
            //{
            //    sRet += $"\n  - Has Attractions";
            //    foreach (var attraction in Attractions)
            //    {
            //        sRet += $"\n {attraction}";
            //    }
            //}
            //else


                sRet += $"\n  - Has no Attractions found ";

            if (Comments != null && Comments.Count > 0)
            {
                sRet += $"\n  - Has Comments";
                foreach (var comment in Comments)
                {
                    sRet += $"\n     {comment}";
                }
            }
            else
                sRet += $"\n  - Has no Comments found ";

           
            sRet += $"\n  - CategoryName is {categoryName}";

            return sRet;
        }



        #region constructors
        public csAttraction() { }
        public csAttraction(csAttraction org)
        {
            this.Seeded = org.Seeded;

            this.AttractionId = org.AttractionId;
            this.Category = org.Category;
            this.AttractionName = org.AttractionName;
            this.Description = org.Description;
        }
        #endregion


        #region randomly seed this instance
        public bool Seeded { get; set; } = false;


        public virtual csAttraction Seed(csSeedGenerator sgen)
        {
            {
                Seeded = true;

                AttractionId = Guid.NewGuid();
                AttractionName = sgen.AttractionName;
                Description = sgen.description.Description;
                Category = sgen.FromEnum<enCategory>();

                return this;
            }
            #endregion
        }
    }
}




