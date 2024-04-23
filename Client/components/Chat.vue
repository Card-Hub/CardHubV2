<script setup lang="ts">
import { ref } from 'vue';
import { useWebSocketStore } from '~/stores/webSocketStore';
import { storeToRefs } from "pinia";

const store = useWebSocketStore();
const { messages} = useWebSocketStore();
// const { lobbyUsers } = storeToRefs(store);
const { sendMessage } = store;
ref
const newMessage1 = ref('');//find a way to get rid fo this
const handleSendMessage = () => {
    console.log("inside handlemessage", newMessage1.value);
    sendMessage(newMessage1.value);
    newMessage1.value = '';
};

const getIcon = (avatar: string) => {
  return new URL(`../assets/icons/avatars/${avatar}.png`, import.meta.url);
};

</script>

<template>
  <div class="chat-container">
    <h2 class="title text-center">Chat</h2>
    <div v-for="(m, index) in messages" :key="index">
        <div v-for="lobbyUser in lobbyUsers as LobbyUser[]">
              <!--<i class="pi pi-user mx-4 text-neutral-300" style="font-size: 1.5rem"></i>-->  
              <img v-if="lobbyUser.Name === m.user" :src="getIcon(lobbyUser.Avatar)" alt="avatar Icon" class="lobby-player-icon-img">
            <span v-if="lobbyUser.Name === m.user" class="text-2xl text-neutral-300">{{ lobbyUser.Name }} </span>
    </div>
        <div class="bg-primary"> {{ m.message }}</div>
    </div>
    <div>
      <input class="input-yeee p-inputtext1" type="text" v-model="newMessage1" placeholder="Type your message..." />
      <button class="send-button" @click="handleSendMessage" :disabled="(newMessage1=='')">Send</button>
    </div>
  </div>
</template>

<style scoped>
/* Add your styling here */
.chat {
    background-color: black;

}

.chat-container {
  background-color: #3d0b0b;
  width: 20%;
  padding: 10px;
  border-radius: 15px;
}

.input-yeee {
  width: 80%;
  height: 30px;
  color:
}

.lobby-player-icon-img {
    width: 2em;
    border-radius: 50%; /* Ensure the player icon is circular */
  overflow: hidden;
  }

.user {
    color: var(--cardhub-red);
}
</style>