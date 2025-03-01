using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CinemaApp.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddIsDeletedToCinemaCapsEdit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Cinemas",
                keyColumn: "Id",
                keyValue: new Guid("4edb6046-8057-4075-a44c-d2afe8e15b28"));

            migrationBuilder.DeleteData(
                table: "Cinemas",
                keyColumn: "Id",
                keyValue: new Guid("dacd2d8c-22bf-461f-9178-8b504345b22d"));

            migrationBuilder.DeleteData(
                table: "Cinemas",
                keyColumn: "Id",
                keyValue: new Guid("ef06e69b-a89e-41e0-9897-a223e05450f7"));

            migrationBuilder.DeleteData(
                table: "Movies",
                keyColumn: "Id",
                keyValue: new Guid("749389a4-5ea6-4534-8537-3069142e58e1"));

            migrationBuilder.DeleteData(
                table: "Movies",
                keyColumn: "Id",
                keyValue: new Guid("f831da15-a68a-4908-bc5d-3d0b528b0fe8"));

            migrationBuilder.InsertData(
                table: "Cinemas",
                columns: new[] { "Id", "IsDeleted", "Location", "Name" },
                values: new object[,]
                {
                    { new Guid("4b4f1c05-6f04-4511-a4d2-d050427e4a5c"), false, "Plovdiv", "Cinema city" },
                    { new Guid("92965bb1-73c1-4c04-be18-884a970bf5f1"), false, "Varna", "Cinemax" },
                    { new Guid("bf8dfb7a-f7ae-40e9-b894-db9329287989"), false, "Sofia", "Cinema city" }
                });

            migrationBuilder.InsertData(
                table: "Movies",
                columns: new[] { "Id", "Description", "Director", "Duration", "Genre", "ReleaseDate", "Title" },
                values: new object[,]
                {
                    { new Guid("d721c3ff-501b-4970-913e-754073c16d1b"), "The Lord of the Rings: The Fellowship of the Ring is a 2001 epic high fantasy adventure film directed by Peter Jackson from a screenplay by Fran Walsh, Philippa Boyens, and Jackson, based on 1954's The Fellowship of the Ring, the first volume of the novel The Lord of the Rings by J. R. R. Tolkien. ", "Peter Jackson", 178, "Fantasy", new DateTime(2001, 5, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Lord of the Rings" },
                    { new Guid("fe5d921f-cb7e-41e2-989a-3990eb54e6b0"), "Harry Potter and the Goblet of Fire is a 2005 fantasy film directed by Mike Newell from a screenplay by Steve Kloves. It is based on the 2000 novel Harry Potter and the Goblet of Fire by J. K. Rowling.", "Mike Newel", 157, "Fantasy", new DateTime(2005, 11, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Harry Potter and the Goblet of Fire" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Cinemas",
                keyColumn: "Id",
                keyValue: new Guid("4b4f1c05-6f04-4511-a4d2-d050427e4a5c"));

            migrationBuilder.DeleteData(
                table: "Cinemas",
                keyColumn: "Id",
                keyValue: new Guid("92965bb1-73c1-4c04-be18-884a970bf5f1"));

            migrationBuilder.DeleteData(
                table: "Cinemas",
                keyColumn: "Id",
                keyValue: new Guid("bf8dfb7a-f7ae-40e9-b894-db9329287989"));

            migrationBuilder.DeleteData(
                table: "Movies",
                keyColumn: "Id",
                keyValue: new Guid("d721c3ff-501b-4970-913e-754073c16d1b"));

            migrationBuilder.DeleteData(
                table: "Movies",
                keyColumn: "Id",
                keyValue: new Guid("fe5d921f-cb7e-41e2-989a-3990eb54e6b0"));

            migrationBuilder.InsertData(
                table: "Cinemas",
                columns: new[] { "Id", "IsDeleted", "Location", "Name" },
                values: new object[,]
                {
                    { new Guid("4edb6046-8057-4075-a44c-d2afe8e15b28"), false, "Plovdiv", "Cinema city" },
                    { new Guid("dacd2d8c-22bf-461f-9178-8b504345b22d"), false, "Sofia", "Cinema city" },
                    { new Guid("ef06e69b-a89e-41e0-9897-a223e05450f7"), false, "Varna", "Cinemax" }
                });

            migrationBuilder.InsertData(
                table: "Movies",
                columns: new[] { "Id", "Description", "Director", "Duration", "Genre", "ReleaseDate", "Title" },
                values: new object[,]
                {
                    { new Guid("749389a4-5ea6-4534-8537-3069142e58e1"), "The Lord of the Rings: The Fellowship of the Ring is a 2001 epic high fantasy adventure film directed by Peter Jackson from a screenplay by Fran Walsh, Philippa Boyens, and Jackson, based on 1954's The Fellowship of the Ring, the first volume of the novel The Lord of the Rings by J. R. R. Tolkien. ", "Peter Jackson", 178, "Fantasy", new DateTime(2001, 5, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Lord of the Rings" },
                    { new Guid("f831da15-a68a-4908-bc5d-3d0b528b0fe8"), "Harry Potter and the Goblet of Fire is a 2005 fantasy film directed by Mike Newell from a screenplay by Steve Kloves. It is based on the 2000 novel Harry Potter and the Goblet of Fire by J. K. Rowling.", "Mike Newel", 157, "Fantasy", new DateTime(2005, 11, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Harry Potter and the Goblet of Fire" }
                });
        }
    }
}
