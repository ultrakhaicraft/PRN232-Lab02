using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PRN232.FUNewsManagement.Repo.Migrations
{
    /// <inheritdoc />
    public partial class SeedInitialData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.InsertData(
                table: "Category",
                columns: new[] { "CategoryID", "CategoryDesciption", "CategoryName", "IsActive", "ParentCategoryID" },
                values: new object[,]
                {
                    { (short)1, "This category can include articles about research findings, faculty appointments and promotions, and other academic-related announcements.", "Academic news", true, null },
                    { (short)2, "This category can include articles about student activities, events, and initiatives, such as student clubs, organizations and sports.", "Student Affairs", true, null },
                    { (short)3, "This category can include articles about incidents and safety measures implemented on campus to ensure the safety of students and faculty.", "Campus Safety", true, null },
                    { (short)4, "This category can include articles about the achievements and accomplishments of former students and alumni, such as graduations, job promotions and career successes.", "Alumni News", true, null },
                    { (short)5, "This category is typically a comprehensive and detailed report created as part of an academic or professional capstone project.", "Capstone Project News", false, null },
                    { (short)7, "WOWIE", "WOWIE", true, null },
                    { (short)8, "Topic related to Billard", "Billard", false, null }
                });

            migrationBuilder.InsertData(
                table: "SystemAccount",
                columns: new[] { "AccountID", "AccountEmail", "AccountName", "AccountPassword", "AccountRole", "AccountStatus" },
                values: new object[,]
                {
                    { (short)1, "EmmaWilliam@FUNewsManagement.org", "Emma William", "$2a$12$mPCblJm1hvWDN/suHR8pJulEmqEKt9KI7kKA9B4xP7czAG4JSrPf.", 2, "Active" },
                    { (short)2, "OliviaJames@FUNewsManagement.org", "Olivia James", "$2a$12$mPCblJm1hvWDN/suHR8pJulEmqEKt9KI7kKA9B4xP7czAG4JSrPf.", 2, "Active" },
                    { (short)3, "IsabellaDavid@FUNewsManagement.org", "Isabella David", "$2a$12$mPCblJm1hvWDN/suHR8pJulEmqEKt9KI7kKA9B4xP7czAG4JSrPf.", 1, "Inactive" },
                    { (short)4, "MichaelCharlotte@FUNewsManagement.org", "Michael Charlotte", "$2a$12$mPCblJm1hvWDN/suHR8pJulEmqEKt9KI7kKA9B4xP7czAG4JSrPf.", 1, "Inactive" },
                    { (short)5, "SteveParis@FUNewsManagement.org", "Steve Paris", "$2a$12$mPCblJm1hvWDN/suHR8pJulEmqEKt9KI7kKA9B4xP7czAG4JSrPf.", 1, "Active" },
                    { (short)6, "KhaiPQ2003@gmail.com", "KhaiPQ", "$2a$12$mPCblJm1hvWDN/suHR8pJulEmqEKt9KI7kKA9B4xP7czAG4JSrPf.", 1, "Active" },
                    { (short)7, "KhaiPQ28@gmail.com", "KhaiPQ", "$2a$12$mPCblJm1hvWDN/suHR8pJulEmqEKt9KI7kKA9B4xP7czAG4JSrPf.", 1, "Active" }
                });

            migrationBuilder.InsertData(
                table: "Tag",
                columns: new[] { "TagID", "Note", "TagName", "TagStatus" },
                values: new object[,]
                {
                    { 0, "Sport", "Sport", "Active" },
                    { 1, "Education Note", "Education", "Active" },
                    { 2, "Technology Note", "Technology", "Active" },
                    { 3, "Research Note", "Research", "Active" },
                    { 4, "Innovation Note", "Innovation", "Active" },
                    { 5, "Campus Life Note", "Campus Life", "Active" },
                    { 6, "Faculty Achievements", "Faculty", "Active" },
                    { 7, "Alumni News", "Alumni", "Active" },
                    { 8, "University Events", "Events", "Active" },
                    { 9, "Campus Resources", "Resources", "Active" }
                });

            migrationBuilder.InsertData(
                table: "NewsArticle",
                columns: new[] { "NewsArticleID", "CategoryID", "CreatedByID", "CreatedDate", "Headline", "ModifiedDate", "NewsContent", "NewsSource", "NewsStatus", "NewsTitle", "UpdatedByID" },
                values: new object[,]
                {
                    { "1", (short)4, (short)1, new DateTime(2024, 5, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "University FU Celebrates Success of Alumni in Various Fields", new DateTime(2024, 5, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "N/A", true, "University FU Celebrates Success of Alumni in Various Fields", (short)1 },
                    { "2", (short)4, (short)1, new DateTime(2024, 5, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "Alumni Association Launches Mentorship Program for Recent Graduates", new DateTime(2024, 5, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Internet", true, "Alumni Association Launches Mentorship Program for Recent Graduates", (short)1 },
                    { "3", (short)1, (short)2, new DateTime(2024, 5, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "Academic Department Announces Groundbreaking Initiatives and Program Enhancements", new DateTime(2024, 5, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "N/A", true, "Academic Department Announces Groundbreaking Initiatives and Program Enhancements", (short)2 },
                    { "4", (short)1, (short)2, new DateTime(2024, 5, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "Renowned Scholar Appointed as Head of AI Department at FU", new DateTime(2024, 5, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "N/A", true, "Renowned Scholar Appointed as Head of AI Department at FU", (short)2 },
                    { "5", (short)1, (short)2, new DateTime(2024, 5, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "New Research Findings Shed Light on STEM", new DateTime(2024, 5, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "N/A", true, "New Research Findings Shed Light on STEM", (short)2 },
                    { "50", (short)2, (short)3, new DateTime(2026, 1, 23, 0, 0, 0, 0, DateTimeKind.Unspecified), "OmegaNice", new DateTime(2026, 1, 23, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "OmegaNice", true, "OmegaNice", (short)3 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Category",
                keyColumn: "CategoryID",
                keyValue: (short)3);

            migrationBuilder.DeleteData(
                table: "Category",
                keyColumn: "CategoryID",
                keyValue: (short)5);

            migrationBuilder.DeleteData(
                table: "Category",
                keyColumn: "CategoryID",
                keyValue: (short)7);

            migrationBuilder.DeleteData(
                table: "Category",
                keyColumn: "CategoryID",
                keyValue: (short)8);

            migrationBuilder.DeleteData(
                table: "NewsArticle",
                keyColumn: "NewsArticleID",
                keyValue: "1");

            migrationBuilder.DeleteData(
                table: "NewsArticle",
                keyColumn: "NewsArticleID",
                keyValue: "2");

            migrationBuilder.DeleteData(
                table: "NewsArticle",
                keyColumn: "NewsArticleID",
                keyValue: "3");

            migrationBuilder.DeleteData(
                table: "NewsArticle",
                keyColumn: "NewsArticleID",
                keyValue: "4");

            migrationBuilder.DeleteData(
                table: "NewsArticle",
                keyColumn: "NewsArticleID",
                keyValue: "5");

            migrationBuilder.DeleteData(
                table: "NewsArticle",
                keyColumn: "NewsArticleID",
                keyValue: "50");

            migrationBuilder.DeleteData(
                table: "SystemAccount",
                keyColumn: "AccountID",
                keyValue: (short)4);

            migrationBuilder.DeleteData(
                table: "SystemAccount",
                keyColumn: "AccountID",
                keyValue: (short)5);

            migrationBuilder.DeleteData(
                table: "SystemAccount",
                keyColumn: "AccountID",
                keyValue: (short)6);

            migrationBuilder.DeleteData(
                table: "SystemAccount",
                keyColumn: "AccountID",
                keyValue: (short)7);

            migrationBuilder.DeleteData(
                table: "Tag",
                keyColumn: "TagID",
                keyValue: 0);

            migrationBuilder.DeleteData(
                table: "Tag",
                keyColumn: "TagID",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Tag",
                keyColumn: "TagID",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Tag",
                keyColumn: "TagID",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Tag",
                keyColumn: "TagID",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Tag",
                keyColumn: "TagID",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Tag",
                keyColumn: "TagID",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Tag",
                keyColumn: "TagID",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Tag",
                keyColumn: "TagID",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Tag",
                keyColumn: "TagID",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Category",
                keyColumn: "CategoryID",
                keyValue: (short)1);

            migrationBuilder.DeleteData(
                table: "Category",
                keyColumn: "CategoryID",
                keyValue: (short)2);

            migrationBuilder.DeleteData(
                table: "Category",
                keyColumn: "CategoryID",
                keyValue: (short)4);

            migrationBuilder.DeleteData(
                table: "SystemAccount",
                keyColumn: "AccountID",
                keyValue: (short)1);

            migrationBuilder.DeleteData(
                table: "SystemAccount",
                keyColumn: "AccountID",
                keyValue: (short)2);

            migrationBuilder.DeleteData(
                table: "SystemAccount",
                keyColumn: "AccountID",
                keyValue: (short)3);
        }
    }
}
