using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

using Configuration;
using Models;
using Models.DTO;

using DbModels;
using DbContext;
using DbRepos;
using Services;
using System.Linq;
using System.Collections.Generic;

//Service namespace is an abstraction of using services without detailed knowledge
//of how the service is implemented.
//Service is used by the application layer using interfaces. Thus, the actual
//implementation of a service can be application dependent without changing code
//at application
namespace Services
{
    //IFriendsService ensures application layer can access csFriendsServiceModel
    //Friends model (without database) OR access csFriendsServiceDbRepos
    //FriendsDbM model (with database)class csFriendsServiceDbRepos without
    //without any code change
    public class csPersonsServiceModel : IPersonsService
    {
        private ILogger<csPersonsServiceModel> _logger = null;
        private object _locker = new object();

        #region only for layer verification
        private Guid _guid = Guid.NewGuid();
        private string _instanceHello;

        public string InstanceHello => _instanceHello;

        static public string Hello { get; } = $"Hello from namespace {nameof(Services)}, class {nameof(csPersonsServiceModel)}" +

            // added after project references is setup
            $"\n   - {csPersonsDbRepos.Hello}" +
            $"\n   - {csMainDbContext.Hello}";
        #endregion

        #region constructors
        public csPersonsServiceModel(ILogger<csPersonsServiceModel> logger)
        {
            _logger = logger;

            //only for layer verification
            _instanceHello = $"Hello from class {this.GetType()} with instance Guid {_guid}. " +
                $"Will use ModelOnly, no repo.";

            _logger.LogInformation(_instanceHello);
        }
        #endregion

        private List<csPerson> _persons = new List<csPerson>();
        private List<csAttraction> _attractions = new List<csAttraction>();


        public Task<adminInfoDbDto> RemoveSeedAsync(loginUserSessionDto usr, bool seeded) => Task.Run(() =>
        {
            lock (_locker) { return RemoveSeed(usr, seeded); }
        });
        public adminInfoDbDto RemoveSeed(loginUserSessionDto usr, bool seeded)
            => throw new NotImplementedException();


        public Task<adminInfoDbDto> SeedAsync(loginUserSessionDto usr, int nrOfItems) => Task.Run(() =>
        {
            lock (_locker) { return Seed(usr, nrOfItems); }
        });
        public adminInfoDbDto Seed(loginUserSessionDto usr, int nrOfItems)

        {
            var _seeder = new csSeedGenerator();

            _persons = _seeder.ToList<csPerson>(nrOfItems);
            _attractions = _seeder.ToList<csAttraction>(nrOfItems);

            #region extending the seeding
            var _seededaddresses = _seeder.ToList<csAddress>(nrOfItems);

            //Now _seededcomments is always the content of the Comments table
            var _seededcomments = _seeder.AllComments.Select(co => new csComment(co)).ToList();


            for (int c = 0; c < nrOfItems; c++)
            {

                //Create between 0 and 5 comments
                var _Comments = _seeder.FromListUnique<csComment>(_seeder.Next(0, 6), _seededcomments);
                _persons[c].Comments = _Comments.ToList<IComment>();

                //var _Attractions = _seeder.FromListUnique<csAttraction>(_seeder.Next(0, 6), _seededAttractions);
                //_persons[c].Attractions = _Attractions.ToList<IAttraction>();


                _Comments = _seeder.FromListUnique<csComment>(_seeder.Next(0, 3), _seededcomments);
                _attractions[c].Comments = _Comments.ToList<IComment>();

                var _addresses = _seeder.FromListUnique<csAddress>(_seeder.Next(0, 4), _seededaddresses);
                _attractions[c].Address = new csAddress().Seed(_seeder);

            }

            //A bit of Linq refresh
            var _info = new adminInfoDbDto();
            _info.nrSeededPersons = _persons.Count();
           // _info.nrSeededAddresses = _persons.Where(i => i.Address != null).Distinct().Count();
            _info.nrSeededComments = _persons.Where(i => i.Comments != null).ToList().Sum(i => i.Comments.Count);

            return _info;

        }
        #endregion

        //In order to make ReadAsync it has to return a deep copy of _Persons.
        //Otherwise another Task could seed or removeseed on the list while first
        //Task is working on the list. Use copy constructor pattern
        public Task<List<IPerson>> ReadPersonsAsync(loginUserSessionDto usr, bool seeded, bool flat, string filter, int pageNumber, int pageSize) => Task.Run(() =>
        {
            lock (_locker) {

                //to create a a copy is simple using linq and copy constructor pattern
                var list = (_persons != null) ? _persons.Select(f => new csPerson(f)).ToList<IPerson>() : null;
                return list;
            }
        });
        public List<IPerson> ReadPersons(loginUserSessionDto usr, bool seeded, bool flat, string filter, int pageNumber, int pageSize) => _persons.ToList<IPerson>();


