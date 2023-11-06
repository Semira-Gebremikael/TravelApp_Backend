using Configuration;
using Models;
using Models.DTO;
using DbModels;
using DbContext;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Reflection.Metadata;
using DbModel;
using Microsoft.Identity.Client;
using System.Linq;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using System.Xml.Linq;
using System;
using System.Net;
using System.Collections.Generic;
using System.ComponentModel.Design;


//DbRepos namespace is a layer to abstract the detailed plumming of
//retrieveing and modifying and data in the database using EFC.

//DbRepos implements database CRUD functionality using the DbContext
namespace DbRepos;

public class csPersonsDbRepos
{
    private ILogger<csPersonsDbRepos> _logger = null;
    #region used before csLoginService is implemented
    private string _dblogin = "sysadmin";
    //private string _dblogin = "gstusr";
    //private string _dblogin = "usr";
    //private string _dblogin = "supusr";
    #endregion

    #region only for layer verification
    private Guid _guid = Guid.NewGuid();
    private string _instanceHello = null;

    static public string Hello { get; } = $"Hello from namespace {nameof(DbRepos)}, class {nameof(csPersonsDbRepos)}";
    public string InstanceHello => _instanceHello;
    #endregion

    #region contructors
    public csPersonsDbRepos()
    {
        _instanceHello = $"Hello from class {this.GetType()} with instance Guid {_guid}.";
    }
    public csPersonsDbRepos(ILogger<csPersonsDbRepos> logger):this()
    {
        _logger = logger;
        _logger.LogInformation(_instanceHello);
    }
    #endregion


    #region Admin repo methods


    //implementation using View
    public async Task<gstusrInfoAllDto> InfoAsync()
    {
        using (var db = csMainDbContext.DbContext(_dblogin))
        {
            var _info = new gstusrInfoAllDto
            {
                Db = new gstusrInfoDbDto
                {
                    #region full seeding
                    nrSeededPersons = await db.Persons.Where(p => p.Seeded).CountAsync(),
                    nrUnseededPersons = await db.Persons.Where(p => !p.Seeded).CountAsync(),
                    // nrAttractionWithPersons = await db.Attractions.Where(f => f.AttractionId != null).CountAsync(),

                    nrSeededAddresses = await db.Addresses.Where(p => p.Seeded).CountAsync(),
                    nrUnseededAddresses = await db.Addresses.Where(p => !p.Seeded).CountAsync(),

                    nrSeededAttractions = await db.Attractions.Where(p => p.Seeded).CountAsync(),
                    nrUnseededAttractions = await db.Attractions.Where(p => !p.Seeded).CountAsync(),
                    #endregion

                    nrSeededComments = await db.Comments.Where(p => p.Seeded).CountAsync(),
                    nrUnseededComments = await db.Comments.Where(p => !p.Seeded).CountAsync(),
                }
            };
            return _info;
        }
    }

