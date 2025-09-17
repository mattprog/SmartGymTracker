import { useState } from 'react';
import { FaUser, FaDumbbell, FaPlus } from 'react-icons/fa';
import './App.css';
import Workout from './WorkoutPage';
import Profile from './Profile';
import BiometricPage from './BiometricPage';
import Progress from './Progress'; 


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
        {currentPage === 'workout' && <Workout />}
        {currentPage === 'biometric' && <BiometricPage/>}
        {currentPage === 'progress' && <Progress/>}
      </div>




      {/* Bottom Blue Bar */}
      {displayBars && (
        <div className="bg-blue-600 text-white flex items-center justify-around px-4 py-3 shadow-inner">
          <div className="flex flex-col items-center cursor-pointer" onClick={() => setCurrentPage('biometric')}>
            <div className="w-6 h-6 bg-gray-200 rounded-full mb-1"></div>
            <span className="text-sm">Biometric Data</span>
          </div>

          <div className="flex flex-col items-center cursor-pointer" onClick={() => setCurrentPage('workout')}>
            <div className="w-12 h-12 bg-white text-blue-600 rounded-full flex items-center justify-center mb-1">
              <FaPlus size={24} />
            </div>
            <span className="text-sm">Add Workout</span>
          </div>

          <div className="flex flex-col items-center cursor-pointer" onClick={() => setCurrentPage('progress')}>
            <div className="w-6 h-6 bg-gray-200 rounded-full mb-1"></div>
            <span className="text-sm">Progress</span>
          </div>
        </div>
      )}
    </div>


  );
}

export default App;




