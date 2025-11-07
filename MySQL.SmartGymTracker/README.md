# Database Setup Instruction

## Setup Recommendations
1. Install:
    - https://www.apachefriends.org/download.html
    - https://netbeans.apache.org/front/main/download/nb24/
    - https://dev.mysql.com/downloads/workbench/
2. Run XAMPP
3. Inside XAMPP start Apcache and then MYSQL
4. Click admin button next to MYSQL

## Importing the Database
1. Open admin panel of MYSQL from XAMPP
2. Go to import section of MYSQL
3. Import `Project_DDLScript.sql`
    - This will create the database schema with blank tables
Now the database will be open on port 3306 for communication

## Adding Database Library to Solution
1. Go to Solution Explorer
2. Right click on the Desired Project to add Dependency
3. Click on `Add -> Shared Project Reference`
4. Go to Browse tab and select Browse
5. Navigate to `.\SmartGymTracker\MySQL.SmartGymTracker\bin\Debug\net9.0`
6. Add references to `MySQL.SmartGymTracker.dll` & `Library.SmartGymTracker.dll`
    - Note: Path may vary slighly and if dlls are not found, build the solution
7. Right click on the Desired Project again
8. Click on `Manage NuGet Packages`
9. Search for `MySql.Data` and install the package by Oracle Corporation
    - Note: Tested on version 9.4.0