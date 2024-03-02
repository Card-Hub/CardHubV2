<script setup lang="ts">

import { storeToRefs } from "pinia";

const store = useWebSocketStore();
const { messages, users, room } = storeToRefs(store);
const { sendMessage } = store;

</script>

<template>
  <NuxtLink to="/playerview">
    <Button>
      Player View
    </Button>
  </NuxtLink>
</template>

<style scoped>

</style>