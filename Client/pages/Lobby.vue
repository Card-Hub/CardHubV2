<script setup lang="ts">

import { storeToRefs } from "pinia";

const store = useWebSocketStore();
const { messages, users, room } = storeToRefs(store);
const { sendMessage } = store;

</script>

<template>

</template>

<style scoped>

</style>