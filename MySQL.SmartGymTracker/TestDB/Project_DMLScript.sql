-- ---------------------------------------------------
-- USERS
-- ---------------------------------------------------
INSERT INTO users (username, password, firstName, lastName, email, phoneNumber, dateOfBirth, gender, privilegeLevel, active)
VALUES
('mc11', 'pwdMC', 'Matthew',  'Cegala',      'mc@fsu.edu',   '111-111-1111', '1990-01-01', 'Male',   'Admin', 1),
('ao22', 'pwdAO', 'Amanda',   'Orama',       'ao@fsu.edu',   '222-222-2222', '1991-02-02', 'Female', 'Admin', 1),
('nh33', 'pwdNH', 'Nicholas', 'Holguin',     'nh@fsu.edu',   '333-333-3333', '1992-03-03', 'Male',   'User',  1),
('as44', 'pwdAS', 'Ashton',   'Singpradith', 'as@fsu.edu',   '444-444-4444', '1993-04-04', 'Male',   'User',  0),
('mh55', 'pwdMH', 'Matthew',  'Hummel',      'mh@fsu.edu',   '555-555-5555', '1994-05-05', 'Male',   'User',  0),
('md66', 'pwdMD', 'Moose',    'Drool',       'md@gmail.com', '666-666-6666', '1995-06-06', 'Male',   'Admin', 1),
('aa77', 'pwdAA', 'Amber',    'Ale',         'aa@aol.com',   '777-777-7777', '1996-07-07', 'Female', 'User',  1),
('le88', 'pwdLE', 'Leo',      'EvilEyes',    'le@yahoo.com', '888-888-8888', '1997-08-08', 'Male',   'User',  1),
('ml99', 'pwdML', 'Mi',       'Lo',          'ml@mail.com',  '999-999-9999', '1998-09-09', 'Male',   'User',  1),
('tp00', 'pwdTP', 'Tom',      'Platz',       'tp@gmail.com', '000-000-0000', '1999-10-10', 'Female', 'User',  1);



-- ---------------------------------------------------
-- GOAL
-- ---------------------------------------------------
INSERT INTO goal (userId, timeCreated, title, description, startDate, targetEndDate, Status)
VALUES
(1,  NOW(),                 'Squat',      '500lbs Squat',    '2025-11-16', '2026-12-31', 'In_Progress'),
(2,  '2025-10-28 12:31:00', 'Marathon',   'Run a marathon',  '2025-10-29', '2026-02-14', 'In_Progress'),
(10, '1975-06-07 12:00:00', 'Train',      'Win Mr. Olympia', '1977-10-21', '1990-10-21', 'Failed'     ),
(1,  '2020-02-20 12:00:00', 'Train',      '225 Squat',       '2020-02-20', '2020-07-15', 'Completed'  ),
(1,  NOW(),                 'Hack Squat', '4 Plate Hack',    '2025-11-15', '2026-08-20', 'Not_Started'),
(6,  NOW(),                 'Run Mile',   'desc',            '2025-11-15', '2026-08-20', 'Not_Started'),
(7,  NOW(),                 'Bench',      'desc',            '2025-11-15', '2026-08-20', 'Not_Started'),
(8,  NOW(),                 'Walk',       'desc',            '2025-11-15', '2026-08-20', 'Not_Started'),
(9,  NOW(),                 'Run Mile',   'desc',            '2025-11-15', '2026-08-20', 'Not_Started'),
(1,  NOW(),                 'Do Cardio',  'desc',            '2025-11-15', '2026-08-20', 'Failed'     );



