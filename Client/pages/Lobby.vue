<script setup lang="ts">

import { storeToRefs } from "pinia";

const store = useWebSocketStore();
const { isPlayer, messages, users, room } = storeToRefs(store);
const { sendMessage } = store;

</script>

<template>
  <div id="dimScreen">
    <div v-if="isPlayer">
      <NuxtLink to="/playerview">
        <Button label="Secondary" severity="secondary">Player View</Button>
      </NuxtLink>
    </div>
    <div v-else>
      <NuxtLink to="/gameboard">
        <Button label="Secondary" severity="secondary">Gameboard</Button>
      </NuxtLink>
    </div>
    
  </div>
</template>

<style scoped>
  #dimScreen {
    width: 100%;
    height: 100%;
    background: rgb(243, 19, 19);
    background: radial-gradient(ellipse at center, rgba(243, 19, 19, 0.5) 0%, rgba(152, 14, 17, 0.7) 40%, rgba(63, 8, 14, 0.9) 95%);
    position: fixed;
    top: 0;
    left: 0;
    z-index: 100;
  }
</style>