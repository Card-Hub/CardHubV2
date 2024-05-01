<script setup lang="ts">
import { useWebSocketStore } from "~/stores/webSocketStore";

const store = useWebSocketStore();
const { tryJoinRoom, sendAvatar } = store;

const availableAvatars = ["lyssie", "ruby", "oli", "femaleJuno", "alex", "andy", "liam", "juno", "pocky", "star", "fairy", "dinoNugget1", "dinoNugget2", "dinoNugget3", "dinoNugget4", "amongusNugget"];
const currentChoice = ref<string>("lyssie");
// sendAvatar(currentChoice.value);

const getIcon = (avatar: string) => {
  return new URL(`../assets/icons/avatars/${ avatar }.png`, import.meta.url);
};

const isCurrentChoice = (avatar: string) => {
  return {
    border: currentChoice.value === avatar ? "2px solid #f31919" : "none",
    boxShadow: currentChoice.value === avatar ? "0px 0px 10px 5px rgba(243, 25, 25, 0.5)" : "none"
  };
};

const selectAvatar = (avatar: string) => {
  currentChoice.value = avatar;
  sendAvatar(avatar);
  isCurrentChoice(avatar);
};

</script>

<template>
  <div class="selection-container">
    <h1 class="text-2xl font-bold text-right">Select an Avatar</h1>
    <div class="player-icons">
      <div class="player-icon" v-for="(avatar) in availableAvatars" :key="avatar"
           :style="isCurrentChoice(avatar)">
        <img :src="getIcon(avatar)" alt="avatar Icon" class="player-icon-img" @click="selectAvatar(avatar)"/>
      </div>
    </div>
  </div>

</template>

<style scoped>
.selection-container {
  width: 75%;
  height: 50%;
  display: flex;
  flex-direction: column;
  justify-content: center;
  align-items: center;
  background: rgb(243, 19, 19, 0.2);
  border-radius: 2px;
  box-shadow: 6px -6px 3px rgba(200, 200, 200, 0.1);
  padding-bottom: .5%;
  margin-top: 5%;
}

.player-icons {
  display: flex;
  flex-wrap: wrap;
  justify-content: center;
  align-items: center;
  margin-left: .2%;
  margin-right: .2%;
}

.player-icon {
  width: 100px; /* Adjust the size of the player icon */
  height: 100px; /* Adjust the size of the player icon */
  border-radius: 50%; /* Ensure the player icon is circular */
  overflow: hidden; /* Ensure the player icon is circular */
  margin-right: 30px; /* Adjust the spacing between player icons */
  margin-bottom: 30px; /* Adjust the spacing between player icons */

  border: 2px solid transparent;
  transition: border-color 0.3s ease;
}

.player-icon:hover {
  border: 2px solid white;
  box-shadow: 0px 0px 10px 5px rgba(243, 25, 25, 0.5);
}

.player-icon-img {
  width: 100%;
  height: 100%;
  object-fit: cover; /* Ensure the player icon fills the circular container */
  background: rgba(255, 255, 255, 0.2); /* Adjust the background color of the player icon and opacity */
}
</style>