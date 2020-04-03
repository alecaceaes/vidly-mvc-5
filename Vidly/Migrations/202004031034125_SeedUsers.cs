namespace Vidly.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SeedUsers : DbMigration
    {
        public override void Up()
        {
            Sql(@"
            INSERT INTO [dbo].[AspNetUsers] ([Id], [Email], [EmailConfirmed], [PasswordHash], [SecurityStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEndDateUtc], [LockoutEnabled], [AccessFailedCount], [UserName]) VALUES (N'2bee20db-b59e-47b2-8321-358797afaca8', N'admin@vidly.com', 0, N'AKxQKheDdt1WFs1MjIswwXQQKv7aLGHCQHTxtRTyYtuS84384TEYBIBVrb4NE8n3MA==', N'eceedf48-9def-4148-8704-56d17d6bd7b8', NULL, 0, 0, NULL, 1, 0, N'admin@vidly.com')
            INSERT INTO [dbo].[AspNetUsers] ([Id], [Email], [EmailConfirmed], [PasswordHash], [SecurityStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEndDateUtc], [LockoutEnabled], [AccessFailedCount], [UserName]) VALUES (N'd6c2fd3f-755e-47ef-bf83-6dc34d00853f', N'guest@vidly.com', 0, N'AKxQKheDdt1WFs1MjIswwXQQKv7aLGHCQHTxtRTyYtuS84384TEYBIBVrb4NE8n3MA==', N'bde59265-9adc-4fea-958f-9683e76b3226', NULL, 0, 0, NULL, 1, 0, N'guest@vidly.com')
            INSERT INTO [dbo].[AspNetRoles] ([Id], [Name]) VALUES (N'4ec60cac-6536-41d0-841e-546e975a7aaa', N'CanManageMovie')
            INSERT INTO [dbo].[AspNetUserRoles] ([UserId], [RoleId]) VALUES (N'2bee20db-b59e-47b2-8321-358797afaca8', N'4ec60cac-6536-41d0-841e-546e975a7aaa')");
        }
        
        public override void Down()
        {
        }
    }
}
