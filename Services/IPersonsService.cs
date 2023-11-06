using System;
using Models;
using Models.DTO;

namespace Services
{
    public interface IPersonsService
	{
        //To test the overall layered structure
        public string InstanceHello { get; }

        //For inital test only, so a limited set on non-async methods in this example
        public gstusrInfoAllDto Info { get; }
        public adminInfoDbDto RemoveSeed(loginUserSessionDto usr, bool seeded);
        public adminInfoDbDto Seed(loginUserSessionDto usr, int nrOfItems);

        public List<IPerson> ReadPersons(loginUserSessionDto usr, bool seeded, bool flat, string filter, int pageNumber, int pageSize);


        //Full set of async methods
        public Task<gstusrInfoAllDto> InfoAsync { get; }
        public Task<adminInfoDbDto> SeedAsync(loginUserSessionDto usr, int nrOfItems);
        public Task<adminInfoDbDto> RemoveSeedAsync(loginUserSessionDto usr, bool seeded);

        public Task<List<IPerson>> ReadPersonsAsync(loginUserSessionDto usr, bool seeded, bool flat, string filter, int pageNumber, int pageSize);
        public Task<IPerson> ReadPersonAsync(loginUserSessionDto usr, Guid id, bool flat);
        public Task<IPerson> DeletePersonAsync(loginUserSessionDto usr, Guid id);
        public Task<IPerson> UpdatePersonAsync(loginUserSessionDto usr, csPersonCUdto item);
        public Task<IPerson> CreatePersonAsync(loginUserSessionDto usr, csPersonCUdto item);

        public Task<List<IAddress>> ReadAddressesAsync(loginUserSessionDto usr, bool seeded, bool flat, string filter, int pageNumber, int pageSize);
        public Task<IAddress> ReadAddressAsync(loginUserSessionDto usr, Guid id, bool flat);
        public Task<IAddress> DeleteAddressAsync(loginUserSessionDto usr, Guid id);
        public Task<IAddress> UpdateAddressAsync(loginUserSessionDto usr, csAddressCUdto item);
        public Task<IAddress> CreateAddressAsync(loginUserSessionDto usr, csAddressCUdto item);

        public Task<List<IComment>> ReadCommentsAsync(loginUserSessionDto usr, bool seeded, bool flat, string filter, int pageNumber, int pageSize);
        public Task<IComment> ReadCommentAsync(loginUserSessionDto usr, Guid id, bool flat);
        public Task<IComment> DeleteCommentAsync(loginUserSessionDto usr, Guid id);
        public Task<IComment> UpdateCommentAsync(loginUserSessionDto usr, csCommentCUdto item);
        public Task<IComment> CreateCommentAsync(loginUserSessionDto usr, csCommentCUdto item);

        public Task<List<IAttraction>> ReadAttractionsAsync(loginUserSessionDto usr, bool seeded, bool flat, string filter, int pageNumber, int pageSize);
        public Task<IAttraction> ReadAttractionAsync(loginUserSessionDto usr, Guid id, bool flat);
        public Task<IAttraction> DeleteAttractionAsync(loginUserSessionDto usr, Guid id);
        public Task<IAttraction> UpdateAttractionAsync(loginUserSessionDto usr, csAttractionCUdto item);
        public Task<IAttraction> CreateAttractionAsync(loginUserSessionDto usr, csAttractionCUdto item);
    }
}

