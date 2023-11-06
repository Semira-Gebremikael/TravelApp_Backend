using System;
namespace Models.DTO
{
	public class gstusrInfoDbDto
	{
        public int nrSeededPersons { get; set; } = 0;
        public int nrUnseededPersons { get; set; } = 0;
        public int nrAttractionWithAddress { get; set; } = 0;

        public int nrSeededAddresses { get; set; } = 0;
        public int nrUnseededAddresses { get; set; } = 0;

        public int nrSeededAttractions { get; set; } = 0;
        public int nrUnseededAttractions { get; set; } = 0;

        public int nrSeededComments { get; set; } = 0;
        public int nrUnseededComments { get; set; } = 0;
    }

    public class gstusrInfoPersonsDto
    {
        public string Country { get; set; } = null;
        public string City { get; set; } = null;
        public int NrPersons { get; set; } = 0;
    }

    public class gstusrInfoAttractionsDto
    {
        public string Country { get; set; } = null;
        public string City { get; set; } = null;
        public int NrAttractions { get; set; } = 0;
    }

    public class gstusrInfoCommentsDto
    {
        public string AuthorCommentText { get; set; } = null;
        public int NrComments { get; set; } = 0;
    }

    public class gstusrInfoAllDto
    {
        public List<vw_attractionsWithNullComments> _attractionsWithNullComments { get; set; }
        public gstusrInfoDbDto Db { get; set; } = null;
        public List<gstusrInfoPersonsDto> Persons { get; set; } = null;
        public List<gstusrInfoAttractionsDto> Attractions { get; set; } = null;
        public List<gstusrInfoCommentsDto> Comments { get; set; } = null;
    }

    public class vw_attractionsWithNullComments
    {
        public Guid AttractionId { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string AttractionName { get; set; }
        public string StreetAddress { get; set; }
        public string Description { get; set; }
        public string CommentText { get; set; }

    }
}

