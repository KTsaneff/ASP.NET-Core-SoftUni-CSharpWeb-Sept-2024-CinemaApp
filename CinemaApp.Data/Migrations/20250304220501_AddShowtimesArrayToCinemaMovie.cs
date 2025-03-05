using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CinemaApp.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddShowtimesArrayToCinemaMovie : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Cinemas",
                keyColumn: "Id",
                keyValue: new Guid("1e115b7a-a0f7-4a3f-b042-3e8712668a80"));

            migrationBuilder.DeleteData(
                table: "Cinemas",
                keyColumn: "Id",
                keyValue: new Guid("46060314-fb71-4625-8aad-cb8a7fceea23"));

            migrationBuilder.DeleteData(
                table: "Cinemas",
                keyColumn: "Id",
                keyValue: new Guid("af081410-f61c-4cd0-a345-7bf2d43463f4"));

            migrationBuilder.DeleteData(
                table: "Movies",
                keyColumn: "Id",
                keyValue: new Guid("5eed4ed5-6a3b-44cb-9379-6484c987044b"));

            migrationBuilder.DeleteData(
                table: "Movies",
                keyColumn: "Id",
                keyValue: new Guid("7f5c5851-05da-4115-bef6-9559f6cddfe4"));

            migrationBuilder.AddColumn<string>(
                name: "Showtimes",
                table: "CinemasMovies",
                type: "varchar(5)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.InsertData(
                table: "Cinemas",
                columns: new[] { "Id", "IsDeleted", "Location", "Name" },
                values: new object[,]
                {
                    { new Guid("6deca6e8-b26a-4039-8425-af293e05016e"), false, "Plovdiv", "Cinema city" },
                    { new Guid("9bfac3ce-0d30-4518-a990-0a3875121170"), false, "Sofia", "Cinema city" },
                    { new Guid("c81759cc-881f-4b6c-8706-01e4c0661854"), false, "Varna", "Cinemax" }
                });

            migrationBuilder.InsertData(
                table: "Movies",
                columns: new[] { "Id", "Description", "Director", "Duration", "Genre", "IsDeleted", "ReleaseDate", "Title" },
                values: new object[,]
                {
                    { new Guid("61ed8eb2-fd98-4dad-a4aa-f6fc2ac68c2e"), "Harry Potter and the Goblet of Fire is a 2005 fantasy film directed by Mike Newell from a screenplay by Steve Kloves. It is based on the 2000 novel Harry Potter and the Goblet of Fire by J. K. Rowling.", "Mike Newel", 157, "Fantasy", false, new DateTime(2005, 11, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Harry Potter and the Goblet of Fire" },
                    { new Guid("972e145c-8b58-4d7b-a4b0-4843f92cdb00"), "The Lord of the Rings: The Fellowship of the Ring is a 2001 epic high fantasy adventure film directed by Peter Jackson from a screenplay by Fran Walsh, Philippa Boyens, and Jackson, based on 1954's The Fellowship of the Ring, the first volume of the novel The Lord of the Rings by J. R. R. Tolkien. ", "Peter Jackson", 178, "Fantasy", false, new DateTime(2001, 5, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Lord of the Rings" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Cinemas",
                keyColumn: "Id",
                keyValue: new Guid("6deca6e8-b26a-4039-8425-af293e05016e"));

            migrationBuilder.DeleteData(
                table: "Cinemas",
                keyColumn: "Id",
                keyValue: new Guid("9bfac3ce-0d30-4518-a990-0a3875121170"));

            migrationBuilder.DeleteData(
                table: "Cinemas",
                keyColumn: "Id",
                keyValue: new Guid("c81759cc-881f-4b6c-8706-01e4c0661854"));

            migrationBuilder.DeleteData(
                table: "Movies",
                keyColumn: "Id",
                keyValue: new Guid("61ed8eb2-fd98-4dad-a4aa-f6fc2ac68c2e"));

            migrationBuilder.DeleteData(
                table: "Movies",
                keyColumn: "Id",
                keyValue: new Guid("972e145c-8b58-4d7b-a4b0-4843f92cdb00"));

            migrationBuilder.DropColumn(
                name: "Showtimes",
                table: "CinemasMovies");

            migrationBuilder.InsertData(
                table: "Cinemas",
                columns: new[] { "Id", "IsDeleted", "Location", "Name" },
                values: new object[,]
                {
                    { new Guid("1e115b7a-a0f7-4a3f-b042-3e8712668a80"), false, "Sofia", "Cinema city" },
                    { new Guid("46060314-fb71-4625-8aad-cb8a7fceea23"), false, "Varna", "Cinemax" },
                    { new Guid("af081410-f61c-4cd0-a345-7bf2d43463f4"), false, "Plovdiv", "Cinema city" }
                });

            migrationBuilder.InsertData(
                table: "Movies",
                columns: new[] { "Id", "Description", "Director", "Duration", "Genre", "IsDeleted", "ReleaseDate", "Title" },
                values: new object[,]
                {
                    { new Guid("5eed4ed5-6a3b-44cb-9379-6484c987044b"), "The Lord of the Rings: The Fellowship of the Ring is a 2001 epic high fantasy adventure film directed by Peter Jackson from a screenplay by Fran Walsh, Philippa Boyens, and Jackson, based on 1954's The Fellowship of the Ring, the first volume of the novel The Lord of the Rings by J. R. R. Tolkien. ", "Peter Jackson", 178, "Fantasy", false, new DateTime(2001, 5, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Lord of the Rings" },
                    { new Guid("7f5c5851-05da-4115-bef6-9559f6cddfe4"), "Harry Potter and the Goblet of Fire is a 2005 fantasy film directed by Mike Newell from a screenplay by Steve Kloves. It is based on the 2000 novel Harry Potter and the Goblet of Fire by J. K. Rowling.", "Mike Newel", 157, "Fantasy", false, new DateTime(2005, 11, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Harry Potter and the Goblet of Fire" }
                });
        }
    }
}
