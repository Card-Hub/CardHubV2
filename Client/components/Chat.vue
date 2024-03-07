<script setup lang="ts">
import { ref } from 'vue';
import { useWebSocketStore } from '~/stores/webSocketStore';

const store = useWebSocketStore();
const {  newMessage, messages } = useWebSocketStore();
const { sendMessage } = store;
const newMessage1 = ref('');
const handleSendMessage = () => {
    console.log("inside handlemessage", newMessage1.value);
    sendMessage(newMessage1.value);
};
</script>

<template>
  <div>
    <p>In chat</p>
    <div v-for="(m, index) in messages" :key="index">
      <div class="bg-primary"> <span class="user">{{ m.user }}:</span> {{ m.message }}</div>
      <!-- <div>{{ m.user }}</div> -->
    </div>
    <div>
      <input class="p-inputtext1" type="text" v-model="newMessage1" placeholder="Type your message..." />
      <button class="send-button" @click="handleSendMessage" :disabled="(newMessage1=='')">Send</button>
    </div>
  </div>
</template>

<style scoped>
/* Add your styling here */
</style>