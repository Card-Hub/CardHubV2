<script setup lang="ts">

import { ref, onMounted } from 'vue';
import PlayingCardTest from "~/components/PlayingCardTest.vue";

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
    if (!response.ok) {
      throw new Error('Network response was not ok');
    }
    cards.value = await response.json();
  } catch (error) {
    console.error('Error fetching cards:', error);
  }
});

</script>

<template>
  <div>
    <h1>Playing Cards</h1>
    <PlayingCardTest :playingCards="cards"></PlayingCardTest>
    <h2>Card Data from C# Backend</h2>

  </div>
</template>