    public async Task<adminInfoDbDto> SeedAsync(loginUserSessionDto usr, int nrOfItems)
    {
        using (var db = csMainDbContext.DbContext(_dblogin))
        {
            var _seeder = new csSeedGenerator();

            var _persons = _seeder.ToList<csPersonDbM>(nrOfItems);
            await db.Persons.AddRangeAsync(_persons);

            // create unique addresses
            var _existingaddresses = await db.Addresses.ToListAsync();
            var _addressesToSeed = _seeder.ToListUnique<csAddressDbM>(nrOfItems, _existingaddresses);

            // Add the unique addresses to the database
            await db.Addresses.AddRangeAsync(_addressesToSeed);
            var _attractions = _seeder.ToList<csAttractionDbM>(nrOfItems);

            foreach (var attraction in _attractions)
            {
                // Assign addresses to attractions. Attractions can be at the same address
                attraction.AddressDbM = _seeder.FromList(_addressesToSeed);

                int numberOfComments = _seeder.Next(0, 21);
                attraction.CommentDbM = _seeder.ToList<csCommentDbM>(numberOfComments);
                foreach (var comment in attraction.CommentDbM)
                {
                    comment.PersonDbM = _seeder.FromList(_persons);
                }

                // Tilldela personer till attraktioner

                attraction.PersonDbM = new List<csPersonDbM>();
                int numberOfPersons = _seeder.Next(0, _persons.Count);
                for (int i = 0; i < numberOfPersons; i++)
                {
                    var p = _seeder.FromList(_persons);
                    attraction.PersonDbM.Add(p);

                }
            }



            // Add attractions to the database
            await db.Attractions.AddRangeAsync(_attractions);
            await db.SaveChangesAsync();


            #endregion
            var _info = new adminInfoDbDto();
            #region full seed
            _info.nrSeededPersons = await db.Persons.CountAsync();
            _info.nrSeededAddresses = await db.Addresses.CountAsync();
            _info.nrSeededAttractions = await db.Attractions.CountAsync();
            _info.nrSeededComments = await db.Comments.CountAsync();
            #endregion

            return _info;

        }
    }


    public async Task<adminInfoDbDto> RemoveSeed(loginUserSessionDto usr, bool seeded)

    {
        using (var db = csMainDbContext.DbContext(_dblogin))
        {
            #region full seeding
            db.Persons.RemoveRange(db.Persons.Where(p => p.Seeded == seeded));
            db.Attractions.RemoveRange(db.Attractions.Where(a => a.Seeded == seeded));
            db.Comments.RemoveRange(db.Comments.Where(c => c.Seeded == seeded));
            db.Addresses.RemoveRange(db.Addresses.Where(a => a.Seeded == seeded));


            var _info = new adminInfoDbDto();
            if (seeded)
            {
                _info.nrSeededPersons = db.ChangeTracker.Entries().Count(entry => (entry.Entity is csPersonDbM) && entry.State == EntityState.Deleted);
                _info.nrSeededAttractions = db.ChangeTracker.Entries().Count(entry => (entry.Entity is csAttractionDbM) && entry.State == EntityState.Deleted);
                _info.nrSeededComments = db.ChangeTracker.Entries().Count(entry => (entry.Entity is csCommentDbM) && entry.State == EntityState.Deleted);
                _info.nrSeededAddresses = db.ChangeTracker.Entries().Count(entry => (entry.Entity is csAddressDbM) && entry.State == EntityState.Deleted);
            }

            else
            {
                _info.nrUnseededPersons = db.ChangeTracker.Entries().Count(entry => (entry.Entity is csPersonDbM) && entry.State == EntityState.Deleted);
                _info.nrUnseededAttractions = db.ChangeTracker.Entries().Count(entry => (entry.Entity is csAttractionDbM) && entry.State == EntityState.Deleted);
                _info.nrUnseededComments = db.ChangeTracker.Entries().Count(entry => (entry.Entity is csCommentDbM) && entry.State == EntityState.Deleted);
                _info.nrUnseededAddresses = db.ChangeTracker.Entries().Count(entry => (entry.Entity is csAddressDbM) && entry.State == EntityState.Deleted);
            }
            await db.SaveChangesAsync();
            return _info;

            #endregion

        }
    }

    #region exploring the ChangeTracker
    private static void ExploreChangeTracker(csMainDbContext db)
    {
        foreach (var e in db.ChangeTracker.Entries())
        {
            if (e.Entity is csComment c)
            {
                Console.WriteLine(e.State);
                Console.WriteLine(c.CommentId);
            }
        }
    }
    #endregion


    #region MapDtoToDbModelAsync

