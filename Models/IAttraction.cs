using System;
namespace Models
{
    public enum enCategory { Museum, Natur, Historia, Arkitektur,Kultur };
    public interface IAttraction
    {
        public Guid AttractionId { get; set; }

        public enCategory Category { get; set; }

        public string AttractionName { get; set; }

        public string Description { get; set; }

        public IAddress Address { get; set; }

        public List<IPerson> Persons { get; set; }

        public List<IComment> Comments { get; set; }




    }
}

