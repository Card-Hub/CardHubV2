<script setup lang="ts">
import { HttpTransportType, type HubConnection, HubConnectionBuilder, LogLevel } from '@microsoft/signalr'
import { ref } from 'vue'

interface UserMessage {
  user: string
  message: string
}

const connection = ref<HubConnection | null>(null)
const messages = ref<UserMessage[]>([])
const users = ref<string[]>([])
const user = ref('')
const room = ref('')

const joinRoom = async (user: string, room: string): Promise<void> => {
  try {
    const joinConnection = new HubConnectionBuilder()
      .withUrl('https://localhost:7085/game', {
        skipNegotiation: true,
        transport: HttpTransportType.WebSockets
      })
      .configureLogging(LogLevel.Information)
      .build()

    joinConnection.on('ReceiveCard', (user: string, message: string) => {
      messages.value.push({ user, message })
      console.log(messages.value)
    })

    joinConnection.on('UsersInRoom', (users) => {
      users.value = users
    })

    joinConnection.onclose(() => {
      connection.value = null
      messages.value = []
      users.value = []
    })

    await joinConnection.start()
    await joinConnection.invoke('JoinRoom', { user, room })
    connection.value = joinConnection
  } catch (e) {
    console.log('HubConnection ERR --- ', e)
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
  <div class="app">
    <h2></h2>
    <template v-if="connection === null">
      <div class="flex flex-col gap-2">
        <InputText type="text" v-model="user" placeholder="Name"/>
        <InputText type="text" v-model="room" placeholder="Game Pin"/>
        <Button type="submit" @click="joinRoom(user, room)" :disabled="!user || !room">Enter</Button>
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
