using System.Collections.Generic;
using System.Xml.Linq;

namespace Models
{
    public class GoodComment
    {
        public string CommentText { get; set; }
        public int Rating { get; set; }
        public GoodComment() { }

        public GoodComment(string commentText, int rating)
        {
            CommentText = commentText;
            Rating = Math.Clamp(rating, 1, 10);
        }
    }


    public class GoodDescription
    {
        public virtual enCategory Category { get; set; }

        public virtual string AttractionName { get; set; }

        public virtual string Description { get; set; }
        public GoodDescription() { }


        public GoodDescription(string description)
        {
            Description = description;
        }

    }


    public interface ISeed<T>
    {
        //In order to separate from real and seeded instances
        public bool Seeded { get; set; }

        //Seeded The instance
        public T Seed(csSeedGenerator seedGenerator);
    }
    
    public class csSeedGenerator : Random
    {

       public string[] _firstnames = "Potter, Voldemort, Adams, Baker, Carter, Dixon, Evans, Fisher, Green, Harrison, Johnson, Keller, Lopez, Mitchell, ,Harry, Lord, Hermione, Albus, Severus, Ron, Draco, Frodo, Gandalf, Sam, Peregrin, Saruman, Smith, Johnson, Williams, Jones, Brown, Davis, Miller,Wilson, Moore, Taylor, Anderson, Thomas, Jackson, White, Harris,Martin, Thompson, Garcia,Martinez, Robinson,Clark, Rodriguez, Lewis, Lee, Walker, Hall, Allen, Young, Hernandez,King,".Split(", ");

       public string[] _lastnames = "Potter, Voldemort, Adams, Baker, Carter, Dixon, Evans, Fisher, Green, Harrison, Johnson, Keller, Lopez, Mitchell, Nelson, Owens, Perez, Quinn, Ramirez, Smith, Turner, Walker,Young, Harris,Clark, Morales, Gonzalez, Davis, Martine, Walker,Young, Harris,Clark, Morales, Gonzalez, Davis, Martine,Thompson, Williams, Wright, Granger, Dumbledore, Snape, Malfoy, Baggins, the Gray, Gamgee, Took, the White".Split(", ");




        public string[][] _city =
            {
                "Stockholm, Göteborg, Malmö, Uppsala, Linköping, Örebro".Split(", "),
                "Oslo, Bergen, Trondheim, Stavanger, Dramen".Split(", "),
                "Köpenhamn, Århus, Odense, Aahlborg, Esbjerg".Split(", "),
                "Helsingfors, Espoo, Tampere, Vaanta, Oulu".Split(", "),
                 "Södermalm,Haga,Västra Hamnen,Gamla Stan,Kungsholmen,Östermalm,Sigtuna,Djurgården,Norrmalm,Uppsala,Göteborg,Malmö".Split(","),
                "Linköping,Örebro,Stockholm,Uppsala,Västerås,Norrköping,Helsingborg,Jönköping,Umeå,Lund,Borås,Eskilstuna,Karlstad".Split(","),
                "Södertälje,Halmstad,Växjö,Sundsvall,Luleå,Trollhättan,Östersund,Motala,Lidköping,Skellefteå,Falun,Uddevalla,Landskrona".Split(","),
                "Kalmar,Trelleborg,Kristianstad,Karlskrona,Skövde,Nyköping,Härnösand,Visby,Örnsköldsvik,Kiruna".Split(","),
             };

        public string[][] _StreetAddress =
            {
                "Svedjevägen, Ringvägen, Vasagatan, Odenplan, Birger Jarlsgatan, Äppelviksvägen, Kvarnbacksvägen".Split(", "),
                "Bygdoy alle, Frognerveien, Pilestredet, Vidars gate, Sågveien, Toftes gate, Gardeveiend".Split(", "),
                "Rolighedsvej, Fensmarkgade, Svanevej, Gröndalsvej, Githersgade, Classensgade, Moltekesvej".Split(", "),
                "Arkandiankatu, Liisankatu, Ruoholahdenkatu, Pohjoistranta, Eerikinkatu, Vauhtitie, Itainen Vaideki".Split(", "),
    
              };

        public string[] _AttractionName = "Vasa Museum, Skansen,ABBA The Museum, Munchmuseet, Frammuseet, ARKEN Museum,Rosenborgs slott,Kiasma,Suomenlinna,Rold Skov, Lappland, Jotunheimen,Preikestolen, Kebnekaise, Jelling stenarna, Suomenlinn,Rauma Gamla stan, Frederiksborgs slott, Nidarosdomen, Stadshuset, Turning Torso, Operahuset,ARoS Aarhus Kunstmuseum, Sibeliusmonumentet, Midsommarfirande, Vasaloppet, Samisk kultur, Sauna, Sibeliusfestivalen, Hygge".Split(", ");

        public string[] _country = "Sweden, Norway, Denmark, Finland".Split(", ");


        public string[] _domains = "icloud.com, me.com, mac.com, hotmail.com, gmail.com".Split(", ");





