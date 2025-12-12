import React from 'react';

const BiometricEntryCard = ({ entry }) => {
  const dateValue = entry.DateEntered ?? entry.dateEntered;
  return (
    <div className="bg-white shadow-md rounded p-4 space-y-1 mb-2">
      <div><strong>Date:</strong> {dateValue ? new Date(dateValue).toLocaleDateString() : "N/A"}</div>
      <div><strong>Weight:</strong> {(entry.Weight ?? entry.weight) ?? "-"} lbs</div>
      <div><strong>Height:</strong> {(entry.Height ?? entry.height) ?? "-"} in</div>
      <div><strong>Body Fat:</strong> {(entry.BodyFatPercentage ?? entry.bodyFatPercentage) ?? "-"}%</div>
      <div><strong>BMI:</strong> {(entry.BMI ?? entry.bmi) ?? "-"}</div>
      <div><strong>Resting Heart Rate:</strong> {(entry.RestingHeartRate ?? entry.restingHeartRate) ?? "-"} bpm</div>
    </div>
  );
};

export default BiometricEntryCard;
