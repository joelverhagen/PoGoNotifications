using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Knapcode.PoGoNotifications.Logic;
using Knapcode.PoGoNotifications.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Xunit;

namespace Knapcode.PoGoNotifications.Test
{
    public class NotificationOptionsTest
    {
        [Theory]
        [MemberData(nameof(IgnoredPokemonData))]
        public void Sandbox(int id, string name, bool ignoredInSmallestArea, bool ignoredElsewhere)
        {
            // Arrange
            var notificationOptions = GetWebsiteNotificationOptions("dev");
            var testOptions = Configuration.TestOptions;
            var target = new IgnoredPokemonService(Options.Create(notificationOptions));

            var insideSmallestArea = CreateEncounter(id, testOptions.PointInsideSmallestArea);
            var outsideSmallestArea = CreateEncounter(id, testOptions.PointOutsideSmallestArea);
            var farAway = CreateEncounter(id, testOptions.FarAwayPoint);

            // Act & Assert
            if (ignoredInSmallestArea)
            {
                Assert.True(ignoredElsewhere, $"{name} is ignored in the smallest area but not elsewhere. That doesn't make sense.");
            }

            AssertIgnored(target, insideSmallestArea, ignoredInSmallestArea, name, "in the smallest area");
            AssertIgnored(target, outsideSmallestArea, ignoredElsewhere, name, "outside the smallest area");
            AssertIgnored(target, farAway, ignoredElsewhere, name, "far away");
        }

        private static string GetBuiltNotificationOptions(string environment)
        {
            var options = GetWebsiteNotificationOptions(environment);
            var ignoredPokemon = BuildIgnoredPokemon();

            if (ignoredPokemon.Length != options.NotificationAreas.Length)
            {
                throw new InvalidOperationException($"The number of areas should be {ignoredPokemon.Length}.");
            }

            for (int i = 0; i < options.NotificationAreas.Length && i < ignoredPokemon.Length; i++)
            {
                options.NotificationAreas[i].Pokemon = ignoredPokemon[i];
            }

            return JsonConvert.SerializeObject(options, Formatting.Indented);
        }

        private static IgnoredPokemon[][] BuildIgnoredPokemon()
        {
            var elsewhere = new Dictionary<int, IgnoredPokemon>();
            var smallestArea = new Dictionary<int, IgnoredPokemon>();

            foreach (var data in IgnoredPokemonData)
            {
                var id = (int)data[0];
                var name = (string)data[1];
                var ignoredInSmallestArea = (bool)data[2];
                var ignoredElsewhere = (bool)data[3];

                if (ignoredElsewhere)
                {
                    elsewhere[id] = new IgnoredPokemon { Ignore = true, Id = id, Name = name };

                    if (!ignoredInSmallestArea)
                    {
                        smallestArea[id] = new IgnoredPokemon { Ignore = false, Id = id, Name = name };
                    }
                }
            }

            var dictionaries = new[] { elsewhere, smallestArea };

            return dictionaries
                .Select(d => d
                    .OrderBy(x => x.Key)
                    .Select(x => x.Value)
                    .ToArray())
                .ToArray();
        }