    private static async Task Map_csAttractionCUdto_To_csAttractionDbM(csMainDbContext db, csAttractionCUdto _itemDtoSrc, csAttractionDbM _itemDst)
    {
        //update AddressDbM from itemDto.AddressId
        _itemDst.AddressDbM = await db.Addresses.FirstOrDefaultAsync(
            a => (a.AddressId == _itemDtoSrc.AddressId));

        //update AttractionsDbM from itemDto.AttractionId list
            var  _Persons = new List<csPersonDbM>();
            foreach (var id in _itemDtoSrc.PersonId)
            {
                var p = await db.Persons.FirstOrDefaultAsync(i => i.PersonId == id);
                if (p == null)
                    throw new ArgumentException($"Item id {id} not found");

                 _Persons.Add(p);
            }
            _itemDst.PersonDbM = _Persons;
  

        //update CommentsDbM from itemDto.CommentId
         var _comments = new List<csCommentDbM>();
        foreach (var id in _itemDtoSrc.CommentId)
        {
            var c = await db.Comments.FirstOrDefaultAsync(i => i.CommentId == id);
                if (c == null)
                throw new ArgumentException($"Item id {id} not found");

            _comments.Add(c);
        }
  
        _itemDst.CommentDbM = _comments;
    }

    private static async Task Map_csCommentCUdto_To_csCommentDbM(csMainDbContext db, csCommentCUdto _itemDtoSrc, csCommentDbM _itemDst)
    {
        // Update PersonDbM based on itemDto.PersonId
        _itemDst.PersonDbM = (_itemDtoSrc.PersonId != null)
            ? await db.Persons.FirstOrDefaultAsync(p => p.PersonId == _itemDtoSrc.PersonId): null;


        // Update AttractionDbM based on itemDto.AttractionId
        _itemDst.AttractionDbM = (_itemDtoSrc.AttractionId != null)
            ? await db.Attractions.FirstOrDefaultAsync(a => a.AttractionId == _itemDtoSrc.AttractionId): null;

    }

    private static async Task Map_csPersonCUdto_To_csPersonDbM(csMainDbContext db, csPersonCUdto _itemDtoSrc, csPersonDbM _itemDst)
    {
        //update AttractionsDbM from itemDto.AttractionId list
        var _attraction = new List<csAttractionDbM>();
        if (_itemDtoSrc.PersonId != null)
        {
            _attraction = new List<csAttractionDbM>();
            foreach (var id in _itemDtoSrc.AttractionId)
            {
                var p = await db.Attractions.FirstOrDefaultAsync(i => i.AttractionId == id);
                if (p == null)
                    throw new ArgumentException($"Item id {id} not found");

                _attraction.Add(p);
            }
            _itemDst.AttractionsDbM = _attraction;
        }

        //update CommentsDbM from itemDto.CommentId
        var _comments = new List<csCommentDbM>();
        if (_itemDtoSrc.CommentId != null)
        {
            _comments = new List<csCommentDbM>();
            foreach (var id in _itemDtoSrc.CommentId)
            {
                var c = await db.Comments.FirstOrDefaultAsync(i => i.CommentId == id);
                if (c == null)
                    throw new ArgumentException($"Item id {id} not found");

                _comments.Add(c);
            }
        }
        _itemDst.CommentsDbM = _comments;

    }


    private static async Task Map_csAddressCUdto_To_csAddressDbM(csMainDbContext db, csAddressCUdto _itemDtoSrc, csAddressDbM _itemDst)
    {
        var _attraction = new List<csAttractionDbM>();
        foreach (var id in _itemDtoSrc.AttractionId)
        {
            var a = await db.Attractions.FirstOrDefaultAsync(i => i.AttractionId == id);
            if (a == null)
                throw new ArgumentException($"Item id {id} not found");

            _attraction.Add(a);
        }
        _itemDst.AttractionsDbM = _attraction;
    }
    #endregion


