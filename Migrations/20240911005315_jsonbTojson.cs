
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace momentum_api.Migrations
{
    /// <inheritdoc />
    public partial class jsonbTojson : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Goal>(
                name: "Goal",
                table: "GoalDocs",
                type: "json",
                nullable: true,
                oldClrType: typeof(Goal),
                oldType: "jsonb",
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Goal>(
                name: "Goal",
                table: "GoalDocs",
                type: "jsonb",
                nullable: true,
                oldClrType: typeof(Goal),
                oldType: "json",
                oldNullable: true);
        }
    }
}
