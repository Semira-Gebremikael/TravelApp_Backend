using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

using Configuration;
using Models;
using Models.DTO;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.Metrics;
using System.Reflection.Emit;
using DbModel;
using System.Linq;

namespace DbModels
{
    //[Index(nameof(CategoryName) = true)]
    [Table("Attractions")]
    public class csAttractionDbM : csAttraction, ISeed<csAttractionDbM>
    {
        [Key]       // for EFC Code first
        public override Guid AttractionId { get; set; }

        [Required]
        public override string AttractionName { get; set; }

        public override string Description { get; set; }

        #region correcting the 1st migration error

       // public override Guid? AddressId { get ; set; }

        [NotMapped]
        public override IAddress Address { get => AddressDbM; set => new NotFiniteNumberException(); }
 
        [JsonIgnore]
        public virtual csAddressDbM AddressDbM { get; set; } = null;

        [NotMapped]
        public override List<IPerson> Persons { get => PersonDbM?.ToList<IPerson>(); set => new NotFiniteNumberException(); }

        [JsonIgnore]
        public virtual  List<csPersonDbM> PersonDbM { get; set; } = null;


        [JsonIgnore]
        public virtual List<csCommentDbM> CommentDbM { get; set; } = null;


        [NotMapped]
        public override List<IComment> Comments { get => CommentDbM?.ToList<IComment>(); set => new NotImplementedException(); }

        //adding more readability to an enum type in the database

        public virtual string strCategory
        {
            get => Category.ToString();
            set { }
        }

        #endregion


        #region randomly seed this instance
        public override csAttractionDbM Seed(csSeedGenerator sgen)
        {
            base.Seed(sgen);
            return this;
        }
        #endregion

        #region Update from DTO
        public csAttractionDbM UpdateFromDTO(csAttractionCUdto org)
        {
            if (org == null) return null;
            
            Category = org.Category;
            Description = org.Description;
            AttractionName = org.AttractionName;

            //We will set this when DbM model is finished
            //AttractionId = org.AttractionId;

            return this;
        }
        #endregion

        #region constructors
        public csAttractionDbM() { }
        public csAttractionDbM(csAttractionCUdto org)
        {
            AttractionId = Guid.NewGuid();
            UpdateFromDTO(org);
        }
        #endregion
    }
}

