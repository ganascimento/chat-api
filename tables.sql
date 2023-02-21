CREATE TABLE [dbo].[TB_ApplicationUser]
(
    [Id] INT NOT NULL PRIMARY KEY IDENTITY,
    [Name] VARCHAR(16) NOT NULL,
    [UserName] NVARCHAR(256) NOT NULL,
    [NormalizedUserName]  NVARCHAR(256) NOT NULL,
    [Email]  NVARCHAR(256) NOT NULL,
    [NormalizedEmail]  NVARCHAR(256) NULL,
    [EmailConfirmed] BIT NOT NULL,
    [PasswordHash] NVARCHAR(MAX) NULL,
    [IsOnline] BIT NULL,
    [ConnectionId] VARCHAR(MAX) NULL,
    [FileNameAvatar] VARCHAR(50) NULL
)

GO

CREATE INDEX [IX_ApplicationUser_NormalizedUserName] ON [dbo].[TB_ApplicationUser] ([NormalizedUserName])

GO

CREATE INDEX [IX_ApplicationUser_NormalizedEmail] ON [dbo].[TB_ApplicationUser] ([NormalizedEmail])

GO

CREATE TABLE [dbo].[TB_ApplicationRole]
(
    [Id] INT NOT NULL PRIMARY KEY IDENTITY,
    [Name] NVARCHAR(256) NOT NULL,
    [NormalizedName]  NVARCHAR(256) NOT NULL
)

GO

CREATE INDEX [IX_ApplicationRole_NormalizedName] ON [dbo].[TB_ApplicationRole] ([NormalizedName])

GO

CREATE TABLE [dbo].[TB_ApplicationUserRole]
(
    [UserId] INT NOT NULL,
    [RoleId] INT NOT NULL,
    PRIMARY KEY ([UserId], [RoleId]),
    CONSTRAINT [FK_ApplicationUserRole_User] FOREIGN KEY ([UserId]) REFERENCES [TB_ApplicationUser]([Id]),
    CONSTRAINT [FK_ApplicationUserRole_Role] FOREIGN KEY ([RoleId]) REFERENCES [TB_ApplicationRole]([Id]),
)

GO

CREATE TABLE [dbo].[TB_Friend]
(
    [UserId] INT NOT NULL FOREIGN KEY ([UserId]) REFERENCES [TB_ApplicationUser]([Id]),
    [FriendId] INT NOT NULL FOREIGN KEY ([FriendId]) REFERENCES [TB_ApplicationUser]([Id]),
    [ConversationId] VARCHAR(36) NOT NULL UNIQUE
)

GO

CREATE TABLE [dbo].[TB_Invitation]
(
    [Id] INT IDENTITY PRIMARY KEY NOT NULL,
    [UserSentId] INT NOT NULL FOREIGN KEY ([UserSentId]) REFERENCES [TB_ApplicationUser]([Id]),
    [UserReceivedId] INT NOT NULL FOREIGN KEY ([UserReceivedId]) REFERENCES [TB_ApplicationUser]([Id]),
    [RequestDate] DATETIME NOT NULL
)

GO

CREATE TABLE [dbo].[TB_Message]
(
    [Id] BIGINT IDENTITY NOT NULL,
    [UserSentId] INT NOT NULL FOREIGN KEY ([UserSentId]) REFERENCES [TB_ApplicationUser]([Id]),
    [ConversationId] VARCHAR(36) NOT NULL,
    [Text] VARCHAR(500) NOT NULL,
    [Pending] BIT NOT NULL,
    [SendDate] DATETIME NOT NULL
)



DROP TABLE TB_Friend
DROP TABLE TB_Invitation
DROP TABLE TB_Message
DROP TABLE TB_ApplicationUserRole
DROP TABLE TB_ApplicationRole
DROP TABLE TB_ApplicationUser