<script setup lang="ts">
const availableAvatars = ['lyssie', 'ruby', 'oli', 'femaleJuno', 'andy', 'juno', 'star', 'fairy'];

const getIcon = (avatar: string) => {
  return new URL(`../assets/icons/avatars/${avatar}.png`, import.meta.url);
};

</script>

<template>
  <div class="selection-container">
    <h1 class="text-2xl font-bold text-right">Select an Avatar</h1>
    <div class="flex flex-wrap justify-center items-center">
      <div v-for="avatar in availableAvatars" :key="avatar" class="avatar-holder relative w-20 h-32 m-2 rounded-md shadow-md p-2">
        <img :src="getIcon(avatar)" alt="avatar icon" class="avatar-icon absolute" />
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
}

.avatar-holder {
  display: flex;
  flex-direction: column;
  justify-content: center;
  align-items: center;
  padding-right: 50px;
}

.avatar-icon {
  width: 100px; /* Adjust the size of the player icon */
  height: 100px; /* Adjust the size of the player icon */
  border-radius: 50%; /* Ensure the player icon is circular */
  overflow: hidden; /* Ensure the player icon is circular */
  margin-right: 10px; /* Adjust the spacing between player icons */
}
</style>