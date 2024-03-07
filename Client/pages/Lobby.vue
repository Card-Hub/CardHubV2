<script setup lang="ts">

import { storeToRefs } from "pinia";

const store = useWebSocketStore();
const { messages, users, room } = storeToRefs(store);
const { sendMessage } = store;

</script>

<template>
  <div id="dimScreen">

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