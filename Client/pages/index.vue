<script setup lang="ts">
import { ref, onMounted } from 'vue';
import Header2 from "~/components/Header2.vue";

const showText = ref(true);

onMounted(() => {
  // Show initial message for a certain duration
  setTimeout(() => {
    showText.value = false;
  }, 2000); // duration
});
const getLogo = () => {
  return new URL('../assets/icons/logos/combination.svg', import.meta.url);
};

const getPearl = () => {
  return new URL('../assets/icons/logos/pearl.svg', import.meta.url);
};
</script>

<template>
  <Header2 v-if="!showText"/>
  <div id="home-page">
    <transition name="fade">
      <div v-if="showText" class="initial-message">
        <img class="pearl-logo" alt="pearl logo" 
             :src= 'getPearl()' />
        <br> <h1> Presents... </h1>
      </div>
    </transition>
    
    <transition name="fade">
      <div v-if="!showText" class="main-info"> 
<!--        <h1> Welcome to </h1> <br>-->
        <img class="cd-logo" alt="Cardhub logo"
             :src= 'getLogo()' />
      </div>
    </transition>
    
    
    <div v-if="!showText" class="join-button">
      <NuxtLink href="/join"> <Button>Play Now</Button> </NuxtLink>
    </div>
  </div>
</template>

<style scoped>
#home-page {
  width: 100%;
  height: 100%;
  display: flex;
  justify-content: center;
  flex-direction: column;
  align-items: center;
}

.initial-message, .main-info {
  text-align: center;
  padding-top: 30vh;
  justify-content: center;
  align-items: center;
  width: 50%;
}


.fade-enter-active, .fade-leave-active {
  transition: opacity 0.5s;
}

.fade-enter, .fade-leave-to {
  opacity: 0;
}


.join-button{
  text-align: center;
  margin-top: 50px;
}
</style>
