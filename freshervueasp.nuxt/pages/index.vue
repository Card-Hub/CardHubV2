<script setup lang="ts">

import { ref, onMounted } from 'vue';
import PlayingCard from "~/components/PlayingCard.vue";

type Card = {
  id: number
  value: string
  suit: string
}

const cards = ref<Array<Card> | null>(null);

//https://vuejs.org/guide/essentials/component-basics.html#passing-props
onMounted(async () => {
  try {
    const response = await fetch('https://localhost:7085/Cards');
    cards.value = await response.json();
  } catch (error) {
    console.error('Error fetching cards:', error);
  }
});

// import { defineComponent } from 'vue'
//
// const URL = 'https://localhost:7085'
// console.log(URL)
//
// type Forecasts = Array<{
//   date: string
//   temperatureC: string
//   temperatureF: string
//   summary: string
// }>
//
// interface Data {
//   loading: boolean
//   post: null | Forecasts
// }
//
// export default defineComponent({
//   data (): Data {
//     return {
//       loading: false,
//       post: null as Forecasts | null
//     }
//   },
//   created () {
//     // fetch the data when the view is created and the data is
//     // already being observed
//     this.fetchData()
//   },
//   watch: {
//     // call again the method if the route changes
//     $route: 'fetchData'
//   },
//   methods: {
//     fetchData (): void {
//       this.post = null
//       this.loading = true
//
//       fetch(`${URL}/weatherforecast`)
//         .then(async r => await r.json())
//         .then(json => {
//           this.post = json as Forecasts
//           this.loading = false
//         }).catch(e => { console.error(e) })
//     }
//   }
// })
</script>

<template>
<!--  <div>-->
<!--    <h1>Weather forecast</h1>-->
<!--    <p>This component demonstrates fetching data from the server.</p>-->
<!--    <div v-if="loading">Loading...</div>-->
<!--    <div>-->
<!--      <table>-->
<!--        <thead>-->
<!--        <tr>-->
<!--          <th>Date</th>-->
<!--          <th>Temp. (C)</th>-->
<!--          <th>Temp. (F)</th>-->
<!--          <th>Summary</th>-->
<!--        </tr>-->
<!--        </thead>-->
<!--        <tbody>-->
<!--        <tr v-for="forecast in post" :key="forecast.date">-->
<!--          <td>{{ forecast.date }}</td>-->
<!--          <td>{{ forecast.temperatureC }}</td>-->
<!--          <td>{{ forecast.temperatureF }}</td>-->
<!--          <td>{{ forecast.summary }}</td>-->
<!--        </tr>-->
<!--        </tbody>-->
<!--      </table>-->
<!--    </div>-->
<!--  </div>-->
  <div>
    <h1>Playing Cards</h1>
    <PlayingCard :playingCards="cards"></PlayingCard>
    <h2>Card Data from C# Backend</h2>
<!--    <ul>-->
<!--      <li v-for="card in cards" :key="card.id">-->
<!--        {{ card.value }} of {{ card.suit }}-->
<!--      </li>-->
<!--    </ul>-->
  </div>
</template>
