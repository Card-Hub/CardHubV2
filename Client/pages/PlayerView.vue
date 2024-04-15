<script setup lang="ts">
import { onMounted, ref } from "vue";
import { useWebSocketStore } from "~/stores/webSocketStore";

// const { $api } = useNuxtApp();

const store = useWebSocketStore();
const { timer } = storeToRefs(store);
const { drawCard } = store;

const selectedCard = ref<UNOCard | null>(null);

</script>

<template>
  <div id="dimScreen">
    <Button @click="drawCard">Draw Card</Button>
    <p>Timer => {{ timer }}</p>
    <PlayerHand />
  </div>
</template>

<style scoped>

#dimScreen {
  width: 100%;
  height: 100%;
  background: linear-gradient(20deg, #000000 0%, #313134 100%);
  position: absolute;
  top: 0;
  left: 0;
  z-index: 100;
}
</style>