        public static IEnumerable<object[]> IgnoredPokemonData
        {
            get
            {
                return new object[][]
                {
                    new object[] { 1, "Bulbasaur", false, false },
                    new object[] { 2, "Ivysaur", false, false },
                    new object[] { 3, "Venusaur", false, false },
                    new object[] { 4, "Charmander", false, false },
                    new object[] { 5, "Charmeleon", false, false },
                    new object[] { 6, "Charizard", false, false },
                    new object[] { 7, "Squirtle", true, true },
                    new object[] { 8, "Wartortle", true, true },
                    new object[] { 9, "Blastoise", true, true },
                    new object[] { 10, "Caterpie", true, true },
                    new object[] { 11, "Metapod", true, true },
                    new object[] { 12, "Butterfree", true, true },
                    new object[] { 13, "Weedle", true, true },
                    new object[] { 14, "Kakuna", true, true },
                    new object[] { 15, "Beedrill", true, true },
                    new object[] { 16, "Pidgey", true, true },
                    new object[] { 17, "Pidgeotto", true, true },
                    new object[] { 18, "Pidgeot", true, true },
                    new object[] { 19, "Rattata", true, true },
                    new object[] { 20, "Raticate", true, true },
                    new object[] { 21, "Spearow", true, true },
                    new object[] { 22, "Fearow", true, true },
                    new object[] { 23, "Ekans", true, true },
                    new object[] { 24, "Arbok", true, true },
                    new object[] { 25, "Pikachu", false, false },
                    new object[] { 26, "Raichu", false, false },
                    new object[] { 27, "Sandshrew", true, true },
                    new object[] { 28, "Sandslash", false, false },
                    new object[] { 29, "Nidoran♀", true, true },
                    new object[] { 30, "Nidorina", true, true },
                    new object[] { 31, "Nidoqueen", false, false },
                    new object[] { 32, "Nidoran♂", true, true },
                    new object[] { 33, "Nidorino", true, true },
                    new object[] { 34, "Nidoking", false, false },
                    new object[] { 35, "Clefairy", true, true },
                    new object[] { 36, "Clefable", false, true },
                    new object[] { 37, "Vulpix", false, false },
                    new object[] { 38, "Ninetales", false, false },
                    new object[] { 39, "Jigglypuff", true, true },
                    new object[] { 40, "Wigglytuff", false, true },
                    new object[] { 41, "Zubat", true, true },
                    new object[] { 42, "Golbat", true, true },
                    new object[] { 43, "Oddish", true, true },
                    new object[] { 44, "Gloom", false, true },
                    new object[] { 45, "Vileplume", false, true },
                    new object[] { 46, "Paras", true, true },
                    new object[] { 47, "Parasect", true, true },
                    new object[] { 48, "Venonat", true, true },
                    new object[] { 49, "Venomoth", true, true },
                    new object[] { 50, "Diglett", false, true },
                    new object[] { 51, "Dugtrio", false, true },
                    new object[] { 52, "Meowth", true, true },
                    new object[] { 53, "Persian", false, true },
                    new object[] { 54, "Psyduck", true, true },
                    new object[] { 55, "Golduck", false, true },
                    new object[] { 56, "Mankey", true, true },
                    new object[] { 57, "Primeape", false, true },
                    new object[] { 58, "Growlithe", false, true },
                    new object[] { 59, "Arcanine", false, false },
                    new object[] { 60, "Poliwag", true, true },
                    new object[] { 61, "Poliwhirl", true, true },
                    new object[] { 62, "Poliwrath", false, false },
                    new object[] { 63, "Abra", false, true },
                    new object[] { 64, "Kadabra", false, true },
                    new object[] { 65, "Alakazam", false, false },
                    new object[] { 66, "Machop", false, true },
                    new object[] { 67, "Machoke", false, true },
                    new object[] { 68, "Machamp", false, false },
                    new object[] { 69, "Bellsprout", true, true },
                    new object[] { 70, "Weepinbell", true, true },
                    new object[] { 71, "Victreebel", true, true },
                    new object[] { 72, "Tentacool", true, true },
                    new object[] { 73, "Tentacruel", false, true },
                    new object[] { 74, "Geodude", false, true },
                    new object[] { 75, "Graveler", false, true },
                    new object[] { 76, "Golem", false, false },
                    new object[] { 77, "Ponyta", false, true },
                    new object[] { 78, "Rapidash", false, true },
                    new object[] { 79, "Slowpoke", true, true },
                    new object[] { 80, "Slowbro", false, true },
                    new object[] { 81, "Magnemite", false, true },
                    new object[] { 82, "Magneton", false, false },
                    new object[] { 83, "Farfetch'd", false, false },
                    new object[] { 84, "Doduo", false, true },
                    new object[] { 85, "Dodrio", false, false },
                    new object[] { 86, "Seel", true, true },
                    new object[] { 87, "Dewgong", true, true },
                    new object[] { 88, "Grimer", false, true },
                    new object[] { 89, "Muk", false, false },
                    new object[] { 90, "Shellder", true, true },
                    new object[] { 91, "Cloyster", true, true },
                    new object[] { 92, "Gastly", true, true },
                    new object[] { 93, "Haunter", true, true },
                    new object[] { 94, "Gengar", false, true },
                    new object[] { 95, "Onix", false, true },
                    new object[] { 96, "Drowzee", true, true },
                    new object[] { 97, "Hypno", true, true },
                    new object[] { 98, "Krabby", true, true },
                    new object[] { 99, "Kingler", true, true },
                    new object[] { 100, "Voltorb", false, true },
                    new object[] { 101, "Electrode", false, false },
                    new object[] { 102, "Exeggcute", false, true },
                    new object[] { 103, "Exeggutor", false, true },
                    new object[] { 104, "Cubone", true, true },
                    new object[] { 105, "Marowak", true, true },
                    new object[] { 106, "Hitmonlee", false, false },
                    new object[] { 107, "Hitmonchan", false, false },
                    new object[] { 108, "Lickitung", true, true },
                    new object[] { 109, "Koffing", true, true },
                    new object[] { 110, "Weezing", false, true },
                    new object[] { 111, "Rhyhorn", true, true },
                    new object[] { 112, "Rhydon", false, true },
                    new object[] { 113, "Chansey", false, false },
                    new object[] { 114, "Tangela", false, false },
                    new object[] { 115, "Kangaskhan", false, false },
                    new object[] { 116, "Horsea", true, true },
                    new object[] { 117, "Seadra", false, true },
                    new object[] { 118, "Goldeen", true, true },
                    new object[] { 119, "Seaking", true, true },
                    new object[] { 120, "Staryu", true, true },
                    new object[] { 121, "Starmie", false, true },
                    new object[] { 122, "Mr. Mime", false, false },
                    new object[] { 123, "Scyther", false, false },
                    new object[] { 124, "Jynx", false, true },
                    new object[] { 125, "Electabuzz", true, true },
                    new object[] { 126, "Magmar", true, true },
                    new object[] { 127, "Pinsir", true, true },
                    new object[] { 128, "Tauros", true, true },
                    new object[] { 129, "Magikarp", true, true },
                    new object[] { 130, "Gyarados", false, false },
                    new object[] { 131, "Lapras", false, false },
                    new object[] { 132, "Ditto", false, false },
                    new object[] { 133, "Eevee", true, true },
                    new object[] { 134, "Vaporeon", false, true },
                    new object[] { 135, "Jolteon", false, true },
                    new object[] { 136, "Flareon", false, true },
                    new object[] { 137, "Porygon", false, false },
                    new object[] { 138, "Omanyte", false, false },
                    new object[] { 139, "Omastar", false, false },
                    new object[] { 140, "Kabuto", false, false },
                    new object[] { 141, "Kabutops", false, false },
                    new object[] { 142, "Aerodactyl", false, false },
                    new object[] { 143, "Snorlax", false, false },
                    new object[] { 144, "Articuno", false, false },
                    new object[] { 145, "Zapdos", false, false },
                    new object[] { 146, "Moltres", false, false },
                    new object[] { 147, "Dratini", false, true },
                    new object[] { 148, "Dragonair", false, true },
                    new object[] { 149, "Dragonite", false, false },
                    new object[] { 150, "Mewtwo", false, false },
                    new object[] { 151, "Mew", false, false }
                };
            }
        }