    #region Persons repo methods
    public async Task<IPerson> ReadPersonAsync(loginUserSessionDto usr, Guid id, bool flat)
    {
        using (var db = csMainDbContext.DbContext(_dblogin))
        {
            if (!flat)
            {
                var _person = db.Persons.AsNoTracking()
                                        .Include(p => p.CommentsDbM)
                                        .Include(p => p.AttractionsDbM)
                                        .ThenInclude(p => p.AddressDbM)
                                        .Where(p => p.PersonId == id);

                return await _person.FirstOrDefaultAsync<IPerson>();
            }
            else
            {
                //Not fully populated, compare the SQL Statements generated
                //remove tracking for all read operations for performance and to avoid recursion/circular access
                var _person = db.Persons.AsNoTracking().Where(p => p.PersonId == id); 
                return await _person.FirstOrDefaultAsync<IPerson>();
            }
        }
    }

    public async Task<List<IPerson>> ReadPersonsAsync(loginUserSessionDto usr, bool seeded, bool flat, string filter, int pageNumber, int pageSize)
    {
        using (var db = csMainDbContext.DbContext(_dblogin))
        {

            filter ??= "";

            if (!flat)
            {
                var _persons = db.Persons.AsNoTracking()
                                         .Include(p => p.CommentsDbM)
                                         .Include(p => p.AttractionsDbM)
                                         .Where(p => (p.CommentsDbM != null) ?p.CommentsDbM.Any() :false);


                return (List<IPerson>)await _persons.Where(i => i.Seeded == seeded && (
                                                   i.FirstName.ToLower().Contains(filter) ||
                                                   i.LastName.ToLower().Contains(filter)))
                                                    .ToListAsync<IPerson>();

            }
            else
            {
                var _persons = db.Persons.AsNoTracking();
                return await _persons.Where(i => i.Seeded == seeded && (
                                            i.FirstName.ToLower().Contains(filter) ||
                                            i.LastName.ToLower().Contains(filter)))
                                            .ToListAsync<IPerson>();

            }
        }
    }

    public async Task<IPerson> DeletePersonAsync(loginUserSessionDto usr, Guid id)
    {
        using (var db = csMainDbContext.DbContext(_dblogin))
        {
            //Find the instance with matching id
            //var _person = db.Persons.Where(p => p.PersonId == id);
            //var _item = await _person.FirstOrDefaultAsync<csPersonDbM>();

            var _person = await db.Persons.Include(p => p.CommentsDbM)
                                  .Include(p => p.AttractionsDbM)
                                  .FirstOrDefaultAsync(p => p.PersonId == id);


            //If the item does not exists
            if (_person == null) throw new ArgumentException($"Item {id} is not found");


            // Remove all comments and attractions belonging to the person
            db.Comments.RemoveRange(_person.CommentsDbM);
            db.Attractions.RemoveRange(_person.AttractionsDbM);


            //delete in the database model
            db.Persons.Remove(_person);

            //write to database in a UoW
            await db.SaveChangesAsync();
            return _person;
        }
    }

    public async Task<IPerson> UpdatePersonAsync(loginUserSessionDto usr, csPersonCUdto itemDto)
    {
        using (var db = csMainDbContext.DbContext(_dblogin))
        {
            var _person1 = db.Persons.Where(i => i.PersonId == itemDto.PersonId);
            var _item = await _person1.Include(i => i.AttractionsDbM)
                                       .Include(i => i.CommentsDbM)
                                       .FirstOrDefaultAsync<csPersonDbM>();

            //If the item does not exists
            if (_item == null) throw new ArgumentException($"Item {itemDto.PersonId} is not existing");

            //Avoid duplicates in the comment table, so check that
            var _person2 = db.Persons.Where(i => ((
                                      i.FirstName == itemDto.FirstName) && (
                                      i.LastName == itemDto.LastName) && (
                                      i.Email == itemDto.Email) && (
                                      i.Birthday == itemDto.Birthday)
                                        ));

            var _existingItem = await _person2.FirstOrDefaultAsync<csPersonDbM>();
            if (_existingItem != null && _existingItem.PersonId != itemDto.PersonId)
                throw new ArgumentException($"Item already exist with id {_existingItem.PersonId}");


            //transfer any changes from DTO to database objects
            //Update individual properties 
            _item.UpdateFromDTO(itemDto);

            await Map_csPersonCUdto_To_csPersonDbM(db, itemDto, _item);


            //write to database model
            db.Persons.Update(_item);

            //write to database in a UoW
            await db.SaveChangesAsync();
            return _item;

        }
    }

