INSERT INTO se.applicants (ApplicantId, FirstName, LastName, ApplyingPosition, PictureCode, Address, Flag, EmailAddress, PhoneNumber, DateOfBirth, DateOfEntry)
VALUES (1, 'Callum', 'Lowley', 'Designer', '', '', 1, 'callum.highley@live.co.uk', '07847521030', 839346665, 9012016);

INSERT INTO se.positions (Position, Seats)
VALUES
  ('Janitor', 7),
  ('Developer', 3),
  ('CEO', 1),
  ('Designer', 2),
  ('Barista', 2),
  ('Manager', 2);

INSERT INTO se.question_answers (Answer_ID, Question_ID, Value, Weight)
VALUES (1, 1, 'No it indangers all employees ', 1);

INSERT INTO se.questions (Question_ID, Question, Category)
VALUES (1, 'Is it safe to sleep on the job', 'Health & Safety');