using System;
using System.Xml.Linq;
using Models;

namespace Models
{
    public class csComment : IComment, ISeed<csComment>, IEquatable<csComment>
    {
        public virtual Guid CommentId { get; set; }

        public virtual string CommentText { get; set; }

        public virtual int Rating { get; set; }

         public virtual IPerson Person { get; set; } = null;

        public virtual IAttraction Attraction { get; set; } = null;



        #region constructors
        public csComment() { }

        public csComment(GoodComment goodComment)
        {
            CommentId = Guid.NewGuid();
            CommentText = goodComment.CommentText;
            Rating = goodComment.Rating;
            Seeded = true;
        }

        #endregion

        #region implementing IEquatable

        public bool Equals(csComment other) => (other != null) ? ((CommentText, Rating) ==
            (other.CommentText, other.Rating)) : false;

        public override bool Equals(object obj) => Equals(obj as csComment);
        public override int GetHashCode() => (CommentText, Rating).GetHashCode();

        #endregion

        #region randomly seed this instance
        public bool Seeded { get; set; } = false;

        public virtual csComment Seed(csSeedGenerator sgen)
        {
            Seeded = true;
            CommentId = Guid.NewGuid();

            var _comment = sgen.CommentText;
            CommentText = _comment.CommentText;
            Rating = _comment.Rating;

            return this;
        }
        #endregion
    }
}

