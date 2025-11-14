-----------------------------------------------------
-- USERS (parent for many tables)
-----------------------------------------------------
INSERT INTO Users (userId, username, password, firstName, lastName, email, phoneNumber, dateOfBirth, gender, privilegeLevel, active)
VALUES
(1,'user1','pass','John','Doe','john1@mail.com','111-111','1990-01-01','M','User',1),
(2,'user2','pass','Jane','Smith','jane2@mail.com','222-222','1991-02-02','F','User',1),
(3,'user3','pass','Mike','Johnson','mike3@mail.com','333-333','1992-03-03','M','Admin',1),
(4,'user4','pass','Anna','Brown','anna4@mail.com','444-444','1993-04-04','F','User',1),
(5,'user5','pass','Tom','Wilson','tom5@mail.com','555-555','1994-05-05','M','User',1),
(6,'user6','pass','Eva','Taylor','eva6@mail.com','666-666','1995-06-06','F','User',1),
(7,'user7','pass','Ryan','Moore','ryan7@mail.com','777-777','1996-07-07','M','User',1),
(8,'user8','pass','Kate','White','kate8@mail.com','888-888','1997-08-08','F','User',1),
(9,'user9','pass','Sam','Lee','sam9@mail.com','999-999','1998-09-09','M','User',1),
(10,'user10','pass','Lily','Hall','lily10@mail.com','000-000','1999-10-10','F','User',1);

-----------------------------------------------------
-- GOAL
-----------------------------------------------------
INSERT INTO Goal (goalId, userId, timeCreated, title, description, startDate, targetEndDate, Status)
VALUES
(1,1,NOW(),'Goal1','desc',NOW(),NOW(),'Not_Started'),
(2,1,NOW(),'Goal2','desc',NOW(),NOW(),'In_Progress'),
(3,2,NOW(),'Goal3','desc',NOW(),NOW(),'Completed'),
(4,2,NOW(),'Goal4','desc',NOW(),NOW(),'Failed'),
(5,3,NOW(),'Goal5','desc',NOW(),NOW(),'In_Progress'),
(6,3,NOW(),'Goal6','desc',NOW(),NOW(),'Not_Started'),
(7,4,NOW(),'Goal7','desc',NOW(),NOW(),'Completed'),
(8,5,NOW(),'Goal8','desc',NOW(),NOW(),'Failed'),
(9,5,NOW(),'Goal9','desc',NOW(),NOW(),'In_Progress'),
(10,6,NOW(),'Goal10','desc',NOW(),NOW(),'Not_Started');

-----------------------------------------------------
-- BIOMETRICS
-----------------------------------------------------
INSERT INTO Biometrics (biometricId, userId, dateEntered, weight, height, bodyFatPercentage, bmi, restingHeartRate)
VALUES
(1,1,NOW(),75,180,18,23,60),
(2,1,NOW(),76,180,17,24,58),
(3,2,NOW(),65,165,20,22,62),
(4,3,NOW(),85,190,15,24,55),
(5,4,NOW(),70,170,22,25,68),
(6,5,NOW(),73,175,19,24,63),
(7,6,NOW(),60,160,21,23,64),
(8,7,NOW(),90,195,14,26,57),
(9,8,NOW(),55,158,23,22,71),
(10,9,NOW(),80,185,16,23,59);

-----------------------------------------------------
-- MESSAGES
-----------------------------------------------------
INSERT INTO Messages (messageId, title, details, timeCreated, type)
VALUES
(1,'System Update','Details..',NOW(),'System'),
(2,'Goal Tips','Details..',NOW(),'Goal'),
(3,'Trend','Details..',NOW(),'Trend'),
(4,'Milestone','Details..',NOW(),'Milestone'),
(5,'Tip','Details..',NOW(),'Tip'),
(6,'Specific','Details..',NOW(),'Specific'),
(7,'System2','Details..',NOW(),'System'),
(8,'Goal2','Details..',NOW(),'Goal'),
(9,'Trend2','Details..',NOW(),'Trend'),
(10,'Tip2','Details..',NOW(),'Tip');

-----------------------------------------------------
-- NOTIFICATION
-----------------------------------------------------
INSERT INTO Notification (userId, messageId, timeSent, read)
VALUES
(1,1,NOW(),0),
(1,2,NOW(),1),
(2,3,NOW(),0),
(2,4,NOW(),1),
(3,5,NOW(),0),
(4,6,NOW(),1),
(5,7,NOW(),0),
(5,8,NOW(),1),
(6,9,NOW(),0),
(7,10,NOW(),1);