-- ---------------------------------------------------
-- BIOMETRICS
-- ---------------------------------------------------
INSERT INTO biometrics (userId, dateEntered, weight, height, bodyFatPercentage, bmi, restingHeartRate)
VALUES
(1,  '2020-02-20', 160, 72, 15,  19.5, 60),
(2,  '2025-11-15', 500, 80, 0.3, 20,   67),
(10, '1980-10-21', 220, 70, 5.5, 24,   62),
(9,  '2025-11-15', 20,  12, 6.7, 20,   55),
(1,  '2023-06-07', 180, 73, 22,  23.1, 85),
(6,  '2025-11-15', 80,  36, 15,  21.1, 63),
(7,  '2025-11-15', 100, 34, 24,  22.2, 64),
(8,  '2025-11-15', 800, 24, 14,  26,   99),
(10, '2025-11-15', 170, 72, 15,  22,   85),
(1,  '2025-11-15', 180, 73, 12,  21,   67);



-- ---------------------------------------------------
-- MESSAGES
-- ---------------------------------------------------
INSERT INTO messages (title, details, timeCreated, type)
VALUES
('System Update',     'Increment 1 Release',    '2025-10-01 23:59:00', 'System'   ),
('System Update',     'Increment 2 Release',    '2025-11-01 23:59:00', 'System'   ),
('System Update',     'Increment 3 Release',    '2025-12-01 23:59:00', 'System'   ),
('Milestone Login',   'Your account is setup',  NOW(),                 'Milestone'),
('Tip of the Day',    'Setup your biometrics',  NOW(),                 'Tip'      ),
('Tip of the Day',    'Log your workouts',      NOW(),                 'Tip'      ),
('Data Trend',        'You have 5 day streak!', NOW(),                 'Trend'    ),
('You made a goal',   'Congrats',               NOW(),                 'Goal'     ),
('You made progress', 'Good Job',               NOW(),                 'Goal'     ),
('To User: mc11',     'Welcome!',               NOW(),                 'Specific' );



-- ---------------------------------------------------
-- NOTIFICATION
-- ---------------------------------------------------
INSERT INTO notification (userId, messageId, timeSent, isRead)
VALUES
(1, 1,  '2025-10-02 00:00:01', 1),
(1, 2,  '2025-11-02 00:00:01', 1),
(1, 3,  '2025-12-02 00:00:01', 0),
(1, 10, NOW(),                 1),
(2, 3,  NOW(),                 0),
(2, 5,  NOW(),                 1),
(3, 6,  NOW(),                 0),
(4, 7,  NOW(),                 1),
(5, 8,  NOW(),                 0),
(6, 9,  NOW(),                 1);



-- ---------------------------------------------------
-- WORKOUTTYPE
-- ---------------------------------------------------
INSERT INTO workout_type (name, description, difficulty)
VALUES
('Push',     'Chest, Shoulders, Triceps', 'Medium'),
('Pull',     'Back, Bicepts',             'Medium'),
('Legs',     'Quads, Hamstrings, Calves', 'Hard'),
('Cardio',   'Cardiovascular work',       'Impossible'),
('Pilates',  'desc',                      'Medium'),
('Crossfit', 'desc',                      'Hard'),
('Running',  'desc',                      'Medium'),
('Cycling',  'desc',                      'Medium'),
('Mobility', 'desc',                      'Easy'),
('Arms',     'Triceps, Biceps',           'Easy');



-- ---------------------------------------------------
-- WORKOUT
-- ---------------------------------------------------
INSERT INTO workout (userId, workoutStart, duration, notes)
VALUES
(1,  '2025-10-01 23:59:00', 30, 'mnotes'),
(1,  NOW(),                 40, 'notes' ),
(2,  NOW(),                 25, 'notes' ),
(1,  NOW(),                 50, 'notes' ),
(1,  NOW(),                 60, 'notes' ),
(6,  NOW(),                 20, 'notes' ),
(7,  NOW(),                 45, 'notes' ),
(8,  NOW(),                 55, 'notes' ),
(9,  NOW(),                 35, 'notes' ),
(10, NOW(),                 65, 'tnotes');