    public async Task<IPerson> CreatePersonAsync(loginUserSessionDto usr, csPersonCUdto itemDto)
       {
        using (var db = csMainDbContext.DbContext(_dblogin))
        {
            var _item = new csPersonDbM();
            _item.PersonId = Guid.NewGuid();
            _item.FirstName = itemDto.FirstName;
            _item.LastName = itemDto.LastName;
            _item.Email = itemDto.Email;
            _item.Birthday = itemDto.Birthday;


            //Update navigation properties
            await Map_csPersonCUdto_To_csPersonDbM(db, itemDto, _item);

            //write to database model
            db.Persons.Add(_item);

            //write to database in a UoW
            await db.SaveChangesAsync();
            return _item;
        }

    }

    #endregion

    #region Addresses repo methods
    public async Task<IAddress> ReadAddressAsync(loginUserSessionDto usr, Guid id, bool flat)
    {
        using (var db = csMainDbContext.DbContext(_dblogin))
        {
            if (flat)
            {
                var _addresses = db.Addresses.AsNoTracking()
                                         .Include(a => a.AttractionsDbM)
                                         .ThenInclude(a => a.PersonDbM)
                                         .Where(a => a.AddressId == id);

                return await _addresses.FirstOrDefaultAsync<IAddress>();
            }
            else
            {
                var _addresses = db.Addresses.AsNoTracking()
                    .Where(i => i.AddressId == id);

                return await _addresses.FirstOrDefaultAsync<IAddress>();
            }
        }
    }

    public async Task<List<IAddress>> ReadAddressesAsync(loginUserSessionDto usr, bool seeded, bool flat, string filter, int pageNumber, int pageSize)
    {
        using (var db = csMainDbContext.DbContext(_dblogin))
        {
            filter ??= "";
            if (!flat)
            {
                //make sure the model is fully populated, try without include.
                //remove tracking for all read operations for performance and to avoid recursion/circular access
                var _addresses = db.Addresses.AsNoTracking()
                                             .Include(a => a.AttractionsDbM );

                return await _addresses .Where(i => i.Seeded == seeded && (
                                        i.StreetAddress.ToLower().Contains(filter) ||
                                        i.City.ToLower().Contains(filter) ||
                                        i.Country.ToLower().Contains(filter)))
                                         .ToListAsync<IAddress>();
            }
            else
            {
                //Not fully populated, compare the SQL Statements generated
                //remove tracking for all read operations for performance and to avoid recursion/circular access
                var _addresses = db.Addresses.AsNoTracking();
                                             
                return await _addresses .Where(i => i.Seeded == seeded && (
                                        i.StreetAddress.ToLower().Contains(filter) ||
                                        i.City.ToLower().Contains(filter) ||
                                        i.Country.ToLower().Contains(filter)))
                                         .ToListAsync<IAddress>();

            }
        }

    }

    public async Task<IAddress> DeleteAddressAsync(loginUserSessionDto usr, Guid id)
    {
        using (var db = csMainDbContext.DbContext(_dblogin))
        {
            //Find the instance with matching id
            var _addresses = db.Addresses.Where(p => p.AddressId == id);
            var _item = await _addresses.FirstOrDefaultAsync<csAddressDbM>();

            //If the item does not exists
            if (_item == null) throw new ArgumentException($"Item {id} is not found");

            //delete in the database model
            db.Addresses.Remove(_item);

            //write to database in a UoW
            await db.SaveChangesAsync();
            return _item;
        }
    }

