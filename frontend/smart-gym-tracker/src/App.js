import { useState } from 'react';
import { FaUser, FaDumbbell, FaPlus, FaHeart, FaChartLine } from 'react-icons/fa';
import './App.css';
import WorkoutPage from './pages/WorkoutPage';
import BiometricPage from './pages/BiometricPage';
import Profile from './pages/Profile';
import Progress from './pages/Progress';
import Admin from './pages/Admin';
import RegistrationPage from './pages/RegistrationPage';
import LoginPage from './pages/LoginPage';
import UserManagement from './pages/UserManagement';






function App() {
  // controls if the bar shows
  const [showBars, setShowBars] = useState(true);

 // dashboard, profile, workout, etc
  const [currentPage, setCurrentPage] = useState('dashboard'); 

  //decide whether or not to show bars
  const displayBars = currentPage !== 'profile';

  return (
    <div className="flex flex-col h-screen bg-gray-100">
      {/* Top Blue Bar */}
      {displayBars && (
        <div className="bg-blue-600 text-white flex items-center justify-between px-4 py-3 shadow-md">
          <FaUser size={24} onClick={() => setCurrentPage('profile')} className="cursor-pointer"/>
          <h1 className="text-xl font-bold">SmartGymTracker</h1>
          <FaDumbbell size={24} onClick={() => setCurrentPage('smarttips')} className="cursor-pointer" />
        </div>
      )}



      {/* Main Content */}
      <div className="flex-1 p-6">
        {currentPage === 'dashboard' && <div>Dashboard Content</div>}
        {currentPage === 'profile' && <Profile/>}
        {currentPage === 'workout' && <WorkoutPage/>}
        {currentPage === 'biometric' && <BiometricPage/>}
        {currentPage === 'progress' && <Progress/>}
        {currentPage === 'admin' && <Admin/>}
        {currentPage === 'registration' && <RegistrationPage />}
        {currentPage === 'login' && <LoginPage setCurrentPage={setCurrentPage} />}
        {currentPage === 'usermanagement' && <UserManagement />}
      </div>





      {/* Bottom Blue Bar */}
      {displayBars && (
        <div className="bg-blue-600 text-white flex items-center justify-around px-4 py-3 shadow-inner">


         {/* Biometric Data */}
         <div className="flex flex-col items-center cursor-pointer" onClick={() => setCurrentPage('biometric')}>
         <FaHeart size={24} className="mb-1" />
         <span className="text-sm">Biometrics</span>
        </div>


              {/* Add Workout */}
          <div className="flex flex-col items-center cursor-pointer" onClick={() => setCurrentPage('workout')}>
            <div className="w-12 h-12 bg-white text-blue-600 rounded-full flex items-center justify-center mb-1">
              <FaPlus size={24} />
            </div>
            <span className="text-sm">Add Workout</span>
          </div>



            {/* Progress/Milestones */}
         <div className="flex flex-col items-center cursor-pointer" onClick={() => setCurrentPage('progress')}>
         <FaChartLine size={24} className="mb-1" />
          <span className="text-sm">Progress</span>
         </div>


           {/* Temporary Admin Button */}
           <div className="flex flex-col items-center cursor-pointer" onClick={() => setCurrentPage('admin')}>
            <div className="w-6 h-6 bg-gray-200 rounded-full mb-1"></div>
            <span className="text-sm">Temp Admin</span>
          </div>

          {/* Temporary Registration Button */}
          <div className="flex flex-col items-center cursor-pointer" onClick={() => setCurrentPage('registration')}>
          <div className="w-6 h-6 bg-gray-200 rounded-full mb-1"></div>
          <span className="text-sm">Register</span>
          </div>

          {/* Temporary Login Button */}
        <div className="flex flex-col items-center cursor-pointer" onClick={() => setCurrentPage('login')}>
        <div className="w-6 h-6 bg-gray-200 rounded-full mb-1"></div>
        <span className="text-sm">Login</span>
        </div>



        {/* Temporary User Management Button */}
        <div className="flex flex-col items-center cursor-pointer" onClick={() => setCurrentPage('usermanagement')}>
        <div className="w-6 h-6 bg-gray-200 rounded-full mb-1"></div>
        <span className="text-sm">User Mgmt</span>
        </div>
        
        </div>
      )}
    </div>


  );
}

export default App;