        private static void AssertIgnored(IgnoredPokemonService target, PokemonEncounter encounter, bool ignored, string name, string label)
        {
            if (ignored)
            {
                Assert.True(target.IsIgnored(encounter), $"{name} should be ignored {label}.");
            }
            else
            {
                Assert.False(target.IsIgnored(encounter), $"{name} should not be ignored {label}.");
            }
        }

        private static PokemonEncounter CreateEncounter(int id, GeoPoint point)
        {
            return new PokemonEncounter
            {
                PokemonId = id,
                Latitude = point.Latitude,
                Longitude = point.Longitude
            };
        }

        private static NotificationOptions GetWebsiteNotificationOptions(string environmentName)
        {
            var options = new NotificationOptions();
            var configuration = GetWebsiteConfiguration(environmentName);
            configuration.Bind(options);
            return options;
        }

        private static IConfigurationRoot GetWebsiteConfiguration(string environmentName)
        {
            var websiteDirectory = GetWebsiteDirectory();
            var builder = new ConfigurationBuilder()
                .SetBasePath(websiteDirectory)
                .AddJsonFile("appsettings.json", optional: true)
                .AddJsonFile($"appsettings.{environmentName}.json", optional: true)
                .AddJsonFile("appsettings.secret.json", optional: true);

            return builder.Build();
        }

        private static string GetWebsiteDirectory()
        {
            return Path.Combine(
                GetRepositoryRoot(),
                "src",
                "Knapcode.PoGoNotifications");
        }

        private static string GetRepositoryRoot()
        {
            var current = Path.GetDirectoryName(Directory.GetCurrentDirectory());

            while (current != null && !DirectoryContains(current, "src"))
            {
                current = Path.GetDirectoryName(current);
            }
            
            return current;
        }

        private static bool DirectoryContains(string directory, string pattern)
        {
            return Directory
                .EnumerateDirectories(directory, pattern, SearchOption.TopDirectoryOnly)
                .Any();
        }
    }
}