-----------------------------------------------------
-- WORKOUTTYPE
-----------------------------------------------------
INSERT INTO WorkoutType (workoutTypeId, name, description, difficulty)
VALUES
(1,'Strength','desc','Hard'),
(2,'Cardio','desc','Medium'),
(3,'HIIT','desc','Hard'),
(4,'Yoga','desc','Easy'),
(5,'Pilates','desc','Medium'),
(6,'Crossfit','desc','Hard'),
(7,'Running','desc','Medium'),
(8,'Cycling','desc','Medium'),
(9,'Mobility','desc','Easy'),
(10,'Boxing','desc','Hard');

-----------------------------------------------------
-- WORKOUT
-----------------------------------------------------
INSERT INTO Workout (workoutId, userId, workoutStart, duration, notes)
VALUES
(1,1,NOW(),30,'notes'),
(2,1,NOW(),40,'notes'),
(3,2,NOW(),25,'notes'),
(4,3,NOW(),50,'notes'),
(5,3,NOW(),60,'notes'),
(6,4,NOW(),20,'notes'),
(7,5,NOW(),45,'notes'),
(8,6,NOW(),55,'notes'),
(9,7,NOW(),35,'notes'),
(10,8,NOW(),65,'notes');

-----------------------------------------------------
-- WORKOUT TYPES PER WORKOUT
-----------------------------------------------------
INSERT INTO WorkoutTypesPerWorkout (workoutTypeId, workoutId)
VALUES
(1,1),
(2,1),
(1,2),
(3,3),
(4,4),
(5,5),
(2,6),
(7,7),
(8,8),
(9,9);

-----------------------------------------------------
-- WORKOUT BIOMETRICS
-----------------------------------------------------
INSERT INTO WorkoutBiometrics (workoutId, steps, averageHeartRate, maxHeartRate, caloriesBurned, feeling, sleepScore)
VALUES
(1,5000,120,160,300,'Good',80),
(2,3500,110,150,250,'Okay',75),
(3,4000,115,155,260,'Good',78),
(4,6000,130,170,350,'Great',82),
(5,4500,118,165,280,'Good',79),
(6,3000,105,145,220,'Tired',70),
(7,5500,125,168,330,'Great',83),
(8,5000,119,162,290,'Good',77),
(9,4800,121,160,310,'Good',81),
(10,6200,135,175,360,'Great',85);

-----------------------------------------------------
-- MUSCLE
-----------------------------------------------------
INSERT INTO Muscle (muscleId, name, description)
VALUES
(1,'Chest','desc'),
(2,'Back','desc'),
(3,'Legs','desc'),
(4,'Arms','desc'),
(5,'Shoulders','desc'),
(6,'Core','desc'),
(7,'Glutes','desc'),
(8,'Calves','desc'),
(9,'Traps','desc'),
(10,'Forearms','desc');

-----------------------------------------------------
-- EXERCISE
-----------------------------------------------------
INSERT INTO Exercise (exerciseId, muscleId, exerciseName, description)
VALUES
(1,1,'Bench Press','desc'),
(2,2,'Deadlift','desc'),
(3,3,'Squat','desc'),
(4,4,'Bicep Curl','desc'),
(5,5,'Overhead Press','desc'),
(6,6,'Plank','desc'),
(7,7,'Hip Thrust','desc'),
(8,3,'Lunge','desc'),
(9,2,'Pull-up','desc'),
(10,6,'Crunch','desc');

-----------------------------------------------------
-- EXERCISE SET
-----------------------------------------------------
INSERT INTO ExerciseSet (exerciseSetId, workoutId, exerciseId, notes, setType)
VALUES
(1,1,1,'notes','LIFTING'),
(2,1,4,'notes','LIFTING'),
(3,2,2,'notes','LIFTING'),
(4,3,3,'notes','LIFTING'),
(5,4,5,'notes','LIFTING'),
(6,5,6,'notes','CARDIO'),
(7,6,7,'notes','LIFTING'),
(8,7,8,'notes','LIFTING'),
(9,8,9,'notes','LIFTING'),
(10,9,10,'notes','CARDIO');

-----------------------------------------------------
-- STRENGTH SETS
-----------------------------------------------------
INSERT INTO StrengthSet (exerciseSetId, setNumber, weight, reps)
VALUES
(1,1,100,8),
(2,1,20,12),
(3,1,150,5),
(4,1,120,10),
(5,1,60,8),
(7,1,180,6),
(8,1,30,15),
(9,1,0,12),
(1,2,105,7),
(3,2,160,4);

-----------------------------------------------------
-- CARDIO SETS
-----------------------------------------------------
INSERT INTO CardioSet (exerciseSetId, duration, distance)
VALUES
(6,10,2.0),
(10,12,2.5),
(6,8,1.6),
(10,9,2.1),
(6,7,1.4),
(10,11,2.4),
(6,9,1.8),
(10,13,2.6),
(6,6,1.2),
(10,14,2.8);