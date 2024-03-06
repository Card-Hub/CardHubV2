<script setup lang="ts">
import { HttpTransportType, type HubConnection, HubConnectionBuilder, LogLevel } from '@microsoft/signalr'
import { ref } from 'vue'
import type { UserMessage } from "../typescript/types"
import Chat from "../components/Chat.vue"

const connection = ref<HubConnection | null>(null)
const messages = ref<UserMessage[]>([])
const users = ref<string[]>([])
const user = ref('')
const room = ref('')
// const handleReceiveMessage = (user: string, message: string): void => {
//   messages.value.push({ user, message });
//   console.log("in handlereceivemessage",messages.value);
// };
// const newMessage = ref('');
// const props = defineProps(['connection'])

const joinRoom = async (user: string, room: string): Promise<void> => {
  try {
    const joinConnection  = new HubConnectionBuilder()
      .withUrl('https://localhost:7085/game', {
        skipNegotiation: true,
        transport: HttpTransportType.WebSockets
      })
      .configureLogging(LogLevel.Information)
      .build()

    joinConnection.on('ReceiveMessage', (user: string, message: string) => {
      messages.value.push({ user, message })
      console.log(messages.value)
    })

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

const closeConnection = async (): Promise<void> => {
  try {
    if (connection.value !== null) { await connection.value.stop() }
  } catch (e) {
    console.log(e)
  }
}
</script>

<template>
  <div class="message-container">
    <h2></h2>
    <template v-if="connection === null">
      <div class="flex flex-col gap-2 make-small-div div-align-center">
        <input class="p-inputtext1" type="text" v-model="user" placeholder="Name"/>
        <input class="p-inputtext1" type="text" v-model="room" placeholder="Game Pin"/>
        <button class="send-button" type="submit" @click="joinRoom(user, room)" :disabled="!user || !room">Enter</Button>
      </div>
    </template>
    <template v-else> <!-- Here is where we go after they have pressed join. May need to send them to new page adn take code from here -->
      <!--Chat :connection="connection" :onReceiveMessage="handleReceiveMessage" /-->
      <Chat :connection="connection"/>
    </template>
  </div>
</template>



<style scoped>

</style>

<!-- 
<p>In chat</p>
<div v-for="(m, index) in messages" :key="index">
  <div class="bg-primary">{{ m.message }}</div>
  <div>{{ m.user }}</div>
</div>
<div>
  <input class="p-inputtext1" type="text" v-model="newMessage" placeholder="Type your message..." />
  <button class="send-button" @click="() => sendMessage(newMessage)" :disabled="!newMessage">Send</button>
</div> -->