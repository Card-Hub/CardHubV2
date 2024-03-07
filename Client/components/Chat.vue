<script setup lang="ts">//this needs to be updated to not have props

import type { UserMessage } from "../typescript/types"
// import { HttpTransportType, type HubConnection, HubConnectionBuilder, LogLevel } from '@microsoft/signalr'
import connection from "../pages/Join.vue"
import type { HubConnection } from '@microsoft/signalr';


const newMessage = ref('');
// const connection = ref<HubConnection[]>([])
const messages = ref<UserMessage[]>([])
const props = defineProps({
  connection: Object as PropType<HubConnection | null>,
  onReceiveMessage: Function as PropType<(user: string, message: string) => void>,
});


const sendMessage = async (message: string): Promise<void> => {
    try {
        console.log("Liam im here1", message);
        if (props.connection !== null && props.connection !== undefined) {
            console.log("Liam im here", message);
            await props.connection.invoke('SendMessage', message)
            newMessage.value = '';
        }
    } catch (e) {
        console.log(e)
    }
}

watchEffect(() => {
  if (props.connection) {
    const joinConnection = props.connection;

    joinConnection.on('ReceiveMessage', (user: string, message: string) => {
      if (props.onReceiveMessage) {
        props.onReceiveMessage(user, message);
      }
      messages.value.push({ user, message });
      console.log("inwatcheffect: ",messages.value);
    });

    // Other event handlers...
  }
});

</script>

<template>
    <div>
        <p>In chat</p>
        <div v-for="(m, index) in messages" :key="index">
        <div class="bg-primary">{{ m.message }}</div>
        <div>{{ m.user }}</div>
    </div>
    <div>
        <input class="p-inputtext1" type="text" v-model="newMessage" placeholder="Type your message..." />

        <button class="send-button" @click="() => sendMessage(newMessage)" :disabled="!newMessage">Send</button>
    </div>
</div>
</template>

<style scoped>

</style>