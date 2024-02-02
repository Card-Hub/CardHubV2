<script setup lang="ts">
import { HttpTransportType, type HubConnection, HubConnectionBuilder, LogLevel } from '@microsoft/signalr'
import { ref } from 'vue'
import toast from '@/utils/toast'

interface UserMessage {
  user: string
  message: string
}

const connection = ref<HubConnection | null>(null)
const messages = ref<UserMessage[]>([])
const users = ref<string[]>([])
const user = ref('')
const room = ref('')

// Allows Gameboard device to join Group
const createRoom = async (): Promise<void> => {
  const { data } = await useApi<string>("game/createroom", { method: 'POST' });
  if (data.value) {
    room.value = data.value;
    console.log('Room created: ', room.value);
    await joinRoom(user.value, room.value);
  }
}

// Allows Player device to join Group
const isRoomCodeValid = ref<boolean | null>(null)
const tryJoinRoom = async (): Promise<void> => {
  if (user.value && room.value) {
    const { data } = await useApi<boolean>(`game/verifycode/${ room.value }`, { method: 'GET' });
    isRoomCodeValid.value = data.value;
    if (isRoomCodeValid.value) {
      await joinRoom(user.value, room.value);
    } else {
      console.log('Invalid room code');
      toast.add({severity:'error',
        summary: 'not found',
        detail:'not found',
        life: 3000
      });
    }
  }
}

const joinRoom = async (user: string, room: string): Promise<void> => {
  try {
    const runtimeConfig = useRuntimeConfig();
    const webSocketUrl = `${ runtimeConfig.public.baseURL }/gamehub`;
    const joinConnection = new HubConnectionBuilder()
        .withUrl(webSocketUrl, {
          skipNegotiation: true,
          transport: HttpTransportType.WebSockets
        })
        .configureLogging(LogLevel.Information)
        .build();

    joinConnection.on('ReceiveCard', (user: string, message: string) => {
      messages.value.push({ user, message });
      console.log(messages.value);
    })

    joinConnection.on('UsersInRoom', (users) => {
      users.value = users;
    })

    joinConnection.onclose(() => {
      connection.value = null;
      messages.value = [];
      users.value = [];
    })

    await joinConnection.start();
    await joinConnection.invoke('JoinRoom', { user, room });
    connection.value = joinConnection;
  } catch (e) {
    console.log('HubConnection ERR --- ', e);
  }
}

const sendMessage = async (message: string): Promise<void> => {
  try {
    if (connection.value !== null) { await connection.value.invoke('SendCard', message) }
  } catch (e) {
    console.log(e)
  }
}

const closeConnection = async (): Promise<void> => {
  try {
    if (connection.value !== null) { await connection.value.stop() }
  } catch (e) {
    console.log(e)
  }
}
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
        <template v-if="isRoomCodeValid">
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
