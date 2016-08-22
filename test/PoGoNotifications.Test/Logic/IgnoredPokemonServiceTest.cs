using System.Collections.Generic;
using System.Linq;
using Knapcode.PoGoNotifications.Logic;
using Knapcode.PoGoNotifications.Models;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;

namespace Knapcode.PoGoNotifications.Test.Logic
{
    public class IgnoredPokemonServiceTest
    {
        private const int PokemonId1 = 1;
        private const int PokemonId2 = 2;

        [Fact]
        public void IsIgnored_DoesNotIgnoreWhentThereAreNoAreas()
        {
            // Arrange
            var options = GetOptions(Enumerable.Empty<NotificationAreaOptions>());
            var encounter = GetEncounterInBothRectangles(PokemonId1);
            var target = new IgnoredPokemonService(options);

            // Act
            var ignored = target.IsIgnored(encounter);

            // Assert
            Assert.False(ignored);
        }

        [Fact]
        public void IsIgnored_IgnoresPokemonInIgnoredList()
        {
            // Arrange
            var options = GetOptions(new[]
            {
                new NotificationAreaOptions
                {
                    Pokemon = new[] { new IgnoredPokemon { Ignore = true, Id = PokemonId1 } },
                    Polygon = GetSmallRectanglePoints()
                }
            });
            var encounter = GetEncounterInBothRectangles(PokemonId1);
            var target = new IgnoredPokemonService(options);

            // Act
            var ignored = target.IsIgnored(encounter);

            // Assert
            Assert.True(ignored);
        }

        [Fact]
        public void IsIgnored_DoesNotIgnorePokemonNotInIgnoredList()
        {
            // Arrange
            var options = GetOptions(new[]
            {
                new NotificationAreaOptions
                {
                    Pokemon = new[] { new IgnoredPokemon { Ignore = true, Id = PokemonId1 } },
                    Polygon = GetSmallRectanglePoints()
                }
            });
            var encounter = GetEncounterInBothRectangles(PokemonId2);
            var target = new IgnoredPokemonService(options);

            // Act
            var ignored = target.IsIgnored(encounter);

            // Assert
            Assert.False(ignored);
        }

        [Fact]
        public void IsIgnored_DoesNotIgnorePokemonIgnoredInFirstAreaButNotSecond()
        {
            // Arrange
            var options = GetOptions(new[]
            {
                new NotificationAreaOptions
                {
                    Pokemon = new[] { new IgnoredPokemon { Ignore = true, Id = PokemonId1 } },
                    Polygon = GetSmallRectanglePoints()
                },
                new NotificationAreaOptions
                {
                    Pokemon = new[] { new IgnoredPokemon { Ignore = false, Id = PokemonId1 } },
                    Polygon = GetLargeRectanglePoints()
                },
            });
            var encounter = GetEncounterInBothRectangles(PokemonId1);
            var target = new IgnoredPokemonService(options);

            // Act
            var ignored = target.IsIgnored(encounter);

            // Assert
            Assert.False(ignored);
        }

        [Fact]
        public void IsIgnored_IgnoresPokemonNotIgnoredInFirstAreaButIgnoredInSecond()
        {
            // Arrange
            var options = GetOptions(new[]
            {
                new NotificationAreaOptions
                {
                    Pokemon = new[] { new IgnoredPokemon { Ignore = false, Id = PokemonId1 } },
                    Polygon = GetSmallRectanglePoints()
                },
                new NotificationAreaOptions
                {
                    Pokemon = new[] { new IgnoredPokemon { Ignore = true, Id = PokemonId1 } },
                    Polygon = GetLargeRectanglePoints()
                },
            });
            var encounter = GetEncounterInBothRectangles(PokemonId1);
            var target = new IgnoredPokemonService(options);

            // Act
            var ignored = target.IsIgnored(encounter);

            // Assert
            Assert.True(ignored);
        }

        [Fact]
        public void IsIgnored_DoesNotIgnorePokemonOutsideOfAllAreas()
        {
            // Arrange
            var options = GetOptions(new[]
            {
                new NotificationAreaOptions
                {
                    Pokemon = new[] { new IgnoredPokemon { Ignore = true, Id = PokemonId1 } },
                    Polygon = GetSmallRectanglePoints()
                },
                new NotificationAreaOptions
                {
                    Pokemon = new[] { new IgnoredPokemon { Ignore = true, Id = PokemonId1 } },
                    Polygon = GetLargeRectanglePoints()
                },
            });
            var encounter = GetEncounterOutsideOfBothRectangles(PokemonId1);
            var target = new IgnoredPokemonService(options);

            // Act
            var ignored = target.IsIgnored(encounter);

            // Assert
            Assert.False(ignored);
        }

