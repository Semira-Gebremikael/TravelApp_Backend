using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Configuration;

//DbModels namespace is the layer which contains all the C# models of
//the database tables Select queries as well as results from a call to a View,
//Stored procedure, or Function.

//C# classes corresponds to table structure (no suffix) or
//specific search results (DTO suffix)
namespace Models;

public class csPerson : IPerson, ISeed<csPerson>
{
    static public string Hello { get; } = $"Hello from namespace {nameof(Models)}, class {nameof(csPerson)}";
    public virtual Guid PersonId { get; set; }

    public virtual string FirstName { get; set; }
    public virtual string LastName { get; set; }

    public virtual string Email { get; set; }
    public virtual DateTime? Birthday { get; set; } = null;

  
     //One Persons can have many  Comment
    public virtual List<IComment> Comments { get; set; } = null;

    //One Persons can have been on many  Attractions
    public virtual List<IAttraction> Attractions { get; set; } = null;


    public string FullName => $"{FirstName} {LastName}";

    public override string ToString()
    {
        var sRet = $"{FullName} [{PersonId}]";

        if (Attractions != null && Attractions.Count > 0)
        {
            sRet += $"\n  - Has Attractions";
            foreach (var Attraction in Attractions)
            {
                sRet += $"\n {Attraction}";
            }
        }
        else
            sRet += $"\n  - Has  no Attractions found ";

        if (Comments != null && Comments.Count > 0)
        {
            sRet += $"\n  - Has Comment";
            foreach (var comment in Comments)
            {
                sRet += $"\n {comment}";
            }
        }
        else
            sRet += $"\n  - Has  no comment found ";


        if (Birthday != null)
        {
            sRet += $"\n  - Has birthday on {Birthday:D}";
        }

        return sRet;
    }

    #region contructors
    public csPerson() { }

    public csPerson(csPerson org)
    {
        this.Seeded = org.Seeded;

        this.PersonId = org.PersonId;
        this.FirstName = org.FirstName;
        this.LastName = org.LastName;
        this.Email = org.Email;

        //using Linq Select and copy contructor to create a list copy
        //this.Attractions  = (org.Attractions != null) ? org.Attractions.Select(p => new csAttraction((csAttraction)p)).ToList<IAttraction>() : null;

        //use the ternary operator to create only if the orginal is not null
        //this.Address = (org.Address != null) ? new csAddress((csAddress)org.Address) : null;
    }
    #endregion

    #region randomly seed this instance
    public bool Seeded { get; set; } = false;

    public virtual csPerson Seed(csSeedGenerator sgen)
    {
        Seeded = true;
        PersonId = Guid.NewGuid();

        FirstName = sgen.FirstName;
        LastName = sgen.LastName;
        Email = sgen.Email(FirstName, LastName).ToString();
        Birthday = (sgen.Bool) ? sgen.getDateTime(1970, 2005) : null;
        return this;

    }
#endregion
}

