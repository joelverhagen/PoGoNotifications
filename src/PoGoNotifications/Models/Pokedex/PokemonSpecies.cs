using System;
using System.Collections.Generic;

namespace Knapcode.PoGoNotifications.Models.Pokedex
{
    public partial class PokemonSpecies
    {
        public PokemonSpecies()
        {
            Pokemon = new HashSet<Pokemon>();
            PokemonSpeciesNames = new HashSet<PokemonSpeciesNames>();
        }

        public int Id { get; set; }
        public string Identifier { get; set; }
        public int? GenerationId { get; set; }
        public int? EvolvesFromSpeciesId { get; set; }
        public int? EvolutionChainId { get; set; }
        public int ColorId { get; set; }
        public int ShapeId { get; set; }
        public int? HabitatId { get; set; }
        public int GenderRate { get; set; }
        public int CaptureRate { get; set; }
        public int BaseHappiness { get; set; }
        public bool IsBaby { get; set; }
        public int HatchCounter { get; set; }
        public bool HasGenderDifferences { get; set; }
        public int GrowthRateId { get; set; }
        public bool FormsSwitchable { get; set; }
        public int Order { get; set; }
        public int? ConquestOrder { get; set; }

        public virtual ICollection<Pokemon> Pokemon { get; set; }
        public virtual ICollection<PokemonSpeciesNames> PokemonSpeciesNames { get; set; }
        public virtual PokemonSpecies EvolvesFromSpecies { get; set; }
        public virtual ICollection<PokemonSpecies> InverseEvolvesFromSpecies { get; set; }
    }
}
