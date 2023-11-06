using System;
using System.ComponentModel.Design;
using System.Diagnostics.Metrics;
using System.Reflection.Emit;
using System.Xml.Linq;

namespace Models.DTO
{
    //DTO is a DataTransferObject, can be instanstiated by the controller logic
    //and represents a, fully instatiable, subset of the Database models
    //for a specific purpose.

    //These DTO are simplistic and used to Update and Create objects in the database
    public class csPersonCUdto
    {
        public virtual Guid? PersonId { get; set; }

        public virtual string FirstName { get; set; }
        public virtual string LastName { get; set; }

        public virtual string Email { get; set; }

        public DateTime? Birthday { get; set; } = null;

        public virtual List<Guid> AttractionId { get; set; } = null;

        public virtual List<Guid> CommentId { get; set; } = null;

        public csPersonCUdto() { }
        public csPersonCUdto(IPerson org)
        {
            PersonId = org.PersonId;
            FirstName = org.FirstName;
            LastName = org.LastName;
            Email = org.Email;
            Birthday = org.Birthday;

            AttractionId = org.Attractions?.Select(i => i.AttractionId).ToList();
            CommentId = org.Comments?.Select(i => i.CommentId).ToList();
        }
    }

    public class csAddressCUdto
    {
        public virtual Guid? AddressId { get; set; }

        public virtual string StreetAddress { get; set; }
        public virtual int ZipCode { get; set; }
        public virtual string City { get; set; }
        public virtual string Country { get; set; }

        public virtual List<Guid> AttractionId { get; set; }


        public csAddressCUdto() { }
        public csAddressCUdto(IAddress org)
        {
            AddressId = org.AddressId;
            StreetAddress = org.StreetAddress;
            ZipCode = org.ZipCode;
            City = org.City;
            Country = org.Country;

            AttractionId = org.Attractions?.Select(i => i.AttractionId).ToList();

        }

    }

    public class csAttractionCUdto
    {
        //cannot be nullable as a Pets has to have an owner even when created

        public virtual Guid? AttractionId { get; set; }

        public virtual string AttractionName { get; set; }

        public virtual string Description { get; set; }

        public Guid AddressId { get; }
        public virtual List<Guid> PersonId { get; set; }
        public virtual List<Guid> CommentId { get; set; }


        public virtual enCategory Category { get; set; }


        public csAttractionCUdto() { }
        public csAttractionCUdto(IAttraction org)
        {
            AttractionId = org.AttractionId;
            Category = org.Category;
            AttractionName = org.AttractionName;
            Description = org.Description;



            AddressId = org.Address.AddressId;
            PersonId = org.Persons?.Select(i => i.PersonId).ToList();
            CommentId = org.Comments?.Select(i => i.CommentId).ToList();

        }
    }
    public class csCommentCUdto
    {
        public virtual Guid? CommentId { get; set; }
        public int Rating { get; set; }
        public string CommentText { get; set; }

        public virtual Guid PersonId { get; set; }
        public virtual Guid AttractionId { get; set; }


        public csCommentCUdto() { }
        public csCommentCUdto(IComment org)
        {
            CommentId = org.CommentId;

            CommentText = org.CommentText;
            Rating = org.Rating;

            PersonId = org.Person.PersonId;
            AttractionId = org.Attraction.AttractionId;
        }
    }
}

