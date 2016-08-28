using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Knapcode.PoGoNotifications.Models.Pokedex
{
    public partial class pokedexContext : DbContext
    {
        public pokedexContext(DbContextOptions<pokedexContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Languages>(entity =>
            {
                entity.ToTable("languages");

                entity.HasIndex(e => e.Official)
                    .HasName("ix_languages_official");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Identifier)
                    .IsRequired()
                    .HasColumnName("identifier")
                    .HasColumnType("varchar")
                    .HasMaxLength(79);

                entity.Property(e => e.Iso3166)
                    .IsRequired()
                    .HasColumnName("iso3166")
                    .HasColumnType("varchar")
                    .HasMaxLength(79);

                entity.Property(e => e.Iso639)
                    .IsRequired()
                    .HasColumnName("iso639")
                    .HasColumnType("varchar")
                    .HasMaxLength(79);

                entity.Property(e => e.Official).HasColumnName("official");

                entity.Property(e => e.Order).HasColumnName("order");
            });

            modelBuilder.Entity<Pokemon>(entity =>
            {
                entity.ToTable("pokemon");

                entity.HasIndex(e => e.IsDefault)
                    .HasName("ix_pokemon_is_default");

                entity.HasIndex(e => e.Order)
                    .HasName("ix_pokemon_order");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.BaseExperience).HasColumnName("base_experience");

                entity.Property(e => e.Height).HasColumnName("height");

                entity.Property(e => e.Identifier)
                    .IsRequired()
                    .HasColumnName("identifier")
                    .HasColumnType("varchar")
                    .HasMaxLength(79);

                entity.Property(e => e.IsDefault).HasColumnName("is_default");

                entity.Property(e => e.Order).HasColumnName("order");

                entity.Property(e => e.SpeciesId).HasColumnName("species_id");

                entity.Property(e => e.Weight).HasColumnName("weight");

                entity.HasOne(d => d.Species)
                    .WithMany(p => p.Pokemon)
                    .HasForeignKey(d => d.SpeciesId)
                    .HasConstraintName("pokemon_species_id_fkey");
            });

            modelBuilder.Entity<PokemonSpecies>(entity =>
            {
                entity.ToTable("pokemon_species");

                entity.HasIndex(e => e.ConquestOrder)
                    .HasName("ix_pokemon_species_conquest_order");

                entity.HasIndex(e => e.Order)
                    .HasName("ix_pokemon_species_order");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.BaseHappiness).HasColumnName("base_happiness");

                entity.Property(e => e.CaptureRate).HasColumnName("capture_rate");

                entity.Property(e => e.ColorId).HasColumnName("color_id");

                entity.Property(e => e.ConquestOrder).HasColumnName("conquest_order");

                entity.Property(e => e.EvolutionChainId).HasColumnName("evolution_chain_id");

                entity.Property(e => e.EvolvesFromSpeciesId).HasColumnName("evolves_from_species_id");

                entity.Property(e => e.FormsSwitchable).HasColumnName("forms_switchable");

                entity.Property(e => e.GenderRate).HasColumnName("gender_rate");

                entity.Property(e => e.GenerationId).HasColumnName("generation_id");

                entity.Property(e => e.GrowthRateId).HasColumnName("growth_rate_id");

                entity.Property(e => e.HabitatId).HasColumnName("habitat_id");

                entity.Property(e => e.HasGenderDifferences).HasColumnName("has_gender_differences");

                entity.Property(e => e.HatchCounter).HasColumnName("hatch_counter");

                entity.Property(e => e.Identifier)
                    .IsRequired()
                    .HasColumnName("identifier")
                    .HasColumnType("varchar")
                    .HasMaxLength(79);

                entity.Property(e => e.IsBaby).HasColumnName("is_baby");

                entity.Property(e => e.Order).HasColumnName("order");

                entity.Property(e => e.ShapeId).HasColumnName("shape_id");

                entity.HasOne(d => d.EvolvesFromSpecies)
                    .WithMany(p => p.InverseEvolvesFromSpecies)
                    .HasForeignKey(d => d.EvolvesFromSpeciesId)
                    .HasConstraintName("pokemon_species_evolves_from_species_id_fkey");
            });

            modelBuilder.Entity<PokemonSpeciesNames>(entity =>
            {
                entity.HasKey(e => new { e.PokemonSpeciesId, e.LocalLanguageId })
                    .HasName("PK_pokemon_species_names");

                entity.ToTable("pokemon_species_names");

                entity.HasIndex(e => e.Name)
                    .HasName("ix_pokemon_species_names_name");

                entity.Property(e => e.PokemonSpeciesId).HasColumnName("pokemon_species_id");

                entity.Property(e => e.LocalLanguageId).HasColumnName("local_language_id");

                entity.Property(e => e.Genus).HasColumnName("genus");

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .HasColumnType("varchar")
                    .HasMaxLength(79);

                entity.HasOne(d => d.LocalLanguage)
                    .WithMany(p => p.PokemonSpeciesNames)
                    .HasForeignKey(d => d.LocalLanguageId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("pokemon_species_names_local_language_id_fkey");

                entity.HasOne(d => d.PokemonSpecies)
                    .WithMany(p => p.PokemonSpeciesNames)
                    .HasForeignKey(d => d.PokemonSpeciesId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("pokemon_species_names_pokemon_species_id_fkey");
            });

            modelBuilder.HasSequence("abilities_id_seq");

            modelBuilder.HasSequence("ability_changelog_id_seq");

            modelBuilder.HasSequence("berries_id_seq");

            modelBuilder.HasSequence("berry_firmness_id_seq");

            modelBuilder.HasSequence("characteristics_id_seq");

            modelBuilder.HasSequence("conquest_episodes_id_seq");

            modelBuilder.HasSequence("conquest_kingdoms_id_seq");

            modelBuilder.HasSequence("conquest_move_displacements_id_seq");

            modelBuilder.HasSequence("conquest_move_effects_id_seq");

            modelBuilder.HasSequence("conquest_move_ranges_id_seq");

            modelBuilder.HasSequence("conquest_stats_id_seq");

            modelBuilder.HasSequence("conquest_warrior_archetypes_id_seq");

            modelBuilder.HasSequence("conquest_warrior_ranks_id_seq");

            modelBuilder.HasSequence("conquest_warrior_skills_id_seq");

            modelBuilder.HasSequence("conquest_warrior_stats_id_seq");

            modelBuilder.HasSequence("conquest_warriors_id_seq");

            modelBuilder.HasSequence("contest_effects_id_seq");

            modelBuilder.HasSequence("contest_types_id_seq");

            modelBuilder.HasSequence("egg_groups_id_seq");

            modelBuilder.HasSequence("encounter_condition_values_id_seq");

            modelBuilder.HasSequence("encounter_conditions_id_seq");

            modelBuilder.HasSequence("encounter_methods_id_seq");

            modelBuilder.HasSequence("encounter_slots_id_seq");

            modelBuilder.HasSequence("encounters_id_seq");

            modelBuilder.HasSequence("evolution_chains_id_seq");

            modelBuilder.HasSequence("evolution_triggers_id_seq");

            modelBuilder.HasSequence("genders_id_seq");

            modelBuilder.HasSequence("generations_id_seq");

            modelBuilder.HasSequence("growth_rates_id_seq");

            modelBuilder.HasSequence("item_categories_id_seq");

            modelBuilder.HasSequence("item_flags_id_seq");

            modelBuilder.HasSequence("item_fling_effects_id_seq");

            modelBuilder.HasSequence("item_pockets_id_seq");

            modelBuilder.HasSequence("items_id_seq");

            modelBuilder.HasSequence("location_areas_id_seq");

            modelBuilder.HasSequence("locations_id_seq");

            modelBuilder.HasSequence("move_battle_styles_id_seq");

            modelBuilder.HasSequence("move_damage_classes_id_seq");

            modelBuilder.HasSequence("move_effect_changelog_id_seq");

            modelBuilder.HasSequence("move_effects_id_seq");

            modelBuilder.HasSequence("move_flags_id_seq");

            modelBuilder.HasSequence("move_targets_id_seq");

            modelBuilder.HasSequence("moves_id_seq");

            modelBuilder.HasSequence("natures_id_seq");

            modelBuilder.HasSequence("pal_park_areas_id_seq");

            modelBuilder.HasSequence("pokeathlon_stats_id_seq");

            modelBuilder.HasSequence("pokedexes_id_seq");

            modelBuilder.HasSequence("pokemon_evolution_id_seq");

            modelBuilder.HasSequence("pokemon_forms_id_seq");

            modelBuilder.HasSequence("pokemon_shapes_id_seq");

            modelBuilder.HasSequence("regions_id_seq");

            modelBuilder.HasSequence("stats_id_seq");

            modelBuilder.HasSequence("super_contest_effects_id_seq");

            modelBuilder.HasSequence("types_id_seq");

            modelBuilder.HasSequence("version_groups_id_seq");

            modelBuilder.HasSequence("versions_id_seq");
        }

        public virtual DbSet<Languages> Languages { get; set; }
        public virtual DbSet<Pokemon> Pokemon { get; set; }
        public virtual DbSet<PokemonSpecies> PokemonSpecies { get; set; }
        public virtual DbSet<PokemonSpeciesNames> PokemonSpeciesNames { get; set; }
    }
}