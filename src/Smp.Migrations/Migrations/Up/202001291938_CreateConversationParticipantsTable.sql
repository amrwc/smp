IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'ConversationParticipants')
BEGIN
	CREATE TABLE ConversationParticipants (
		ConversationId UNIQUEIDENTIFIER NOT NULL,
		UserId UNIQUEIDENTIFIER NOT NULL,
		CreatedAt DATETIME NOT NULL,

		CONSTRAINT PK_ConversationParticipants PRIMARY KEY (ConversationId, UserId),
		CONSTRAINT FK_ConversationParticipants_UserId_Users_Id FOREIGN KEY (UserId) REFERENCES Users(Id)
	);
END