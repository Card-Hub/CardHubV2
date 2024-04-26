<script setup lang="ts">
import { ref } from 'vue';
import { useWebSocketStore } from '~/stores/webSocketStore';
import { storeToRefs } from "pinia";

const store = useWebSocketStore();
const { isPlayer, messages, user} = useWebSocketStore();
const { lobbyUsers } = storeToRefs(store);
const { sendMessage } = store;
// ref
const newMessage1 = ref('');//find a way to get rid fo this
const handleSendMessage = () => {
    console.log("inside handlemessage", newMessage1.value);
    sendMessage(newMessage1.value);
    newMessage1.value = '';
};

const getIcon = (avatar: string) => {
  if (avatar == "" || avatar == null) {
    return new URL(`../assets/icons/avatars/lyssie.png`, import.meta.url);
  }
  else {
    return new URL(`../assets/icons/avatars/${avatar}.png`, import.meta.url);
  }
};

const getIconGivenName = (name: string) => {
  console.log(lobbyUsers.value.length);
  let url;
  lobbyUsers.value.forEach(function (lu: LobbyUser) {
    console.log(lu.Name);
    console.log(lu.Avatar);
    if (lu.Name == name) {
    console.log("hfj");
      url = getIcon(lu.Avatar);
    }
  });
  if (url != null) {
    return url;
  }
  else {
    return getIcon("");
  }
};

</script>

<template>
  <div v-if="isPlayer">
    <div class="chat-container">
<!--      <h2 class="title text-center">Chat</h2>-->
      <div class="all-messages">
        <div v-for="(m, index) in messages" :key="index">
          <div class="each-message-container">
            <div class="system-message" v-if="m.user == 'System'">
              {{m.message}}
            </div>
            <div v-else-if="m.user != user" class="others-message-container">
              <img :src="getIconGivenName(m.user)" alt="avatar Icon" class="lobby-player-icon-img-chat">
              <div class="message-and-name">
                <span class="text-2xl text-neutral-300 username">{{ m.user }} </span>
                <div class="message-container"> {{ m.message }}</div>
              </div>
            </div>
            <div v-else class="my-message-container">
              <div class="message-and-name">
                <span class="text-2xl text-neutral-300 username">{{ m.user }} </span>
                <div class="message-container"> {{ m.message }}</div>
              </div>
              <img :src="getIconGivenName(m.user)" alt="avatar Icon" class="lobby-player-icon-img-chat">
            </div>
            <!--<i class="pi pi-user mx-4 text-neutral-300" style="font-size: 1.5rem"></i>-->
          </div>
        </div>
      </div>
      <div>
        <InputText class="input-yeee p-inputtext1" type="text" v-model="newMessage1" placeholder="Type your message..." />
        <Button class="send-button" @click="handleSendMessage" :disabled="(newMessage1=='')">Send</Button>
      </div>
    </div>
  </div>
  
  <div v-else-if="!isPlayer">
    <div class="chat-container">
      <h2 class="title text-center">Chat</h2>
      <div class="all-messages">
        <div v-for="(m, index) in messages" :key="index">
          <div class="each-message-container">
            <div class="system-message" v-if="m.user == 'System'">
              {{m.message}}
            </div>
            <div v-else-if="m.user != user" class="others-message-container">
              <img :src="getIconGivenName(m.user)" alt="avatar Icon" class="lobby-player-icon-img-chat">
              <div class="message-and-name">
                <span class="text-2xl text-neutral-300 username">{{ m.user }} </span>
                <div class="message-container"> {{ m.message }}</div>
              </div>
            </div>
            <div v-else class="my-message-container">
              <div class="message-and-name">
                <span class="text-2xl text-neutral-300 username">{{ m.user }} </span>
                <div class="message-container"> {{ m.message }}</div>
              </div>
              <img :src="getIconGivenName(m.user)" alt="avatar Icon" class="lobby-player-icon-img-chat">
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
  
</template>

<style scoped>
/* Add your styling here */
.chat {
    background-color: black;

}

.chat-container {
background-image: linear-gradient(#860e0e,
#3f0404);
  width: 100%;
  min-width: 200px;
  padding: 10px;
  border-radius: 15px;
  position:relative;
}
.each-message-container {
  margin-bottom: .5em;
}
.system-message {
  background: #280101;
  border-radius: 1em;
  padding: .5em;
  text-align: center;
}

.others-message-container {
  /*background-color: #dcd47f;*/
  display: grid;
  grid-template-columns: 15% 1fr;
  column-gap: .8em;
}

.my-message-container {
  /*background-color: #dcd47f;*/
  display: grid;
  grid-template-columns: 1fr 15%;
  column-gap: .8em;
}

.username {
  font-size: .8em;
  color: #d5d5d5;
  line-height: 1;
  padding: .5em;
}
.message-container {
  background-color: #000000;
  color: #ffffff;
  padding: 5px;
  border-radius: 0 .5em .5em .5em;
  
}
.input-yeee {
  width: 80%;
  height: 30px;
}
.message-and-name {
  padding:0;
  
}
.all-messages {
  overflow: auto;
}

.lobby-player-icon-img-chat {
  margin-top: 10px;
  width: 100%;
  border-radius: 50%; /* Ensure the player icon is circular */
  overflow: hidden;
  background-color: rgba(248, 182, 182, 0.4);
  
}
.user {
    color: var(--cardhub-red);
}

.send-button {
  margin-top: 10px;
  margin-left: 15px;
  height: 30px;
  background-color: #860e0e;
  color: white;
  border-radius: 5px;
  border: none;
  cursor: pointer;
}

/*.send-button:hover {
  background-color: #3f0404;
} */
</style>