-- ---------------------------------------------------
-- WORKOUT TYPES PER WORKOUT
-- ---------------------------------------------------
INSERT INTO workout_types_per_workout (workoutTypeId, workoutId)
VALUES
(1,1),
(2,4),
(1,2),
(1,3),
(1,4),
(2,3),
(1,6),
(1,7),
(1,8),
(1,9);



-- ---------------------------------------------------
-- WORKOUT BIOMETRICS
-- ---------------------------------------------------
INSERT INTO workout_biometrics (workoutId, steps, averageHeartRate, maxHeartRate, caloriesBurned, feeling, sleepScore)
VALUES
(1,  5000, 120, 160, 300, 'Good',  80),
(2,  3500, 110, 150, 250, 'Okay',  75),
(3,  4000, 115, 155, 260, 'Good',  78),
(4,  6000, 130, 170, 350, 'Great', 82),
(5,  4500, 118, 165, 280, 'Good',  79),
(6,  3000, 105, 145, 220, 'Tired', 70),
(7,  5500, 125, 168, 330, 'Great', 83),
(8,  5000, 119, 162, 290, 'Good',  77),
(9,  4800, 121, 160, 310, 'Good',  81),
(10, 6200, 135, 175, 360, 'Great', 85);



-- ---------------------------------------------------
-- MUSCLE
-- ---------------------------------------------------
INSERT INTO muscle (name, description)
VALUES
('Chest',     'desc'),
('Back',      'desc'),
('Legs',      'desc'),
('Arms',      'desc'),
('Shoulders', 'desc'),
('Core',      'desc'),
('Glutes',    'desc'),
('Calves',    'desc'),
('Traps',     'desc'),
('Forearms',  'desc');



-- ---------------------------------------------------
-- EXERCISE
-- ---------------------------------------------------
INSERT INTO exercise (muscleId, exerciseName, description)
VALUES
(1, 'Bench Press',    'desc'),
(2, 'Deadlift',       'desc'),
(3, 'Squat',          'desc'),
(4, 'Bicep Curl',     'desc'),
(5, 'Overhead Press', 'desc'),
(6, 'Plank',          'desc'),
(7, 'Hip Thrust',     'desc'),
(3, 'Lunge',          'desc'),
(2, 'Pull-up',        'desc'),
(6, 'Crunch',         'desc');



-- ---------------------------------------------------
-- EXERCISE SET
-- ---------------------------------------------------
INSERT INTO exercise_set (workoutId, exerciseId, notes, setType)
VALUES
(1, 1,  'notes', 'Strength'),
(1, 4,  'notes', 'Strength'),
(2, 2,  'notes', 'Strength'),
(3, 3,  'notes', 'Strength'),
(4, 5,  'notes', 'Strength'),
(5, 6,  'notes', 'Cardio'),
(6, 7,  'notes', 'Strength'),
(7, 8,  'notes', 'Strength'),
(8, 9,  'notes', 'Strength'),
(9, 10, 'notes', 'Cardio');



-- ---------------------------------------------------
-- STRENGTH SETS
-- ---------------------------------------------------
INSERT INTO strength_set (exerciseSetId, setNumber, weight, reps)
VALUES
(1, 1, 100, 8),
(2, 1, 20,  12),
(3, 1, 150, 5),
(4, 1, 120, 10),
(5, 1, 60,  8),
(7, 1, 180, 6),
(8, 1, 30,  15),
(9, 1, 0,   12),
(1, 2, 105, 7),
(3, 2, 160, 4);

-- ---------------------------------------------------
-- CARDIO SETS
-- ---------------------------------------------------
INSERT INTO cardio_set (exerciseSetId, duration, distance)
VALUES
(1,  10, 2.0),
(2,  12, 2.5),
(3,  8,  1.6),
(4,  9,  2.1),
(5,  7,  1.4),
(6,  11, 2.4),
(7,  9,  1.8),
(8,  13, 2.6),
(9,  6,  1.2),
(10, 14, 2.8);