import React from 'react';

const BiometricEntryCard = ({ entry }) => {
  return (
    <div className="bg-white shadow-md rounded p-4 space-y-1 mb-2">
      <div><strong>Date:</strong> {entry.DateEntered}</div>
      <div><strong>Weight:</strong> {entry.Weight} lbs</div>
      <div><strong>Height:</strong> {entry.Height} in</div>
      <div><strong>Body Fat:</strong> {entry.BodyFatPercentage}%</div>
      <div><strong>BMI:</strong> {entry.BMI}</div>
      <div><strong>Resting Heart Rate:</strong> {entry.RestingHeartRate} bpm</div>
    </div>
  );
};

export default BiometricEntryCard;
