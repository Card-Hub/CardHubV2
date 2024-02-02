<script setup lang="ts">
import { ref } from 'vue';
import { storeToRefs } from 'pinia'
import {useWebSocketStore} from "~/stores/webSocketStore";

const isValidRoomCode = ref<boolean | null>(null);

const store = useWebSocketStore();
const { connection, messages, cards, users, user, room } = storeToRefs(store);
const { createRoom, tryJoinRoom } = store;

</script>

<template>
  <Toast/>
  <div class="app">
    <h2></h2>
    <template v-if="connection === null">
      <div class="flex flex-col gap-4">
        <InputText type="text" v-model="user" placeholder="Name"/>
        <InputText type="text" v-model="room" placeholder="Game Pin"/>
        <Button type="submit" @click="tryJoinRoom" :disabled="!user || !room">Enter</Button>
      </div>
      <div>
        <Button @click="createRoom">Create Room</Button>
        <template v-if="isValidRoomCode">
          <NuxtLink to="/WeatherForecast"/>
        </template>
      </div>
    </template>
    <template v-else>
      <p>In chat</p>
      <div v-for="(m, index) in messages" :key="index">
        <div class="bg-primary">{{ m.message }}</div>
        <div>{{ m.user }}</div>
      </div>
    </template>
  </div>
</template>

<style scoped>

</style>
