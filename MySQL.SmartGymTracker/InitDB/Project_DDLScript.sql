-- Create Database Schema to be Used
DROP DATABASE IF EXISTS smart_gym_tracker;
CREATE DATABASE smart_gym_tracker;
USE smart_gym_tracker;



-- Delete Old Tables if they Exist
DROP TABLE IF EXISTS notification;
DROP TABLE IF EXISTS messages;
DROP TABLE IF EXISTS goal;
DROP TABLE IF EXISTS cardio_set;
DROP TABLE IF EXISTS strength_set;
DROP TABLE IF EXISTS exercise_set;
DROP TABLE IF EXISTS exercise;
DROP TABLE IF EXISTS muscle;
DROP TABLE IF EXISTS workout_types_per_workout;
DROP TABLE IF EXISTS workout_type;
DROP TABLE IF EXISTS exercise_biometrics;
DROP TABLE IF EXISTS workout;
DROP TABLE IF EXISTS biometrics;
DROP TABLE IF EXISTS users;



-- Create Tables
CREATE TABLE users (
userId	    	INT			 PRIMARY KEY AUTO_INCREMENT,
username	    VARCHAR(50)	 UNIQUE,
password	    VARCHAR(100) NOT NULL,
firstName	    VARCHAR(50)  NOT NULL,
lastName	    VARCHAR(50)  NOT NULL,
email		    VARCHAR(254) NOT NULL,
phoneNumber	    VARCHAR(20)  NOT NULL,
dateOfBirth	    DATE         NOT NULL,
gender          VARCHAR(20)  NOT NULL,
privilegeLevel  ENUM('User', 'Admin') NOT NULL DEFAULT 'User',
active			BOOLEAN      NOT NULL DEFAULT TRUE
);


CREATE TABLE biometrics (
biometricsId        INT      PRIMARY KEY AUTO_INCREMENT,
userId              INT      NOT NULL,
dateEntered         DATE     NOT NULL,
weight              FLOAT    NOT NULL,
height              INT      NOT NULL,
bodyFatPercentage   FLOAT    NOT NULL,
bmi                 FLOAT    NOT NULL,
restingHeartRate    INT      NOT NULL,
FOREIGN KEY (userId) REFERENCES users(userId)
  ON UPDATE CASCADE ON DELETE CASCADE
);


CREATE TABLE workout (
workoutId		    INT      PRIMARY KEY AUTO_INCREMENT,
userId			    INT      NOT NULL,
workoutStart	    DATETIME NOT NULL,
duration         	INT      NOT NULL,
notes			    LONGTEXT,
FOREIGN KEY (userId) REFERENCES users(userId)
  ON UPDATE CASCADE ON DELETE CASCADE
);


CREATE TABLE workout_biometrics (
workoutId         INT,
steps             INT,
averageHeartRate  INT,
maxHeartRate      INT,
caloriesBurned    INT,
feeling           VARCHAR(50),
sleepScore        INT,

PRIMARY KEY (workoutId),
FOREIGN KEY (workoutId) REFERENCES workout(workoutId)
  ON UPDATE CASCADE ON DELETE CASCADE
);


CREATE TABLE workout_type (
workoutTypeId	INT         PRIMARY KEY AUTO_INCREMENT,
name			VARCHAR(50) NOT NULL,
difficulty		VARCHAR(50) NOT NULL,
description		LONGTEXT
);


CREATE TABLE workout_types_per_workout (
workoutTypeId   INT,
workoutId       INT,
PRIMARY KEY (workoutTypeId, workoutId),
FOREIGN KEY (workoutTypeId) REFERENCES workout_type(workoutTypeId)
  ON UPDATE CASCADE ON DELETE CASCADE,
FOREIGN KEY (workoutId) REFERENCES workout(workoutId)
  ON UPDATE CASCADE ON DELETE CASCADE
);


CREATE TABLE muscle (
muscleId    INT             PRIMARY KEY AUTO_INCREMENT,
name        VARCHAR(50)     NOT NULL,
description LONGTEXT
);


CREATE TABLE exercise (
exerciseId      INT         PRIMARY KEY AUTO_INCREMENT,
exerciseName    VARCHAR(50) NOT NULL,
muscleId        INT         NOT NULL,
description     LONGTEXT,
FOREIGN KEY (muscleId) REFERENCES muscle(muscleId)
  ON UPDATE CASCADE ON DELETE CASCADE
);


CREATE TABLE exercise_set (
exerciseSetId	INT AUTO_INCREMENT,
workoutId	    INT,
exerciseId	    INT,
notes		    LONGTEXT,
setType         ENUM('Strength', 'Cardio') NOT NULL,
PRIMARY KEY (exerciseSetId, workoutId, exerciseId),
FOREIGN KEY (workoutId) REFERENCES workout(workoutId)
  ON UPDATE CASCADE ON DELETE CASCADE,
FOREIGN KEY (exerciseId) REFERENCES exercise(exerciseId)
  ON UPDATE CASCADE ON DELETE CASCADE
);


CREATE TABLE strength_set (
exerciseSetId INT,
setNumber     INT,
weight        FLOAT NOT NULL,
reps          INT   NOT NULL,
PRIMARY KEY (exerciseSetId, setNumber),
FOREIGN KEY (exerciseSetId) REFERENCES exercise_set(exerciseSetId)
  ON UPDATE CASCADE ON DELETE CASCADE
);


CREATE TABLE cardio_set (
exerciseSetId INT PRIMARY KEY,
distance INT NOT NULL,
duration INT NOT NULL,
FOREIGN KEY (exerciseSetId) REFERENCES exercise_set(exerciseSetId)
  ON UPDATE CASCADE ON DELETE CASCADE
);


CREATE TABLE goal (
goalId		  INT PRIMARY KEY AUTO_INCREMENT,
userId		  INT NOT NULL,
timeCreated   DATETIME NOT NULL,
title		  VARCHAR(50) NOT NULL,
description   LONGTEXT,
startDate	  DATE,
targetEndDate DATE,
status	  ENUM('Not_Started', 'In_Progress', 'Completed', 'Failed') NOT NULL,
FOREIGN KEY (userId) REFERENCES users(userId)
  ON UPDATE CASCADE ON DELETE CASCADE
);


CREATE TABLE messages (
messageId	  INT PRIMARY KEY AUTO_INCREMENT,
title	      VARCHAR(50) NOT NULL,
details	      LONGTEXT NOT NULL,
timeCreated   DATETIME NOT NULL,
type	      ENUM('System', 'Milestone', 'Tip', 'Trend', 'Goal', 'Specific') NOT NULL
);


CREATE TABLE notification (
userId       INT,
messageId    INT,
timeSent	 DATETIME NOT NULL,
read		 BOOLEAN NOT NULL DEFAULT FALSE,
PRIMARY KEY (userId, messageId),
FOREIGN KEY (userId) REFERENCES users(userId)
  ON UPDATE CASCADE ON DELETE CASCADE,
FOREIGN KEY (messageId) REFERENCES message(messageId)
  ON UPDATE CASCADE ON DELETE CASCADE
);