    public async Task<IAddress> UpdateAddressAsync(loginUserSessionDto usr, csAddressCUdto itemDto)
    {
        using (var db = csMainDbContext.DbContext(_dblogin))
        {
            var _addresses1 = db.Addresses.Where(i => i.AddressId == itemDto.AddressId);
            var _item = await _addresses1.Include(i => i.AttractionsDbM)
                                         .FirstOrDefaultAsync<csAddressDbM>();

            //If the item does not exists
            if (_item == null) throw new ArgumentException($"Item {itemDto.AddressId} is not existing");

            //I cannot have duplicates in the Addresses table, so check that
            var _addresses2 = db.Addresses.Where(i => ((
                                         i.StreetAddress == itemDto.StreetAddress) &&(
                                         i.ZipCode == itemDto.ZipCode) && (
                                         i.City == itemDto.City) && (
                                         i.Country == itemDto.Country)));

            var _existingItem = await _addresses2.FirstOrDefaultAsync<csAddressDbM>();
            if (_existingItem != null && _existingItem.AddressId != itemDto.AddressId)
                throw new ArgumentException($"Item already exist with id {_existingItem.AddressId}");

            //Update individual properties 
            _item.UpdateFromDTO(itemDto);

            await Map_csAddressCUdto_To_csAddressDbM(db, itemDto, _item);

            //write to database model
            db.Addresses.Update(_item);

            //write to database in a UoW
            await db.SaveChangesAsync();
            return _item;

        }
    }

    public async Task<IAddress> CreateAddressAsync(loginUserSessionDto usr, csAddressCUdto itemDto)
    {
        {
            if (itemDto.AddressId != null)
                throw new ArgumentException($"{nameof(itemDto.AddressId)} must be null when creating a new object");

            using (var db = csMainDbContext.DbContext(_dblogin))
            {
                //I cannot have duplicates in the Addresses table, so check that
                var _addresses2 = db.Addresses
                  .Where(i => ((i.StreetAddress == itemDto.StreetAddress) &&
                     (i.ZipCode == itemDto.ZipCode) &&
                     (i.City == itemDto.City) &&
                     (i.Country == itemDto.Country)));
                var _existingItem = await _addresses2.FirstOrDefaultAsync<csAddressDbM>();
                if (_existingItem != null)
                    throw new ArgumentException($"Item already exist with id {_existingItem.AddressId}");

                //transfer any changes from DTO to database objects
                //Update individual properties 
                var _item = new csAddressDbM(itemDto);

                //write to database model
                db.Addresses.Add(_item);

                //write to database in a UoW
                await db.SaveChangesAsync();
                return _item;
            }
        }
    }
    #endregion

    #region Comment repo methods
    public async Task<IComment> ReadCommentAsync(loginUserSessionDto usr, Guid id, bool flat)
    {
        using (var db = csMainDbContext.DbContext(_dblogin))
        {
            if (flat)
            {
                //make sure the model is fully populated, try without include.
                //remove tracking for all read operations for performance and to avoid recursion/circular access
                var _comment = db.Comments.AsNoTracking()
                                          .Include(c => c.AttractionDbM)
                                          .ThenInclude(c => c.PersonDbM)
                                          .Where(c => c.CommentId == id);

                return await _comment .FirstOrDefaultAsync<IComment>();
            }
            else
            {
                    var _comment = db.Comments.AsNoTracking()
                                              .Where(c => c.CommentId == id);

                return await _comment.FirstOrDefaultAsync<IComment>();
            }
        }
    }

