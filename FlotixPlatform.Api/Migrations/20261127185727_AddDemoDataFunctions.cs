using FlotixPlatform.Api.DemoData.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FlotixPlatform.Api.Migrations
{
    /// <summary>
    ///     Installs the demo data PL/pgSQL functions (<c>DemoData/Scripts/flotix_demo_data.sql</c>) and generates
    ///     about 90 days (3 months) of demo history for every account that already exists. New accounts receive it at
    ///     sign-up (<c>DemoData:SeedOnSignUp</c>). The data model does not change.
    /// </summary>
    public partial class AddDemoDataFunctions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(DemoDataScript.Read());

            // Backfill: accounts created before this migration. A failure in one account is reported as a warning and
            // never stops the migration (see flotix_seed_demo_data_for_all_users).
            migrationBuilder.Sql("SELECT public.flotix_seed_demo_data_for_all_users(90);");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""
                                 SELECT public.flotix_clear_demo_data(user_id)
                                 FROM (SELECT DISTINCT user_id FROM public.demo_data_rows) AS accounts;

                                 DROP FUNCTION IF EXISTS public.flotix_clear_demo_data(integer);
                                 DROP FUNCTION IF EXISTS public.flotix_seed_demo_data_for_all_users(integer);
                                 DROP FUNCTION IF EXISTS public.flotix_seed_demo_data(integer, integer);
                                 DROP FUNCTION IF EXISTS public.flotix_demo_person_id(integer, text, integer);
                                 DROP FUNCTION IF EXISTS public.flotix_demo_owner_id(integer);
                                 DROP TABLE IF EXISTS public.demo_data_rows;
                                 """);
        }
    }
}