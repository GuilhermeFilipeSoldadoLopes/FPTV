CREATE DATABASE FPTV_DB;
use FPTV_DB;

DROP TABLE userTable;
CREATE TABLE UserTable(
	user_Id int primary key,
	user_Type varchar(50) NOT NULL,
	name varchar(50) NOT NULL,
	flag varchar(255),
	biography varchar(500),
	profile_image varchar(255),
	registration_Date datetime NOT NULL
);

CREATE TABLE Pictures(
	picture_Id int primary key,

)

DROP TABLE CustomerPW
CREATE TABLE CustomerPW 
ID INT PRIMARY KEY,
PASSWORD VARCHAR(255)
);

DROP TABLE UserAccount
CREATE TABLE UserAccount(
	user_Account_Id int primary key,
	authentication_Type varchar(50) NOT NULL,
	email varchar(250) NOT NULL unique,
	password varchar(25) NOT NULL,
	validated bit default 0 NOT NULL,
	user_Id int NOT NULL,
	FOREIGN KEY (user_Id) REFERENCES UserTable (user_Id)
);

DROP TABLE LoginLog
CREATE TABLE LoginLog(
	id int primary key,
	authentication_Type varchar(50) NOT NULL,
	date datetime NOT NULL,
	user_Account_id int NOT NULL,
	FOREIGN KEY (user_Account_id) REFERENCES UserAccount (user_Account_Id)
);

CREATE TABLE ErrorLog(
	id int primary key,
	error varchar(250) NOT NULL,
	date datetime NOT NULL,
	user_Id int NOT NULL,
	FOREIGN KEY (user_Id) REFERENCES UserTable (user_Id)
);

CREATE TABLE FavTeamsList(
	id int primary key,
	name varchar(100) NOT NULL,
	team_image varchar(250) NOT NULL,
	user_Id int NOT NULL,
	FOREIGN KEY (user_Id) REFERENCES UserTable (user_Id)
);

CREATE TABLE FavPlayerList(
	id int primary key,
	name varchar(100) NOT NULL,
	team varchar(50) NOT NULL,
	player_image varchar(250) NOT NULL,
	user_Id int NOT NULL,
	FOREIGN KEY (user_Id) REFERENCES UserTable (user_Id)
);

CREATE TABLE Topics(
	topics_id int primary key,
	title varchar(100) NOT NULL,
	content varchar(500) NOT NULL,
	date datetime NOT NULL,
	user_Id int NOT NULL,
	FOREIGN KEY (user_Id) REFERENCES UserTable (user_Id)
);

CREATE TABLE Comments(
	comment_Id int primary key,
	date datetime NOT NULL,
	comment varchar(1000) NOT NULL,
	topics_id int NOT NULL,
	user_Id int NOT NULL,
	FOREIGN KEY (user_Id) REFERENCES UserTable (user_id),
	FOREIGN KEY (topics_id) REFERENCES Topics (topics_id)
);

CREATE TABLE Reactions(
	reactions_Id int primary key,
	reaction varchar(50) NOT NULL,
	user_Id int NOT NULL,
	comment_Id int NOT NULL,
	FOREIGN KEY (user_Id) REFERENCES UserTable (user_Id),
	FOREIGN KEY (comment_Id) REFERENCES Comments (comment_Id)
);

CREATE TABLE Token(
	token_Id int primary key,
	token varchar(30) NOT NULL unique,
	email varchar(250) NOT NULL,
	start_time datetime NOT NULL,
	end_time datetime NOT NULL,
	user_Account_Id int NOT NULL,
	FOREIGN KEY (user_Account_id) REFERENCES UserAccount (user_Account_Id)
);

CREATE TABLE Mail(
	mail_Id int primary key,
	message varchar(500) NOT NULL,
	sender_Mail varchar(250) NOT NULL,
	receiver_Mail varchar(250) NOT NULL,
	sended_date datetime NOT NULL,	
	user_Account_Id int NOT NULL,
	FOREIGN KEY (user_Account_id) REFERENCES UserAccount (user_Account_Id)
);



DROP TABLE IF EXISTS userTable
DROP TABLE IF EXISTS CustomerPW
DROP TABLE IF EXISTS UserAccount
DROP TABLE IF EXISTS LoginLog
DROP TABLE IF EXISTS ErrorLog
DROP TABLE IF EXISTS FavTeamsList
DROP TABLE IF EXISTS FavPlayerList
DROP TABLE IF EXISTS Topics
DROP TABLE IF EXISTS Comments
DROP TABLE IF EXISTS Reactions
DROP TABLE IF EXISTS Token
DROP TABLE IF EXISTS Mail






