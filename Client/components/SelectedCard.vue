<script setup lang="ts">
import { defineProps } from 'vue';
import CardDisplay from './CardDisplay.vue';

const props = defineProps(['selectedCard']);
</script>

<template>
  <div class="selected-card">
    <CardDisplay v-if="selectedCard" :card="selectedCard" />
  </div>
</template>

<style scoped>
  .selected-card {
    display: flex;
    justify-content: center;
    align-items: center;
  }
</style>