       public GoodComment[] _commentText = {

            //About 
            new GoodComment("Nice place for a walk, but not very exciting.", 3),
            new GoodComment("Beautiful architecture and tranquil atmosphere", 4),
            new GoodComment("One of my favorite places in Copenhagen!", 6),
            new GoodComment("A must for anyone who loves history.", 9),
            new GoodComment("A good place to get panoramic views of the city", 4),
            new GoodComment("A nice place to visit with the family?", 2),
            new GoodComment("The architecture was really impressive.", 2),
            new GoodComment("The tower was okay, but expected more.", 4),
            new GoodComment("Visited during a school trip, had a fantastic time.", 4),
            new GoodComment("The most beautiful place I visited.", 4),
            new GoodComment("Lovely place to relax at.",4),
            new GoodComment("Worth a visit for the view from the top.", 7),
            new GoodComment("Interesting, but not as impressive as I expected.", 9),
            new GoodComment("A bit crowded, but worth a visit.", 8),
            new GoodComment("A peaceful oasis in the middle of the city.",6),
            new GoodComment("Fantastic attraction!",9),
            new GoodComment("Magnificent waterfall and great photo opportunities.",9),
            new GoodComment("Beautiful place, but it can get crowded during peak season.", 6),
            new GoodComment("Incredible exhibits and engaging stories.",2),
            new GoodComment("The best place to learn about Iceland's history..", 5),
            new GoodComment("Fantastic view of the city.", 8),
            new GoodComment("Nice place for a walk, but not very exciting.", 6),
            new GoodComment("Beautiful architecture and tranquil atmosphere", 8),
            new GoodComment("One of my favorite places in Copenhagen!", 3),
            new GoodComment("Fantastic museum! A must-visit in Stockholm",8),
            new GoodComment("Impressive ships, but a bit crowded.",7),
            new GoodComment("Loved the history and exhibitions.",4),
            new GoodComment("Great place to visit.",2),
            new GoodComment("A lovely place to spend the day, especially with kids.",7),
            new GoodComment("Fun for the whole family.",3),
            new GoodComment("So much to see and learn!", 6),
            new GoodComment("I recommend a visit",5),
            new GoodComment("Super fun amusement park! Highly recommended..",3),
            new GoodComment("Good rides, but a bit expensive.",5),
            new GoodComment("An amazing place for both children and adults.",7),
            new GoodComment("Interesting exhibitions, but could use some updates.",2),
        };

        GoodDescription[] _description = {
            new GoodDescription("Historical ship museum in Stockholm, featuring the 17th-century warship Vasa."),
            new GoodDescription("Open-air museum and zoo in Stockholm showcasing Swedish culture and nature.."),
            new GoodDescription("Modern art institution in Copenhagen featuring contemporary art exhibitions."),
            new GoodDescription("Historic castle in Copenhagen housing royal treasures and gardens."),
            new GoodDescription("Helsinki's national museum of contemporary art with modern architecture."),
            new GoodDescription("Historic fortress near Helsinki with maritime history.."),
            new GoodDescription("Extensive forest in Denmark for hiking and nature experiences."),
            new GoodDescription("Northern Scandinavia with the Northern Lights, winter sports, and Sami culture."),
            new GoodDescription("HNorway's highest mountain range with spectacular hiking."),
            new GoodDescription("Impressive cliff in Norway with a breathtaking view."),
            new GoodDescription("Sweden's highest mountain and a hiking destination."),
            new GoodDescription("Denmark with historic runestones and burial mounds."),
            new GoodDescription("Finnish medieval town center with wooden house."),
            new GoodDescription("Magnificent castle in Denmark surrounded by gardens."),
            new GoodDescription("Impressive cathedral in Trondheim, Norway."),
            new GoodDescription("Stockholm City Hall with distinctive architecture and an observation tower."),
            new GoodDescription("Malmö skyscraper with a unique spiral design."),
            new GoodDescription("Modern opera house in Oslo for cultural events."),
            new GoodDescription("Art museum in Aarhus, Denmark, known for its rainbow walkway."),
            new GoodDescription("Sculpture in Helsinki commemorating composer Jean Sibelius."),
            new GoodDescription("Traditional Scandinavian festival at the summer solstice."),
            new GoodDescription(" Sweden's oldest cross-country skiing race held in Mora."),
            new GoodDescription("Indigenous culture in northern Scandinavia with unique music and clothing."),
        };


        public string AttractionName => _AttractionName[this.Next(0, _AttractionName.Length)];

        public string FirstName => _firstnames[this.Next(0, _firstnames.Length)];
        public string LastName => _lastnames[this.Next(0, _lastnames.Length)];
        public string FullName => $"{FirstName} {LastName}";

        public DateTime getDateTime(int? fromYear = null, int? toYear = null)
        {
            bool dateOK = false;
            DateTime _date = default;
            while (!dateOK)
            {
                fromYear ??= DateTime.Today.Year;
                toYear ??= DateTime.Today.Year + 1;

                try
                {
                    int year = this.Next(Math.Min(fromYear.Value, toYear.Value),
                        Math.Max(fromYear.Value, toYear.Value));
                    int month = this.Next(1, 13);
                    int day = this.Next(1, 32);

                    _date = new DateTime(year, month, day);
                    dateOK = true;
                }
                catch
                {
                    dateOK = false;
                }
            }

            return DateTime.SpecifyKind(_date, DateTimeKind.Utc);  //Used for Postgres compatibility - only UTC is supported
        }

