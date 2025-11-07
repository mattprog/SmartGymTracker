# SmartGymTracker

This is a group project for CEN4090L.

Smart Gym Tracker is a web-based fitness tracking application that lets users log workouts, record biometric data, track milestones, and view trends. Smart Gym Tracker utilizes a React + Tailwind CSS frontend, C# backend, and a MySQL database to create a simple but powerful gym tracker app that allows users to simply enter their workout data and see trends. 

### Group Members
  * Matthew Cegala
  * Amanda Orama
  * Nicholas Holguin
  * Matthew Hummel
  * Ashton Singpradith

## Increment Release Plan
### Increments 1 Features:

* Create much of the documentation for the project
* Provide shells of the frontend, backend, and database
* Create the framework for class libraries that include the following:
    * Workout
    * Sets
        * Strength Set
        * Cardio Set
    * Rep
    * Biometrics
    * Workout Biometrics
    * Admin Screens
        * Workout Types
        * Muscles
        * Exercises

### Increment 2 Features:

* Get the frontend, backend, and database communicating
* Users and Identity Management (IDM)
* Login Screen
    * Creates new user
    * Forgot my password
    * Populate user permissions and data upon login
* Admin login
    * Admin specific pages
    * User Management Page

### Increment 3 Features:
* Complete backend setup
    * Allow for full system communication
* Trends of Data
    * See graphs of how data has changed over time
* Notifications
    * Create user inbox dashboard messages to populate
* Smart Tips and Suggestions
    * See goals and tips based off data trends
* Milestone and Progress Tracking
    * Personalized goals creation
    * See progress over time towards these user created goals

## File Structure Notes
### Documentation (Including IT, RD, and Progress Report)
```
├── Documentation.SmartGymTracker
│
├── README.md
```
### General Folders
```
├── Documentation.SmartGymTracker
```
### Frontend Folders
```
├── frontend/smart-gym-tracker
```
### Backend Folders
```
├── SmartGymTracker.Api
│
├── SmartGymTracker.Metrics.API
```
### Database Folders
```
├── Library.SmartGymTracker
│
└── MySQL.SmartGymTracker
    ├── InitDB
    ├── TestDB
    └── README.md
```

## Known Bugs
* Currently none at this time, all code builds.
* Frontend and database work based on tests performed with mock data.
* Backend not fully operational at this time.
