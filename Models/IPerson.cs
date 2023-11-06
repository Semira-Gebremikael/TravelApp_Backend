using System;
using System.Reflection.Emit;

namespace Models
{

    public interface IPerson
    {
        public Guid PersonId { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string Email { get; set; }
        public DateTime? Birthday { get; set; }

        public List<IComment> Comments { get; set; }
        public List<IAttraction> Attractions { get; set; }

    }


}


