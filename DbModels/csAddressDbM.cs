using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Linq;

using Configuration;
using Models;
using Models.DTO;
using DbModels;

namespace DbModel
{
    [Index(nameof(StreetAddress), nameof(ZipCode), nameof(City), nameof(Country), IsUnique = true)]
    [Table("Addresses")]

    public class csAddressDbM : csAddress, ISeed<csAddressDbM>, IEquatable<csAddressDbM>
    {
        [Key]
        public override Guid AddressId { get; set; }

        [Required]
        public override string StreetAddress { get; set; }

        [Required]
        public override int ZipCode { get; set; }

        [Required]
        public override string City { get; set; }

        [Required]
        public override string Country { get; set; }

        public override bool Seeded { get; set; } = false;

        [JsonIgnore]
        public virtual List<csAttractionDbM> AttractionsDbM { get; set; } = null;

        [NotMapped] //application layer can continue to read a List of Attraction without code change
        public override List<IAttraction> Attractions { get => AttractionsDbM?.ToList<IAttraction>(); set => new NotImplementedException(); }


        #region implementing IEquatable

        public bool Equals(csAddressDbM other) => (other != null) ?((StreetAddress, ZipCode, City, Country) ==
            (other.StreetAddress, other.ZipCode, other.City, other.Country)) :false;

        public override bool Equals(object obj) => Equals(obj as csAddressDbM);
        public override int GetHashCode() => (StreetAddress, ZipCode, City, Country).GetHashCode();

        #endregion

        #region randomly seed this instance
        public override csAddressDbM Seed(csSeedGenerator sgen)
        {
            base.Seed(sgen);
            return this;
        }
        #endregion

        #region Update from DTO
        public csAddressDbM UpdateFromDTO(csAddressCUdto org)
        {
            if (org == null) return null;

            StreetAddress = org.StreetAddress;
            ZipCode = org.ZipCode;
            City = org.City;
            Country = org.Country;

            return this;
        }
        #endregion

        #region constructors
        public csAddressDbM() { }
        public csAddressDbM(csAddressCUdto org)
        {
            AddressId = Guid.NewGuid();
            UpdateFromDTO(org);
        }
        #endregion
    }
}

