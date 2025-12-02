import { useState } from 'react';
import { FaUser, FaDumbbell, FaPlus, FaHeart, FaChartLine, FaSignOutAlt, FaArrowLeft, FaHome } from 'react-icons/fa';
import './App.css';
import WorkoutPage from './pages/WorkoutPage';
import BiometricPage from './pages/BiometricPage';
import Profile from './pages/Profile';
import Progress from './pages/Progress';
import Admin from './pages/Admin';
import RegistrationPage from './pages/RegistrationPage';
import LoginPage from './pages/LoginPage';
import UserManagement from './pages/UserManagement';
import ForgotPassword from './pages/ForgotPassword';
import Dashboard from './pages/Dashboard';
import SmartTips from './pages/SmartTips';

function App() {
  const [currentPage, setCurrentPage] = useState('dashboard'); 

  const displayBars = ![
    "login",
    "registration",
    "profile",
    "adminDashboard",
    "usermanagement",
    "admin"
  ].includes(currentPage);

  const handleLogout = () => setCurrentPage("login");

  return (
    <>
      {["login", "registration", "forgotPassword"].includes(currentPage) ? (
        <div className="min-h-screen">
          {currentPage === "login" && <LoginPage setCurrentPage={setCurrentPage} />}
          {currentPage === "registration" && <RegistrationPage setCurrentPage={setCurrentPage} />}
          {currentPage === "forgotPassword" && <ForgotPassword setCurrentPage={setCurrentPage} />}
          
        </div>
      ) : (
        <div className="flex flex-col h-screen bg-gray-100">



          {/* Top Blue Bar - FIXED */}
          {displayBars && (
            <div className="fixed top-0 left-0 right-0 bg-blue-600 text-white flex items-center justify-between px-4 py-3 shadow-md z-50">
              <FaUser size={24} onClick={() => setCurrentPage('profile')} className="cursor-pointer"/>
              <h1 className="text-xl font-bold">SmartGymTracker</h1>
              <div className="flex items-center gap-3">
                <FaSignOutAlt size={24} onClick={handleLogout} className="cursor-pointer" />
              </div>
            </div>
          )}



           {/* Profile Page (fullscreen, not scrollable) */}
          {currentPage === 'profile' && (
          <div className="flex-1 bg-blue-600 flex flex-col">
          <Profile setCurrentPage={setCurrentPage} />
          </div>
            )}
          



          {/* Main Content - scrollable */}
          <div className="flex-1 overflow-auto pt-16 pb-16 px-6">
            {currentPage === 'dashboard' && <Dashboard />}
      
            {currentPage === 'workout' && <WorkoutPage />}
            {currentPage === 'biometric' && <BiometricPage />}
            {currentPage === 'progress' && <Progress />}
            {currentPage === 'smarttips' && <SmartTips />}

            {currentPage === 'admin' && (
              <div className="min-h-screen flex flex-col items-center bg-gray-100 p-6 w-full">
                <button
                  onClick={() => setCurrentPage('adminDashboard')}
                  className="flex items-center gap-2 text-blue-600 font-bold mb-6 text-lg"
                >
                  <FaArrowLeft /> Back to Home
                </button>
                <div className="w-full max-w-4xl">
                  <Admin className="w-full" />
                </div>
              </div>
            )}

            {currentPage === 'usermanagement' && (
              <div className="min-h-screen flex flex-col items-center bg-gray-100 p-6 w-full">
                <button
                  onClick={() => setCurrentPage('adminDashboard')}
                  className="flex items-center gap-2 text-blue-600 font-bold mb-6 text-lg"
                >
                  <FaArrowLeft /> Back to Home
                </button>
                <div className="w-full max-w-4xl">
                  <UserManagement className="w-full" />
                </div>
              </div>
            )}
            
            {currentPage === "adminDashboard" && (
              <div className="flex flex-col h-full bg-gray-100">
                <div className="fixed top-0 left-0 right-0 bg-blue-600 text-white flex items-center justify-between px-6 py-3 shadow-md z-50">
                  <h2 className="text-2xl font-bold flex items-center gap-2">
                    SmartGymTracker <FaDumbbell size={28} />
                  </h2>
                  <FaSignOutAlt size={24} onClick={handleLogout} className="cursor-pointer" />
                </div>

                <div className="h-16"></div>

                <p className="text-gray-700 text-3xl font-extrabold mt-6 mb-8 text-center">
                  Manage users and workouts here
                </p>

                <div className="flex flex-col items-center justify-center flex-1 gap-6 px-6 w-full">
                  <div className="bg-white shadow-md rounded p-6 flex flex-col items-center gap-4 w-full sm:w-96 lg:w-1/2">
                    <FaUser size={36} className="text-blue-600" />
                    <button
                      onClick={() => setCurrentPage("usermanagement")}
                      className="bg-blue-600 text-white px-4 py-2 rounded w-full hover:bg-blue-700"
                    >
                      User Management
                    </button>
                  </div>

                  <div className="bg-white shadow-md rounded p-6 flex flex-col items-center gap-4 w-full sm:w-96 lg:w-1/2">
                    <FaDumbbell size={36} className="text-blue-600" />
                    <button
                      onClick={() => setCurrentPage("admin")}
                      className="bg-blue-600 text-white px-4 py-2 rounded w-full hover:bg-blue-700"
                    >
                      Workout Management
                    </button>
                  </div>
                </div>
              </div>
            )}
          </div>

          

          {/* Bottom Blue Bar - FIXED */}
          {displayBars && (
            <div className="fixed bottom-0 left-0 right-0 bg-blue-600 text-white flex items-center justify-around px-4 py-3 shadow-inner z-50">
              <div className="flex flex-col items-center cursor-pointer" onClick={() => setCurrentPage('dashboard')}>
                <FaHome size={24} className="mb-1" />
                <span className="text-sm">Home</span>
              </div>
              <div className="flex flex-col items-center cursor-pointer" onClick={() => setCurrentPage('biometric')}>
                <FaHeart size={24} className="mb-1" />
                <span className="text-sm">Biometrics</span>
              </div>
              <div className="flex flex-col items-center cursor-pointer" onClick={() => setCurrentPage('workout')}>
                <div className="w-12 h-12 bg-white text-blue-600 rounded-full flex items-center justify-center mb-1">
                  <FaPlus size={24} />
                </div>
                <span className="text-sm">Add Workout</span>
              </div>
              <div className="flex flex-col items-center cursor-pointer" onClick={() => setCurrentPage('smarttips')}>
                <FaDumbbell size={24} className="mb-1" />
                <span className="text-sm">Tips</span>
              </div>
              <div className="flex flex-col items-center cursor-pointer" onClick={() => setCurrentPage('progress')}>
                <FaChartLine size={24} className="mb-1" />
                <span className="text-sm">Progress</span>
              </div>
            </div>
          )}

        </div>
      )}
    </>
  );
}

export default App;
