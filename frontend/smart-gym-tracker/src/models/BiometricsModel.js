// src/models/BiometricsModel.js
export class Biometrics {
  constructor(
    biometricsId = -1,
    userId = -1,
    dateEntered = "",
    weight = -1,
    height = -1,
    bodyFatPercentage = -1,
    bmi = -1,
    restingHeartRate = -1
  ) {
    this.biometricsId = biometricsId;
    this.userId = userId;
    this.dateEntered = dateEntered;
    this.weight = weight;
    this.height = height;
    this.bodyFatPercentage = bodyFatPercentage;
    this.bmi = bmi;
    this.restingHeartRate = restingHeartRate;
  }
}