    public async Task<List<IComment>> ReadCommentsAsync(loginUserSessionDto usr, bool seeded, bool flat, string filter, int pageNumber, int pageSize)
    {
        using (var db = csMainDbContext.DbContext(_dblogin))
        {
            if (!flat)
            {
                var _comment = db.Comments.AsNoTracking().Include(c => c.AttractionDbM)
                                                          .ThenInclude(c =>c.AddressDbM)
                                                          .Include(c => c.PersonDbM);

                return await _comment.ToListAsync<IComment>();

            }
            else
            {
                var _comment = db.Comments.AsNoTracking();
                return await _comment.ToListAsync<IComment>();

            }

        }
    }

    public async Task<IComment> DeleteCommentAsync(loginUserSessionDto usr, Guid id)
    {
        using (var db = csMainDbContext.DbContext(_dblogin))
        {
            var _comment = db.Comments.Where(c => c.CommentId == id);
            var _item = await _comment.FirstOrDefaultAsync<csCommentDbM>();


            if (_item == null) throw new ArgumentException($"Item {id} is not found");

            db.Comments.Remove(_item);

            await db.SaveChangesAsync();
            return _item;

        }
    }

    public async Task<IComment> UpdateCommentAsync(loginUserSessionDto usr, csCommentCUdto itemDto)
    {
        using (var db = csMainDbContext.DbContext(_dblogin))
        {
            var _comment1 = db.Comments.Where(i => i.CommentId == itemDto.CommentId);
            var _item = await _comment1.Include(i => i.AttractionDbM)
                                       .Include(i => i.PersonDbM)
                                       .FirstOrDefaultAsync<csCommentDbM>();

            //If the item does not exists
            if (_item == null) throw new ArgumentException($"Item {itemDto.CommentId} is not existing");

            //Avoid duplicates in the comment table, so check that
            var _comment2 = db.Comments.Where(i => ((
                                      i.CommentText == itemDto.CommentText) && (
                                      i.Rating == itemDto.Rating)));

            var _existingItem = await _comment2.FirstOrDefaultAsync<csCommentDbM>();
            if (_existingItem != null && _existingItem.CommentId != itemDto.CommentId)
                throw new ArgumentException($"Item already exist with id {_existingItem.CommentId}");


            //transfer any changes from DTO to database objects
            //Update individual properties 
            _item.UpdateFromDTO(itemDto);

            //Update navigation properties
            await Map_csCommentCUdto_To_csCommentDbM(db, itemDto, _item);


            //write to database model
            db.Comments.Update(_item);

            //write to database in a UoW
            await db.SaveChangesAsync();
            return _item;




            
        }
    }

    public async Task<IComment> CreateCommentAsync(loginUserSessionDto usr, csCommentCUdto itemDto)
    {
      
        using (var db = csMainDbContext.DbContext(_dblogin))
        {
            //Avoid duplicates in the Quotes table, so check that
            var _comment = new csCommentDbM();
            _comment.CommentId = Guid.NewGuid();
            _comment.CommentText = itemDto.CommentText;
            _comment.Rating = itemDto.Rating;


            //transfer any changes from DTO to database objects
            //Update individual properties 
            var _item = new csCommentDbM(itemDto);

            //Update navigation properties
            await Map_csCommentCUdto_To_csCommentDbM(db, itemDto, _item);

            //write to database model
            db.Comments.Add(_item);

            //write to database in a UoW
            await db.SaveChangesAsync();
            return _item;

        }
    }
    #endregion

    #region Attraction repo methods
    public async Task<IAttraction> ReadAttractionAsync(loginUserSessionDto usr, Guid id, bool flat)
    {
        using (var db = csMainDbContext.DbContext(_dblogin))
        {
            if (!flat)
            {
                var _attraction = db.Attractions.AsNoTracking()
                                        .Include(a => a.AddressDbM)
                                        .Include(a => a.CommentDbM)
                                        .Include(a => a.PersonDbM)
                                        .Where(a => a.AttractionId == id);


                return await _attraction.FirstOrDefaultAsync<IAttraction>();
            }
            else
            {
                //Not fully populated, compare the SQL Statements generated
                //remove tracking for all read operations for performance and to avoid recursion/circular access
                var _attraction = db.Attractions.AsNoTracking().Where(p => p.AttractionId == id);
                return await _attraction.FirstOrDefaultAsync<IAttraction>();
            }
        }
    }