        [Fact]
        public void IsIgnored_IgnoresPokemonIgnoredInFirstAreaButNotMentionedInSecondArea()
        {
            // Arrange
            var options = GetOptions(new[]
            {
                new NotificationAreaOptions
                {
                    Pokemon = new[] { new IgnoredPokemon { Ignore = true, Id = PokemonId1 } },
                    Polygon = GetSmallRectanglePoints()
                },
                new NotificationAreaOptions
                {
                    Pokemon = new IgnoredPokemon[0],
                    Polygon = GetLargeRectanglePoints()
                },
            });
            var encounter = GetEncounterOnlyInLargeRectangle(PokemonId1);
            var target = new IgnoredPokemonService(options);

            // Act
            var ignored = target.IsIgnored(encounter);

            // Assert
            Assert.True(ignored);
        }

        [Fact]
        public void IsIgnored_IgnoresPokemonIgnoredInFirstAreaButNotMentionedAndNotInSecondArea()
        {
            // Arrange
            var options = GetOptions(new[]
            {
                new NotificationAreaOptions
                {
                    Pokemon = new[] { new IgnoredPokemon { Ignore = true, Id = PokemonId1 } },
                    Polygon = GetLargeRectanglePoints()
                },
                new NotificationAreaOptions
                {
                    Pokemon = new IgnoredPokemon[0],
                    Polygon = GetSmallRectanglePoints()
                },
            });
            var encounter = GetEncounterOnlyInLargeRectangle(PokemonId1);
            var target = new IgnoredPokemonService(options);

            // Act
            var ignored = target.IsIgnored(encounter);

            // Assert
            Assert.True(ignored);
        }

        [Fact]
        public void IsIgnored_IgnoresPokemonIgnoredInFirstAreaButNotIgnoredAndNotInSecondArea()
        {
            // Arrange
            var options = GetOptions(new[]
            {
                new NotificationAreaOptions
                {
                    Pokemon = new[] { new IgnoredPokemon { Ignore = true, Id = PokemonId1 } },
                    Polygon = GetLargeRectanglePoints()
                },
                new NotificationAreaOptions
                {
                    Pokemon = new[] { new IgnoredPokemon { Ignore = false, Id = PokemonId1 } },
                    Polygon = GetSmallRectanglePoints()
                },
            });
            var encounter = GetEncounterOnlyInLargeRectangle(PokemonId1);
            var target = new IgnoredPokemonService(options);

            // Act
            var ignored = target.IsIgnored(encounter);

            // Assert
            Assert.True(ignored);
        }

        private GeoPoint[] GetSmallRectanglePoints()
        {
            return new[]
            {
                new GeoPoint { Latitude = 0, Longitude = 0 },
                new GeoPoint { Latitude = 0, Longitude = 2 },
                new GeoPoint { Latitude = 1, Longitude = 2 },
                new GeoPoint { Latitude = 1, Longitude = 0 }
            };
        }

        private GeoPoint[] GetLargeRectanglePoints()
        {
            return new[]
            {
                new GeoPoint { Latitude = 0, Longitude = 0 },
                new GeoPoint { Latitude = 0, Longitude = 2 },
                new GeoPoint { Latitude = 4, Longitude = 2 },
                new GeoPoint { Latitude = 4, Longitude = 0 }
            };
        }

        private PokemonEncounter GetEncounterInBothRectangles(int pokemonId)
        {
            return new PokemonEncounter
            {
                Latitude = 0.5,
                Longitude = 1.5,
                PokemonId = pokemonId
            };
        }

        private PokemonEncounter GetEncounterOnlyInLargeRectangle(int pokemonId)
        {
            return new PokemonEncounter
            {
                Latitude = 3,
                Longitude = 1,
                PokemonId = pokemonId
            };
        }

        private PokemonEncounter GetEncounterOutsideOfBothRectangles(int pokemonId)
        {
            return new PokemonEncounter
            {
                Latitude = 0.5,
                Longitude = 3,
                PokemonId = pokemonId
            };
        }

        private IOptions<NotificationOptions> GetOptions(IEnumerable<NotificationAreaOptions> areaOptions)
        {
            var options = new Mock<IOptions<NotificationOptions>>();
            options
                .Setup(x => x.Value)
                .Returns(() => new NotificationOptions
                {
                    NotificationAreas = areaOptions.ToArray()
                });

            return options.Object;
        }
    }
}
