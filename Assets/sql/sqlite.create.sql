CREATE TABLE applicants (
    ApplicantId INTEGER PRIMARY KEY AUTOINCREMENT,
    FirstName VARCHAR(32),
    LastName VARCHAR(32),
    ApplyingPosition VARCHAR(32),
    PictureCode LONGTEXT,
    Address VARCHAR(300),
    Flag INT(11),
    EmailAddress VARCHAR(64),
    PhoneNumber VARCHAR(16),
    DateOfBirth INT(11),
    DateOfEntry INT(11)
);

CREATE TABLE interview (
    Interview_ID INTEGER PRIMARY KEY AUTOINCREMENT,
    Foundation_ID INT(11),
    Flag INT(11),
    Result INT(11),
    Answers LONGTEXT,
    Applicant_Id INT(11)
);

CREATE TABLE interview_foundation (
    Foundation_ID INTEGER PRIMARY KEY AUTOINCREMENT,
    Name VARCHAR(16),
    Category VARCHAR(64),
    Position VARCHAR(64)
);

CREATE TABLE interview_questions (
    RowIndex INTEGER PRIMARY KEY AUTOINCREMENT,
    Foundation_ID INT(11),
    Question_ID INT(11),
    Weight INT(11),
	Position VARCHAR(64)
);

CREATE TABLE positions (
    ID INTEGER PRIMARY KEY NOT NULL AUTOINCREMENT,
    Position TEXT,
    Seats INT(11)
);

CREATE TABLE question_answers (
    Answer_ID INTEGER PRIMARY KEY AUTOINCREMENT,
    Question_ID INT(11),
    Value VARCHAR(32),
    Weight INT(11)
);

CREATE TABLE questions (
    Question_ID INTEGER PRIMARY KEY AUTOINCREMENT,
    Question VARCHAR(64),
    Category VARCHAR(64)
);