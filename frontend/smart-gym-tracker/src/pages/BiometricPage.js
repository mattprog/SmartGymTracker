import { useCallback, useEffect, useState } from "react";
import BiometricEntryCard from "../biometriccomponents/BiometricEntryCard";
import { fetchBiometrics, postBiometrics } from "../services/BiometricsService";

const emptyBio = {
  DateEntered: "",
  Weight: "",
  Height: "",
  BodyFatPercentage: "",
  BMI: "",
  RestingHeartRate: "",
};

function BiometricPage({ user }) {
  const userId = user?.UserId ?? user?.userId ?? null;
  const [biometrics, setBiometrics] = useState(emptyBio);
  const [history, setHistory] = useState([]);
  const [loading, setLoading] = useState(false);
  const [saving, setSaving] = useState(false);
  const [error, setError] = useState("");

  const loadHistory = useCallback(async () => {
    setLoading(true);
    const data = await fetchBiometrics(userId);
    const normalized = (data ?? []).map((entry) => ({
      ...entry,
      id: entry.biometricsId ?? entry.BiometricsId,
    }));
    setHistory(normalized);
    setLoading(false);
  }, [userId]);

  useEffect(() => {
    if (!userId) return;
    void loadHistory();
  }, [userId, loadHistory]);

  const handleChange = (e) => {
    const { name, value } = e.target;
    setBiometrics({ ...biometrics, [name]: value });
  };

  const handleSave = async () => {
    if (!userId) {
      setError("Please log in to save biometrics.");
      return;
    }
    if (!biometrics.DateEntered) {
      setError("Please enter a date.");
      return;
    }

    setError("");
    setSaving(true);

    const payload = {
      userId,
      dateEntered: biometrics.DateEntered,
    };
    if (biometrics.Weight) payload.weight = parseFloat(biometrics.Weight);
    if (biometrics.Height) payload.height = parseFloat(biometrics.Height);
    if (biometrics.BodyFatPercentage) payload.bodyFatPercentage = parseFloat(biometrics.BodyFatPercentage);
    if (biometrics.BMI) payload.bmi = parseFloat(biometrics.BMI);
    if (biometrics.RestingHeartRate) payload.restingHeartRate = parseInt(biometrics.RestingHeartRate, 10);

    const res = await postBiometrics(payload);
    setSaving(false);
    if (res?.error) {
      setError(res.error);
      return;
    }

    setBiometrics(emptyBio);
    await loadHistory();
  };

  if (!userId) {
    return (
      <div className="max-w-md mx-auto bg-white shadow-md rounded p-6 space-y-4 text-center">
        <p className="text-gray-700">
          Please log in to view and record your biometrics.
        </p>
      </div>
    );
  }

  return (
    <div className="max-w-md mx-auto space-y-6">
      <div className="bg-white shadow-md rounded p-6 space-y-4">
        <h1 className="text-2xl font-bold mb-4 text-center">Enter Biometrics</h1>

        {error && (
          <div className="bg-red-100 text-red-700 rounded px-3 py-2 text-sm">
            {error}
          </div>
        )}

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
            min="0"
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
            min="0"
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
            min="0"
          />
        </div>

        <button
          onClick={handleSave}
          disabled={saving}
          className="w-full bg-blue-600 text-white font-bold py-2 px-4 rounded hover:bg-blue-700 disabled:opacity-60"
        >
          {saving ? "Saving..." : "Save"}
        </button>
      </div>

      <div>
        {loading ? (
          <p className="text-center text-gray-600">Loading history...</p>
        ) : history.length === 0 ? (
          <p className="text-center text-gray-500">No biometrics recorded yet.</p>
        ) : (
          history.map((entry) => (
            <BiometricEntryCard key={entry.id ?? entry.biometricsId} entry={entry} />
          ))
        )}
      </div>
    </div>
  );
}

export default BiometricPage;
