using System;
using DbContext;
using DbRepos;
using Microsoft.Extensions.Logging;
using Models;
using Models.DTO;

namespace Services
{
    public class csPersonsServiceDb : IPersonsService
    {
        private csPersonsDbRepos _repo = null;
        private ILogger<csPersonsServiceDb> _logger = null;

        #region only for layer verification
        private Guid _guid = Guid.NewGuid();
        private string _instanceHello;

        public string InstanceHello => _instanceHello;
 
        static public string Hello { get; } = $"Hello from namespace {nameof(Services)}, class {nameof(csPersonsServiceDb)}" +

            // added after project references is setup
            $"\n   - {csPersonsDbRepos.Hello}" +
            $"\n   - {csMainDbContext.Hello}";
        #endregion

        #region constructors
        public csPersonsServiceDb(ILogger<csPersonsServiceDb> logger)
        {
            //only for layer verification
            _instanceHello = $"Hello from class {this.GetType()} with instance Guid {_guid}. ";

            _logger = logger;
            _logger.LogInformation(_instanceHello);
        }

        public csPersonsServiceDb(csPersonsDbRepos repo, ILogger<csPersonsServiceDb> logger)
        {
            //only for layer verification
            _instanceHello = $"Hello from class {this.GetType()} with instance Guid {_guid}. " +
                $"Will use repo {repo.GetType()}.";

            _logger = logger;
            _logger.LogInformation(_instanceHello);

            _repo = repo;
            
        }
        #endregion

        #region Simple 1:1 calls in this case, but as Services expands, this will no longer be the case
        public Task<gstusrInfoAllDto> InfoAsync => _repo.InfoAsync();

        public Task<adminInfoDbDto> SeedAsync(loginUserSessionDto usr, int nrOfItems) => _repo.SeedAsync(usr, nrOfItems);
        public Task<adminInfoDbDto> RemoveSeedAsync(loginUserSessionDto usr, bool seeded) => _repo.RemoveSeed(usr, seeded);

        public Task<List<IPerson>> ReadPersonsAsync(loginUserSessionDto usr, bool seeded, bool flat, string filter, int pageNumber, int pageSize) => _repo.ReadPersonsAsync(usr, seeded, flat, filter, pageNumber, pageSize);
        public Task<IPerson> ReadPersonAsync(loginUserSessionDto usr, Guid id, bool flat) => _repo.ReadPersonAsync(usr, id, flat);
        public Task<IPerson> DeletePersonAsync(loginUserSessionDto usr, Guid id) => _repo.DeletePersonAsync(usr, id);
        public Task<IPerson> UpdatePersonAsync(loginUserSessionDto usr, csPersonCUdto item) => _repo.UpdatePersonAsync(usr, item);
        public Task<IPerson> CreatePersonAsync(loginUserSessionDto usr, csPersonCUdto item) => _repo.CreatePersonAsync(usr, item);

        public Task<List<IAddress>> ReadAddressesAsync(loginUserSessionDto usr, bool seeded, bool flat, string filter, int pageNumber, int pageSize) => _repo.ReadAddressesAsync(usr, seeded, flat, filter, pageNumber, pageSize);
        public Task<IAddress> ReadAddressAsync(loginUserSessionDto usr, Guid id, bool flat) => _repo.ReadAddressAsync(usr, id, flat);
        public Task<IAddress> DeleteAddressAsync(loginUserSessionDto usr, Guid id) => _repo.DeleteAddressAsync(usr, id);
        public Task<IAddress> UpdateAddressAsync(loginUserSessionDto usr, csAddressCUdto item) => _repo.UpdateAddressAsync(usr, item);
        public Task<IAddress> CreateAddressAsync(loginUserSessionDto usr, csAddressCUdto item) => _repo.CreateAddressAsync(usr, item);

        public Task<List<IComment>> ReadCommentsAsync(loginUserSessionDto usr, bool seeded, bool flat, string filter, int pageNumber, int pageSize) => _repo.ReadCommentsAsync(usr, seeded, flat, filter, pageNumber, pageSize);
        public Task<IComment> ReadCommentAsync(loginUserSessionDto usr, Guid id, bool flat) => _repo.ReadCommentAsync(usr, id, flat);
        public Task<IComment> DeleteCommentAsync(loginUserSessionDto usr, Guid id) => _repo.DeleteCommentAsync(usr, id);
        public Task<IComment> UpdateCommentAsync(loginUserSessionDto usr, csCommentCUdto item) => _repo.UpdateCommentAsync(usr, item);
        public Task<IComment> CreateCommentAsync(loginUserSessionDto usr, csCommentCUdto item) => _repo.CreateCommentAsync(usr, item);

        public Task<List<IAttraction>> ReadAttractionsAsync(loginUserSessionDto usr, bool seeded, bool flat, string filter, int pageNumber, int pageSize) => _repo.ReadAttractionsAsync(usr, seeded, flat, filter, pageNumber, pageSize);
        public Task<IAttraction> ReadAttractionAsync(loginUserSessionDto usr, Guid id, bool flat) => _repo.ReadAttractionAsync(usr, id, flat);
        public Task<IAttraction> DeleteAttractionAsync(loginUserSessionDto usr, Guid id) => _repo.DeleteAttractionAsync(usr, id);
        public Task<IAttraction> UpdateAttractionAsync(loginUserSessionDto usr, csAttractionCUdto item) => _repo.UpdateAttractionAsync(usr, item);
        public Task<IAttraction> CreateAttractionAsync(loginUserSessionDto usr, csAttractionCUdto item) => _repo.CreateAttractionAsync(usr, item);
        #endregion
        

        #region The non-Async methods are not implemented using DbRepos
        public gstusrInfoAllDto Info => throw new NotImplementedException();

        public adminInfoDbDto Seed(loginUserSessionDto usr, int nrOfItems) => throw new NotImplementedException();
        public adminInfoDbDto RemoveSeed(loginUserSessionDto usr, bool seeded) => throw new NotImplementedException();

        public List<IPerson> ReadPersons(loginUserSessionDto usr, bool seeded, bool flat, string filter, int pageNumber, int pageSize) => throw new NotImplementedException();
        #endregion
    }
}