    public async Task<List<IAttraction>> ReadAttractionsAsync(loginUserSessionDto usr, bool seeded, bool flat, string filter, int pageNumber, int pageSize)
    {
        using (var db = csMainDbContext.DbContext(_dblogin))
         {
            filter ??= "";


            if (!flat)
            {
                var _attractions = await db.Attractions.AsNoTracking()
                    .Include(a => a.AddressDbM)
                    .Include(a => a.CommentDbM)
                    .Where(attraction => attraction.CommentDbM.Count == 0)
                    .ToListAsync<IAttraction>();

                return _attractions;
            }
            else
            {
                var _attractions = await db.Attractions.AsNoTracking()
                    .Include(a => a.CommentDbM)
                    .Where(attraction => attraction.CommentDbM.Count == 0)
                    .ToListAsync<IAttraction>();

                return _attractions;
            }
        }
    }

    

    public async Task<IAttraction> DeleteAttractionAsync(loginUserSessionDto usr, Guid id)
    {
        using (var db = csMainDbContext.DbContext(_dblogin))
        {
            var _attraction = db.Attractions
                .Where(a => a.AttractionId == id);
            var _item = await _attraction.FirstOrDefaultAsync<csAttractionDbM>();

            //If the item does not exists
            if (_item == null) throw new ArgumentException($"Item {id} is not found");

            db.Attractions.Remove(_item);

            await db.SaveChangesAsync();
            return _item;
        }
    }

    public async Task<IAttraction> UpdateAttractionAsync(loginUserSessionDto usr, csAttractionCUdto itemDto)
    {
        using (var db = csMainDbContext.DbContext(_dblogin))
        {
            //Find the instance with matching id and read the navigation properties.
            var _attraction1 = db.Attractions.Where(i => i.AttractionId == itemDto.AttractionId);
            var _item = await _attraction1.Include(i => i.AddressDbM)
                                           .Include(i => i.PersonDbM)
                                           .Include(i => i.CommentDbM)
                                           .FirstOrDefaultAsync<csAttractionDbM>();

            //If the item does not exists
            if (_item == null) throw new ArgumentException($"Item {itemDto.AttractionId} is not existing");

            //transfer any changes from DTO to database objects
            //Update individual properties
            _item.UpdateFromDTO(itemDto);

            //Update navigation properties
            await Map_csAttractionCUdto_To_csAttractionDbM(db, itemDto, _item);

            //write to database model
            db.Attractions.Update(_item);

            //write to database in a UoW
            await db.SaveChangesAsync();
            return _item;
        }
    }

    public async Task<IAttraction> CreateAttractionAsync(loginUserSessionDto usr, csAttractionCUdto itemDto)

    {
        using (var db = csMainDbContext.DbContext(_dblogin))
        {
            var _item = new csAttractionDbM();
            _item.AttractionId = Guid.NewGuid();
            _item.AttractionName = itemDto.AttractionName;
            _item.Description = itemDto.Description;
            _item.Category = itemDto.Category;


            //Update navigation properties
            await Map_csAttractionCUdto_To_csAttractionDbM(db, itemDto, _item);

            //write to database model
            db.Attractions.Add(_item);

            //write to database in a UoW
            await db.SaveChangesAsync();
            return _item;
        }
    }

    #region to view 
    public async Task<gstusrInfoAllDto> AttractionsWithNullComments()
    {
        using (var db = csMainDbContext.DbContext(_dblogin))
        {
            var info = new gstusrInfoAllDto();
            info._attractionsWithNullComments = await db._attractionsWithNullComments.ToListAsync();

            return info;
        }
    }

    #endregion


    #endregion
}



