CREATE TABLE Users (
	UserID int NOT NULL,
	Username varchar(MAX) NOT NULL,
	Email varchar(MAX) NOT NULL,

	PRIMARY KEY (UserID),
);

CREATE TABLE Books (
	ISBN varchar(32) NOT NULL,
	Title varchar(MAX) NOT NULL,
	Authors varchar(MAX) NOT NULL,

	PRIMARY KEY (ISBN),
);

CREATE TABLE Items (
	ItemID int NOT NULL,
	ISBN varchar(32) NOT NULL,

	PRIMARY KEY (ItemID),
	FOREIGN KEY (ISBN) REFERENCES Books(ISBN),
);

CREATE TABLE BorrowedItems (
	BorrowID int NOT NULL,
	UserID int NOT NULL,
	ItemID int NOT NULL,
	DueDate Date NOT NULL,

	PRIMARY KEY (BorrowID),
	FOREIGN KEY (UserId) REFERENCES Users(UserId),
	FOREIGN KEY (ItemId) REFERENCES Items(ItemId),
);