<script setup lang="ts">
import {useApi} from "@/composables/useApi";

type Forecasts = Array<{
  date: string
  temperatureC: number
  temperatureF: number
  summary: string
}>

// Example usage of data fetching from the ASP.NET Core backend
// Use the useApi composable for this
const {data, pending, error} = await useApi<Forecasts>("/weatherforecast");

</script>

<template>
  <div>
    <h1>Weather forecast</h1>
    <p>This component demonstrates fetching data from the server.</p>
    <div v-if="error != null">The backend is not running or there is an error in the API call.</div>
    <div v-else-if="pending">Loading data from the API...</div>
    <div v-else>
      <table>
        <thead>
        <tr>
          <th>Date</th>
          <th>Temp. (C)</th>
          <th>Temp. (F)</th>
          <th>Summary</th>
        </tr>
        </thead>
        <tbody>
        <tr v-for="forecast in data as Forecasts" :key="forecast.date">
          <td>{{ forecast.date }}</td>
          <td>{{ forecast.temperatureC }}</td>
          <td>{{ forecast.temperatureF }}</td>
          <td>{{ forecast.summary }}</td>
        </tr>
        </tbody>
      </table>
    </div>
  </div>
</template>