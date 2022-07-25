using Microsoft.EntityFrameworkCore.Migrations;
using System;

#nullable disable

namespace ECAS.AdministrationService.Migrations;

public partial class Initialization : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        _ = migrationBuilder.CreateTable(
            name: "AbpAuditLogs",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uuid", nullable: false),
                ApplicationName = table.Column<string>(type: "character varying(96)", maxLength: 96, nullable: true),
                UserId = table.Column<Guid>(type: "uuid", nullable: true),
                UserName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                TenantId = table.Column<Guid>(type: "uuid", nullable: true),
                TenantName = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true),
                ImpersonatorUserId = table.Column<Guid>(type: "uuid", nullable: true),
                ImpersonatorUserName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                ImpersonatorTenantId = table.Column<Guid>(type: "uuid", nullable: true),
                ImpersonatorTenantName = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true),
                ExecutionTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                ExecutionDuration = table.Column<int>(type: "integer", nullable: false),
                ClientIpAddress = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true),
                ClientName = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true),
                ClientId = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true),
                CorrelationId = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true),
                BrowserInfo = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: true),
                HttpMethod = table.Column<string>(type: "character varying(16)", maxLength: 16, nullable: true),
                Url = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                Exceptions = table.Column<string>(type: "text", nullable: true),
                Comments = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                HttpStatusCode = table.Column<int>(type: "integer", nullable: true),
                ExtraProperties = table.Column<string>(type: "text", nullable: true),
                ConcurrencyStamp = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: true)
            },
            constraints: table =>
            {
                _ = table.PrimaryKey("PK_AbpAuditLogs", x => x.Id);
            });

        _ = migrationBuilder.CreateTable(
            name: "AbpFeatureValues",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uuid", nullable: false),
                Name = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                Value = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                ProviderName = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true),
                ProviderKey = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true)
            },
            constraints: table =>
            {
                _ = table.PrimaryKey("PK_AbpFeatureValues", x => x.Id);
            });

        _ = migrationBuilder.CreateTable(
            name: "AbpPermissionGrants",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uuid", nullable: false),
                TenantId = table.Column<Guid>(type: "uuid", nullable: true),
                Name = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                ProviderName = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                ProviderKey = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false)
            },
            constraints: table =>
            {
                _ = table.PrimaryKey("PK_AbpPermissionGrants", x => x.Id);
            });

        _ = migrationBuilder.CreateTable(
            name: "AbpSettings",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uuid", nullable: false),
                Name = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                Value = table.Column<string>(type: "character varying(2048)", maxLength: 2048, nullable: false),
                ProviderName = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true),
                ProviderKey = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true)
            },
            constraints: table =>
            {
                _ = table.PrimaryKey("PK_AbpSettings", x => x.Id);
            });

        _ = migrationBuilder.CreateTable(
            name: "AbpAuditLogActions",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uuid", nullable: false),
                TenantId = table.Column<Guid>(type: "uuid", nullable: true),
                AuditLogId = table.Column<Guid>(type: "uuid", nullable: false),
                ServiceName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                MethodName = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true),
                Parameters = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                ExecutionTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                ExecutionDuration = table.Column<int>(type: "integer", nullable: false),
                ExtraProperties = table.Column<string>(type: "text", nullable: true)
            },
            constraints: table =>
            {
                _ = table.PrimaryKey("PK_AbpAuditLogActions", x => x.Id);
                _ = table.ForeignKey(
                    name: "FK_AbpAuditLogActions_AbpAuditLogs_AuditLogId",
                    column: x => x.AuditLogId,
                    principalTable: "AbpAuditLogs",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        _ = migrationBuilder.CreateTable(
            name: "AbpEntityChanges",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uuid", nullable: false),
                AuditLogId = table.Column<Guid>(type: "uuid", nullable: false),
                TenantId = table.Column<Guid>(type: "uuid", nullable: true),
                ChangeTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                ChangeType = table.Column<byte>(type: "smallint", nullable: false),
                EntityTenantId = table.Column<Guid>(type: "uuid", nullable: true),
                EntityId = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                EntityTypeFullName = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                ExtraProperties = table.Column<string>(type: "text", nullable: true)
            },
            constraints: table =>
            {
                _ = table.PrimaryKey("PK_AbpEntityChanges", x => x.Id);
                _ = table.ForeignKey(
                    name: "FK_AbpEntityChanges_AbpAuditLogs_AuditLogId",
                    column: x => x.AuditLogId,
                    principalTable: "AbpAuditLogs",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        _ = migrationBuilder.CreateTable(
            name: "AbpEntityPropertyChanges",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uuid", nullable: false),
                TenantId = table.Column<Guid>(type: "uuid", nullable: true),
                EntityChangeId = table.Column<Guid>(type: "uuid", nullable: false),
                NewValue = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: true),
                OriginalValue = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: true),
                PropertyName = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                PropertyTypeFullName = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false)
            },
            constraints: table =>
            {
                _ = table.PrimaryKey("PK_AbpEntityPropertyChanges", x => x.Id);
                _ = table.ForeignKey(
                    name: "FK_AbpEntityPropertyChanges_AbpEntityChanges_EntityChangeId",
                    column: x => x.EntityChangeId,
                    principalTable: "AbpEntityChanges",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        _ = migrationBuilder.CreateIndex(
            name: "IX_AbpAuditLogActions_AuditLogId",
            table: "AbpAuditLogActions",
            column: "AuditLogId");

        _ = migrationBuilder.CreateIndex(
            name: "IX_AbpAuditLogActions_TenantId_ServiceName_MethodName_Executio~",
            table: "AbpAuditLogActions",
            columns: new[] { "TenantId", "ServiceName", "MethodName", "ExecutionTime" });

        _ = migrationBuilder.CreateIndex(
            name: "IX_AbpAuditLogs_TenantId_ExecutionTime",
            table: "AbpAuditLogs",
            columns: new[] { "TenantId", "ExecutionTime" });

        _ = migrationBuilder.CreateIndex(
            name: "IX_AbpAuditLogs_TenantId_UserId_ExecutionTime",
            table: "AbpAuditLogs",
            columns: new[] { "TenantId", "UserId", "ExecutionTime" });

        _ = migrationBuilder.CreateIndex(
            name: "IX_AbpEntityChanges_AuditLogId",
            table: "AbpEntityChanges",
            column: "AuditLogId");

        _ = migrationBuilder.CreateIndex(
            name: "IX_AbpEntityChanges_TenantId_EntityTypeFullName_EntityId",
            table: "AbpEntityChanges",
            columns: new[] { "TenantId", "EntityTypeFullName", "EntityId" });

        _ = migrationBuilder.CreateIndex(
            name: "IX_AbpEntityPropertyChanges_EntityChangeId",
            table: "AbpEntityPropertyChanges",
            column: "EntityChangeId");

        _ = migrationBuilder.CreateIndex(
            name: "IX_AbpFeatureValues_Name_ProviderName_ProviderKey",
            table: "AbpFeatureValues",
            columns: new[] { "Name", "ProviderName", "ProviderKey" },
            unique: true);

        _ = migrationBuilder.CreateIndex(
            name: "IX_AbpPermissionGrants_TenantId_Name_ProviderName_ProviderKey",
            table: "AbpPermissionGrants",
            columns: new[] { "TenantId", "Name", "ProviderName", "ProviderKey" },
            unique: true);

        _ = migrationBuilder.CreateIndex(
            name: "IX_AbpSettings_Name_ProviderName_ProviderKey",
            table: "AbpSettings",
            columns: new[] { "Name", "ProviderName", "ProviderKey" },
            unique: true);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        _ = migrationBuilder.DropTable(
            name: "AbpAuditLogActions");

        _ = migrationBuilder.DropTable(
            name: "AbpEntityPropertyChanges");

        _ = migrationBuilder.DropTable(
            name: "AbpFeatureValues");

        _ = migrationBuilder.DropTable(
            name: "AbpPermissionGrants");

        _ = migrationBuilder.DropTable(
            name: "AbpSettings");

        _ = migrationBuilder.DropTable(
            name: "AbpEntityChanges");

        _ = migrationBuilder.DropTable(
            name: "AbpAuditLogs");
    }
}

