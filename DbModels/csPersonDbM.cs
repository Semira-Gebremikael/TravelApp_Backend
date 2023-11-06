using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;
using System.Linq;

using Configuration;
using Models;
using Models.DTO;
using DbModel;

//DbModels namespace is the layer which contains all the C# models of
//the database tables Select queries as well as results from a call to a View,
//Stored procedure, or Function.

//C# classes corresponds to table structure (no suffix) or
//specific search results (DTO suffix)
namespace DbModels;

[Index(nameof(FirstName), nameof(LastName))]
[Index(nameof(LastName), nameof(FirstName))]
[Table("Persons")]
public class csPersonDbM : csPerson, ISeed<csPersonDbM>
{
    static public new string Hello { get; } = $"Hello from namespace {nameof(DbModels)}, class {nameof(csPersonDbM)}";

    [Key]       // for EFC Code first
    public override Guid PersonId { get; set; }

    [Required]
    public override string FirstName { get; set; }

    [JsonIgnore]
    public virtual List<csCommentDbM> CommentsDbM { get; set; }

    [NotMapped]
    public override List<IComment> Comments { get => CommentsDbM?.ToList<IComment>(); set => new NotImplementedException(); }


    [JsonIgnore]
    public virtual List<csAttractionDbM> AttractionsDbM { get; set; } = null;


    [NotMapped]
    public override List<IAttraction> Attractions { get => AttractionsDbM?.ToList<IAttraction>(); set => new NotImplementedException(); }


    #region randomly seed this instance
    public override csPersonDbM Seed(csSeedGenerator sgen)
    {
        base.Seed(sgen);
        return this;
    }
    #endregion

    #region Update from DTO
    public csPersonDbM UpdateFromDTO(csPersonCUdto org)
    {
        FirstName = org.FirstName;
        LastName = org.LastName;
        Birthday = org.Birthday;
        Email = org.Email;


        return this;
    }
    #endregion

    #region constructors
    public csPersonDbM() { }
    public csPersonDbM(csPersonCUdto org)
    {
        PersonId = Guid.NewGuid();
        UpdateFromDTO(org);
    }
    #endregion

}

