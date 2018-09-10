CREATE TABLE Books (
	BookId int NOT NULL IDENTITY(1,1),
	ISBN varchar(32) NOT NULL,
	Title varchar(MAX) NOT NULL,
	Authors varchar(MAX) NOT NULL,

	PRIMARY KEY (BookID),
);

CREATE TABLE Items (
	ItemId int NOT NULL IDENTITY(1,1),
	BookId int NOT NULL,

	PRIMARY KEY (ItemId),
	FOREIGN KEY (BookId) REFERENCES Books(BookId),
);

CREATE TABLE BorrowedItems (
	BorrowId int NOT NULL IDENTITY(1,1),
	UserId nvarchar(128) NOT NULL,
	ItemId int NOT NULL,
	DueDate Date NOT NULL,

	PRIMARY KEY (BorrowId),
	FOREIGN KEY (UserId) REFERENCES AspNetUsers(Id),
	FOREIGN KEY (ItemId) REFERENCES Items(ItemId),
);

INSERT INTO Books (ISBN, Title, Authors)
VALUES
('111111', 'Snivy is the best pokemon: A thesis', 'Matthew Langford'),
('222222', 'All my friends are dead', 'Mr. Stegosaurus'),
('333333', 'How I learned to stop worrying and love the SQL', 'A. Anderson, B. Banderson'),
('444444', 'CAKE', 'Mr. Miss');

INSERT INTO Items (BookId)
VALUES
(1),(1),(1),(1),(2),(3),(3),(3),(4),(4);