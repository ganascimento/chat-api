CREATE PROCEDURE SP_GET_MESSAGES
(
		@UserId INT
)

AS

CREATE TABLE #TEMP
(
		[MessageId] BIGINT,
		[UserSentId] INT,
		[ConversationId] VARCHAR(36),
		[Text] VARCHAR(500),
		[Pending] BIT,
		[SendDate] DATETIME
)

CREATE TABLE #TEMP_ID_MESSAGES
(
		[MessageId] INT
)

CREATE TABLE #TEMP_ID_CONVERSATION
(
		[ConversationId] VARCHAR(36)
)

CREATE TABLE #TEMP_FRIENDS
(
		[FriendId] INT,
		[ConversationId] VARCHAR(36)
)

INSERT	INTO #TEMP_ID_CONVERSATION
(
		[ConversationId]
)
SELECT	[TB_Message].[ConversationId]
FROM	[TB_Message] WITH (NOLOCK)
INNER	JOIN [TB_Friend] WITH (NOLOCK)
ON		[TB_Message].[ConversationId] = [TB_Friend].[ConversationId]
WHERE	[TB_Friend].[UserId] = @UserId OR 
		[TB_Friend].[FriendId] = @UserId
ORDER	BY [TB_Message].[SendDate] DESC

DECLARE @IdConversation VARCHAR(MAX)
DECLARE MY_CURSOR CURSOR
FOR		SELECT	DISTINCT TOP 8
				[ConversationId]
		FROM	#TEMP_ID_CONVERSATION

OPEN	MY_CURSOR
FETCH	NEXT FROM MY_CURSOR INTO @IdConversation
WHILE	@@FETCH_STATUS = 0
BEGIN
		INSERT	INTO #TEMP_ID_MESSAGES
		(
				[MessageId]
		)
		SELECT	TOP 20
				[Id]
		FROM	[TB_Message] WITH (NOLOCK)
		WHERE	[ConversationId] = @IdConversation
		ORDER	BY [SendDate] DESC

		FETCH	NEXT FROM MY_CURSOR INTO @IdConversation
END
CLOSE MY_CURSOR
DEALLOCATE MY_CURSOR

INSERT	INTO #TEMP
(
		[MessageId],
		[UserSentId],
		[ConversationId],
		[Text],
		[Pending],
		[SendDate]
)
SELECT	[Id],
		[UserSentId],
		[ConversationId],
		[Text],
		[Pending],
		[SendDate]
FROM	[TB_Message] WITH (NOLOCK)
WHERE   [Id] IN (SELECT	* FROM	#TEMP_ID_MESSAGES)

INSERT	INTO #TEMP_FRIENDS
(
		[FriendId],
		[ConversationId]
)
SELECT	[UserId],
		[ConversationId]
FROM	[TB_Friend] WITH (NOLOCK)
WHERE	[FriendId] = @UserId

INSERT	INTO #TEMP_FRIENDS
(
		[FriendId],
		[ConversationId]
)
SELECT	[FriendId],
		[ConversationId]
FROM	[TB_Friend] WITH (NOLOCK)
WHERE	[UserId] = @UserId

SELECT	* 
FROM	#TEMP
ORDER	BY [MessageId] ASC

SELECT	[Id],
		[Name],
		[IsOnline],
		[FileNameAvatar],
		[Email]
FROM	[TB_ApplicationUser] WITH (NOLOCK)
WHERE	[Id] IN
		(
				SELECT	[FriendId]
				FROM	#TEMP_FRIENDS
		)

SELECT	*
FROM	#TEMP_FRIENDS

SELECT	[Id],
		[UserSentId]
FROM	[TB_Invitation]
WHERE	[UserReceivedId] = @UserId

SELECT	[Id],
		[Name],
		[FileNameAvatar]
FROM	[TB_ApplicationUser]
WHERE	[Id] IN
		(
				SELECT	[UserSentId]
				FROM	[TB_Invitation]
				WHERE	[UserReceivedId] = @UserId
		)