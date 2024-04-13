<script setup lang="ts">
const availableAvatars = ['andy', 'fairy', 'femaleJuno', 'juno', 'lyssie', 'oli', 'ruby', 'star'];

const getIcon = (avatar: string) => {
  return new URL(`../../assets/icons/avatars/${avatar}.png`, import.meta.url);
};

</script>

<template>
  <div class="selection-container">
    <h1 class="text-2xl font-bold text-right">Select an Avatar</h1>
    <div class="flex flex-wrap justify-center items-center">
      <div v-for="avatar in availableAvatars" :key="avatar" class="relative w-20 h-32 m-2 bg-white rounded-md shadow-md p-2">
        <img :src="getIcon(avatar)" alt="avatar icon" class="absolute bottom-5 right-2 w-14 h-14" />
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
  background: rgb(243, 19, 19, 0.8);
  border-radius: 2px;
  box-shadow: 6px -6px 3px rgba(200, 200, 200, 0.4);
}
</style>