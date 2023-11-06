using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Reflection.Emit;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;


using Models;
using Models.DTO;

namespace DbModels
{
    [Table("Comments")]
    public class csCommentDbM : csComment, ISeed<csCommentDbM>, IEquatable<csCommentDbM>
    {
        [Key]       // for EFC Code first
        public override Guid CommentId { get; set; }

        public override string CommentText { get; set; }

        public override int Rating { get; set; }

        #region correcting the 1st migration error
        //a comment can be related to 0 or many friends

        [JsonIgnore]
        public virtual csPersonDbM PersonDbM { get; set; } = null;

        [NotMapped] //application layer can continue to read a List of Pets without code change
        public override IPerson Person { get => PersonDbM; set => new NotImplementedException(); }


        [JsonIgnore]
        public virtual csAttractionDbM AttractionDbM { get; set; } = null;

        [NotMapped]
        public override IAttraction Attraction { get => AttractionDbM; set => new NotImplementedException(); }


        #endregion


        #region constructors
        public csCommentDbM() : base() { }
        public csCommentDbM(GoodComment goodComment) : base(goodComment) { }
        public csCommentDbM(csCommentCUdto org)
        {
            CommentId = Guid.NewGuid();
            UpdateFromDTO(org);
        }
        #endregion

        #region implementing IEquatable

        public bool Equals(csCommentDbM other) => (other != null) ? ((CommentText, Rating) ==
            (other.CommentText, other.Rating)) : false;

        public override bool Equals(object obj) => Equals(obj as csCommentDbM);
        public override int GetHashCode() => (CommentText, Rating).GetHashCode();

        #endregion

        #region randomly seed this instance
        public override csCommentDbM Seed(csSeedGenerator sgen)
        {
            base.Seed(sgen);
            return this;
        }
        #endregion

        #region Update from DTO
        public csComment UpdateFromDTO(csCommentCUdto org)
        {
            if (org == null) return null;

            CommentText = org.CommentText;
            Rating = org.Rating;

            return this;
        }
        #endregion
    }
}