        // General random truefalse
        public bool Bool => (this.Next(0, 10) < 5) ? true : false;

       

        public string Email(string fname = null, string lname = null)
        {
            fname ??= FirstName;
            lname ??= LastName;

            return $"{fname}.{lname}@{_domains[this.Next(0, _domains.Length)]}";
        }

        public string Phone => $"{this.Next(700, 800)} {this.Next(100, 1000)} {this.Next(100, 1000)}";

        public string Country => _country[this.Next(0, _country.Length)];

        public string City(string Country = null)
        {

            var cIdx = this.Next(0, _city.Length);
            if (Country != null)
            {
                //Give a City in that specific country
                cIdx = Array.FindIndex(_country, c => c.ToLower() == Country.Trim().ToLower());

                if (cIdx == -1) throw new Exception("Country not found");
            }

            return _city[cIdx][this.Next(0, _city[cIdx].Length)];
        }

    public string StreetAddress(string Country = null)
    {

        var cIdx = this.Next(0, _city.Length);
        if (Country != null)
        {
            //Give a City in that specific country
            cIdx = Array.FindIndex(_country, c => c.ToLower() == Country.Trim().ToLower());

            if (cIdx == -1) throw new Exception("Country not found");
        }

        return $"{_StreetAddress[cIdx][this.Next(0, _StreetAddress[cIdx].Length)]} {this.Next(1, 51)}";
    }

    public int ZipCode => this.Next(10101, 100000);

    #region Seed from own datastructures
    public TEnum FromEnum<TEnum>() where TEnum : struct
    {
        if (typeof(TEnum).IsEnum)
        {

            var _names = typeof(TEnum).GetEnumNames();
            var _name = _names[this.Next(0, _names.Length)];

            return Enum.Parse<TEnum>(_name);
        }
        throw new ArgumentException("Not an enum type");
    }


        public TItem FromList<TItem>(List<TItem> items)
        {
            return items[this.Next(0, items.Count)];
        }
        
    #endregion

    #region generate seeded Lists
    public List<TItem> ToList<TItem>(int NrOfItems)
        where TItem : ISeed<TItem>, new()
    {
        //Create a list of seeded items
        var _list = new List<TItem>();
        for (int c = 0; c < NrOfItems; c++)
        {
            _list.Add(new TItem().Seed(this));
        }
        return _list;
    }

    public List<TItem> ToListUnique<TItem>(int tryNrOfItems, List<TItem> appendToUnique = null)
         where TItem : ISeed<TItem>, IEquatable<TItem>, new()
    {
        //Create a list of uniquely seeded items
        HashSet<TItem> _set = (appendToUnique == null) ? new HashSet<TItem>() : new HashSet<TItem>(appendToUnique);

        while (_set.Count < tryNrOfItems)
        {
            var _item = new TItem().Seed(this);

            int _preCount = _set.Count();
            int tries = 0;
            do
            {
                _set.Add(_item);
                if (++tries >= 5)
                {
                    //it takes more than 5 tries to generate a random item.
                    //Assume this is it. return the list
                    return _set.ToList();
                }
            } while (!(_set.Count > _preCount));
        }

        return _set.ToList();
    }


        public List<TItem> FromListUnique<TItem>(int tryNrOfItems, List<TItem> list = null)
            where TItem : ISeed<TItem>, IEquatable<TItem>, new()
        {
            //Create a list of uniquely seeded items
            HashSet<TItem> _set = new HashSet<TItem>();

            while (_set.Count < tryNrOfItems)
            {
                var _item = list[this.Next(0, list.Count)];

                int _preCount = _set.Count();
                int tries = 0;
                do
                {
                    _set.Add(_item);
                    if (++tries >= 5)
                    {
                        //it takes more than 5 tries to generate a random item.
                        //Assume this is it. return the list
                        return _set.ToList();
                    }
                } while (!(_set.Count > _preCount));
            }

            return _set.ToList();
        }

        #endregion


        #region Comment
        public List<GoodComment> AllComments => _commentText.ToList<GoodComment>();

        public List<GoodComment> Comments(int NrOfItems)
        {
            //Create a list of seeded items
            var _list = new List<GoodComment>();
            for (int c = 0; c < NrOfItems; c++)
            {
                _list.Add(CommentText);
            }
            return _list;
        }
        public GoodComment CommentText => _commentText[this.Next(0, _commentText.Length)];

        #endregion

        #region Description
        public List<GoodDescription> AllDescriptions => _description.ToList<GoodDescription>();

        public List<GoodDescription> Descriptions(int NrOfItems)
        {
            //Create a list of seeded items
            var _list = new List<GoodDescription>();
            for (int c = 0; c < NrOfItems; c++)
            {
                _list.Add(description);
            }
            return _list;
        }
        public GoodDescription description => _description[this.Next(0, _description.Length)];


        #endregion

    }
}