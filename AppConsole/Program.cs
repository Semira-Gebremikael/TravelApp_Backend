using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;


using Configuration;
using Models;
using Models.DTO;
using Services;

using DbContext;
using DbModels;
using DbRepos;
using Microsoft.Extensions.Logging;
using System.Data.Common;

//ConsoleAPP namespace is the top layer in the stack and contains the business
//, i.e. application logic. Using the other layers this layer can easily
//be switched from one type of application to another

//Once all the layers are setup with its own references. ConsoleApp will
//depend ONLY on Configuration, DbModels and Services.
//This allows the Application to be independed from any database implementation
namespace ConsoleApp;

class Program
{
    //used for seeding
    const int _nrSeeds = 1000;
    const int _nrUsers = 40;
    const int _nrSuperUsers = 40;

    //used when huge nr of data, read pages of _readerPageSize items, instead of all items
    const int _readerPageSize = 1000;

    static async Task Main(string[] args)
    {
        //Allows a Console App to use .NET Dependecy Injection pattern,
        //by runnign the App within a host
        HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);

        #region Dependency Injection of Logger
        //Add your own Services to use with DI
        builder.Services.AddSingleton<ILoggerProvider, csInMemoryLoggerProvider>();
        #endregion

        #region Dependency Injection
        //DI injects the DbRepos into csFriendService
        builder.Services.AddScoped<csPersonsDbRepos>();

       // builder.Services.AddSingleton<IPersonsService, csPersonsServiceModel>(); 
        builder.Services.AddScoped<IPersonsService, csPersonsServiceDb>();
        #endregion

        //Build the host
        using IHost host = builder.Build();

        #region To be removed for the real application
        //Verification(host);
        #endregion

        var _service = host.Services.CreateAsyncScope().ServiceProvider.GetRequiredService<IPersonsService>();
        await PersonServiceSnapshot(_service);

        //Terminate the host and the Application properly
        await host.RunAsync();
    }

    #region used for basic verificaton
    private static void Verification(IHost host)
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("Verification start");
        Console.WriteLine("------------------");
        //to verify the layers are accessible
        Console.ForegroundColor = ConsoleColor.Gray;
        Console.WriteLine("\nLayer access:");
        Console.WriteLine(csAppConfig.Hello);
        Console.WriteLine(csPerson.Hello);

        Console.WriteLine(csMainDbContext.Hello);
        Console.WriteLine(csPersonsDbRepos.Hello);

        Console.WriteLine(csLoginService.Hello);
        Console.WriteLine(csJWTService.Hello);
        Console.WriteLine(csPersonsServiceModel.Hello);
        Console.WriteLine(csPersonsServiceDb.Hello);

        //to verify connection strings can be read from appsettings.json
        Console.WriteLine($"\nDbConnections:\nDbLocation: {csAppConfig.DbSetActive.DbLocation}" +
            $"\nDbServer: {csAppConfig.DbSetActive.DbServer}");
        Console.WriteLine("DbUserLogins in DbSet:");
        foreach (var item in csAppConfig.DbSetActive.DbLogins)
        {
            Console.WriteLine($" DbUserLogin: {item.DbUserLogin}" +
                            $"\n DbConnection: {item.DbConnection}\nConString: <secret>");
        }

        //to verify usersecret access
        Console.WriteLine($"\nUser secrets:\n{csAppConfig.SecretMessage}");

        // nDependency Injection
        Console.WriteLine($"\nDependency Injection:");
        Console.WriteLine($"Service Scope 1:");
        using (IServiceScope serviceScope = host.Services.CreateAsyncScope())
        {
            IServiceProvider provider = serviceScope.ServiceProvider;

            /*IPersonsService personsService = provider.GetRequiredService<IPersonsService>();
              Console.WriteLine($"\nService instance 1:\n{personsService.InstanceHello}\n");*/
        }

        Console.WriteLine($"\nCustomer Logger, InMemoryLoggerProvider:");
        var customLoggerService = host.Services.CreateScope()
                    .ServiceProvider.GetRequiredService<ILoggerProvider>();
        foreach (var item in ((csInMemoryLoggerProvider)customLoggerService).Messages)
        {
            Console.WriteLine($"  -- {item}\n");
        }


        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("\nVerification end");
        Console.WriteLine("------------------\n\n");
    }
    #endregion

    #region used when seeding of model IPerson IAddress, IAttraction, IComment
    private static async Task PersonServiceSnapshot(IPersonsService personService)
    {

        loginUserSessionDto _usr = new loginUserSessionDto { UserRole = "sysadmin" };

        //var _info = await personService.RemoveSeedAsync(_usr, true);
        //Console.WriteLine($"\n{_info.nrSeededPersons} Persons removed");

         var _info = await personService.SeedAsync(_usr, _nrSeeds);
        Console.WriteLine($"{_info.nrSeededPersons} persons seeded");

        var _list = await personService.ReadPersonsAsync(_usr, true, false, null, 0, 100);
        Console.WriteLine("\nFirst 5 persons");
        _list.Take(5).ToList().ForEach(p => Console.WriteLine(p));

        Console.WriteLine("\nLast 5 persons");
        _list.TakeLast(5).ToList().ForEach(p => Console.WriteLine(p));
    }

    private static async Task PersonServiceInfo(IPersonsService personService)
    {
        var info = await personService.InfoAsync;
        Console.WriteLine($"\nPersonServiceInfo:");
        Console.WriteLine($"Nr of seeded persons: {info.Db.nrSeededPersons}");
        Console.WriteLine($"Nr of unseeded persons: {info.Db.nrUnseededPersons}");
        //Console.WriteLine($"Nr of persons with address: {info.Db.nrPersonsWithAddress}");

        Console.WriteLine($"Nr of addresses: {info.Db.nrSeededAddresses}");
        Console.WriteLine($"Nr of unseeded addresses: {info.Db.nrUnseededAddresses}");

        Console.WriteLine($"Nr of attraction: {info.Db.nrSeededAttractions}");
        Console.WriteLine($"Nr of unseeded pets: {info.Db.nrUnseededAttractions}");

        Console.WriteLine($"Nr of comments: {info.Db.nrSeededComments}");
        Console.WriteLine($"Nr of unseeded comments: {info.Db.nrUnseededComments}");
        Console.WriteLine();
    }
    #endregion



    #region used when seeding of model IUser
    private static async Task LoginServiceSnapshot(ILoginService loginService)
    {
        var _info = await loginService.SeedAsync(_nrUsers, _nrSuperUsers);
        Console.WriteLine($"{_info.NrUsers} users seeded");
        Console.WriteLine($"{_info.NrSuperUsers} superusers seeded");
    }
    #endregion

    #region used for login
    private static async Task LoginServiceLogin(ILoginService loginService)
    {
        var _usrCreds = new loginCredentialsDto { UserNameOrEmail = "user1", Password = "user1" };

        try
        {
            var _usr = await loginService.LoginUserAsync(_usrCreds);
            Console.WriteLine($"\n{_usr.UserName} logged in");
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
    #endregion
}