        public Task<gstusrInfoAllDto> InfoAsync => Task.Run(() =>
        {
            lock (_locker) { return Info; }
        });
        public gstusrInfoAllDto Info => new gstusrInfoAllDto
        {
            Db = new gstusrInfoDbDto
            {
                nrSeededPersons = _persons.Count(i => i.Seeded),
                nrUnseededPersons = _persons.Count(i => !i.Seeded),
               // nrPersonsWithAddress = _persons.Count(p => p.Address == null),

                //nrSeededAddresses = _persons.Where(i => i.Seeded && i.Address != null).Distinct().Count(),
                //nrUnseededAddresses = _persons.Where(i => !i.Seeded && i.Address != null).Distinct().Count(),

                //nrSeededAttractions = _persons.Where(i => i.Seeded && i.Attractions != null).ToList().Sum(i => i.Attractions.Count),
                //nrUnseededAttractions = _persons.Where(i => !i.Seeded && i.Attractions != null).ToList().Sum(i => i.Attractions.Count),

                nrSeededComments = _persons.Where(i => i.Seeded && i.Comments != null).Sum(i => i.Comments.Count()),
                nrUnseededComments = _persons.Where(i => !i.Seeded && i.Comments != null).Sum(i => i.Comments.Count())
            }
        };



        #region not implemented

        public Task<IPerson> ReadPersonAsync(loginUserSessionDto usr, Guid id, bool flat) => throw new NotImplementedException();
        public Task<IPerson> DeletePersonAsync(loginUserSessionDto usr, Guid id) => throw new NotImplementedException();
        public Task<IPerson> UpdatePersonAsync(loginUserSessionDto usr, csPersonCUdto item) => throw new NotImplementedException();
        public Task<IPerson> CreatePersonAsync(loginUserSessionDto usr, csPersonCUdto item) => throw new NotImplementedException();

        public Task<List<IAddress>> ReadAddressesAsync(loginUserSessionDto usr, bool seeded, bool flat, string filter, int pageNumber, int pageSize) => throw new NotImplementedException();
        public Task<IAddress> ReadAddressAsync(loginUserSessionDto usr, Guid id, bool flat) => throw new NotImplementedException();
        public Task<IAddress> DeleteAddressAsync(loginUserSessionDto usr, Guid id) => throw new NotImplementedException();
        public Task<IAddress> UpdateAddressAsync(loginUserSessionDto usr, csAddressCUdto item) => throw new NotImplementedException();
        public Task<IAddress> CreateAddressAsync(loginUserSessionDto usr, csAddressCUdto item) => throw new NotImplementedException();

        public Task<List<IComment>> ReadCommentsAsync(loginUserSessionDto usr, bool seeded, bool flat, string filter, int pageNumber, int pageSize) => throw new NotImplementedException();
        public Task<IComment> ReadCommentAsync(loginUserSessionDto usr, Guid id, bool flat) => throw new NotImplementedException();
        public Task<IComment> DeleteCommentAsync(loginUserSessionDto usr, Guid id) => throw new NotImplementedException();
        public Task<IComment> UpdateCommentAsync(loginUserSessionDto usr, csCommentCUdto item) => throw new NotImplementedException();
        public Task<IComment> CreateCommentAsync(loginUserSessionDto usr, csCommentCUdto item) => throw new NotImplementedException();

        public Task<List<IAttraction>> ReadAttractionsAsync(loginUserSessionDto usr, bool seeded, bool flat, string filter, int pageNumber, int pageSize) => throw new NotImplementedException();

        public Task<IAttraction> ReadAttractionAsync(loginUserSessionDto usr, Guid id, bool flat) => throw new NotImplementedException();
        public Task<IAttraction> DeleteAttractionAsync(loginUserSessionDto usr, Guid id) => throw new NotImplementedException();
        public Task<IAttraction> UpdateAttractionAsync(loginUserSessionDto usr, csAttractionCUdto item) => throw new NotImplementedException();
        public Task<IAttraction> CreateAttractionAsync(loginUserSessionDto usr, csAttractionCUdto item) => throw new NotImplementedException();

        
        #endregion
    }
}
