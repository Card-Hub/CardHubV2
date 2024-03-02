<script setup lang="ts">
import { ref, onMounted } from 'vue';

const cards = ref<Array<StandardCard> | null>(null);

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
    <PlayingCard :playingCards="cards"></PlayingCard>
    <h2>Card Data from C# Backend</h2>
  </div>
</template>
