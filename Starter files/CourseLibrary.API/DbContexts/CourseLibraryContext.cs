using CourseLibrary.API.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace CourseLibrary.API.DbContexts;

public class CourseLibraryContext : DbContext
{
    public CourseLibraryContext(DbContextOptions<CourseLibraryContext> options)
       : base(options)
    {
    }

    // base DbContext constructor ensures that Books and Authors are not null after
    // having been constructed.  Compiler warning ("uninitialized non-nullable property")
    // can safely be ignored with the "null-forgiving operator" (= null!)

    public DbSet<Author> Authors { get; set; } = null!;
    public DbSet<Course> Courses { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // seed the database with dummy data
        modelBuilder.Entity<Author>().HasData(
            new Author("Berry", "Griffin Beak Eldritch", "Ships")
            {
                Id = Guid.Parse("d28888e9-2ba9-473a-a40f-e38cb54f9b35"),
                DateOfBirth = new DateTime(1980, 7, 23)
            },
            new Author("Nancy", "Swashbuckler Rye", "Rum")
            {
                Id = Guid.Parse("da2fd609-d754-4feb-8acd-c4f9ff13ba96"),
                DateOfBirth = new DateTime(1978, 5, 21)
            },
            new Author("Eli", "Ivory Bones Sweet", "Singing")
            {
                Id = Guid.Parse("2902b665-1190-4c70-9915-b9c2d7680450"),
                DateOfBirth = new DateTime(1957, 12, 16)
            },
            new Author("Arnold", "The Unseen Stafford", "Singing")
            {
                Id = Guid.Parse("102b566b-ba1f-404c-b2df-e2cde39ade09"),
                DateOfBirth = new DateTime(1957, 3, 6)
            },
            new Author("Seabury", "Toxic Reyson", "Maps")
            {
                Id = Guid.Parse("5b3621c0-7b12-4e80-9c8b-3398cba7ee05"),
                DateOfBirth = new DateTime(1956, 11, 23)
            },
            new Author("Rutherford", "Fearless Cloven", "General debauchery")
            {
                Id = Guid.Parse("2aadd2df-7caf-45ab-9355-7f6332985a87"),
                DateOfBirth = new DateTime(1981, 4, 5)
            },
            new Author("Atherton", "Crow Ridley", "Rum")
            {
                Id = Guid.Parse("2ee49fe3-edf2-4f91-8409-3eb25ce6ca51"),
                DateOfBirth = new DateTime(1982, 10, 11)
            }
            );

        modelBuilder.Entity<Course>().HasData(
            new Course("Sword Fighting for Beginners")
            {
                Id = Guid.Parse("a3f1b8c0-6c71-4c75-9f3c-9a6c7d2e1111"),
                AuthorId = Guid.Parse("d28888e9-2ba9-473a-a40f-e38cb54f9b35"),
                Description = "Every pirate needs to know how to swing a sword without losing a finger. In this course you'll learn the basics of sword fighting and how to look intimidating while doing it."
            },
            new Course("Treasure Map Reading 101")
            {
                Id = Guid.Parse("b7d4f20c-89b4-41c3-92af-1e8a3d7e2222"),
                AuthorId = Guid.Parse("2902b665-1190-4c70-9915-b9c2d7680450"),
                Description = "Not all X's mark the spot. Learn how to read treasure maps, decipher pirate symbols, and avoid digging holes in the wrong places."
            },
            new Course("Advanced Parrot Communication")
            {
                Id = Guid.Parse("c8a73f95-02c4-4d16-9a9b-3f4b5e9a3333"),
                AuthorId = Guid.Parse("da2fd609-d754-4feb-8acd-c4f9ff13ba96"),
                Description = "A good pirate listens to their parrot. In this course you'll learn how to understand, train, and occasionally negotiate with your feathered companion."
            },
            new Course("Storm Navigation Mastery")
            {
                Id = Guid.Parse("d91f3c62-8e1a-4a70-b9b7-7c2d6a1b4444"),
                AuthorId = Guid.Parse("d28888e9-2ba9-473a-a40f-e38cb54f9b35"),
                Description = "Storms can sink ships and spirits alike. Learn the techniques pirates use to survive raging seas and keep their treasure dry."
            },
            new Course("Hidden Treasure Logistics")
            {
                Id = Guid.Parse("e24a7d80-ff5e-4e63-8e57-5a8d0c2c5555"),
                AuthorId = Guid.Parse("2902b665-1190-4c70-9915-b9c2d7680450"),
                Description = "Burying treasure is easy. Remembering where you buried it is harder. This course covers proper treasure storage, secrecy, and retrieval strategies."
            },
            new Course("Pirate Negotiation Tactics")
            {
                Id = Guid.Parse("f3b9c1de-22fa-4a0b-96e6-2d9e4f6f6666"),
                AuthorId = Guid.Parse("da2fd609-d754-4feb-8acd-c4f9ff13ba96"),
                Description = "Not every encounter needs to end in cannon fire. Learn how pirates negotiate, bluff, and occasionally walk away with more treasure than they started with."
            },
            new Course("Cannon Operation and Safety")
            {
                Id = Guid.Parse("10c5e7a1-1b44-4b68-83b8-9d1a2b3c7777"),
                AuthorId = Guid.Parse("d28888e9-2ba9-473a-a40f-e38cb54f9b35"),
                Description = "Cannons are powerful tools of persuasion. This course teaches you how to load, aim, and fire cannons without accidentally sinking your own ship."
            },
            new Course("Night Raids and Silent Boarding")
            {
                Id = Guid.Parse("21d6f8b2-2c55-4c79-94c9-a2b3c4d58888"),
                AuthorId = Guid.Parse("2902b665-1190-4c70-9915-b9c2d7680450"),
                Description = "Some of the best pirate victories happen under the cover of darkness. Learn stealth boarding techniques and how to move quietly across a creaky deck."
            },
            new Course("Rum Brewing Fundamentals")
            {
                Id = Guid.Parse("32e709c3-3d66-4d8a-a5da-b3c4d5e69999"),
                AuthorId = Guid.Parse("da2fd609-d754-4feb-8acd-c4f9ff13ba96"),
                Description = "A pirate crew runs on rum. In this course you'll learn the basics of brewing, storing, and protecting your precious barrels from thirsty crewmates."
            },
            new Course("Surviving on Deserted Islands")
            {
                Id = Guid.Parse("43f81ad4-4e77-4e9b-b6eb-c4d5e6f7aaaa"),
                AuthorId = Guid.Parse("d28888e9-2ba9-473a-a40f-e38cb54f9b35"),
                Description = "Sometimes things don't go according to plan. Learn essential survival skills for when you're stranded on a deserted island with nothing but a shovel and bad luck."
            }
        );

        // fix to allow sorting on DateTimeOffset when using Sqlite, based on
        // https://blog.dangl.me/archive/handling-datetimeoffset-in-sqlite-with-entity-framework-core/
        if (Database.ProviderName == "Microsoft.EntityFrameworkCore.Sqlite")
        {
            // Sqlite does not have proper support for DateTimeOffset via Entity Framework Core, see the limitations
            // here: https://docs.microsoft.com/en-us/ef/core/providers/sqlite/limitations#query-limitations
            // To work around this, when the Sqlite database provider is used, all model properties of type DateTimeOffset
            // use the DateTimeOffsetToBinaryConverter
            // Based on: https://github.com/aspnet/EntityFrameworkCore/issues/10784#issuecomment-415769754 
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                var properties = entityType.ClrType.GetProperties()
                    .Where(p => p.PropertyType == typeof(DateTimeOffset)
                        || p.PropertyType == typeof(DateTimeOffset?));
                foreach (var property in properties)
                {
                    modelBuilder.Entity(entityType.Name)
                        .Property(property.Name)
                        .HasConversion(new DateTimeOffsetToBinaryConverter());
                }
            }
        }

        base.OnModelCreating(modelBuilder);
    }
}


