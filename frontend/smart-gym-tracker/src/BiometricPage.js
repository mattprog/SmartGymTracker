import { useState } from 'react';

function BiometricPage() {
  const [biometrics, setBiometrics] = useState({
    DateEntered: '',
    Weight: '',
    Height: '',
    BodyFatPercentage: '',
    BMI: '',
    RestingHeartRate: ''
  });

  const [history, setHistory] = useState([]);

  const handleChange = (e) => {
    const { name, value } = e.target;
    setBiometrics({ ...biometrics, [name]: value });
  };

  const handleSave = () => {
    if (!biometrics.DateEntered) {
      alert('Please enter a date.');
      return;
    }

    setHistory([biometrics, ...history]); // add new entry to top of history
    setBiometrics({ DateEntered: '', Weight: '', Height: '', BodyFatPercentage: '', BMI: '', RestingHeartRate: '' });
  };

  return (
    <div className="max-w-md mx-auto space-y-6">
      {/* Input Form */}
      <div className="bg-white shadow-md rounded p-6 space-y-4">
        <h1 className="text-2xl font-bold mb-4 text-center">Enter Biometrics</h1>

        <div>
          <label className="block mb-1 font-medium">Date:</label>
          <input
            type="date"
            name="DateEntered"
            value={biometrics.DateEntered}
            onChange={handleChange}
            className="w-full border rounded px-3 py-2"
          />
        </div>

        <div>
          <label className="block mb-1 font-medium">Weight (lbs):</label>
          <input
            type="number"
            name="Weight"
            value={biometrics.Weight}
            onChange={handleChange}
            className="w-full border rounded px-3 py-2"
            placeholder=""
            min ="0"
          />
        </div>

        <div>
          <label className="block mb-1 font-medium">Height (inches):</label>
          <input
            type="number"
            name="Height"
            value={biometrics.Height}
            onChange={handleChange}
            className="w-full border rounded px-3 py-2"
            placeholder=""
            min = "0"
          />
        </div>

        <div>
          <label className="block mb-1 font-medium">Body Fat (%):</label>
          <input
            type="number"
            name="BodyFatPercentage"
            value={biometrics.BodyFatPercentage}
            onChange={handleChange}
            className="w-full border rounded px-3 py-2"
            placeholder=""
            min="0"
          />
        </div>

        <div>
          <label className="block mb-1 font-medium">BMI:</label>
          <input
            type="number"
            name="BMI"
            value={biometrics.BMI}
            onChange={handleChange}
            className="w-full border rounded px-3 py-2"
            placeholder=""
            min="0"
          />
        </div>

        <div>
          <label className="block mb-1 font-medium">Resting Heart Rate (bpm):</label>
          <input
            type="number"
            name="RestingHeartRate"
            value={biometrics.RestingHeartRate}
            onChange={handleChange}
            className="w-full border rounded px-3 py-2"
            placeholder=""
            min="0"
          />
        </div>

        <button
          onClick={handleSave}
          className="w-full bg-blue-600 text-white font-bold py-2 px-4 rounded hover:bg-blue-700"
        >
          Save
        </button>
      </div>

      {/* History */}
      {history.length > 0 && (
        <div className="space-y-4">
          {history.map((entry, index) => (
            <div key={index} className="bg-white shadow-md rounded p-4 space-y-1">
              <div><strong>Date:</strong> {entry.DateEntered}</div>
              <div><strong>Weight:</strong> {entry.Weight} lbs</div>
              <div><strong>Height:</strong> {entry.Height} in</div>
              <div><strong>Body Fat:</strong> {entry.BodyFatPercentage}%</div>
              <div><strong>BMI:</strong> {entry.BMI}</div>
              <div><strong>Resting Heart Rate:</strong> {entry.RestingHeartRate} bpm</div>
            </div>
          ))}
        </div>
      )}
    </div>
  );
}

export default BiometricPage;
