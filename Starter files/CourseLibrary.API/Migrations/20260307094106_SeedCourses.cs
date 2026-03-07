using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CourseLibrary.API.Migrations
{
    /// <inheritdoc />
    public partial class SeedCourses : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Courses",
                keyColumn: "Id",
                keyValue: new Guid("40ff5488-fdab-45b5-bc3a-14302d59869a"));

            migrationBuilder.DeleteData(
                table: "Courses",
                keyColumn: "Id",
                keyValue: new Guid("5b1c2b4d-48c7-402a-80c3-cc796ad49c6b"));

            migrationBuilder.DeleteData(
                table: "Courses",
                keyColumn: "Id",
                keyValue: new Guid("d173e20d-159e-4127-9ce9-b0ac2564ad97"));

            migrationBuilder.DeleteData(
                table: "Courses",
                keyColumn: "Id",
                keyValue: new Guid("d8663e5e-7494-4f81-8739-6e0de1bea7ee"));

            migrationBuilder.UpdateData(
                table: "Authors",
                keyColumn: "Id",
                keyValue: new Guid("102b566b-ba1f-404c-b2df-e2cde39ade09"),
                column: "DateOfBirth",
                value: 1264248815616000120L);

            migrationBuilder.UpdateData(
                table: "Authors",
                keyColumn: "Id",
                keyValue: new Guid("2902b665-1190-4c70-9915-b9c2d7680450"),
                column: "DateOfBirth",
                value: 1264753115136000120L);

            migrationBuilder.UpdateData(
                table: "Authors",
                keyColumn: "Id",
                keyValue: new Guid("5b3621c0-7b12-4e80-9c8b-3398cba7ee05"),
                column: "DateOfBirth",
                value: 1264066560000000120L);

            migrationBuilder.UpdateData(
                table: "Authors",
                keyColumn: "Id",
                keyValue: new Guid("d28888e9-2ba9-473a-a40f-e38cb54f9b35"),
                column: "DateOfBirth",
                value: 1279360106496000180L);

            migrationBuilder.UpdateData(
                table: "Authors",
                keyColumn: "Id",
                keyValue: new Guid("da2fd609-d754-4feb-8acd-c4f9ff13ba96"),
                column: "DateOfBirth",
                value: 1277955145728000180L);

            migrationBuilder.InsertData(
                table: "Courses",
                columns: new[] { "Id", "AuthorId", "Description", "Title" },
                values: new object[,]
                {
                    { new Guid("10c5e7a1-1b44-4b68-83b8-9d1a2b3c7777"), new Guid("d28888e9-2ba9-473a-a40f-e38cb54f9b35"), "Cannons are powerful tools of persuasion. This course teaches you how to load, aim, and fire cannons without accidentally sinking your own ship.", "Cannon Operation and Safety" },
                    { new Guid("21d6f8b2-2c55-4c79-94c9-a2b3c4d58888"), new Guid("2902b665-1190-4c70-9915-b9c2d7680450"), "Some of the best pirate victories happen under the cover of darkness. Learn stealth boarding techniques and how to move quietly across a creaky deck.", "Night Raids and Silent Boarding" },
                    { new Guid("32e709c3-3d66-4d8a-a5da-b3c4d5e69999"), new Guid("da2fd609-d754-4feb-8acd-c4f9ff13ba96"), "A pirate crew runs on rum. In this course you'll learn the basics of brewing, storing, and protecting your precious barrels from thirsty crewmates.", "Rum Brewing Fundamentals" },
                    { new Guid("43f81ad4-4e77-4e9b-b6eb-c4d5e6f7aaaa"), new Guid("d28888e9-2ba9-473a-a40f-e38cb54f9b35"), "Sometimes things don't go according to plan. Learn essential survival skills for when you're stranded on a deserted island with nothing but a shovel and bad luck.", "Surviving on Deserted Islands" },
                    { new Guid("a3f1b8c0-6c71-4c75-9f3c-9a6c7d2e1111"), new Guid("d28888e9-2ba9-473a-a40f-e38cb54f9b35"), "Every pirate needs to know how to swing a sword without losing a finger. In this course you'll learn the basics of sword fighting and how to look intimidating while doing it.", "Sword Fighting for Beginners" },
                    { new Guid("b7d4f20c-89b4-41c3-92af-1e8a3d7e2222"), new Guid("2902b665-1190-4c70-9915-b9c2d7680450"), "Not all X's mark the spot. Learn how to read treasure maps, decipher pirate symbols, and avoid digging holes in the wrong places.", "Treasure Map Reading 101" },
                    { new Guid("c8a73f95-02c4-4d16-9a9b-3f4b5e9a3333"), new Guid("da2fd609-d754-4feb-8acd-c4f9ff13ba96"), "A good pirate listens to their parrot. In this course you'll learn how to understand, train, and occasionally negotiate with your feathered companion.", "Advanced Parrot Communication" },
                    { new Guid("d91f3c62-8e1a-4a70-b9b7-7c2d6a1b4444"), new Guid("d28888e9-2ba9-473a-a40f-e38cb54f9b35"), "Storms can sink ships and spirits alike. Learn the techniques pirates use to survive raging seas and keep their treasure dry.", "Storm Navigation Mastery" },
                    { new Guid("e24a7d80-ff5e-4e63-8e57-5a8d0c2c5555"), new Guid("2902b665-1190-4c70-9915-b9c2d7680450"), "Burying treasure is easy. Remembering where you buried it is harder. This course covers proper treasure storage, secrecy, and retrieval strategies.", "Hidden Treasure Logistics" },
                    { new Guid("f3b9c1de-22fa-4a0b-96e6-2d9e4f6f6666"), new Guid("da2fd609-d754-4feb-8acd-c4f9ff13ba96"), "Not every encounter needs to end in cannon fire. Learn how pirates negotiate, bluff, and occasionally walk away with more treasure than they started with.", "Pirate Negotiation Tactics" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Courses",
                keyColumn: "Id",
                keyValue: new Guid("10c5e7a1-1b44-4b68-83b8-9d1a2b3c7777"));

            migrationBuilder.DeleteData(
                table: "Courses",
                keyColumn: "Id",
                keyValue: new Guid("21d6f8b2-2c55-4c79-94c9-a2b3c4d58888"));

            migrationBuilder.DeleteData(
                table: "Courses",
                keyColumn: "Id",
                keyValue: new Guid("32e709c3-3d66-4d8a-a5da-b3c4d5e69999"));

            migrationBuilder.DeleteData(
                table: "Courses",
                keyColumn: "Id",
                keyValue: new Guid("43f81ad4-4e77-4e9b-b6eb-c4d5e6f7aaaa"));

            migrationBuilder.DeleteData(
                table: "Courses",
                keyColumn: "Id",
                keyValue: new Guid("a3f1b8c0-6c71-4c75-9f3c-9a6c7d2e1111"));

            migrationBuilder.DeleteData(
                table: "Courses",
                keyColumn: "Id",
                keyValue: new Guid("b7d4f20c-89b4-41c3-92af-1e8a3d7e2222"));

            migrationBuilder.DeleteData(
                table: "Courses",
                keyColumn: "Id",
                keyValue: new Guid("c8a73f95-02c4-4d16-9a9b-3f4b5e9a3333"));

            migrationBuilder.DeleteData(
                table: "Courses",
                keyColumn: "Id",
                keyValue: new Guid("d91f3c62-8e1a-4a70-b9b7-7c2d6a1b4444"));

            migrationBuilder.DeleteData(
                table: "Courses",
                keyColumn: "Id",
                keyValue: new Guid("e24a7d80-ff5e-4e63-8e57-5a8d0c2c5555"));

            migrationBuilder.DeleteData(
                table: "Courses",
                keyColumn: "Id",
                keyValue: new Guid("f3b9c1de-22fa-4a0b-96e6-2d9e4f6f6666"));

            migrationBuilder.UpdateData(
                table: "Authors",
                keyColumn: "Id",
                keyValue: new Guid("102b566b-ba1f-404c-b2df-e2cde39ade09"),
                column: "DateOfBirth",
                value: 1264248815616000060L);

            migrationBuilder.UpdateData(
                table: "Authors",
                keyColumn: "Id",
                keyValue: new Guid("2902b665-1190-4c70-9915-b9c2d7680450"),
                column: "DateOfBirth",
                value: 1264753115136000060L);

            migrationBuilder.UpdateData(
                table: "Authors",
                keyColumn: "Id",
                keyValue: new Guid("5b3621c0-7b12-4e80-9c8b-3398cba7ee05"),
                column: "DateOfBirth",
                value: 1264066560000000060L);

            migrationBuilder.UpdateData(
                table: "Authors",
                keyColumn: "Id",
                keyValue: new Guid("d28888e9-2ba9-473a-a40f-e38cb54f9b35"),
                column: "DateOfBirth",
                value: 1279360106496000120L);

            migrationBuilder.UpdateData(
                table: "Authors",
                keyColumn: "Id",
                keyValue: new Guid("da2fd609-d754-4feb-8acd-c4f9ff13ba96"),
                column: "DateOfBirth",
                value: 1277955145728000120L);

            migrationBuilder.InsertData(
                table: "Courses",
                columns: new[] { "Id", "AuthorId", "Description", "Title" },
                values: new object[,]
                {
                    { new Guid("40ff5488-fdab-45b5-bc3a-14302d59869a"), new Guid("2902b665-1190-4c70-9915-b9c2d7680450"), "In this course you'll learn how to sing all-time favourite pirate songs without sounding like you actually know the words or how to hold a note.", "Singalong Pirate Hits" },
                    { new Guid("5b1c2b4d-48c7-402a-80c3-cc796ad49c6b"), new Guid("d28888e9-2ba9-473a-a40f-e38cb54f9b35"), "Commandeering a ship in rough waters isn't easy.  Commandeering it without getting caught is even harder.  In this course you'll learn how to sail away and avoid those pesky musketeers.", "Commandeering a Ship Without Getting Caught" },
                    { new Guid("d173e20d-159e-4127-9ce9-b0ac2564ad97"), new Guid("da2fd609-d754-4feb-8acd-c4f9ff13ba96"), "Every good pirate loves rum, but it also has a tendency to get you into trouble.  In this course you'll learn how to avoid that.  This new exclusive edition includes an additional chapter on how to run fast without falling while drunk.", "Avoiding Brawls While Drinking as Much Rum as You Desire" },
                    { new Guid("d8663e5e-7494-4f81-8739-6e0de1bea7ee"), new Guid("d28888e9-2ba9-473a-a40f-e38cb54f9b35"), "In this course, the author provides tips to avoid, or, if needed, overthrow pirate mutiny.", "Overthrowing Mutiny" }
                });
        }
    }
}
